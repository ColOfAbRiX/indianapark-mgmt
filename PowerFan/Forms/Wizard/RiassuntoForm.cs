using System;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PowerFan.Forms.New
{
    /// <summary>
    /// Form per la visualizzazione del riassunto
    /// </summary>
    public partial class RiassuntoForm : WizardForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RiassuntoForm"/> class.
        /// </summary>
        /// <param name="quantita">Il numero di biglietti da stampare</param>
        /// <param name="biglietto">The biglietto.</param>
        public RiassuntoForm( uint quantita, Model.Biglietto biglietto )
        {
            InitializeComponent();
            this.label1.Text = String.Format( this.label1.Text, quantita, biglietto.ToString() );
        }

        private void NavigationHandler( object source, IndianaPark.Tools.Navigation.NavigationEventArgs e )
        {
            this.Sail( e.Status );
        }
    }
}
