using System;
using System.Windows.Forms;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PowerFan.Forms.New
{
    /// <summary>
    /// Form per richiedere quanti biglietti stampare
    /// </summary>
    public partial class QuantityForm : WizardForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityForm"/> class.
        /// </summary>
        public QuantityForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityForm"/> class.
        /// </summary>
        /// <param name="start">La quantità di biglietti da stampare</param>
        public QuantityForm( uint start )
        {
            InitializeComponent();
            this.upDownCounter1.Counter = (int)start;
        }

        private void CancelClickHandler( object sender, EventArgs e )
        {
            this.Sail( Tools.Navigation.NavigationAction.Back );
        }

        private void NextClickHandler( object sender, EventArgs e )
        {
            if( this.upDownCounter1.Counter == 0 )
            {
                System.Windows.Forms.MessageBox.Show(
                    PowerFan.Properties.Resources.NoTicketsText,
                    PowerFan.Properties.Resources.NoTicketsTitle,
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning
                );
                return;
            }

            this.Sail( Tools.Navigation.NavigationAction.Next );
        }

        private void FormClosedHandler( object sender, FormClosedEventArgs e )
        {
            this.Sail( Tools.Navigation.NavigationAction.Cancel );
        }

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        /// <returns></returns>
        public override object GetData()
        {
            return (uint)Math.Abs( this.upDownCounter1.Counter );
        }
    }
}
