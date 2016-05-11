using System.Windows.Forms;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Form con le proprietà di base comuni al progetto
    /// </summary>
    /// <remarks>
    /// Viene usata per avere una base di configurazione comune a tutte le form che verranno usate nei progetti, come
    /// un'icona, delle proprietà predefinite, ancoraggi, ... Vedere le proprietà nell'IDE
    /// </remarks>
    public partial class BaseDialogForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDialogForm"/> class.
        /// </summary>
        public BaseDialogForm()
        {
            InitializeComponent();
        }
    }
}
