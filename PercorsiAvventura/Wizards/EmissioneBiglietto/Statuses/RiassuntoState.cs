using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Rappresenta lo stato che visualizza all'operatore un riassunto e chiede conferma della stampa
    /// </summary>
    public sealed class RiassuntoState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, IndianaPark.Tools.Navigation.NavigationEventArgs e )
        {
            this.OnStatusChangeRequested( e.Status );
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="RiassuntoState"/> class.
        /// </summary>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <remarks>
        /// Ogni stato ricorda il suo precedente, che è lo stato che lo ha creato e non quello da cui si arriva
        /// </remarks>
        public RiassuntoState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
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
            this.StateForm = new Forms.RiassuntoForm( builder.Storage, builder.BuildPartialResult() );
            base.EnterState( builder );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
