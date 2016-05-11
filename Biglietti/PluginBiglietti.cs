using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using IndianaPark.Biglietti.Wizard;
using IndianaPark.Plugin;
using IndianaPark.Tools;
using IndianaPark.Tools.Logging;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Security.Licensing;
using IndianaPark.Licensing;

namespace IndianaPark.Biglietti
{
    /// <summary>
    /// Plugin per la gestione dell'emissioni di biglietti per le attrazioni del parco
    /// </summary>
#if( RELEASE || LICENSE )
    [System.ComponentModel.LicenseProvider( typeof( IpLicenseProvider ) )]
    [EnabledFeatures( SoftwareFeatures.Biglietti )]
#endif
    public class PluginBiglietti : IndianaparkPlugin
    {
        #region Fields

        #region Class-wise Fields

        private static PluginBiglietti ms_GlobalInstance;

        #endregion Class-wise Fields

        #region Internal Fields

        private readonly List<IAttrazione> m_listaAttrazioni = new List<IAttrazione>();
        private readonly Pannelli.NewTicketPanel m_mainPanel = new IndianaPark.Biglietti.Pannelli.NewTicketPanel();
#if( RELEASE || LICENSE )
        private System.ComponentModel.License m_pluginLicense;
#endif

        #endregion Internal Fields

        #endregion Fields

        #region Events

        #region Event Handlers

        /// <summary>
        /// Gestisce il completamento del Wizard avviando la stampa del biglietto
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void WizardCompletedHandler( object sender, Tools.Navigation.NavigationEventArgs e )
        {
            this.Host.GraphicHost.GetMainForm().Show();

            if( e.Status == NavigationAction.Finish )
            {
                // Recupero il report
                var builder = (BigliettiBuilder)((Tools.Wizard.Wizard)sender).Builder;
                if( builder.BuildResult() )
                {
                    Logger.Default.Write( "Wizard Biglietti completed succesfully" );

                    var printableTicket = builder.GetResult();
                    var report = printableTicket.GetPrintableReport();

                    // Stampa del report, se presente
                    if( report != null )
                    {
                        PluginBiglietti.Print( report );
                        report.Close();
                    }
                }
                else
                {
                    Logger.Default.Write( "Wizard Biglietti completed with errors: the model is not updated", Verbosity.ErrorDebug | Verbosity.User );
                }
            }

            // Finalizzo il Wizard
            ((Tools.Wizard.Wizard)sender).Dispose();
        }

        #endregion Event Handlers

        #endregion Events

        #region Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Biglietti"/> class.
        /// </summary>
        /// <param name="host">Contenitore del plugin</param>
        /// <exception cref="ArgumentNullException">Il riferimento al contenitore del plugin non può essere <c>null</c></exception>
        public PluginBiglietti( IPluginHost host ) : base( host )
        {
#if( RELEASE || LICENSE )
            this.m_pluginLicense = System.ComponentModel.LicenseManager.Validate( this.GetType(), this );
#endif
            if( PluginBiglietti.ms_GlobalInstance != null )
            {
                throw new ApplicationException( this.GetType() + " instantiated more than once." );
            }
            PluginBiglietti.ms_GlobalInstance = this;

            this.m_mainPanel.EmissioneRequest += this.EmettiBiglietti;

            // Inizializzazione dei parametri di configurazione con i valori di default
            this.Parameters.Add( "SearchDirectory",
                new PluginConfigValue( "SearchDirectory", "".GetType(), Properties.Resources.PluginDirectoryDescription ) );
            this.Parameters.Add( "AutoForward",
                new PluginConfigValue( "AutoForward", true, IndianaPark.Biglietti.Properties.Resources.AutoForwardDescription ) );
            this.Parameters.Add( "DefaultPrinterSettings",
                new PluginConfigValue( "DefaultPrinterSettings", typeof( PrinterSettings ), IndianaPark.Biglietti.Properties.Resources.DefaultPrinterDescription ) );
            this.Parameters.Add( "AlwaysAskPrinter",
                new PluginConfigValue( "AlwaysAskPrinter", true, IndianaPark.Biglietti.Properties.Resources.AlwaysAskPrinter ) );

            this.Parameters["SearchDirectory"].Value = Properties.Resources.DefaultPluginDirectory;

            // Configuro il caricamento della configurazione
            this.RegisterLoadingFunctions( this.LoadConfiguration );
        }

        #endregion Constructors

        #region Class-wise Methods

        /// <summary>
        /// Recupera l'istanza globale del plugin.
        /// </summary>
        /// <remarks>
        /// Questo metodo non istanzia il plugin, che deve essere istanziato dal suo <see cref="IPluginManager"/>
        /// </remarks>
        /// <returns>L'istanza globale del Plugin</returns>
        internal static PluginBiglietti GetGlobalInstance()
        {
            if( PluginBiglietti.ms_GlobalInstance == null )
            {
                throw new ApplicationException( typeof( PluginBiglietti ) + " is not istantiated." );
            }

            return PluginBiglietti.ms_GlobalInstance;
        }

        /// <summary>
        /// Controlla la presenza di una stampante di default. In caso non sia stata impostata la richiede all'utente
        /// </summary>
        /// <returns><c>true</c> se la stampa deve procedere, <c>false</c> altrimenti</returns>
        private static bool SetDefaultPrinter()
        {
            // Controllo se è presente una stampante di default
            if( PluginBiglietti.GetGlobalParameter( "DefaultPrinterSettings" ).Value == null || (bool)PluginBiglietti.GetGlobalParameter( "AlwaysAskPrinter" ).Value )
            {
                // Finestra di dialogo per la scelta dei parametri
                var printDialog = new PrintDialog
                {
                    AllowPrintToFile = false,
                    AllowSomePages = false,
                    PrintToFile = false,
                    ShowHelp = false,
                    ShowNetwork = true
                };

                while( true )
                {
                    var dr = printDialog.ShowDialog();

                    // Se la stampante va scelta una volta per tutte, non posso permettere l'annullamento
                    if( !(bool)PluginBiglietti.GetGlobalParameter( "AlwaysAskPrinter" ).Value && dr != DialogResult.OK )
                    {
                        continue;
                    }

                    // L'utente può voler non stampare
                    if( dr == DialogResult.OK )
                    {
                        PluginBiglietti.GetGlobalParameter( "DefaultPrinterSettings" ).Value =
                            printDialog.PrinterSettings;
                        return true;
                    }

                    // Chiedo conferma prima di uscire
                    var result =
                        MessageBox.Show(
                            IndianaPark.Biglietti.Properties.Resources.CancelPrintingText,
                            IndianaPark.Biglietti.Properties.Resources.CancelPrintingTitle,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning );

                    if( result == DialogResult.Yes )
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Permette di accedere all'istanza globale dei parametri di configurazione del plugin da qualunque punto dell'Assembly
        /// </summary>
        /// <returns>I parametri di configurazione del plugin globale. <c>null</c> se il parametro non esiste</returns>
        internal static IConfigValue GetGlobalParameter( string name )
        {
            if( PluginBiglietti.ms_GlobalInstance.Parameters.ContainsKey( name ) )
            {
                return PluginBiglietti.ms_GlobalInstance.Parameters[name];
            }

            return null;
        }

        /// <summary>
        /// Stampa un Report
        /// </summary>
        /// <param name="report">Il report da stampare con i dati associati</param>
        public static void Print( ReportDocument report )
        {
            // Controllo la configurazione della stampante exception se procedere con la stampa dopo la configurazione
            if( !PluginBiglietti.SetDefaultPrinter() )
            {
                MessageBox.Show( IndianaPark.Biglietti.Properties.Resources.NoPrintButAddText,
                                 IndianaPark.Biglietti.Properties.Resources.NoPrintButAddTitle, MessageBoxButtons.OK,
                                 MessageBoxIcon.Information );
                return;
            }

            var msg = new Forms.EsecuzioneStampa();

            msg.Show();
            Application.DoEvents();

            // Stampa del documento
            try
            {
                // Configurazione della stampante
                var settings = (PrinterSettings)(PluginBiglietti.GetGlobalParameter( "DefaultPrinterSettings" ).Value);
                /*
                // I margini vanno impostati come quelli del report. Non so perché
                var pgs = new PageSettings();
                var pts = new PrinterSettings();
                report.PrintOptions.CopyTo( pts, pgs );
                settings.DefaultPageSettings.Margins = pgs.Margins;

                // Settaggio
                report.PrintOptions.CopyFrom( settings, settings.DefaultPageSettings );
                */
                // Stampa
                report.PrintOptions.PrinterName = settings.PrinterName;
                report.PrintToPrinter( 1, false, 0, 0 );
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Printing error", Verbosity.WarningDebug | Verbosity.User );
            }


            msg.Close();
        }

        #endregion Class-wise Methods

        #region Internal Methods

        /// <summary>
        /// Avvia la procedura per l'emissione di biglietti
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EmettiBiglietti( object sender, EventArgs e )
        {
            var attrazioni = new List<Wizard.TipoAttrazioneState.AttrazioneData>();

            // Creo la struttura dati per la visualizzazione della form
            foreach( var a in this.m_listaAttrazioni )
            {
                attrazioni.Add(
                    new Wizard.TipoAttrazioneState.AttrazioneData
                    {
                        // TODO Valutare di sostituire con un Factory/Builder
                        Nome = a.GetDisplayName(),
                        NextState = a.GetFirstWizard(),
                        Builder = a.GetBuilder()
                    }
                );
            }

            // Via al Wizard
            var wizard = new Tools.Wizard.Wizard( new Wizard.BigliettiBuilder() );

            this.Host.GraphicHost.GetMainForm().Hide();

            wizard.WizardCompleted += this.WizardCompletedHandler;
            wizard.StartWizard( new Wizard.TipoAttrazioneState( wizard, attrazioni )  );
        }

        #endregion Internal Methods

        #region Public Methods

        /// <summary>
        /// Funzione utilizzata per l'inizializzazione del plugin. Viene chiamata una volta quando il programma si
        /// avvia ed il plugin viene caricato.
        /// </summary>
        /// <returns>
        /// True se il caricamento ha avuto esito positivo, False altrimenti
        /// </returns>
        public override bool Load()
        {
            return true;
        }

        /// <summary>
        /// Funzione utilizzata per la chiusura del plugin.
        /// </summary>
        public override void Dispose()
        {
            this.SaveConfiguration();
#if( RELEASE || LICENSE )
            if( this.m_pluginLicense != null )
            {
                this.m_pluginLicense.Dispose();
                this.m_pluginLicense = null;
            }
#endif
            this.OnPluginDisposed();
        }

        /// <summary>
        /// Utilizzata per recuperare un'interfaccia grafica per l'interazione con l'utente. Il controllo utente
        /// recuperato viene, di solito, aggiunto alla finestra principale del programma
        /// </summary>
        /// <returns>
        /// Il pannello da aggiungere all'interfaccia grafica. Null in caso non si debba aggiungere niente
        /// </returns>
        public override System.Windows.Forms.Control GetCommandPanel()
        {
            return this.m_mainPanel;
        }

        /// <summary>
        /// Utilizzata per recuperare un elemento di un menù a tendina da aggiungere al menu principale della GUI.
        /// </summary>
        /// <returns>
        /// Il menu da aggiungere. Null in caso non si debba aggiungere niente
        /// </returns>
        public override System.Windows.Forms.ToolStripItem GetDropdownMenu()
        {
            var bigliettiToolStripMenuItem = new ToolStripMenuItem();

            bigliettiToolStripMenuItem.Name = "bigliettiMenuItem";
            bigliettiToolStripMenuItem.Text = "Configura stampante...";
            bigliettiToolStripMenuItem.Click += delegate { PluginBiglietti.SetDefaultPrinter(); };

            //return bigliettiToolStripMenuItem;
            return null;
        }

        /// <summary>
        /// Avvia le routine di gestione del plugin. Il suo scopo è quello di differire l'esecuzione delle routine del
        /// plugin dalle procedure di caricamento; un esempio d'uso è l'impostazione di configurazioni prima dell'
        /// avvio del plugin
        /// </summary>
        /// <param name="owner"></param>
        /// <remarks>
        /// Il suo scopo è quello di differire l'esecuzione delle routine del plugin dalle procedure di caricamento;
        /// un esempio d'uso è l'impostazione di configurazioni prima dell'avvio del plugin
        /// </remarks>
        public override void Run( IPlugin owner )
        {
            // Inizializzo i miei sottoplugin
            this.Host.Manager.RunFromPath( this.Parameters["SearchDirectory"].Value.ToString(), this, true );

            // Aggiungo la mia interfaccia
            this.Host.GraphicHost.AttachInterface( this );

            // Cerco quali dei plugin caricati hanno attrazioni per cui emettere biglietti
            foreach( var plugin in this.Host.Manager.GetOwned( this ) )
            {
                if( plugin.ImplementsInterface( typeof( IAttrazione ) ) )
                {
                    this.m_listaAttrazioni.Add( (IAttrazione)plugin );
                }
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}
