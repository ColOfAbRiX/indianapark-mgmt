using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;
using IndianaPark.PercorsiAvventura.Forms;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Rappresenta lo stato dove si inserisce il nominativo per la creazione o verifica di un abbonamento
    /// </summary> 
    public sealed class TipoAbbonamentoState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
		#region Enumerations 

        /// <summary>
        /// Possibili azioni che la form <see cref="SceltaAbbonamentoForm"/> può richiedere
        /// </summary>
        internal enum AbbonamentoAction
        {
            /// <summary>
            /// Richiesta di emettere un nuovo abbonamento
            /// </summary>
            NewSubscription,
            /// <summary>
            /// Richiesta di convalidare ed usare un abbonamento vecchio
            /// </summary>
            OldSubscription
        }

		#endregion Enumerations 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, NavigationEventArgs e )
        {
            if( e.Status == NavigationAction.Next )
            {
                //var data = (StateData)this.m_navigationForm.GetInput();
                this.NextState = this.StatePool.GetUniqueType( new NominativoAbbonamentoState( this.Wizard, this ) );
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
        public TipoAbbonamentoState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
            this.StateForm = new SceltaAbbonamentoForm();
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettoPercorsiBuilder builder )
        {
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}