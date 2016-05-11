using IndianaPark.Tools.Wizard;

namespace IndianaPark.PowerFan.Wizard
{
    /// <summary>
    /// Rappresenta lo stato in cui l'utente seleziona quanti biglietti stampare
    /// </summary>
    public class QuantityState : PowerfanBaseState
    {
        private uint m_quantita;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityState"/> class.
        /// </summary>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <remarks>
        /// Ogni stato ricorda il suo precedente, che è lo stato che lo ha creato e non quello da cui si arriva
        /// </remarks>
        public QuantityState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
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
        public override void EnterState( PowerFanBuilder builder )
        {
            this.StateForm = new Forms.New.QuantityForm( this.m_quantita );
            base.EnterState( builder );
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( PowerFanBuilder builder )
        {
            builder.Storage.Quantity = this.m_quantita;
        }

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, Tools.Navigation.NavigationEventArgs e )
        {
            this.m_quantita = WizardForm.ConvertUserData<uint>( (Forms.New.QuantityForm)source );

            this.NextState = this.StatePool.GetUniqueType( new InsertPriceState( this.Wizard, this ) );
            this.OnStatusChangeRequested( e.Status );
        }
    }
}
