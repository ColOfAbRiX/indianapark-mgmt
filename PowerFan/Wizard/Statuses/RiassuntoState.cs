using System;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PowerFan.Wizard
{
    /// <summary>
    /// Rappresenta lo stato in cui viene visualizzato il riassunto della procedura
    /// </summary>
    public class RiassuntoState : PowerfanBaseState
    {
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

        /// <summary>
        /// Avvia la gestione dello stato
        /// </summary>
        /// <param name="builder"></param>
        /// <remarks>
        /// La procedura standard prevede che questo metodo sia chiamato mentre è ancora attivo il gestore degli eventi
        /// dello stato precedente.
        /// </remarks>
        public override void EnterState(PowerFanBuilder builder)
        {
            var biglietto = (Model.Biglietto)Activator.CreateInstance( builder.Storage.TipoBiglietto, (decimal)builder.Storage.Prezzo );
            this.StateForm = new Forms.New.RiassuntoForm( builder.Storage.Quantity, biglietto );
            base.EnterState( builder );
        }

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, Tools.Navigation.NavigationEventArgs e )
        {
            this.OnStatusChangeRequested( e.Status );
        }
    }
}
