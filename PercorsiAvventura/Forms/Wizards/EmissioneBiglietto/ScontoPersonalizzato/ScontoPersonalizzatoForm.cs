using System;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// Finestra di dialogo per la selezione della tipologia di abbonamento
    /// </summary>
    public partial class ScontoPersonalizzatoForm : WizardForm
    {
		#region Fields 

        private Wizard.ScontoCreator m_type;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        private void ScontoFissoClickHandler( object sender, EventArgs e )
        {
            this.m_type = new Wizard.ScontoFissoCreator();
            this.Sail( NavigationAction.Next );
        }

        private void ScontoPercentualeClickHandler( object sender, EventArgs e )
        {
            this.m_type = new Wizard.ScontoPercentualeCreator();
            this.Sail( NavigationAction.Next );
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
        public ScontoPersonalizzatoForm()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        public override object GetData()
        {
            return this.m_type;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
