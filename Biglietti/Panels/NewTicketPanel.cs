using System;
using System.Windows.Forms;

namespace IndianaPark.Biglietti.Pannelli
{
    /// <summary>
    /// Interfaccia principale del plugin
    /// </summary>
    public partial class NewTicketPanel : UserControl
    {
		#region Events 

        /// <summary>
        /// Richiesta di avvio del wizard per l'emissione biglietti
        /// </summary>
        public event EventHandler EmissioneRequest;

		#region Event Handlers 

        private void EmissioneBigliettiClickHandler( object sender, EventArgs e )
        {
            if( this.EmissioneRequest != null )
            {
                this.EmissioneRequest( sender, e );
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="NewTicketPanel"/> class.
        /// </summary>
        public NewTicketPanel()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#endregion Methods 
    }
}
