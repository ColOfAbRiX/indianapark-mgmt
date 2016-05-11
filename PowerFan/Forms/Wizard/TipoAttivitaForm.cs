using System;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PowerFan.Forms.New
{
    /// <summary>
    /// Finestra di dialogo per la selezione della tipologia di abbonamento
    /// </summary>
    public partial class TipoAttivitaForm : WizardForm
    {
		#region Fields 

        private Model.Biglietto m_biglietto;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        private void ArrampicataClickHandler( object sender, EventArgs e )
        {
            this.m_biglietto = new Model.BigliettoArrampicata( 0 );
            this.Sail( NavigationAction.Next );
        }

        private void PowerfanClickHandler( object sender, EventArgs e )
        {
            this.m_biglietto = new Model.BigliettoPowerFan( 0 );
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
        /// Initializes a new instance of the <see cref="TipoAttivitaForm"/> class.
        /// </summary>
        public TipoAttivitaForm()
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
            return this.m_biglietto;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
