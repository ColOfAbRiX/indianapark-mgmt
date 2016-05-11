using System;
using System.Windows.Forms;
using IndianaPark.Plugin;
using IndianaPark.Tools.Logging;
using System.IO;

namespace IndianaPark.Forms
{
    /// <summary>
    /// Classe rappresentante la finestra principale del programma
    /// </summary>
    public partial class MainWindow : Form, IPluginGraphicHost
    {
		#region Fields 

        private readonly PluginManager m_pluginManager;

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Costruttore.
        /// </summary>
        public MainWindow()
        {
            this.m_pluginManager = new PluginManager() { GraphicHost = this, ScanRecursively = SearchOption.AllDirectories };

            // Inizializzazione della finestra
            this.Enabled = false;
            SplashScreen.ShowSplash();
            SplashScreen.SetLastStep( 4 );
            SplashScreen.IncrementStep( Properties.Resources.IncrementStepValue );

            InitializeComponent();

            try
            {
                // Caricamento dei plugin
                SplashScreen.IncrementStep( Properties.Resources.IncrementStepValue1 );
                this.m_pluginManager.LoadAll();

                // Esecuzione dei plugin
                SplashScreen.IncrementStep( Properties.Resources.IncrementStepValue2 );
                this.m_pluginManager.RunAll( null );
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Generic exception occured while loading plugins", Verbosity.WarningDebug | Verbosity.User );
            }

            SplashScreen.IncrementStep( Properties.Resources.IncrementStepValue3 );
            SplashScreen.CloseSplash();
            this.Enabled = true;
            this.Focus();
        }

		#endregion Constructors 

		#region Internal Methods 

        private void MainWindow_FormClosed( object sender, FormClosedEventArgs e )
        {
            this.m_pluginManager.Dispose();
        }

        private void informazioniToolStripMenuItem_Click( object sender, EventArgs e )
        {
            using( About aboutForm = new About() )
            {
                aboutForm.ShowDialog( this );
            }
        }

        private void esciToolStripMenuItem_Click( object sender, EventArgs e )
        {
            this.Dispose();
            this.Close();
        }

        private void pluginCaricatiToolStripMenuItem_Click( object sender, EventArgs e )
        {
            using( Plugins p = new Plugins( this.m_pluginManager ) )
            {
                p.ShowDialog( this );
            }
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Permette di ricavare la finestra principale
        /// </summary>
        public Form GetMainForm()
        {
            return this;
        }

        /// <summary>
        /// Utilizzato per aggiungere il pannello comandi del plugin specificato all'host del plugin
        /// </summary>
        /// <param name="plugin">Il plugin da aggiungere nella grafica</param>
        public void AttachInterface( IPluginViewable plugin )
        {
            // Aggiungo il pannello comandi nel riquadro principale
            Control uc = plugin.GetCommandPanel();
            if( uc != null )
            {
                this.flpPlugins.Controls.Add( uc );
            }

            ToolStripItem tsi = plugin.GetDropdownMenu();
            if( tsi != null )
            {
                //this.sezioniToolStripMenuItem.DropDownItems.Insert( 0, tsi );
                this.mnuMainWindow.Items.Insert( 2, tsi );
            }

            this.Refresh();
        }

        /// <summary>
        /// Utilizzato per rimuovere il pannello comandi del plugin specificato all'host del plugin
        /// </summary>
        /// <param name="plugin">Il plugin da aggiungere nella grafica</param>
        public void DetachInterface( IPluginViewable plugin )
        {
            Control uc = plugin.GetCommandPanel();
            if( uc != null )
            {
                this.flpPlugins.Controls.Remove( uc );
                this.Refresh();
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
