using System.Windows.Forms;
using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Navigation;
using IndianaPark.PercorsiAvventura.Forms;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Rappresenta lo stato dove si seleziona lo sconto personale da applicare ad una tipologia clienti
    /// </summary>
    /// <remarks>
    /// Questo stato modifica lo <see cref="ClientiPartial.ScontoPersonale"/> di un preciso elemento
    /// di <see cref="BigliettoPercorsiBuilder.PercorsiStorage"/>
    /// </remarks>
    public sealed class SceltaScontiState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
		#region Nested Classes 

        /// <summary>
        /// Utilizzata per trasferire i dati dalla View <see cref="SceltaScontoForm"/> al Controller <see cref="SceltaScontiState"/>
        /// </summary>
        internal class StateData
        {
            /// <summary>
            /// Lo sconto scelto dall'utente nella form
            /// </summary>
            public Model.ISconto ScontoPersonale { get; set; }

            /// <summary>
            /// Se impostato a <c>true</c> indica la richiesta di creare uno sconto personalizzato
            /// </summary>
            public bool CustomRequested { get; set; }
        }

		#endregion Nested Classes 

		#region Fields 

		#region Internal Fields 

        private readonly int m_index;
        private Model.ISconto m_scontoScelto;

		#endregion Internal Fields 

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
            // Se non viene richiesto di proseguire, non faccio niente exception propago lo stato
            if( e.Status != NavigationAction.Next )
            {
                this.OnStatusChangeRequested( e.Status );
                return;
            }

            var data = WizardForm.ConvertUserData<StateData>( this.UserData );

            // Controllo se è stato richiesto di inserire uno sconto personalizzato
            if( !data.CustomRequested )
            {
                this.m_scontoScelto = data.ScontoPersonale;
                this.NextState = this.StatePool.GetUniqueType( (EmissioneBaseState<BigliettoPercorsiBuilder>)this.PreviousState );

                this.OnStatusChangeRequested( NavigationAction.Next );
            }
            else
            {
                this.NextState = null;

                var customWizard = new Tools.Wizard.Wizard( new CustomDiscountBuilder() );

                customWizard.WizardCompleted += CustomDiscountWizardHandler;
                customWizard.StartWizard( new TipoScontoState( customWizard, null ) );
            }
        }

        /// <summary>
        /// Gestisce la terminazione del wizard per la creazione di uno sconto personalizzato
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void CustomDiscountWizardHandler( object sender, NavigationEventArgs e )
        {
            var wizard = ((Tools.Wizard.Wizard)sender);

            if( e.Status == NavigationAction.Finish )
            {
                // Recupero ed assegno lo sconto appena creato
                var nuovoSconto = ((CustomDiscountBuilder)wizard.Builder).GetResult();

                this.m_scontoScelto = nuovoSconto;
                this.NextState =
                    this.StatePool.GetUniqueType( (EmissioneBaseState<BigliettoPercorsiBuilder>)this.PreviousState );

                this.OnStatusChangeRequested( NavigationAction.Next );

                wizard.Dispose();
            }
            else//if( exception.Status == NavigationAction.Back )
            {
                this.EnterState( null );
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SceltaScontiState"/> class.
        /// </summary>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <param name="clienteIndex">L'indice corrispondente all'elemento di <see cref="BigliettoPercorsiBuilder.PercorsiStorage.Clienti"/>
        /// da modificare.</param>
        public SceltaScontiState( Tools.Wizard.Wizard wizard, IState previous, int clienteIndex ) : base( wizard, previous )
        {
            this.m_index = clienteIndex;
            this.StateForm = new SceltaScontoForm();
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettoPercorsiBuilder builder )
        {
            builder.Storage.Clienti[this.m_index].ScontoPersonale = this.m_scontoScelto;
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Rappresenta lo stato dove si chiede conferma dell'eliminazione degli sconti e si effettua praticamente l'operazione
    /// </summary>
    /// <remarks>
    /// Esattmente come <see cref="SceltaScontiState"/>, questo stato elimina lo <see cref="ClientiPartial.ScontoPersonale"/>
    /// di un preciso elemento di <see cref="BigliettoPercorsiBuilder.PercorsiStorage"/>
    /// </remarks>
    public sealed class EliminaScontiState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
		#region Fields 

        private readonly int m_index;
        private System.Windows.Forms.DialogResult m_result;

        #endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="EliminaScontiState"/> class.
        /// </summary>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <param name="clienteIndex">L'indice corrispondente all'elemento di <see cref="BigliettoPercorsiBuilder.PercorsiStorage.Clienti"/>
        /// da modificare.</param>
        public EliminaScontiState( Tools.Wizard.Wizard wizard, IState previous, int clienteIndex ) : base( wizard, previous )
        {
            this.m_index = clienteIndex;
            this.NextState = this.StatePool.GetUniqueType( (EmissioneBaseState<BigliettoPercorsiBuilder>)this.PreviousState );
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato
        /// </summary>
        /// <param name="builder"></param>
        /// <remarks>
        /// La procedura standard prevede che questo metodo sia chiamato mentre è ancora attivo il gestore degli eventi
        /// dello stato precedente.
        /// </remarks>
        public override void EnterState( BigliettoPercorsiBuilder builder )
        {
            // Controlla se è abilitato l'eliminazione automatica
            if( (bool)PluginPercorsi.GetGlobalParameter( "AskDeleteOnZero" ).Value )
            {
                this.m_result = DialogResult.Yes;
                this.OnStatusChangeRequested( NavigationAction.Next );
                return;
            }

            var bw = new System.ComponentModel.BackgroundWorker();

            // Fà una domanda all'operatore
            bw.DoWork +=
                delegate
                {
                    this.m_result =
                        System.Windows.Forms.MessageBox.Show(
                            Properties.Resources.DeleteScontoText,
                            Properties.Resources.DeleteScontoTitle,
                            System.Windows.Forms.MessageBoxButtons.YesNo,
                            System.Windows.Forms.MessageBoxIcon.Exclamation,
                            System.Windows.Forms.MessageBoxDefaultButton.Button2 );
                };

            // A lavoro completato manda avanti l'esecuzione
            bw.RunWorkerCompleted +=
                delegate
                {
                    if( this.m_result == DialogResult.Yes )
                    {
                        this.OnStatusChangeRequested( NavigationAction.Next );
                        return;
                    }
                    this.OnStatusChangeRequested( NavigationAction.Back );
                };

            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettoPercorsiBuilder builder )
        {
            if( this.m_result == DialogResult.Yes )
            {
                builder.Storage.Clienti[this.m_index].ScontoPersonale = null;
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
