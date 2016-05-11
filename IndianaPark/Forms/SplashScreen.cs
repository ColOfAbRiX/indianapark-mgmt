using System;
using System.Threading;
using System.Windows.Forms;

namespace IndianaPark.Forms
{
    /// <summary>
    /// Splash Screen. Schermata di informazioni sul caricamento del programma
    /// </summary>
    public partial class SplashScreen : Form
    {
        #region Internal Fields

        private static readonly object ms_Locker = new object();
        private static SplashScreen ms_Splash;
        private static Thread ms_Thread;

        #endregion

        #region Constructors

        /// <summary>
        /// Costruttore
        /// </summary>
        private SplashScreen()
        {
            InitializeComponent();
            labelMessage.Text = "";
            progressBar.Value = 0;
            progressBar.Maximum = 1;
        }

        #endregion

        #region Accesso statico

        /// <summary>
        /// Visualizza lo splashscreen avviandolo in un thread separato
        /// </summary>
        public static void ShowSplash()
        {
            // Se è gia stato caricato esco
            if( ms_Thread != null )
            {
                return;
            }

            lock( ms_Locker )
            {
                ms_Thread = new Thread( FormProcedure ) { Name = "SplashScreen", IsBackground = true };
                ms_Thread.SetApartmentState( ApartmentState.STA );
                ms_Thread.Start();
            }

            // Mi devo assicurare di avere istanziato exception salvato l'oggetto prima di uscire
            while( ms_Splash == null )
            {
                Thread.Sleep( 10 );
            }
        }

        /// <summary>
        /// Chiude lo splashscreen
        /// </summary>
        public static void CloseSplash()
        {
            // Se il thread non esiste non lo posso chiudere
            if( ms_Thread == null )
            {
                return;
            }

            // Chiudo la form
            Thread.Sleep( 1000 );
            ms_Splash.CloseForm();

            // Elimino i riferimenti
            lock( ms_Locker )
            {
                ms_Thread = null;
                ms_Splash = null;
            }
        }

        /// <summary>
        /// Avanza la barra di uno step ed imposta un messaggio
        /// </summary>
        /// <param name="message">Messaggio da visualizzare</param>
        public static void IncrementStep(string message)
        {
            if( ms_Thread == null )
            {
                return;
            }

            ms_Splash.UpdateForm( message, ms_Splash.progressBar.Maximum, ms_Splash.progressBar.Value + 1 );
        }

        /// <summary>
        /// Imposta il numero totale di step da eseguire
        /// </summary>
        /// <param name="value">Numero di step totali</param>
        public static void SetLastStep( uint value )
        {
            if( ms_Thread == null )
            {
                return;
            }

            ms_Splash.UpdateForm( ms_Splash.labelMessage.Text, (int)value, ms_Splash.progressBar.Value );
        }
        
        /// <summary>
        /// Procedura su thread separato che inizializza la finestra
        /// </summary>
        private static void FormProcedure()
        {
            lock( ms_Locker )
            {
                SplashScreen.ms_Splash = new SplashScreen();
            }
            Application.Run( SplashScreen.ms_Splash );
        }

        #endregion

        #region Metodi Interni

        /// <summary>
        /// Delegato per aggiornare i valori della form.
        /// </summary>
        /// <param name="message">Messaggio da visualizzare</param>
        /// <param name="max">Valore massimo della barra</param>
        /// <param name="current">Valore corrente della barra</param>
        private delegate void UpdateFormDelegate( string message, int max, int current );

        /// <summary>
        /// Aggiorna i valori della form. La procedura è thread-safe
        /// </summary>
        /// <param name="message">Messaggio da visualizzare</param>
        /// <param name="max">Valore massimo della barra</param>
        /// <param name="current">Valore corrente della barra</param>
        private void UpdateForm( string message, int max, int current )
        {
            try
            {
                // Controllo se mi trovo nel thread principale della finestra
                if( this.InvokeRequired )
                {
                    // Invoco l'aggiornamento sul thread principale
                    var d = new UpdateFormDelegate( UpdateForm );
                    this.Invoke( d, new object[] { message, max, current } );
                }
                else
                {
                    // Aggiorno i dati
                    this.SuspendLayout();

                    this.labelMessage.Text = message;

                    current = Math.Min( max, current );
                    this.progressBar.Step = 1;
                    this.progressBar.Maximum = max;
                    this.progressBar.Value = current;
                    this.progressBar.PerformLayout();

                    this.ResumeLayout();
                }
            }
            catch( InvalidOperationException ioex )
            {
            }
        }

        /// <summary>
        /// Delegato per chiudere la form. La procedura è thread-safe
        /// </summary>
        private delegate void CloseFormDelegate();

        /// <summary>
        /// Metodo d'istanza per chiudere la form thread-safe
        /// </summary>
        private void CloseForm()
        {
            if( this.InvokeRequired )
            {
                this.Invoke( new CloseFormDelegate( CloseForm ) );
            }
            else
            {
                this.Close();
            }
        }

        #endregion

    }
}
