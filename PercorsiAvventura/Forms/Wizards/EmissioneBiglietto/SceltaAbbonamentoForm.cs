using System;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// Finestra di dialogo per la selezione della tipologia di abbonamento
    /// </summary>
    public partial class SceltaAbbonamentoForm : WizardForm
    {
		#region Fields 

        Wizard.TipoAbbonamentoState.AbbonamentoAction m_action;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        private void NuovoClickHandler( object sender, EventArgs e )
        {
            this.m_action = Wizard.TipoAbbonamentoState.AbbonamentoAction.NewSubscription;
            this.Sail( NavigationAction.Next );
        }

        private void VecchioClickHandler( object sender, EventArgs e )
        {
            this.m_action = Wizard.TipoAbbonamentoState.AbbonamentoAction.OldSubscription;
            this.Sail( NavigationAction.Next );
        }

        private void EscapeClickHandler( object sender, EventArgs e )
        {
            this.Sail( NavigationAction.Cancel );
        }

        private void BackClickHandler( object sender, EventArgs e )
        {
            this.Sail( NavigationAction.Back );
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SceltaAbbonamentoForm"/> class.
        /// </summary>
        public SceltaAbbonamentoForm()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        /// <returns></returns>
        public override object GetData()
        {
            return this.m_action;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
