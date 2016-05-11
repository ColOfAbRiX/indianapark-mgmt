using System;
using System.Linq;
using System.Windows.Forms;
using IndianaPark.Licensing;
using IndianaPark.PercorsiAvventura.Persistence;
using IndianaPark.Tools.Security.Licensing;
using IndianaPark.Tools.Wizard;
using IndianaPark.Plugin;

namespace IndianaPark.PercorsiAvventura
{
    /// <summary>
    /// Plugin che gestisce le operazioni relative ai Percorsi Avventura
    /// </summary>
    [PluginOwners( "IndianaPark.Biglietti.PluginBiglietti" )]
#if( RELEASE || LICENSE )
    [System.ComponentModel.LicenseProvider( typeof( IpLicenseProvider ) )]
    [EnabledFeatures( SoftwareFeatures.Percorsi )]
#endif
    public sealed class PluginPercorsi : IndianaparkPlugin, Biglietti.IAttrazione
    {
		#region Fields 

        #region Class-wise Fields

        private static PluginPercorsi ms_GlobalInstance;

		#endregion Class-wise Fields 

		#region Internal Fields 

        private Persistence.IModelDataAccess m_modelPersistence;
        private readonly Model.Parco m_parco;
#if( RELEASE || LICENSE )
        private System.ComponentModel.License m_pluginLicense;
#endif

        #endregion Internal Fields

        #region Public Fields

        /// <summary>
        /// Permette di accedere alla persistenza del modello, per caricare/salvare il modello
        /// </summary>
        /// <value>The model persistence.</value>
        public Persistence.IModelDataAccess ModelPersistence
        {
            get { return this.m_modelPersistence; }
            private set { this.m_modelPersistence = value; }
        }

        #endregion Public Fields

        #endregion Fields

        #region Events 

        #region Event Handlers

        /// <summary>
        /// Gestisce la richiesta di avviare la procedura di uscita per un cliente
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ExitCodeReadyHandler( object sender, EventArgs e )
        {
            var escapePanel = (Pannelli.ClienteEscape)sender;
            UscitaCliente( escapePanel.CodiceNominativo, escapePanel.CodiceCliente );
            escapePanel.ClearCodice();
        }

        /// <summary>
        /// Gestise la richiesta di visualizzazione della lista clienti
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ViewListRequestHandler( object sender, EventArgs e )
        {
            var a = new Forms.ClientiView();
            a.ShowDialog( this.Host.GraphicHost.GetMainForm() );
        }

        #endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginPercorsi"/> class.
        /// </summary>
        /// <remarks>
        /// Può essere creata al più un'istanza di questo plugin.
        /// </remarks>
        /// <param name="host">Contenitore del plugin</param>
        /// <exception cref="ArgumentNullException">Il riferimento al contenitore del plugin non può essere <c>null</c></exception>
        public PluginPercorsi( IPluginHost host ) : base( host )
        {
#if( RELEASE || LICENSE )
            this.m_pluginLicense = System.ComponentModel.LicenseManager.Validate( this.GetType(), this );
#endif

            // Controllo del Singleton
            if( PluginPercorsi.ms_GlobalInstance != null )
            {
                throw new ApplicationException( this.GetType() + " instantiated more than once." );
            }
            PluginPercorsi.ms_GlobalInstance = this;

            // Inizializzazione parametri di default
            InitializeParameters();

            // Inizializzazione della persistenza
            InitializePersistence();

            // Va inizializzato sempre dopo i parametri
            this.m_parco = Model.Parco.GetParco();
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Permette di accedere all'istanza globale dei parametri di configurazione del plugin da qualunque punto dell'Assembly
        /// </summary>
        /// <remarks>
        /// L'istanza globale corrisponde all'ultimo <see cref="PluginPercorsi"/> istanziato.
        /// </remarks>
        /// <returns>I parametri di configurazione del plugin globale.</returns>
        internal static IConfigValue GetGlobalParameter( string name )
        {
            if( String.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name", "The parameter name cannot be null" );
            }

            return PluginPercorsi.ms_GlobalInstance.Parameters[name];
        }

        /// <summary>
        /// Recupera l'istanza globale del plugin.
        /// </summary>
        /// <remarks>
        /// Questo metodo non istanzia il plugin, che deve essere istanziato dal suo <see cref="IPluginManager"/>
        /// </remarks>
        /// <returns>L'istanza globale del Plugin</returns>
        internal static PluginPercorsi GetGlobalInstance()
        {
            if( PluginPercorsi.ms_GlobalInstance == null )
            {
                throw new ApplicationException( typeof( PluginPercorsi ) + " is not istantiated." );
            }

            return PluginPercorsi.ms_GlobalInstance;
        }

        #endregion Class-wise Methods 

		#region Internal Methods 

        private void UscitaCliente( int codiceNominativo, int codiceCliente )
        {
            var nominativo = Model.Parco.GetParco().TrovaNominativo( codiceNominativo );

            // Controllo se al codice corrisponde un nominativo
            if( nominativo == null )
            {
                System.Windows.Forms.MessageBox.Show(
                    String.Format( Properties.Resources.NominativoNotFoundText, codiceNominativo ),
                    Properties.Resources.NominativoNotFoundTitle,
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning
                );
                return;
            }

            var cliente = nominativo.TrovaCliente( codiceCliente );

            // Controllo se al codice corrisponde un cliente
            if( cliente == null )
            {
                System.Windows.Forms.MessageBox.Show(
                    String.Format( Properties.Resources.ClienteNotFoundText, codiceCliente ),
                    Properties.Resources.ClienteNotFoundTitle,
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning
                );
                return;
            }

            // Se il cliente è gia uscito avviso...
            if( cliente.Uscito )
            {
                System.Windows.Forms.MessageBox.Show(
                    String.Format( Properties.Resources.ClienteAlreadyExitedText, codiceCliente ),
                    Properties.Resources.ClienteAlreadyExitedTitle,
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning
                );
                return;
            }

            // Controllo se gestire le multe
            if( (bool)PluginPercorsi.GetGlobalParameter( "ManageFine" ).Value )
            {
                this.GestisciMulta( cliente );
            }

            // Tolgo il cliente dalla lista dei briefing exception lo segnalo come uscito
            cliente.UscitaCliente();

            System.Windows.Forms.MessageBox.Show(
                Properties.Resources.ClienteExitText,
                Properties.Resources.ClienteExitTitle,
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information
            );

            var rimanenti = from c in nominativo.GetRawList()
                            where c.Uscito == false
                            group c by c.Uscito into rimasti
                            select rimasti.Count();

            // Avviso l'utente se tutti i clienti del nominativo sono usciti
            if( rimanenti.Count() == 0 )
            {
                System.Windows.Forms.MessageBox.Show(
                    String.Format( Properties.Resources.NominativoEsauritoText, nominativo.Nome ),
                    Properties.Resources.NominativoEsauritoTitle,
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information
                );
            }

            this.ModelPersistence.SaveModel();
        }

        private void GestisciMulta( Model.Cliente cliente )
        {
            throw new NotImplementedException();
        }

        private void InitializeParameters()
        {
            // Inizializzazione dei parametri di configurazione
            this.Parameters.Add( "AutoForward",
                new PluginConfigValue( "AutoForward", true, "Se attivato permette di non mostrare le finestre con una sola scelta exception di proseguire in automatico" ) );
            this.Parameters.Add( "NominativoCodeSize",
                new PluginConfigValue( "NominativoCodeSize", 3, "La lunghezza in numero di cifre del codice del nominativo" ) );
            this.Parameters.Add( "ClienteCodeSize",
                new PluginConfigValue( "ClienteCodeSize", 3, "La lunghezza in numero di cifre del codice del cliente" ) );
            this.Parameters.Add( "AskDeleteOnZero",
                new PluginConfigValue( "AskDeleteOnZero", false, "Indica se quando un contatore arriva a zero applicare l'azione di elimina automaticamente" ) );
            this.Parameters.Add( "ManageFine",
                new PluginConfigValue( "ManageFine", false, "Indica se utilizzare il sistema di multe per i ritardi" ) );
            this.Parameters.Add( "SeeForwardMinutes",
                new PluginConfigValue( "SeeForwardMinutes", new TimeSpan( 0, 20, 0 ), "Conteggia i clienti di ritorno nei prossimi N minuti" ) );
            this.Parameters.Add( "OpeningTime",
                new PluginConfigValue( "OpeningTime", new TimeSpan( 10, 0, 0 ), "Orario di apertura del parco" ) );
            this.Parameters.Add( "ClosingTime",
                new PluginConfigValue( "ClosingTime", new TimeSpan( 20, 0, 0 ), "Orario di chiusura del parco" ) );
            this.Parameters.Add( "LastBriefingBefore",
                new PluginConfigValue( "LastBriefingBefore", new TimeSpan( 0, 30, 0 ), "Intervallo di tempo antecedente alla chiusura del parco da cui non sono più permessi briefings" ) );
            this.Parameters.Add( "NotturnaTime",
                new PluginConfigValue( "NotturnaTime", new TimeSpan( 21, 0, 0 ), "Ora di inizio della notturna" ) );

            // Vincoli ai parametri
            this.Parameters["OpeningTime"].Changing +=
                delegate( object sender, PluginConfigWriteAccessArgument e )
                {
                    // OrarioApertura < OrarioChiusura
                    if( (TimeSpan)e.NewValue > (TimeSpan)this.Parameters["ClosingTime"].Value )
                    {
                        e.NewValue = e.OldValue;
                    }
                };
            this.Parameters["ClosingTime"].Changing +=
                delegate( object sender, PluginConfigWriteAccessArgument e )
                {
                    // OrarioChiusura > OrarioApertura
                    if( (TimeSpan)e.NewValue < (TimeSpan)this.Parameters["OpeningTime"].Value )
                    {
                        e.NewValue = e.OldValue;
                    }
                };
            this.Parameters["LastBriefingBefore"].Changing +=
                delegate( object sender, PluginConfigWriteAccessArgument e )
                {
                    // BriefingBuffer < OrarioChiusura - OrarioApertura
                    if( (TimeSpan)e.NewValue >=
                        (TimeSpan)this.Parameters["ClosingTime"].Value - (TimeSpan)this.Parameters["OpeningTime"].Value )
                    {
                        e.NewValue = e.OldValue;
                    }
                };
        }

        private void InitializePersistence()
        {
            LoadingFunction dataAccessor = delegate
            {
                this.ModelPersistence = ModelAccessFactory.CreateDataAccessor( this.Host.Persistence );
                var loaded = this.m_modelPersistence.LoadModel();

                if( !loaded )
                {
                    System.Windows.Forms.MessageBox.Show( Properties.Resources.ModelNotLoadedText,
                                                          Properties.Resources.ModelNotLoadedTitle,
                                                          System.Windows.Forms.MessageBoxButtons.OK,
                                                          System.Windows.Forms.MessageBoxIcon.Error );
                }
            };

            dataAccessor();
            
            // Configuro il caricamento della configurazione
            LoadingFunction percorsi = null;
            percorsi += this.LoadConfiguration;
            percorsi += dataAccessor;
            this.RegisterLoadingFunctions( percorsi );
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
            // Inizializzazione del parco
            this.m_parco.Init();

            this.OnPluginLoaded();

            return true;
        }

        /// <summary>
        /// Funzione utilizzata per la chiusura del plugin.
        /// </summary>
        public override void Dispose()
        {
            this.ModelPersistence.SaveModel();
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
            var mainPanel = new Pannelli.MainWindowPanel();
            mainPanel.GetClienteEscape().CodiceReady += ExitCodeReadyHandler;
            return mainPanel;
        }

        /// <summary>
        /// Utilizzata per recuperare un elemento di un menù a tendina da aggiungere al menu principale della GUI.
        /// </summary>
        /// <returns>
        /// Il menu da aggiungere. Null in caso non si debba aggiungere niente
        /// </returns>
        public override System.Windows.Forms.ToolStripItem GetDropdownMenu()
        {
            var mnuTotale = new ToolStripMenuItem();
            mnuTotale.Name = "mnuTotale";
            mnuTotale.Text = "Totale di cassa";
            mnuTotale.Click += delegate { new Forms.TotaleCassa().ShowDialog( this.Host.GraphicHost.GetMainForm() ); };

            var mnuListaClienti = new ToolStripMenuItem();
            mnuListaClienti.Name = "mnuListaClienti";
            mnuListaClienti.Text = "Lista dei Clienti";
            mnuListaClienti.Click += delegate { new Forms.ClientiView().ShowDialog( this.Host.GraphicHost.GetMainForm() ); };

            var mnuPlugin = new ToolStripMenuItem();
            mnuPlugin.Name = "mnuPlugin";
            mnuPlugin.Text = "Percorsi";
            mnuPlugin.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
                mnuListaClienti,
                mnuTotale
            } );

            return mnuPlugin;
        }

        /// <summary>
        /// Recupera il nome visualizzato per l'attrazione
        /// </summary>
        /// <returns>Il nome dell'attrazione che verrà visualizzato all'utente</returns>
        public string GetDisplayName()
        {
            return Properties.Resources.AttrazioneDisplayedName;
        }

        /// <summary>
        /// Recupera lo stato di partenza del wizard
        /// </summary>
        /// <returns>Lo stato di partenza del wizard per l'emissione dei biglietti</returns>
        public Type GetFirstWizard()
        {
            return typeof( Wizard.TipoBigliettoState );
        }

        /// <summary>
        /// Recupera il costruttore dei dati utilizzato dall'attrazione
        /// </summary>
        /// <returns>Il costruttore dati</returns>
        public IBuilder GetBuilder()
        {
            return new Wizard.BigliettoPercorsiBuilder();
        }

        /// <summary>
        /// Avvia le routine di gestione del plugin. Il suo scopo è quello di differire l'esecuzione delle routine del
        /// plugin dalle procedure di caricamento; un esempio d'uso è l'impostazione di configurazioni prima dell'
        /// avvio del plugin
        /// </summary>
        /// <remarks>
        /// Il suo scopo è quello di differire l'esecuzione delle routine del plugin dalle procedure di caricamento;
        /// un esempio d'uso è l'impostazione di configurazioni prima dell'avvio del plugin
        /// </remarks>
        public override void Run( IPlugin owner )
        {
            if( owner == null )
            {
                throw new ArgumentException( "This plugin can't run standalone", "owner" );
            }
            this.OnPluginRunning();

            // Aggiungo il mio pannello alla GUI
            this.Host.GraphicHost.AttachInterface( this );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
