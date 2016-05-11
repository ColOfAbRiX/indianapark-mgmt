using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using IndianaPark.Biglietti.Forms.New;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;
using IndianaPark.Tools;

namespace IndianaPark.Biglietti.Wizard
{
    /// <summary>
    /// Rappresenta lo stato dove si sceglie la tipologia di attrazione
    /// </summary>
    public class TipoAttrazioneState : EmissioneBaseState<BigliettiBuilder>
    {
		#region Nested Classes 

        /// <summary>
        /// Utilizzata come dati di ingresso per la creazione della form delle attrazioni e per trasferire i
        /// dati dalla View <see cref="TipoAttrazioneForm"/> al Controller <see cref="TipoAttrazioneState"/>
        /// </summary>
        public class AttrazioneData
        {
            #region Fields 

            #region Internal Fields 

            private Type m_nextState;

            #endregion Internal Fields

            #region Public Fields 

            /// <summary>
            /// Prossimo stato che la selezione permette di raggiungere.
            /// </summary>
            /// <remarks>
            /// Il valore deve implementare <see cref="IState"/>.
            /// </remarks>
            public Type NextState
            {
                get
                {
                    return this.m_nextState;
                }
                set
                {
                    if( value != null )
                    {
                        if( !value.ImplementsInterface( typeof(IState) ) )
                        {
                            throw new ArgumentOutOfRangeException( "value", "The parameter must be implement IState" );
                        }
                    }

                    this.m_nextState = value;
                }
            }

            /// <summary>
            /// Nome visualizzato sul pulsante
            /// </summary>
            public string Nome { get; set; }

            /// <summary>
            /// Il builder per l'attrazione scelta
            /// </summary>
            public IBuilder Builder { get; set; }

            #endregion Public Fields

            #endregion Fields 
        }
		
        #endregion Nested Classes 

		#region Fields 

        protected readonly Tools.TypedObjectPool<Tools.Wizard.Wizard> m_wizardPool = new IndianaPark.Tools.TypedObjectPool<Tools.Wizard.Wizard>();
        private readonly bool m_autoForward = (bool)PluginBiglietti.GetGlobalParameter( "AutoForward" ).Value;
        private readonly List<AttrazioneData> m_listaAttrazioni;
        private IPrintableTickets m_attrazioneStorage;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, NavigationEventArgs e )
        {
            this.NextState = null;

            if( e.Status == NavigationAction.Next )
            {
                // La mia form mi dice quale stato l'utente ha selezionato
                this.StartSubWizard( WizardForm.ConvertUserData<AttrazioneData>(this.UserData) );
            }
            else
            {
                this.OnStatusChangeRequested( e.Status );
            }
        }

        /// <summary>
        /// Gestisce il completamento del sottowizard
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void SubWizardCompletedHandler( object source, NavigationEventArgs e )
        {
            if( e.Status == NavigationAction.Finish || e.Status == NavigationAction.Cancel )
            {
                if( e.Status == NavigationAction.Finish )
                {
                    // Recupero il risultato del builder del wizard interno
                    this.m_attrazioneStorage = (IPrintableTickets)((Tools.Wizard.Wizard)source).Builder.GetResult();
                }

                // Lo stato di terminazione del wizard interno viene propagato su quello esterno
                this.NextState = null;

                this.OnStatusChangeRequested( e.Status );

                // Finalizzo il wizard
                ((Tools.Wizard.Wizard)source).Dispose();
            }
            else if( this.Gui != null )
            {
                this.Gui.Show();
            }
            else
            {
                // Se sono qui c'è qualche problema, perchè dovrei far vedere una GUI che non c'è...
                System.Diagnostics.Trace.WriteLine( String.Format( "Problem: the wizard is not finished, but there's no GUI to show, in {0}", this.GetType().Name ) );
                this.OnStatusChangeRequested( NavigationAction.Cancel );
            }
        }

        #endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoAttrazioneState"/> class.
        /// </summary>
        /// <param name="listaAttrazioni">La lista delle attrazioni </param>
        public TipoAttrazioneState( Tools.Wizard.Wizard wizard, IList<AttrazioneData> listaAttrazioni ) : base( wizard, null )
        {
            this.m_listaAttrazioni = (List<AttrazioneData>)listaAttrazioni;

            // Se l'auto-forward è abilitato evito di creare la Form
            if( !this.m_autoForward || this.m_listaAttrazioni.Count > 1 )
            {
                this.StateForm = new Forms.New.TipoAttrazioneForm( this.m_listaAttrazioni );
            }
        }

		#endregion Constructors 

        #region Internal Methods 

        private void StartSubWizard( AttrazioneData startData )
        {
            if( startData.NextState == null )
            {
                return;
            }

            var emissioneWizard = new Tools.Wizard.Wizard( startData.Builder );

            // Non lavoro direttamente con le istanze perchè ho la necessità di passare argomenti al costruttore
            IState nextState = (IState)Activator.CreateInstance( startData.NextState, new object[] { emissioneWizard, null } );

            // Notare il commento del commento
            /* Note:
                * Se l'utente va molto avanti col wizard, poi ritorna a questo punto exception sceglie ancora la stessa
                * attrazione di prima, quello che ha fatto precedentemente viene perso. Al momento è opportuno
                * mantenere le così così invece di perdere tempo a risolvere.
            **/
            //emissioneWizard = this.m_wizardPool.GetUniqueType( emissioneWizard );

            emissioneWizard.CancelState = new StateFactory<CancelWizardState>();

            // Registro il gestore del completamento solo una volta
            emissioneWizard.WizardCompleted -= this.SubWizardCompletedHandler;
            emissioneWizard.WizardCompleted += this.SubWizardCompletedHandler;

            // Si può navigare tra stati di wizard diversi, ma questi rimangono registrati ai loro wizard
            emissioneWizard.StartWizard( nextState );
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato
        /// </summary>
        public override void EnterState( BigliettiBuilder builder )
        {
            // Se non ci sono attrazioni non faccio niente
            if( this.m_listaAttrazioni.Count == 0 )
            {
                MessageBox.Show(
                    IndianaPark.Biglietti.Properties.Resources.NoAttractionsText,
                    IndianaPark.Biglietti.Properties.Resources.NoAttractionsTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                this.OnStatusChangeRequested( NavigationAction.Cancel );
                return;
            }

            // Controllo se devo fare l'auto-forward
            if( this.m_autoForward && this.m_listaAttrazioni.Count == 1 )
            {
                // In background viene fatto partire il nuovo Wizard
                using( var bw = new System.ComponentModel.BackgroundWorker() )
                {
                    bw.RunWorkerCompleted += delegate
                    {
                        this.StartSubWizard( this.m_listaAttrazioni[0] );
                    };
                    bw.RunWorkerAsync();
                }
            }
            else if( this.Gui != null )
            {
                this.Gui.Show();
            }
        }

        /// <summary>
        /// Termina l'esecuzione dello stato. Viene chiamato ogni volta che lo stato termina di essere quello corrente
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettiBuilder builder )
        {
            // Controllo null perchè se scelgo di uscire la variabile può essere nulla
            if( this.m_attrazioneStorage != null )
            {
                // Salvo il risultato del builder del wizard interno in nel builder del wizard esterno
                builder.Storage.TicketObject = this.m_attrazioneStorage;
            }
        }

        #endregion Public Methods 

		#endregion Methods 
    }


    /// <summary>
    /// E' il costruttore di un pulsante di selezione gia formattato.
    /// </summary>
    internal class ChoiseButtonCreator : Tools.Controls.ControlRepeater.ControlCreatorBase
    {
        #region Fields

        private readonly Size m_size;
        private int m_tabIndex;

        #endregion Fields

        #region Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiseButtonCreator"/> class.
        /// </summary>
        /// <param name="buttonSize">La dimensione del pulsante</param>
        public ChoiseButtonCreator( Size buttonSize )
        {
            this.m_size = buttonSize;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Crea un nuovo controllo.
        /// </summary>
        /// <returns>Il nuovo controllo.</returns>
        public override Control CreateNewControl()
        {
            var newButton = new Button
            {
                UseVisualStyleBackColor = true,
                Size = this.m_size,
                Font = new Font( new FontFamily( "Tahoma" ), (float)15.75, FontStyle.Bold ),
                TabIndex = this.m_tabIndex++
            };

            return newButton;
        }

        #endregion Public Methods

        #endregion Methods
    }
}
