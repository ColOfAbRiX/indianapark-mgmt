using System;
using System.Windows.Forms;

namespace IndianaPark.Tools.Navigation
{
    /// <summary>
    /// Generica form abilitata alla navigazione.
    /// </summary>
    /// <remarks>
    /// <para>Questa form viene impostata con i parametri più comuni da usare per le form di navigazione, a sua volta è
    /// un <see cref="INavigator"/> exception può essere gestito come tale da altri controlli.</para>
    /// <para>La classe è una astratta e non può essere usata per progettare le form attraverso VisualStudio</para>
    /// </remarks>
    public partial class NavigationForm : Form, INavigator
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Tempo di fade-in della form in secondi
        /// </summary>
        private const double mc_dblShowTime = 0.25;
        /// <summary>
        /// Stato dell'oggetto di navigazione
        /// </summary>
        private NavigationAction m_nsStatus = NavigationAction.Nothing;
        /// <summary>
        /// Indica se effettuare una transizione in apertura oppure no
        /// </summary>
        /// <value><c>true</c> significa di effettuare la transizione</value>
        private const bool mc_bShowTransition = true;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Indica se l'utente ha effettuato un'operazione manualmente
        /// </summary>
        /// <value>
        /// <c>true</c> se l'azione è cominciata dall'utente; <c>false</c> altrimenti.
        /// </value>
        /// <remarks>
        /// Ogni volta che l'utente interagisce generando un'azione di navigazione è necessario impostare
        /// questo campo a true.
        /// </remarks>
        public bool IsUserAction { get; protected set; }

        /// <summary>
        /// Lo stato dell'oggetto navigatore
        /// </summary>
        /// <remarks>
        /// La <see cref="NavigationForm"/> è una implementazione di <see cref="INavigator"/> exception quindi può avanzare
        /// richieste di navigazione. Il campo mostra in quale stato di navigazione si trova la form.
        /// </remarks>
        public NavigationAction NavigationStatus
        {
            get { return this.m_nsStatus; }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Generato quando viene richiesta un'azione di navigazione
        /// </summary>
        /// <remarks>
        /// Richiama i metodi protetti <see cref="NavigateNext"/>, <see cref="NavigateBack"/>, <see cref="NavigateCancel"/>, 
        /// <see cref="NavigateFinish"/> perchè in alcune parti di codice sono ancora usati.
        /// </remarks>
        public event NavigationEvent Navigate;
        /// <summary>
        /// Generato quando viene richiesto di navigare in avanti
        /// </summary>
        protected event NavigationEvent NavigateNext;
        /// <summary>
        /// Generato quando viene richiesto di navigare indietro
        /// </summary>
        protected event NavigationEvent NavigateBack;
        /// <summary>
        /// Generato quando viene richiesto di annullare la navigazione
        /// </summary>
        protected event NavigationEvent NavigateCancel;
        /// <summary>
        /// Generato quando viene richiesto di terminare la navigazione
        /// </summary>
        protected event NavigationEvent NavigateFinish;

		#region Raisers 

        /// <summary>
        /// Genera uno qualsiasi degli eventi di navigazione interni
        /// </summary>
        /// <remarks>Rimpiazzata dall'uso di <see cref="Navigate"/> ma deve rimanere perchè diverso codice fa
        /// ancora uso dei vecchi eventi.</remarks>
        /// <param name="action">Il tipo di evento da eseguire</param>
        /// <returns>Indica se annullare l'azione (<c>true</c> annulla)</returns>
        [Obsolete]
        protected bool OnNavigate( NavigationAction action )
        {
            // Creo l'oggetto per gestire i dati
            var nea = new NavigationEventArgs( action );
            NavigationEvent reqEvent = null;

            // Trovo l'evento da lanciare
            switch( action )
            {
                case NavigationAction.Cancel:
                    reqEvent = this.NavigateCancel;
                    break;

                case NavigationAction.Back:
                    reqEvent = this.NavigateBack;
                    break;

                case NavigationAction.Next:
                    reqEvent = this.NavigateNext;
                    break;

                case NavigationAction.Finish:
                    reqEvent = this.NavigateFinish;
                    break;

                case NavigationAction.Nothing:
                default:
                    break;
            }

            // Se ci sono handlers li eseguo
            if( reqEvent != null )
            {
                reqEvent( this, nea );
                return nea.Cancel;
            }

            // False significa "procedi normalmente"
            return false;
        }

		#endregion Raisers 

		#region Event Handlers 

        /// <summary>
        /// Gestisce la chiusura della form. La chiusura della form corrisponde alla richiesta di annullamento navigazione,
        /// con richiesta <see cref="NavigationAction.Cancel"/>
        /// </summary>
        /// <param name="sender">La sorgente dell'evento.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        private void FormClosingHandler(object sender, FormClosingEventArgs e)
        {
            // Questo evento viene chiamato anche in caso di chiusura software della form, ma in quel caso va chiusa direttamente!!
            if( this.NavigationStatus == NavigationAction.Nothing )
            {
                this.Sail( NavigationAction.Cancel );
            }
            else
            {
                this.Hide();
            }
        }

        /// <summary>
        /// Esegue le azioni di default quando l'istanza successiva alla corrente richiede di tornare al passo precedente
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected virtual void BackDefaultHandler(object sender, NavigationEventArgs e)
        {
            // Quando la form successiva si chiude, visualizzo quella corrente
            this.Show();
        }

        /// <summary>
        /// Esegue le azioni di default quando l'istanza corrente richiede di passare al passo successivo
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected virtual void NextDefaultHandler( object sender, NavigationEventArgs e )
        {
            // Quando richiedo di passare alla form successiva, nascondo quella corrente
            this.Hide();
        }

        /// <summary>
        /// Esegue le azioni di default quando la form corrente richiede di terminare correttamente l'operazione
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected virtual void FinishDefaultHandler( object sender, NavigationEventArgs e )
        {
        }

        /// <summary>
        /// Gestisce l'evento di chiusura dell'istanza NavigationForm richiamata dopo l'istanza corrente. Viene
        /// utilizzata per far si che il passo i-esimo chieda al (i - 1)-esimo a sua volta di chiudersi.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected virtual void NextFinishDefaultHandler( object sender, NavigationEventArgs e )
        {
            this.Sail( NavigationAction.Finish );
        }

        /// <summary>
        /// Esegue le azioni di default quando la form corrente richiede di annullare l'operazione
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected virtual void CancelDefaultHandler( object sender, NavigationEventArgs e )
        {
            if( this.IsUserAction )
            {
                e.Cancel = MessageBox.Show( "Chiudendo la pagina perderai quanto fatto! Vuoi uscire?", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Warning ) == DialogResult.Yes ? false : true;
            }
        }

        /// <summary>
        /// Gestisce l'evento di chiusura dell'istanza NavigationForm richiamata dopo l'istanza corrente. Viene
        /// utilizzata per far si che il passo i-esimo chieda al (i - 1)-esimo a sua volta di chiudersi.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected virtual void NextCancelDefaultHandler( object sender, NavigationEventArgs e )
        {
            this.Sail( NavigationAction.Cancel );
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

        /// <summary>
        /// Utilizzata per creare effetti di transizione visibile/nascosta sulla form
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void VisibleChangedHandler( object sender, EventArgs e )
        {
            // Partenza del timer per l'opacità
            if( mc_bShowTransition )
            {
                this.Opacity = this.Visible ? 0 : 1;
                this.tmrOpacity.Enabled = true;
            }
        }

        /// <summary>
        /// Funzione operativa per la transizione visibile/nascosta
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OpacityTickHandler( object sender, EventArgs e )
        {
            // Il Try-Catch è usato perchè a volte il timer crea problemi dopo l'eliminazione della form
            try
            {
                // Modifica dell'opacità
                double step = (this.tmrOpacity.Interval / 1000.0) / mc_dblShowTime;
                step *= this.Enabled ? 1 : -1;

                // Controllo dei limiti
                this.Opacity += Math.Min( Math.Max( step, 0 ), 1 );
                if( this.Opacity == 0 || this.Opacity == 1 )
                {
                    this.tmrOpacity.Enabled = false;
                }
            }
            catch
            {
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Costruttore standard
        /// </summary>
        public NavigationForm()
        {
            InitializeComponent();
            // Utilizzato per tradurre internamente l'evento RequireNaviagationAction in eventi NavigateXXX
            this.Navigate += this.SmistaNavigation;
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Wrapper utilizzato per richiamare automaticamente <see cref="NavigationForm.OnNavigate"/> all'esecuzione dell'evento <see cref="NavigationForm.Sail"/>.
        /// Viene utilizzata per tradurre internamente l'evento <see cref="NavigationForm.Navigate"/> negli eventi NavigateXXX
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void SmistaNavigation( object sender, NavigationEventArgs e )
        {
            this.OnNavigate( e.Status );
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Utilizzato per richiedere al navigatore un'azione di navigazione. Per informazioni su cosa e quali sono le azioni
        /// di navigazione vedere <see cref="NavigationAction"/>
        /// </summary>
        /// <remarks>Ad ogni chiamata di questo metodo deve corrispondere lo scatenarsi dell'evento <see cref="INavigator.Navigate"/></remarks>
        /// <param name="action">Tipo di azione richiesta</param>
        public void Sail( NavigationAction action )
        {
            // Cambio lo stato interno
            this.m_nsStatus = action;

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
                this.m_nsStatus = NavigationAction.Nothing;
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}