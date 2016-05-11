using System.Windows.Forms;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;
using System;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Finestra di dialogo per l'inserimento generico di dati tramite un input box
    /// </summary>
    /// <remarks>
    /// Ogni implementazione di questa classe astratta deve implementare i metodi per controllare la validità dei
    /// dati e per gestire cosa fare quando l'utente chiede di proseguire con i dati errati.
    /// Implementa il pattern Template Method
    /// </remarks>
    public abstract partial class InputForm : WizardForm
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Il risultato dell'input dell'utente
        /// </summary>
        protected string m_result = "";

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Indica se i dati inseriti sono validi
        /// </summary>
        /// <value>Se <c>true</c> indica che i dati sono validi</value>
        /// <remarks>
        /// La validità dei dati è stabilita tramite il metodo <see cref="CheckData"/> e viene aggiornata ogni volta
        /// che il testo inserito cambia.
        /// </remarks>
        public bool IsDataValid { get; protected set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Occurs when the <see cref="P:System.Windows.Forms.Control.Text"/> property value changes.
        /// </summary>
        /// <remarks>
        /// L'evento viene sollevato dopo che è stata impostata la proprietà <see cref="IsDataValid"/>
        /// </remarks>
        public event EventHandler DataChanged;

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
                this.WrongData();
                this.m_text.Focus();

                // Annullo l'operazione
                e.Cancel = true;
                return;
            }

            this.m_result = this.m_text.Text;
            this.Sail( NavigationAction.Next );
        }

        /// <summary>
        /// Gestisce le richieste di <see cref="NavigationAction.Back"/> e <see cref="NavigationAction.Cancel"/>
        /// del controllo di navigazione della form
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void NavigateBackCancelHandler( object source, NavigationEventArgs e )
        {
            if( e.Status == NavigationAction.Back || e.Status == NavigationAction.Cancel )
            {
                this.Sail( e.Status );
            }
        }

        /// <summary>
        /// Controlla la validità dell'input dell'utente ad ogni inserimento
        /// </summary>
        /// <remarks>
        /// Ogni volta che l'utente cambia il testo inserito questo metodo ne controlla la validità e aggiorna la
        /// proprietà interna <see cref="IsDataValid"/>
        /// </remarks>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TextChangedHandler( object sender, EventArgs e )
        {
            this.IsDataValid = this.CheckData( ((TextBox)sender).Text );
            if( this.DataChanged != null )
            {
                this.DataChanged( sender, e );
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputForm"/> class.
        /// </summary>
        /// <param name="title">Titolo della finestra</param>
        /// <param name="text">Testo descrittivo per l'utente</param>
        public InputForm( string title, string text ) : this( title, text, "" )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputForm"/> class.
        /// </summary>
        /// <param name="title">Titolo della finestra</param>
        /// <param name="text">Testo descrittivo per l'utente</param>
        /// <param name="value">Valore iniziale del TextBox</param>
        public InputForm( string title, string text, string value )
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
        /// Metodo richiamato quando si richiede un'azione di navigazione e i dati non sono validi
        /// </summary>
        protected abstract void WrongData();

        /// <summary>
        /// Controlla il formato dei dati dell'input
        /// </summary>
        /// <remarks>
        /// Ogni classe che specializza questa form deve sovrascrivere questo metodo per controllare la correttezza dei
        /// dati inseriti dall'utente
        /// </remarks>
        /// <param name="value">Il valore da controllare. E' sempre una stringa perchè deriva da un TextBox</param>
        /// <returns><c>true</c> se il dato è nel formato corretto e la procedura può proseguire, <c>false</c> altrimenti.</returns>
        protected abstract bool CheckData( string value );

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Recupera il testo inserito dall'utente
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
