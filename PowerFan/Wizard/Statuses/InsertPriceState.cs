using IndianaPark.Tools.Wizard;

namespace IndianaPark.PowerFan.Wizard
{
    /// <summary>
    /// Stato in cui iene richiesto il prezzo di un biglietto
    /// </summary>
    public class InsertPriceState : PowerfanBaseState
    {
        private double m_price;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertPriceState"/> class.
        /// </summary>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <remarks>
        /// Ogni stato ricorda il suo precedente, che è lo stato che lo ha creato e non quello da cui si arriva
        /// </remarks>
        public InsertPriceState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
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
            this.StateForm = new Tools.Controls.NumericInputForm( "Prezzo del biglietto", "Prezzo di un biglietto" );
            base.EnterState( builder );
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( PowerFanBuilder builder )
        {
            builder.Storage.Prezzo = this.m_price;
        }

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, IndianaPark.Tools.Navigation.NavigationEventArgs e )
        {
            var value = WizardForm.ConvertUserData<string>( (Tools.Controls.NumericInputForm)source );

            if( e.Status == IndianaPark.Tools.Navigation.NavigationAction.Next )
            {
                this.m_price = double.Parse( value );
            }
            else
            {
                this.m_price = 0;
            }

            this.NextState = this.StatePool.GetUniqueType( new RiassuntoState( this.Wizard, this ) );
            this.OnStatusChangeRequested( e.Status );
        }
    }
}
