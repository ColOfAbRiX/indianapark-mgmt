using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    ///<summary>
    /// Rappresenta lo stato dove si sceglie il tipo base di sconto personalizzato
    ///</summary>
    public class TipoScontoState : EmissioneBaseState<CustomDiscountBuilder>
    {
        private ScontoCreator m_scontoCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoScontoState"/> class.
        /// </summary>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <remarks>
        /// Ogni stato ricorda il suo precedente, che è lo stato che lo ha creato e non quello da cui si arriva
        /// </remarks>
        public TipoScontoState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
            this.StateForm = new Forms.ScontoPersonalizzatoForm();
        }

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, IndianaPark.Tools.Navigation.NavigationEventArgs e )
        {
            this.m_scontoCreator = null;
            this.NextState = null;

            if( e.Status == IndianaPark.Tools.Navigation.NavigationAction.Next )
            {
                // Recupero il tipo di sconto
                this.NextState = this.StatePool.GetUniqueType( new InputScontoState( this.Wizard, this ) );
                this.m_scontoCreator = WizardForm.ConvertUserData<ScontoCreator>( this.UserData );
            }

            this.OnStatusChangeRequested( e.Status );
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( CustomDiscountBuilder builder )
        {
            builder.SetScontoCreator( this.m_scontoCreator );
            base.ExitState( builder );
        }
    }
}
