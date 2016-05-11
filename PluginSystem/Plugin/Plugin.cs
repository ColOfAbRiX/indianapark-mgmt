using System;
using System.Collections.Generic;
namespace IndianaPark.Plugin
{
    /// <summary>
    /// Implementazione generica ed astratta per un plugin di default
    /// </summary>
    public abstract class IndianaparkPlugin : IPluginRunnable, IPluginViewable
    {
		#region Delegates 

        /// <summary>
        /// Delegato che rappresenta le funzioni di configurazione
        /// </summary>
        protected delegate void LoadingFunction();

		#endregion Delegates 

		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Contiene il proprietario del plugin
        /// </summary>
        private IPluginHost m_host;
        /// <summary>
        /// Contiene i dati di configurazione
        /// </summary>
        private readonly Dictionary<string, IConfigValue> m_config = new Dictionary<string, IConfigValue>( 10 );

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Oggetto contenitore del plugin. Utilizzato per fare interagire il plugin con l'ambiente esterno
        /// </summary>
        /// <value>Oggetto contenitore del plugin.</value>
        public IPluginHost Host
        {
            get
            {
                return this.m_host;
            }
            set
            {
                this.m_host = value;
                this.OnHostChanged();
            }
        }

        /// <summary>
        /// Il percorso assoluto della DLL contenente il plugin.
        /// </summary>
        /// <value>Il percorso assoluto della DLL contenente il plugin.</value>
        public string Location
        {
            get
            {
                return System.Reflection.Assembly.GetAssembly( this.GetType() ).Location;
            }
        }

        /// <summary>
        /// Nome del plugin. Deve essere la stringa che contiene il nome della classe del plugin
        /// </summary>
        /// <value>Il nome del plugin</value>
        public string Name
        {
            get { return this.GetType().ToString(); }
        }

        /// <summary>
        /// Insieme dei parametri di configurazione del plugin
        /// </summary>
        public Dictionary<string, IConfigValue> Parameters
        {
            get
            {
                return this.m_config;
            }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Evento che si scatena quando il plugin ha terminato di caricarsi
        /// </summary>
        public event PluginEventHandler PluginLoaded;
        /// <summary>
        /// Evento che si scatena quando il plugin è stato finalizzato per la terminazione
        /// </summary>
        public event PluginEventHandler PluginDisposed;
        /// <summary>
        /// Evento che si scatena quando viene chiamata la routine Run() del plugin
        /// </summary>
        public event PluginEventHandler PluginRun;
        /// <summary>
        /// Evento che si scatena quando viene cambiato l'host del plugin
        /// </summary>
        public event PluginEventHandler HostChanged;
        /// <summary>
        /// Scatenato ogni volta che un valore di configurazione cambia
        /// </summary>
        public event PluginEventHandler ConfigChanged;

		#region Raisers 

        /// <summary>
        /// Scatena l'evento PluginRunning
        /// </summary>
        protected void OnPluginRunning()
        {
            if( this.PluginRun != null )
            {
                var ea = new EventArgs();
                this.PluginRun( this, ea );
            }
        }

        /// <summary>
        /// Scatena l'evento PluginLoaded
        /// </summary>
        protected void OnPluginLoaded()
        {
            if( this.PluginLoaded != null )
            {
                var ea = new EventArgs();
                this.PluginLoaded( this, ea );
            }
        }

        /// <summary>
        /// Scatena l'evento PluginDisposed
        /// </summary>
        protected void OnPluginDisposed()
        {
            if( this.PluginDisposed != null )
            {
                var ea = new EventArgs();
                this.PluginDisposed( this, ea );
            }
        }

        /// <summary>
        /// Scatena l'evento HostChanged
        /// </summary>
        protected void OnHostChanged()
        {
            if( this.HostChanged != null )
            {
                var ea = new EventArgs();
                this.HostChanged( this, ea );
            }
        }

        /// <summary>
        /// Scatena l'evento ConfigChanged
        /// </summary>
        protected void OnConfigChanged()
        {
            if( this.ConfigChanged != null )
            {
                var ea = new EventArgs();
                this.ConfigChanged( this, ea );
            }
        }

		#endregion Raisers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="host">Contenitore del plugin</param>
        /// <exception cref="ArgumentNullException">Il riferimento al contenitore del plugin non può essere <c>null</c></exception>
        protected IndianaparkPlugin( IPluginHost host )
        {
            if( host == null )
            {
                throw new ArgumentNullException( "host", "You must specify a plugin host" );
            }

            this.m_host = host;
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Salva tutti i parametri di configurazione del plugin nel database
        /// </summary>
        /// <remarks>
        /// Questa funzione deve essere chiamata solamente quando il gestore della persistenza è associato al plugin
        /// </remarks>
        protected void SaveConfiguration()
        {
            if( this.Host.Persistence == null || !this.Host.Persistence.IsConnectionInitialized  )
            {
                return;
            }

            // TODO L'oggetto che effettua il salvataggio non deve essere hard-coded!!
            var config = new Plugin.Persistence.PluginConfigDBPersistence( this.Host.Persistence );

            // Salvo tutti i parametri
            config.SaveAllParameters( this.Parameters, this.Name );
        }

        /// <summary>
        /// Carica i parametri di configurazione memorizzati nella persistenza
        /// </summary>
        /// <remarks>
        /// Questa funzione deve essere chiamata solamente quando il gestore della persistenza è associato al plugin
        /// </remarks>
        protected void LoadConfiguration()
        {
            if( this.Host.Persistence == null || !this.Host.Persistence.IsConnectionInitialized )
            {
                return;
            }

            // TODO L'oggetto che effettua il salvataggio non deve essere hard-coded!!
            var config = new Plugin.Persistence.PluginConfigDBPersistence( this.Host.Persistence );

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

            // Faccio in modo che i parametri si auto-salvino quando il loro valore viene cambiato
            foreach( var param in Parameters.Values )
            {
                if( param.GetType().GetInterface( typeof( IConfigPersistent ).ToString(), true ) != null )
                {
                    ((IConfigPersistent)param).OwnerName = this.Name;
                    ((IConfigPersistent)param).Persistence = config;
                }
            }

            // Salvo i dati in caso mancassero sul database
            config.SaveAllParameters( this.Parameters, this.Name );
        }

        /// <summary>
        /// Registra le funzioni di caricamento dati da database per essere chiamate ogni volta
        /// che il database diventa pronto
        /// </summary>
        /// <param name="configuring">Le funzioni di configurazione da chiamare</param>
        protected void RegisterLoadingFunctions( LoadingFunction configuring )
        {
            // Controlla la connessione, exception in caso chiama i configuratori
            EventHandler connectionCheck = delegate
            {
                if( Host.Persistence.IsConnectionInitialized && configuring != null )
                {
                    configuring();
                }
            };

            // Controlla la persistenza, exception in caso chiama il controllo connessione
            EventHandler persistenceCheck = delegate
            {
                if( Host.Persistence != null )
                {
                    // Registra una volta per persistenza, il controllo della connessione
                    Host.Persistence.ConnectionChanged -= connectionCheck;
                    Host.Persistence.ConnectionChanged += connectionCheck;

                    // Tenta il controllo connessione
                    connectionCheck( null, null );
                }
            };

            // Registra una volta per host, il controllo della persistenza
            Host.PersistenceChanged -= persistenceCheck;
            Host.PersistenceChanged += persistenceCheck;

            // Tenta il controllo persistenza
            persistenceCheck( null, null );
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
        public abstract bool Load();

        /// <summary>
        /// Funzione utilizzata per la chiusura del plugin.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Utilizzata per recuperare un'interfaccia grafica per l'interazione con l'utente. Il controllo utente
        /// recuperato viene, di solito, aggiunto alla finestra principale del programma
        /// </summary>
        /// <returns>
        /// Il pannello da aggiungere all'interfaccia grafica. Null in caso non si debba aggiungere niente
        /// </returns>
        public abstract System.Windows.Forms.Control GetCommandPanel();

        /// <summary>
        /// Utilizzata per recuperare un elemento di un menù a tendina da aggiungere al menu principale della GUI.
        /// </summary>
        /// <returns>
        /// Il menu da aggiungere. Null in caso non si debba aggiungere niente
        /// </returns>
        public abstract System.Windows.Forms.ToolStripItem GetDropdownMenu();

        /// <summary>
        /// Restituisce un oggetto String che rappresenta l'oggetto corrente.
        /// </summary>
        /// <returns>
        /// Oggetto String che rappresenta l'oggetto corrente
        /// </returns>
        public override string ToString()
        {
            return Name;
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
        public abstract void Run( IPlugin owner );

		#endregion Public Methods 

		#endregion Methods 
    }
}
