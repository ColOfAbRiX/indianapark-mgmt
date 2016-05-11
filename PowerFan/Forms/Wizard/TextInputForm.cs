using System.Windows.Forms;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard.New;

namespace IndianaPark.PowerFan.Forms.New
{
    /// <summary>
    /// Finestra di dialogo per l'inserimento di stringhe generiche
    /// </summary>
    public partial class TextInputForm : WizardForm
    {
        /*
		#region Nested Classes 

        /// <summary>
        /// Utilizzata per esportare il risultato dell'input dell'utente
        /// </summary>
        public class TextInputData
        {
    		#region Fields 

            /// <summary>
            /// Il testo inserito dall'utente
            /// </summary>
            public string Text { get; set; }

    		#endregion Fields 
        }

		#endregion Nested Classes 
        */
		#region Fields 

        /// <summary>
        /// Il risultato dell'input dell'utente
        /// </summary>
        protected string m_result = "";

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Gestisce la richiesta di <see cref="NavigationAction.Next"/> del controllo di navigazione della form
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void NavigateNextHandler( object source, NavigationEventArgs e )
        {
            if( e.Status != NavigationAction.Next )
            {
                return;
            }

            // Controllo la validità dei dati prima di proseguire
            if( !this.CheckData(this.m_text.Text) )
            {
                this.m_text.Focus();

                // Annullo l'operazione
                e.Cancel = true;
                return;
            }

            this.m_result = this.m_text.Text;
            this.RequireNavigationAction( NavigationAction.Next );
        }

        /// <summary>
        /// Gestisce la richiesta di <see cref="NavigationAction.Back"/> del controllo di navigazione della form
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void NavigateBackCancelHandler( object source, NavigationEventArgs e )
        {
            if( e.Status == NavigationAction.Back )
            {
                this.RequireNavigationAction( NavigationAction.Back );
            }
            if( e.Status == NavigationAction.Cancel )
            {
                this.RequireNavigationAction( NavigationAction.Cancel );
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputForm"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        public TextInputForm( string title, string text ) : this( title, text, "" )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputForm"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="value">The value.</param>
        public TextInputForm( string title, string text, string value )
        {
            InitializeComponent();

            base.Text = title;
            this.m_lableTitle.Text = text;
            this.m_lableTitle.Location = new System.Drawing.Point( (this.Width - m_lableTitle.Width) / 2, m_lableTitle.Location.Y - m_lableTitle.Height / 2 + 20 );
            this.m_text.Text = value;

            this.m_navigator.Navigate += this.NavigateBackCancelHandler;
            this.m_navigator.Navigate += this.NavigateNextHandler;
        }

		#endregion Constructors 

        #region Internal Methods

        /// <summary>
        /// Controlla il formato dei dati dell'input
        /// </summary>
        /// <param name="value">Il valore da controllare</param>
        /// <returns><c>true</c> se il dato è nel formato corretto e la procedura può proseguire, <c>false</c> altrimenti.</returns>
        protected virtual bool CheckData( string value )
        {
            // Controllo se è stato inserito del testo
            if( value == "" )
            {
                MessageBox.Show(
                    "E' necessario inserire un valore per proseguire!",
                    "Attenzione!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning
                );

                return false;
            }

            return true;
        }

        #endregion Internal Methods

        #region Public Methods

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        /// <returns>Un valore stringa contenente l'input immesso dall'utente</returns>
        public override object GetData()
        {
            return this.m_result;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
