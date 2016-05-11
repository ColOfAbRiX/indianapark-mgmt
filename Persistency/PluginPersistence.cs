using System;
using System.Data.Common;
using System.Windows.Forms;
using IndianaPark.Licensing;
using IndianaPark.Plugin;
using IndianaPark.Tools.Security.Licensing;

namespace IndianaPark.Persistence
{
    /// <summary>
    /// Plugin per la gestione della persistenza dei dati.
    /// </summary>
#if( RELEASE || LICENSE )
    [System.ComponentModel.LicenseProvider( typeof( IpLicenseProvider ) )]
    [EnabledFeatures( SoftwareFeatures.Persistence )]
#endif
    public class PluginPersistence : IndianaparkPlugin, IPersistence
    {
		#region Fields 

		#region Class-wise Fields 

        private static PluginPersistence ms_GlobalInstance;

		#endregion Class-wise Fields 

		#region Internal Fields 

        private PersistenceFactory m_persistenceProvider;
        private string m_lastConnString;
#if( RELEASE || LICENSE )
        private System.ComponentModel.License m_pluginLicense;
#endif

        #endregion Internal Fields

        #region Public Fields

        /// <summary>
        /// Indica se la connessione è stata inizializzata
        /// </summary>
        /// <value>
        /// 	<c>true</c> se la connessione è stata inizializzata <c>false</c>, altrimenti.
        /// </value>
        public bool IsConnectionInitialized
        {
            get
            {
                if( this.m_persistenceProvider != null )
                {
                    var tmp = this.m_persistenceProvider.GetConnection();
                    return tmp != null && tmp.State == System.Data.ConnectionState.Open;
                }

                return false;
            }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginPersistence"/> class.
        /// </summary>
        /// <param name="host">Contenitore del plugin</param>
        /// <exception cref="ArgumentNullException">Il riferimento al contenitore del plugin non può essere <c>null</c></exception>
        public PluginPersistence( IPluginHost host ) : base( host )
        {
#if( RELEASE || LICENSE )
            this.m_pluginLicense = System.ComponentModel.LicenseManager.Validate( this.GetType(), this );
#endif

            if( PluginPersistence.ms_GlobalInstance != null )
            {
                throw new ApplicationException( this.GetType() + " instanziated more than once." );
            }
            PluginPersistence.ms_GlobalInstance = this;

            // Imposto i parametri di configurazione
            this.Parameters.Add( "DatabaseType",
                new PluginConfigValue( "DatabaseType", PersistenceFactory.SupportedDatabase.NoDatabase, "Tipo di database in uso" ) );
            this.Parameters.Add( "DatabaseHost",
                new PluginConfigValue( "DatabaseHost", "", "Host in cui cercare il DBMS" ) );
            this.Parameters.Add( "DatabaseUsername",
                new PluginConfigValue( "DatabaseUsername", "", "Username per l'autenticazione sul database" ) );
            this.Parameters.Add( "DatabasePassword",
                new PluginConfigValue( "DatabasePassword", "", "Password di autenticazione sul database" ) );
            this.Parameters.Add( "DatabaseSchema",
                new PluginConfigValue( "DatabaseSchema", "", "Database da utilizzare" ) );

            // Aggiornamenti al variare dei parametri di connessione
            this.Parameters["DatabaseType"].Changed += delegate { this.CreateConnection(); };
            this.Parameters["DatabaseHost"].Changed += delegate { this.CreateConnection(); };
            this.Parameters["DatabaseUsername"].Changed += delegate { this.CreateConnection(); };
            this.Parameters["DatabasePassword"].Changed += delegate { this.CreateConnection(); };
            this.Parameters["DatabaseSchema"].Changed += delegate { this.CreateConnection(); };

            // Carico la configurazione salvata sul file in locale
            this.LoadLocalConfig();

            // Creo la connessione
            this.CreateConnection();

            // Configuro il caricamento della configurazione
            this.RegisterLoadingFunctions( this.LoadConfiguration );
        }

		#endregion Constructors 

        /// <summary>
        /// Carica la configurazione da un file XML
        /// </summary>
        private void LoadLocalConfig()
        {
            var config = new Plugin.Persistence.PluginConfigLocalPersistence( IndianaPark.Persistence.Properties.Resources.LocalConfigFile );

            // Carico la configurazione dal database
            var parameters = config.LoadAllParameters( this.Name );

            // Unisco i dati caricati con i parametri di default
            foreach( var param in parameters.Values )
            {
                if( this.Parameters.ContainsKey( param.Name ) )
                {
                    this.Parameters[param.Name].Value = param.Value;
                }
                else
                {
                    this.Parameters.Add( param.Name, param );
                }
            }

            config.SaveAllParameters( this.Parameters, this.Name );
        }

        private void SaveLocalConfig()
        {
            var config = new Plugin.Persistence.PluginConfigLocalPersistence( IndianaPark.Persistence.Properties.Resources.LocalConfigFile );
            config.SaveAllParameters( this.Parameters, this.Name );
        }

        #region Class-wise Methods 

        /// <summary>
        /// Recupera l'oggetto per la gestione della persistenza
        /// </summary>
        /// <returns></returns>
        public static IPersistence GetPersistence()
        {
            return PluginPersistence.ms_GlobalInstance;
        }

        /// <summary>
        /// Permette di accedere all'istanza globale dei parametri di configurazione del plugin da qualunque punto dell'Assembly
        /// </summary>
        /// <returns>I parametri di configurazione del plugin globale. <c>null</c> se il parametro non esiste</returns>
        internal static IConfigValue GetGlobalParameter( string name )
        {
            if( PluginPersistence.ms_GlobalInstance.Parameters.ContainsKey( name ) )
            {
                return PluginPersistence.ms_GlobalInstance.Parameters[name];
            }

            return null;
        }

        /// <summary>
        /// Recupera l'istanza globale del plugin.
        /// </summary>
        /// <remarks>
        /// Questo metodo non istanzia il plugin, che deve essere istanziato dal suo <see cref="IPluginManager"/>
        /// </remarks>
        /// <returns>L'istanza globale del Plugin</returns>
        internal static PluginPersistence GetGlobalInstance()
        {
            if( PluginPersistence.ms_GlobalInstance == null )
            {
                throw new ApplicationException( typeof( PluginPersistence ) + " is not istantiated." );
            }

            return PluginPersistence.ms_GlobalInstance;
        }

		#endregion Class-wise Methods 

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
            this.Host.Persistence = this;
            return true;
        }

        /// <summary>
        /// Funzione utilizzata per la chiusura del plugin.
        /// </summary>
        public override void Dispose()
        {
            this.SaveLocalConfig();
            this.SaveConfiguration();

            if( IsConnectionInitialized )
            {
                this.m_persistenceProvider.GetConnection().Close();
            }

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
        public override Control GetCommandPanel()
        {
            return null;
        }

        /// <summary>
        /// Utilizzata per recuperare un elemento di un menù a tendina da aggiungere al menu principale della GUI.
        /// </summary>
        /// <returns>
        /// Il menu da aggiungere. Null in caso non si debba aggiungere niente
        /// </returns>
        public override ToolStripItem GetDropdownMenu()
        {
            return null;
        }

        /// <summary>
        /// Crea la connessione al database.
        /// </summary>
        /// <remarks>
        /// La connessione viene creata utilizzando i parametri del plugin.
        /// </remarks>
        private void CreateConnection()
        {
            var tmpProvider = 
                Persistence.PersistenceFactory.GetFactory(
                    (PersistenceFactory.SupportedDatabase)(this.Parameters["DatabaseType"].Value) );

            // Ottengo la connessione al database
            if( tmpProvider != null )
            {
                // Creo la stringa di connessione
                var cnStr = tmpProvider.GetConnectionString(
                    (string)this.Parameters["DatabaseHost"].Value,
                    (string)this.Parameters["DatabaseUsername"].Value,
                    (string)this.Parameters["DatabasePassword"].Value,
                    (string)this.Parameters["DatabaseSchema"].Value
                );

                // La nuova connessione viene creata solo se c'è un cambio nella stringa.
                if( cnStr != null && cnStr != this.m_lastConnString )
                {
                    this.m_lastConnString = cnStr;

                    // Se c'è da fare un cambiamento, prima chiudo la vecchia connessione
                    if( this.IsConnectionInitialized )
                    {
                        this.m_persistenceProvider.GetConnection().Close();
                    }

                    tmpProvider.CreateConnection( cnStr );
                    this.m_persistenceProvider = tmpProvider;
                    this.OnConnectionChanged();

                    if( !this.IsConnectionInitialized )
                    {
                        MessageBox.Show( Properties.Resources.ConnectionFailedText,
                                         Properties.Resources.ConnectionFailedTitle, MessageBoxButtons.OK,
                                         MessageBoxIcon.Error );
                    }
                }
            }
            else
            {
                this.m_lastConnString = "";

                // Se c'è da fare un cambiamento, prima chiudo la vecchia connessione
                if( this.IsConnectionInitialized )
                {
                    this.m_persistenceProvider.GetConnection().Close();
                }
            }
        }

        /// <summary>
        /// Ottiene la connessione al database
        /// </summary>
        /// <returns>La connessione al database</returns>
        /// <remarks>
        /// La connessione ottenuta è sempre gia aperta
        /// </remarks>
        public DbConnection GetConnection()
        {
            if( !IsConnectionInitialized )
            {
                this.CreateConnection();
            }

            if( this.m_persistenceProvider == null )
            {
                return null;
            }

            return this.m_persistenceProvider.GetConnection();
        }

        public event EventHandler ConnectionChanged;

        /// <summary>
        /// Avvia l'evento ConnectionChanged
        /// </summary>
        protected void OnConnectionChanged()
        {
            if( this.ConnectionChanged != null )
            {
                this.ConnectionChanged( this, new EventArgs() );
            }
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
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
