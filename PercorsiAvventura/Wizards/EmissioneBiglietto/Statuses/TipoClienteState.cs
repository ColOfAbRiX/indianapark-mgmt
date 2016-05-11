using System;
using System.Collections.Generic;
using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;
using System.Linq;
using IndianaPark.PercorsiAvventura.Forms;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Rappresenta lo stato dove si inserisce il numero di clienti per ogni tipologia cliente e se aggiungere sconti
    /// </summary>
    /// <remarks>
    /// Questo stato manipola direttamente i dati del builder, quindi è necessario caricare ogni volta i dati.
    /// </remarks>
    public sealed class TipoClienteState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
		#region Nested Classes 

        /// <summary>
        /// Utilizzata nel caso <see cref="StateData.Action"/> valga <see cref="TipoClienteAction.Sconta"/>
        /// </summary>
        internal class ActionScontaData : StateData
        {
            /// <summary>
            /// Il cliente selezionato a cui aggiungere lo sconto
            /// </summary>
            public int SelectedCliente { get; set; }
        }

        /// <summary>
        /// Utilizzata per trasferire i dati dalla View <see cref="TipoClienteForm"/> al Controller <see cref="TipoClienteState"/>
        /// </summary>
        internal class StateData
        {
		    #region Fields 

            /// <summary>
            /// L'azione che lo stato deve intraprendere
            /// </summary>
            /// <value>The action.</value>
            public TipoClienteAction Action { get; set; }

            /// <summary>
            /// Contiene le informazioni sui clienti modificate dall'utente nella form.
            /// </summary>
            public List<ClientiPartial> InfoClienti { get; set; }

		    #endregion Fields 
        }

		#endregion Nested Classes 

		#region Enumerations 

        /// <summary>
        /// Rappresenta le possibili azioni che l'utente può compiere dalla form
        /// </summary>
        internal enum TipoClienteAction
        {
            /// <summary>
            /// L'utente richiede di scontare una tipologia cliente
            /// </summary>
            Sconta,
            /// <summary>
            /// L'utente richiede di eliminare uno sconto da una tipologia cliente
            /// </summary>
            Elimina,
            /// <summary>
            /// L'utente ha terminato le operazioni e desidera andare avanti.
            /// </summary>
            Avanti,
            /// <summary>
            /// Richiede di recuperare semplicemente i dati
            /// </summary>
            Recupera
        }

		#endregion Enumerations 

		#region Fields 

        private BigliettoPercorsiBuilder m_builder;
        private List<ClientiPartial> m_infoClienti;
        private Model.TipoBiglietto m_riferimentoBiglietto;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Gestisce le richieste da parte della form di aggiornare i suoi dati
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void UpdateFormDataHandler( object sender, EventArgs e )
        {
            var viewData = WizardForm.ConvertUserData<StateData>( this.UserData );

            this.m_infoClienti = viewData.InfoClienti;
            this.m_builder.Storage.Clienti = viewData.InfoClienti;

            this.UpdatePartialPrice();
        }

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, NavigationEventArgs e )
        {
            // Recupero le informazioni generali dalla form
            var viewData = WizardForm.ConvertUserData<StateData>( this.UserData );
            this.m_infoClienti = viewData.InfoClienti;

            // Controllo quali azioni specifiche devo svolgere
            if( e.Status == NavigationAction.Next )
            {
                int selected;

                switch( viewData.Action )
                {
                    case TipoClienteAction.Sconta:
                        selected = this.AddClienteRow( ((ActionScontaData)viewData).SelectedCliente );
                        this.NextState = new SceltaScontiState( this.Wizard, this, selected );
                        break;

                    case TipoClienteAction.Elimina:
                        selected = ((ActionScontaData)viewData).SelectedCliente;
                        this.NextState = new EliminaScontiState( this.Wizard, this, selected );
                        break;

                    case TipoClienteAction.Avanti:
                        this.NextState = new SceltaBriefingState( this.Wizard, this );
                        break;
                }
            }

            this.OnStatusChangeRequested( e.Status );
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoAttrazioneState"/> class.
        /// </summary>
        public TipoClienteState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Aggiorna il prezzo totale derivante dall'emissione dei biglietti
        /// </summary>
        private void UpdatePartialPrice()
        {
            this.UserData.SetInformations( this.m_builder.BuildPartialResult() );
        }

        /// <summary>
        /// Aggiunge uno sconto ad un cliente
        /// </summary>
        /// <param name="selected">L'indice dell'elemento che l'utente ha selezionato.</param>
        /// <returns>L'indice del nuovo elemento aggiunto</returns>
        private int AddClienteRow(int selected)
        {
            // Il cliente viene aggiunto sempre in fondo!
            var newCliente = m_infoClienti[selected].Clone();
            if( newCliente.Quantita == 0 )
            {
                newCliente.Quantita = 1;
            }
            this.m_infoClienti.Add( newCliente );
            this.m_infoClienti[selected].Quantita = 0;

            return m_infoClienti.IndexOf( newCliente );
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato
        /// </summary>
        public override void EnterState( BigliettoPercorsiBuilder builder )
        {
            // Durante l'esecuzione è necessario aggiornare il prezzo parziale
            this.m_builder = builder;

            // Se è memorizzato un builder, vado a recuperare da lui la lista clienti
            if( m_builder.Storage.Clienti != null )
            {
                this.m_infoClienti = builder.Storage.Clienti;
            }

            // Recupero le informazioni si tipi di cliente relativi al tipo biglietto scelto
            if( this.m_riferimentoBiglietto != builder.Storage.TipoBiglietto )
            {
                this.m_riferimentoBiglietto = builder.Storage.TipoBiglietto;
                this.m_infoClienti = ClientiPartial.GetFromParco( m_riferimentoBiglietto ).ToList();
            }

            // Elimino i duplicati derivanti dall'add/remove di sconti
            this.m_infoClienti = ClientiPartial.CompactData( this.m_infoClienti ).ToList();
            this.m_builder.Storage.Clienti = m_infoClienti;

            this.StateForm = new TipoClienteForm( m_infoClienti );
            this.UserData.UpdateInformations += this.UpdateFormDataHandler;
            this.UpdatePartialPrice();

            base.EnterState( builder );
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettoPercorsiBuilder builder )
        {
            builder.Storage.Clienti = m_infoClienti;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}