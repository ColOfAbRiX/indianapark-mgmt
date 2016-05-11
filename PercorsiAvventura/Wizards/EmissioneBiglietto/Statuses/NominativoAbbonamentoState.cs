using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Rappresenta lo stato dove si inserisce il nominativo con cui registrare/con cui è registrato l'abbonato
    /// </summary> 
    public sealed class NominativoAbbonamentoState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
        #region Fields 

        private string m_nominativo;

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
            if( e.Status == NavigationAction.Next )
            {
                // Recupero il testo del nominativo
                this.m_nominativo = WizardForm.ConvertUserData<string>( this.UserData ).Trim();
                this.NextState = this.StatePool.GetUniqueType( null );
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
        public NominativoAbbonamentoState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
            this.StateForm = new Tools.Controls.TextInputForm( Properties.Resources.NuovoAbbonamentoNominativoTitle, Properties.Resources.NuovoAbbonamentoNominativoText );
        }

        #endregion Constructors

        #region Public Methods 

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettoPercorsiBuilder builder )
        {
            // Salvo il nominativo inserito nel builder
            builder.Storage.Nominativo = this.m_nominativo;
        }

        #endregion Public Methods

        #endregion Methods
    }
}
