using System;
using System.Windows.Forms;
using IndianaPark.Tools.Navigation;

namespace IndianaPark.Tools.Wizard
{
    /// <summary>
    /// Classe astratta che rappresenta una form abilitata alla navigazione all'interno di un Wizard
    /// </summary>
    public class WizardForm : Form, INavigator, IUserData
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Lo stato dell'oggetto navigatore
        /// </summary>
        public NavigationAction NavigationStatus { get; private set; }

        /// <summary>
        /// Indica se i dati sono validi
        /// </summary>
        /// <remarks>
        /// In questo tipo di form i dati sono sempre validi e possono sempre essere recuperati.
        /// </remarks>
        /// <value><c>true</c> se i dati sono validi, <c>false</c> altrimenti</value>
        public virtual bool IsDataValid
        {
            get
            {
                return true;
            }
            protected set
            {
                return;
            }
        }

        #endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Generato quando viene richiesta un'azione di navigazione
        /// </summary>
        public event NavigationEvent Navigate;

        /// <summary>
        /// Evento che indica la richiesta da parte della form di aggiornare i suoi dati
        /// </summary>
        public event EventHandler UpdateInformations;

		#region Raisers 

        /// <summary>
        /// Avvia l'evento <see cref="WizardForm.UpdateInformations"/>
        /// </summary>
        protected void OnUpdateDataRequest()
        {
            if( this.UpdateInformations != null )
            {
                this.UpdateInformations( this, new EventArgs() );
            }
        }

		#endregion Raisers 

		#region Event Handlers 

        /// <summary>
        /// Gestisce la chiusura della form. La chiusura della form corrisponde alla richiesta di annullamento navigazione,
        /// con richiesta <see cref="NavigationAction.Cancel"/>
        /// </summary>
        /// <param name="sender">La sorgente dell'evento.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void FormClosingHandler( object sender, FormClosingEventArgs e )
        {
            this.Sail( NavigationAction.Cancel );

            /*
            // Questo evento viene chiamato anche in caso di chiusura software della form, ma in quel caso va chiusa direttamente!!
            if( this.NavigationStatus == NavigationAction.Nothing )
            {
                this.Sail( NavigationAction.Cancel );
            }
            else
            {
                this.Hide();
            }
            */
        }

        /// <summary>
        /// Questo metodo gestisce le azioni globali fatte da tasti di scelta rapida, come l'aver
        /// premuto ESC oppure INVIO
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        private void KeyPressHandler( object sender, KeyPressEventArgs e )
        {
            // Gestione dei tasti di scelta rapida della form
            switch( e.KeyChar )
            {
                case (char)27:
                    // Con ESC l'azione di default è "Esci"
                    this.Sail( NavigationAction.Cancel );
                    break;

                case '\r':
                    // Con INVIO l'azione di default è "Avanti"
                    this.Sail( NavigationAction.Next );
                    break;

                default:
                    break;
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardForm"/> class.
        /// </summary>
        public WizardForm()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Converte di dati di una sorgente di dati in un qualsiasi formato desiderato verificando che la
        /// conversione sia possibile
        /// </summary>
        /// <typeparam name="TType">Il tipo di dato in uscita</typeparam>
        /// <param name="source">Il contenitore dei dati</param>
        /// <returns>
        /// Il dato che l'<see cref="IUserData"/> contiene convertito nel tipo corrett.
        /// </returns>
        public static TType ConvertUserData<TType>( IUserData source )
        {
            object data = source.GetData();
            Type dataType = typeof(TType);

            if( data == null )
            {
                return default( TType );
            }

            if( !(data is TType) && !data.GetType().IsSubclassOf( dataType ) )
            {
                throw new Exception();
            }

            return (TType)data;
        }

		#endregion Class-wise Methods 

		#region Internal Methods 

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( WizardForm ) );
            this.SuspendLayout();
            // 
            // WizardForm
            // 
            resources.ApplyResources( this, "$this" );
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "WizardForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.KeyPressHandler );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormClosingHandler );
            this.ResumeLayout( false );

        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        public virtual object GetData()
        {
            return null;
        }

        /// <summary>
        /// Fornisce le informazioni necessare per aggiornare il suo stato
        /// </summary>
        /// <remarks>
        /// La funzione è virtuale e deve essere implementata dove necessario
        /// </remarks>
        /// <param name="info">Le informazioni</param>
        public virtual void SetInformations( object info )
        {
        }

        /// <summary>
        /// Utilizzato per richiedere al navigatore un'azione di navigazione. Per informazioni su cosa e quali sono le azioni
        /// di navigazione vedere <see cref="NavigationAction"/>
        /// </summary>
        /// <remarks>Ad ogni chiamata di questo metodo deve corrispondere lo scatenarsi dell'evento <see cref="INavigator.Navigate"/></remarks>
        /// <param name="action">Tipo di azione richiesta</param>
        public void Sail( NavigationAction action )
        {
            // Cambio lo stato interno
            this.NavigationStatus = action;

            var nea = new NavigationEventArgs( action );
            if( this.Navigate == null )
            {
                return;
            }

            this.Navigate( this, nea );

            if( !nea.Cancel )
            {
                // Chiudo la form corrente
                this.Hide();
            }
            else
            {
                // Riporto sullo stato di Nothing
                this.NavigationStatus = NavigationAction.Nothing;
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
