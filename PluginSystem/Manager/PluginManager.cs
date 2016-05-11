using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using IndianaPark.Tools.Logging;
using IndianaPark.Tools;

namespace IndianaPark.Plugin
{
    /// <summary>
    /// Classe concreta del gestore dei plugin. La classe implementa sia IPluginManager che IPluginHost,
    /// pertanto svolge una doppia funzione
    /// </summary>
    public class PluginManager : IPluginManager, IDisposable
    {
		#region Fields 

		#region Internal Fields 

        private IPersistence m_persistence;
        private IPluginGraphicHost m_graphicHost;
        private readonly List<IPlugin> m_pluginsInst = new List<IPlugin>( 5 );
        private readonly Dictionary<string, IPlugin> m_pluginsName = new Dictionary<string, IPlugin>( 5 );
        private readonly Dictionary<string, IList<IPlugin>> m_pluginsPath = new Dictionary<string, IList<IPlugin>>( 5 );

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Host grafico associato con il gestore di plugin
        /// </summary>
        public IPluginGraphicHost GraphicHost
        {
            get { return m_graphicHost; }
            set
            {
                m_graphicHost = value;
                this.OnGraphicHostChanged();
            }
        }

        /// <summary>
        /// Gestore dei plugin. Viene restituito sempre l'istanza corrente, exception non è possibile modificarla
        /// </summary>
        public IPluginManager Manager
        {
            get { return this; }
            set { }
        }

        /// <summary>
        ///   Oggetto che si occupa delle operazioni di base per la persistenza dei dati, come la connessione ad un database
        /// </summary>
        /// <value>Il gestore della persistenza</value>
        public IPersistence Persistence
        {
            get { return m_persistence; }
            set
            {
                m_persistence = value;
                this.OnPersistenceChanged();
            }
        }

        /// <summary>
        /// Indica se cercare i plugin in maniera ricorsiva oppure no dalla directory di avvio
        /// </summary>
        public SearchOption ScanRecursively { get; set; }

        /// <summary>
        /// Recupera l'istanza del plugin con il nome specificato
        /// </summary>
        /// <returns>L'istanza del plugin cercato o null se non presente</returns>
        public IPlugin this[string name]
        {
            get
            {
                if( !this.m_pluginsName.ContainsKey( name ) )
                {
                    return null;
                }

                return this.m_pluginsName[name];
            }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Avviene quando il gestore della GUI viene cambiato
        /// </summary>
        public event EventHandler GraphicHostChanged;
        /// <summary>
        /// Avviene quando il gestore della persistenza viene cambiato
        /// </summary>
        public event EventHandler PersistenceChanged;

		#region Raisers 

        /// <summary>
        /// Avvia l'evento GraphicHostChanged
        /// </summary>
        protected void OnGraphicHostChanged()
        {
            if( this.GraphicHostChanged != null )
            {
                this.GraphicHostChanged( this, new EventArgs() );
            }
        }

        /// <summary>
        /// Avvia l'evento PesistenceChanged
        /// </summary>
        protected void OnPersistenceChanged()
        {
            if( this.PersistenceChanged != null )
            {
                this.PersistenceChanged( this, new EventArgs() );
            }
        }

		#endregion Raisers 

		#endregion Events 

		#region Methods 

		#region Internal Methods 

        /// <summary>
        /// Restituisce un enumeratore per l'intero insieme.
        /// </summary>
        /// <returns>
        /// Oggetto IEnumerator per l'intero insieme.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.m_pluginsName.GetEnumerator();
        }

        /// <summary>
        /// Aggiunge un plugin all'insieme dei plugin gestiti
        /// </summary>
        /// <param name="plugin">Il plugin da aggiungere</param>
        protected void AddToSet( IPlugin plugin )
        {
            // Aggiungo il plugin alla lista delle istanze
            if( !this.m_pluginsInst.Contains( plugin ) )
            {
                this.m_pluginsInst.Add( plugin );
            }

            // Aggiungo il plugin alla lista dei nomi
            if( !this.m_pluginsName.ContainsKey( plugin.Name ) )
            {
                this.m_pluginsName.Add( plugin.Name, plugin );
            }

            // Aggiungo il plugin alla lista delle directory
            if( !this.m_pluginsPath.ContainsKey(plugin.Location) )
            {
                this.m_pluginsPath.Add(plugin.Location, new List<IPlugin>( 5 ) );
            }
            if( !this.m_pluginsPath[plugin.Location].Contains( plugin ) )
            {
                this.m_pluginsPath[plugin.Location].Add( plugin );
            }
        }

        /// <summary>
        /// Rimuove un plugin all'insieme dei plugin gestiti
        /// </summary>
        /// <param name="plugin">Il plugin da rimuovere</param>
        protected void RemoveFromSet( IPlugin plugin )
        {
            // Rimuovo il plugin dalla lista delle istanze
            if( this.m_pluginsInst.Contains( plugin ) )
            {
                this.m_pluginsInst.Remove( plugin );
            }

            // Rimuovo il plugin dalla lista dei nomi
            if( this.m_pluginsName.ContainsKey( plugin.Name ) )
            {
                this.m_pluginsName.Remove( plugin.Name );
            }

            // Rimuovo il plugin dalla lista delle directory
            if( this.m_pluginsPath.ContainsKey( plugin.Location ) )
            {
                if( this.m_pluginsPath[plugin.Location].Contains( plugin ) )
                {
                    this.m_pluginsPath[plugin.Location].Remove( plugin );
                }

                if( this.m_pluginsPath.Count == 0 )
                {
                    this.m_pluginsPath.Remove( plugin.Location );
                }
            }
        }

        /// <summary>
        /// La funzione carica un plugin da un assembly specificato. Lo istanzia, chiama la funzione di inizializzazione
        /// e poi lo aggiunge alla lista dei plugin caricati
        /// </summary>
        /// <param name="asm">L'assembly da cui caricare il plugin</param>
        /// <param name="name">Il nome della classe del plugin</param>
        /// <returns><c>true</c> se il plugin è stato caricato correttamente, <c>false</c> altrimenti</returns>
        protected bool LoadFromAssembly( Assembly asm, string name )
        {
            // Recupero il tipo di dato supposto del plugin
            Type type = asm.GetType( name );
            if( type == null || type.IsAbstract )
            {
                return false;
            }

            // Controllo se implementa l'interfaccia per i plugin
            if( type.GetInterface( typeof(IPlugin).ToString(), true ) != null )
            {
                Logger.Default.Write( "Found plugin " + type + " in file " + asm.Location, Verbosity.InformationDebug | Verbosity.Data );

                // Istanzio ed inizializzo il plugin
                var iip = Activator.CreateInstance( type, this ) as IPlugin;
                if( iip != null )
                {
                    iip.Host = this;

                    if( !iip.Load() )
                    {
                        Logger.Default.Write( "Failed to load " + type + " in file " + asm.Location, Verbosity.InformationDebug | Verbosity.Data );
                        return false;
                    }

                    this.AddToSet( iip );
                    Logger.Default.Write( "Succesfully loaded " + type + " from file " + asm.Location, Verbosity.InformationDebug | Verbosity.Debug );
                    return true;
                }
                return false;
            }

            //Logging.WriteLine( "Plugin " + type + " not found in file " + asm.Location );
            return false;
        }

        /// <summary>
        /// Esegue la procedura principale di tutti i plugin caricati.
        /// </summary>
        /// <remarks>
        /// Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/>
        /// </remarks>
        /// <param name="owner">Il chiamante dell'esecuzione del plugin</param>
        /// <param name="strict">Se impostato a <c>true</c> permette di eseguire solo il plugin dipendente dal <paramref name="owner"/> ignorando i
        /// plugin senza nessun owner</param>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo su tutti i plugin, <c>false</c> altrimenti
        /// </returns>
        protected bool RunAll( IPlugin owner, bool strict )
        {
            bool test = true;

            // Eseguo tutti i plugin
            foreach( IPlugin plugin in this.m_pluginsInst )
            {
                test &= this.Run( plugin.Name, owner, strict );
            }

            return test;
        }

        /// <summary>
        /// Recupera la lista dei percorsi assoluti di DLL
        /// </summary>
        /// <param name="customPath">Il percorso relativo alla directory d'esecuzione</param>
        /// <param name="searchOption">Ricorsività della ricerca.</param>
        /// <returns>La lista delle DLL trovate</returns>
        /// <remarks>
        /// Se viene passato il percorso relativo di una DLL viene restituito semplicemente il percorso assoluto di quel file. Se viene
        /// passato il percorso relativo di una directory viene restituita la lista delle DLL in quella directory.
        /// </remarks>
        protected string[] GetFileList( string customPath, SearchOption searchOption )
        {
            customPath = Path.GetFullPath( ".\\" + customPath );

            string path = Path.GetDirectoryName( customPath );
            string name = Path.GetFileName( customPath );

            string[] fileList = Directory.GetFiles( path, "*.dll", searchOption );
            if( !string.IsNullOrEmpty( name ) )
            {
                // Deve essere passato il percorso di una DLL...
                if( String.Compare( Path.GetExtension( customPath ).ToLower(), ".dll", false ) != 0 )
                {
                    return new string[0];
                }

                // Se il path è di un file, restituisco solo quel file
                fileList = new[] { Path.Combine( path, name ) };
            }

            return fileList;
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="PluginManager"/> is reclaimed by garbage collection.
        /// </summary>
        public void Dispose()
        {
            this.UnloadAll();
        }

        /// <summary>
        /// Carica tutti i plugin.
        /// </summary>
        /// <remarks>
        /// <para>La ricerca avviene a partire dalla directory di esecuzione del programma e prosegue in tutte le sue sottodirectory.</para>
        /// <para>La funzione scarica tutti i plugin precedentemente caricati.</para>
        /// <para>Il caricamento comprende la ricerca dei plugin, l'istanziazione e la chiamata di <see cref="IPlugin.Load"/>.</para>
        /// </remarks>
        /// <returns>
        /// Il numero di plugin caricati con successo.
        /// </returns>
        public int LoadAll()
        {
            return this.LoadAll( "" );
        }

        /// <summary>
        /// Scarica tutti i plugin avviati
        /// </summary>
        /// <remarks>
        /// Prima di scaricare il plugin viene chiamato il metodo <see cref="IPlugin.Dispose"/> di ogni plugin.
        /// </remarks>
        /// <returns>
        /// <c>true</c> se tutti i plugin sono stati scaricato, <c>false</c> altrimenti
        /// </returns>
        public bool UnloadAll()
        {
            // Copio la lista perchè non posso modificarla mentre ci itero sopra
            var plugins = new IPlugin[this.m_pluginsInst.Count];
            this.m_pluginsInst.CopyTo( plugins, 0 );

            bool test = true;

            // Copio la lista perchè non posso modificarla mentre ci itero sopra
            foreach( IPlugin plugin in plugins )
            {
                test &= Unload( plugin );
            }

            return test;
        }

        /// <summary>
        /// Esegue la procedura principale di tutti i plugin non dipendono da nessun altro plugin
        /// </summary>
        /// <remarks>
        /// Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/>
        /// </remarks>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo su tutti i plugin, <c>false</c> altrimenti
        /// </returns>
        public bool RunIndipendents()
        {
            return RunAll( null, false );
        }

        /// <summary>
        /// Restituisce un enumeratore per l'intero insieme.
        /// </summary>
        /// <returns>
        /// Oggetto IEnumerator&lt;KeyValuePair&lt;string, IPlugin&gt;&gt; per l'intero insieme.
        /// </returns>
        public IEnumerator<KeyValuePair<string, IPlugin>> GetEnumerator()
        {
            return this.m_pluginsName.GetEnumerator();
        }

        /// <summary>
        /// Carica tutti i plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException">Il percorso del file da caricare <paramref name="subPath"/> deve essere specificato.</exception>
        /// <remarks>
        /// <para>La ricerca avviene a partire dalla directory di esecuzione del programma e prosegue in tutte le sue sottodirecyory.</para>
        /// <para>La funzione scarica tutti i plugin precedentemente caricati.</para>
        /// <para>Il caricamento comprende la ricerca dei plugin, l'istanziazione e la chiamata di <see cref="IPlugin.Load"/>.</para>
        /// </remarks>
        /// <param name="subPath">Il percorso relativo partire dalla directory di esecuzione del programma.</param>
        /// <returns>
        /// Il numero di plugin caricati con successo.
        /// </returns>
        public int LoadAll( string subPath )
        {
            if( subPath == null )
            {
                throw new ArgumentNullException( "subPath" );
            }

            // Svuoto la lista dei plugin caricati
            UnloadAll();

            // Recupero la lista delle .dll nella dir indicata a partire da quella di avvio exception sottodir
            string[] pluginFiles = Directory.GetFiles( ".\\" + subPath, "*.dll", this.ScanRecursively );

            // Carico tutti i plugin della DLL
            Array.ForEach( pluginFiles, plugin => LoadFromPath( plugin ) );

            return this.m_pluginsName.Count;
        }

        /// <summary>
        /// Carica dalla DLL indicata tutti i plugin in essa implementati.
        /// </summary>
        /// <exception cref="ArgumentNullException">Il percorso del file da caricare <paramref name="fileSubpath"/> deve essere specificato.</exception>
        /// <remarks>
        /// <para>
        /// <paramref name="fileSubpath"/> è un percorso relativo alla directory di esecuzione del programma, exception può essere il percorso di un
        /// file DLL o di una directory. Per maggiori informazioni vedere <see cref="PluginManager.GetFileList"/>.
        /// </para>
        /// <para>Il caricamento comprende la ricerca dei plugin, l'istanziazione e la chiamata di <see cref="IPlugin.Load"/></para>
        /// </remarks>
        /// <param name="fileSubpath">Il percorso relativo della DLL a partire dalla directory di esecuzione del programma.</param>
        /// <returns>
        /// <c>true</c> se almeno un plugin stato caricato dalla DLL, <c>false</c> altrimenti.
        /// </returns>
        public bool LoadFromPath( string fileSubpath )
        {
            if( fileSubpath == null )
            {
                throw new ArgumentNullException( "fileSubpath" );
            }

            bool result = false;

            try
            {
                string[] pluginFiles = this.GetFileList( fileSubpath, SearchOption.TopDirectoryOnly );
                foreach( string pluginPath in pluginFiles )
                {
                    // Controllo che il file esista
                    if( !File.Exists( pluginPath ) )
                    {
                        continue;
                    }

                    // Caricamento della DLL
                    Assembly asm = Assembly.LoadFile( pluginPath );
                    if( asm == null )
                    {
                        continue;
                    }

                    // Cerco i tipi di dato dentro la DLL
                    foreach( Type type in asm.GetTypes() )
                    {
                        result |= LoadFromAssembly( asm, type.ToString() );
                    }
                }
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Exception occured while loading plugin file " + fileSubpath, Verbosity.WarningDebug | Verbosity.User );
            }

            return result;
        }

        /// <summary>
        /// Carica uno specifico plugin
        /// </summary>
        /// <exception cref="ArgumentNullException">Il nome del plugin da caricare <paramref name="name"/> deve essere specificato.</exception>
        /// <remarks>
        /// <para>Il caricamento comprende la ricerca dei plugin, l'istanziazione e la chiamata di <see cref="IPlugin.Load"/></para>
        /// <para>La ricerca avviene a partire dalla directory di esecuzione del programma e prosegue in tutte le sue sottodirecyory.</para>
        /// </remarks>
        /// <param name="name">Il nome del plugin (ovvero il nome della classe che implementa l'interfaccia plugin)</param>
        /// <returns>
        /// <c>true</c> se il plugin è stato caricato con successo, <c>false</c> altrimenti
        /// </returns>
        public bool LoadFromName( string name )
        {
            if( String.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name" );
            }

            return LoadFromPath( "", name );
        }

        /// <summary>
        /// Scarica dalla memoria tutti i plugin presenti dalla DLL indicata
        /// </summary>
        /// <exception cref="ArgumentNullException">Il percorso del file da caricare <paramref name="fileSubpath"/> deve essere specificato</exception>
        /// <remarks>
        /// <para>Prima di scaricare il plugin viene chiamato il metodo <see cref="IPlugin.Dispose"/> di ogni plugin.</para>
        /// <para>Il percorso di ricerca è relativo alla directory di esecuzione del programma.</para>
        /// </remarks>
        /// <param name="fileSubpath">Il percorso relativo della DLL a partire dalla directory di esecuzione del programma</param>
        /// <returns>
        /// <c>true</c> se tutti i plugin della DLL sono stati scaricato, <c>false</c> altrimenti
        /// </returns>
        public bool UnloadFromPath( string fileSubpath )
        {
            if( String.IsNullOrEmpty( fileSubpath ) )
            {
                throw new ArgumentNullException( "fileSubpath" );
            }

            string path = Path.GetFullPath( ".\\" + fileSubpath );
            if( this.m_pluginsPath.ContainsKey( path ) )
            {
                // Copio la lista perchè non posso modificarla mentre ci itero sopra
                var plugins = new IPlugin[this.m_pluginsPath[path].Count];
                this.m_pluginsPath[path].CopyTo( plugins, 0 );

                bool test = true;

                // Scarico tutti i plugin exception tengo traccia dell'avvenuto scaricamento
                foreach( IPlugin plugin in plugins )
                {
                    test &= Unload( plugin );
                }

                return test;
            }

            return false;
        }

        /// <summary>
        /// Scarica il plugin indicato
        /// </summary>
        /// <exception cref="ArgumentNullException">Il nome del plugin da caricare <paramref name="name"/> deve essere specificato</exception>
        /// <remarks>
        /// <para>Prima di scaricare il plugin viene chiamato il metodo <see cref="IPlugin.Dispose"/> del plugin stesso</para>
        /// <para>Il percorso di ricerca è relativo alla directory di esecuzione del programma.</para>
        /// </remarks>
        /// <param name="name">Il nome della classe del plugin da scaricare</param>
        /// <returns>
        /// <c>true</c> se lo scaricamento ha esito positivo, <c>false</c> altrimenti
        /// </returns>
        public bool Unload( string name )
        {
            if( string.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name" );
            }

            // Se il plugin esiste lo rimuovo
            if( this.m_pluginsName.ContainsKey( name ) )
            {
                return Unload( this.m_pluginsName[name] );
            }

            return false;
        }

        /// <summary>
        /// Scarica il plugin indicato
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificata in <paramref name="plugin"/> un'istanza valida del plugin da scaricare</exception>
        /// <remarks>
        /// <para>Prima di scaricare il plugin viene chiamato il metodo <see cref="IPlugin.Dispose"/> del plugin stesso</para>
        /// <para>Il percorso di ricerca è relativo alla directory di esecuzione del programma.</para>
        /// </remarks>
        /// <param name="plugin">Istanza del plugin da scaricare</param>
        /// <returns>
        /// <c>true</c> se lo scaricamento ha esito positivo, <c>false</c> altrimenti
        /// </returns>
        public bool Unload( IPlugin plugin )
        {
            if( plugin == null )
            {
                throw new ArgumentNullException( "plugin" );
            }

            if( !this.m_pluginsInst.Contains( plugin ) )
            {
                return false;
            }

            // Se è specificato rimuovo il plugin
            try
            {
                plugin.Dispose();
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Exception occured while UNloading plugin file " + plugin.Name, Verbosity.WarningDebug | Verbosity.User );
            }

            RemoveFromSet( plugin );
            Logger.Default.Write( "Unloaded plugin " + plugin.Name, Verbosity.InformationDebug | Verbosity.Data );
            return true;

        }

        /// <summary>
        /// Esegue la procedura principale di tutti i plugin caricati
        /// </summary>
        /// <param name="owner">Il chiamante dell'esecuzione del plugin</param>
        /// <remarks>
        /// <para>Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/></para>
        /// <para>Per essere caricato il plugin deve dipendere dall'<paramref name="owner"/> indicato</para>
        /// </remarks>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo su tutti i plugin, <c>false</c> altrimenti
        /// </returns>
        public bool RunAll( IPlugin owner )
        {
            return RunAll( owner, false );
        }

        /// <summary>
        /// Esegue la procedura principale di tutti e soli i plugi che dipendono dal chiamante
        /// </summary>
        /// <remarks>
        /// Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/>
        /// </remarks>
        /// <param name="owner">Il chiamante dell'esecuzione del plugin</param>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo su tutti i plugin, <c>false</c> altrimenti
        /// </returns>
        public bool RunOwneds( IPlugin owner )
        {
            return RunAll( owner, true );
        }

        /// <summary>
        /// Esegue la procedura principale di tutti i plugin contenuti in una DLL.
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificato il percorso del file <paramref name="fileSubpath"/></exception>
        /// <remarks>
        /// <para>
        /// Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/>
        /// </para>
        /// <para>
        /// <paramref name="fileSubpath"/> è un percorso relativo alla directory di esecuzione del programma, exception può essere il percorso di un
        /// file DLL o di una directory. Per maggiori informazioni vedere <see cref="PluginManager.GetFileList"/>
        /// </para>
        /// </remarks>
        /// <param name="fileSubpath">Nome del plugin da avviare</param>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo su tutti i plugin, <c>false</c> altrimenti
        /// </returns>
        public bool RunFromPath( string fileSubpath )
        {
            return RunFromPath( fileSubpath, null, false );
        }

        /// <summary>
        /// Esegue la procedura principale di un plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificato il nome del plugin <paramref name="name"/></exception>
        /// <remarks>Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/></remarks>
        /// <param name="name">Nome del plugin da avviare</param>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo, <c>false</c> altrimenti
        /// </returns>
        public bool Run( string name )
        {
            return Run( name, null, false );
        }

        /// <summary>
        /// Controlla se è stato caricato il plugin specificato
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificato il nome del plugin <paramref name="name"/></exception>
        /// <param name="name">Il nome del plugin da cercare</param>
        /// <returns>
        /// <c>true</c> se il plugin indicato è stato caricato, <c>false</c> altrimenti
        /// </returns>
        public bool IsLoaded(string name)
        {
            if( string.IsNullOrEmpty(name) )
            {
                throw new ArgumentNullException( "name" );
            }

            return this.m_pluginsName.ContainsKey( name );
        }

        /// <summary>
        /// Recupera l'istanza del plugin con il nome specificato
        /// </summary>
        /// <param name="name">Il nome del plugin completo di namespace</param>
        /// <returns>
        /// L'istanza del plugin cercato o null se non presente
        /// </returns>
        public IPlugin GetFromName( string name )
        {
            return this[name];
        }

        /// <summary>
        /// Restituisce la lista dei plugin che appartiene al file specificato
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificato il percorso del file <paramref name="fileSubpath"/></exception>
        /// <remarks>
        /// <para>
        /// <paramref name="fileSubpath"/> è un percorso relativo alla directory di esecuzione del programma, exception può essere il percorso di un
        /// file DLL o di una directory. Per maggiori informazioni vedere <see cref="PluginManager.GetFileList"/>
        /// </para>
        /// </remarks>
        /// <param name="fileSubpath">Nome del plugin da avviare</param>
        /// <returns>La lista dei plugin contenuti nel file specificato</returns>
        public IList<IPlugin> GetFromPath( string fileSubpath )
        {
            if( String.IsNullOrEmpty( fileSubpath ) )
            {
                throw new ArgumentNullException( "fileSubpath" );
            }

            string[] fileList = GetFileList( fileSubpath, SearchOption.TopDirectoryOnly );
            IList<IPlugin> outputList = new List<IPlugin>();

            Array.ForEach( fileList, file => outputList.Union( m_pluginsPath[file] ) );

            return outputList;
        }

        /// <summary>
        /// Restituisce la lista dei plugin che dipendono dall'oggeto indicato
        /// </summary>
        /// <param name="owner">Il proprietario</param>
        /// <returns>La lista dei plugin che dipendono dall'oggetto indicato</returns>
        public IList<IPlugin> GetOwned( IPlugin owner )
        {
            IList<IPlugin> outputList = new List<IPlugin>();

            this.m_pluginsInst.ForEach( plugin =>
            {
                object[] attributes = plugin.GetType().GetCustomAttributes( typeof( PluginOwnersAttribute ), true );
                if( attributes.GetLength( 0 ) > 0 )
                {
                    if( ((PluginOwnersAttribute[])attributes)[0].Contains( owner.GetType().FullName ) )
                    {
                        outputList.Add( plugin );
                    }
                }
            } );

            return outputList;
        }

        /// <summary>
        /// Carica uno specifico plugin da una DLL
        /// </summary>
        /// <exception cref="ArgumentNullException">Il percorso del file da caricare <paramref name="fileSubpath"/> deve essere specificato</exception>
        /// <exception cref="ArgumentNullException">Il nome del plugin da caricare <paramref name="name"/> deve essere specificato</exception>
        /// <remarks>
        /// <para>La ricerca avviene nella directory di esecuzione del programma.</para>
        /// <para>La funzione scarica tutti i plugin precedentemente caricati.</para>
        /// <para>Il caricamento comprende la ricerca dei plugin, l'istanziazione e la chiamata di <see cref="IPlugin.Load"/></para>
        /// </remarks>
        /// <param name="fileSubpath">Il percorso relativo della DLL a partire dalla directory di esecuzione del programma</param>
        /// <param name="name">Il nome del plugin (ovvero il nome della classe che implementa l'interfaccia plugin)</param>
        /// <returns>
        /// <c>true</c> se il plugin è stato caricato con successo, <c>false</c> altrimenti
        /// </returns>
        public bool LoadFromPath( string fileSubpath, string name )
        {
            if( fileSubpath == null )
            {
                throw new ArgumentNullException( "fileSubpath" );
            }
            if( string.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name" );
            }

            try
            {
                bool result = true;

                string[] pluginFiles = this.GetFileList( fileSubpath, SearchOption.TopDirectoryOnly );
                Array.ForEach( pluginFiles, pluginPath =>
                {
                    // Controllo che il file esista
                    if( !File.Exists( pluginPath ) )
                    {
                        result = false;
                    }

                    // Caricamento della DLL
                    Assembly asm = Assembly.LoadFile( pluginPath );
                    if( asm == null )
                    {
                        result = false;
                    }

                    // Eseguo l'effettivo caricamento del plugin
                    result &= LoadFromAssembly( asm, name );
                } );

                return result;
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Generic exception occured while loading plugin file " + fileSubpath, Verbosity.WarningDebug | Verbosity.User );
            }
                
            return false;
        }

        /// <summary>
        /// Esegue la procedura principale di tutti i plugin contenuti in una DLL.
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificato il percorso del file <paramref name="fileSubpath"/></exception>
        /// <remarks>
        /// <para>Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/></para>
        /// <para>
        /// <paramref name="fileSubpath"/> è un percorso relativo alla directory di esecuzione del programma, exception può essere il percorso di un
        /// file DLL o di una directory. Per maggiori informazioni vedere <see cref="PluginManager.GetFileList"/>
        /// </para>
        /// </remarks>
        /// <param name="fileSubpath">Percorso del file in cui cercare i plugin da caricare.</param>
        /// <param name="owner">Il chiamante dell'esecuzione del plugin</param>
        /// <param name="strict">Se impostato a <c>true</c> permette di eseguire solo il plugin dipendente dal <paramref name="owner"/> ignorando i
        /// plugin senza nessun owner</param>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo su tutti i plugin, <c>false</c> altrimenti
        /// </returns>
        public bool RunFromPath( string fileSubpath, IPlugin owner, bool strict )
        {
            if( fileSubpath == null )
            {
                throw new ArgumentNullException( "fileSubpath" );
            }

            bool test = true;

            Array.ForEach( this.GetFileList( fileSubpath, SearchOption.TopDirectoryOnly ), file =>
            {
                if( this.m_pluginsPath.ContainsKey( file ) )
                {
                    // Eseguo tutti i plugin della dll
                    foreach( IPlugin plugin in this.m_pluginsPath[file] )
                    {
                        test &= this.Run( plugin.Name, owner, strict );
                    }
                }
            } );

            return test;
        }

        /// <summary>
        /// Esegue la procedura principale di un plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificato il nome del plugin <paramref name="name"/></exception>
        /// <remarks>Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/></remarks>
        /// <param name="name">Nome del plugin da avviare</param>
        /// <param name="owner">Il chiamante dell'esecuzione del plugin</param>
        /// <param name="strict">Se impostato a <c>true</c> permette di eseguire solo il plugin dipendente dal <paramref name="owner"/> ignorando i
        /// plugin senza nessun owner</param>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo, <c>false</c> altrimenti
        /// </returns>
        public bool Run( string name, IPlugin owner, bool strict )
        {
            if( string.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name" );
            }

            // Controllo che il plugin esista
            if( !this.m_pluginsName.ContainsKey( name ) )
            {
                return false;
            }

            // Recupero il plugin
            IPlugin iip = this.m_pluginsName[name];

            // Controllo le proprietà d'accesso al plugin
            object[] attributes = iip.GetType().GetCustomAttributes( typeof( PluginOwnersAttribute ), true );
            if( attributes.GetLength( 0 ) > 0 )
            {
                if( owner == null )
                {
                    Logger.Default.Write( String.Format( "The plugin {0} has an owner, but no one is specified. I cannot run the plugin.", iip.Name ), Verbosity.InformationDebug | Verbosity.Data );
                    return false;
                }

                if( !((PluginOwnersAttribute[])attributes)[0].Contains( owner.GetType().FullName ) )
                {
                    Logger.Default.Write( String.Format( "The client {0} object is not the owner of the plugin {1}. I cannot run it.", owner.GetType().FullName, iip.Name ), Verbosity.InformationDebug | Verbosity.Data );
                    return false;
                }
            }
            else if( strict )
            {
                Logger.Default.Write( String.Format( "The client {0} object is not the owner of the plugin {1}. I cannot run it.", owner.GetType().FullName, iip.Name ), Verbosity.InformationDebug | Verbosity.Data );
                return false;
            }

            // E' una chiamata a me stesso...
            if( owner == iip )
            {
                return false;
            }

            // Controllo che implementi l'interfaccia
            //if( iip.GetType().GetInterface( typeof( IPluginRunnable ).ToString(), true ) == null )
            if( !iip.ImplementsInterface( typeof(IPluginRunnable) ) )
            {
                return false;
            }

            // Eseguo la procedura
            try
            {
                ((IPluginRunnable)iip).Run( owner );
                Logger.Default.Write(String.Format( "Executed plugin {0}", iip.Name ), Verbosity.InformationDebug | Verbosity.Debug );
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, String.Format( "Generic exception occured while running plugin {0}", iip.Name ), Verbosity.WarningDebug | Verbosity.User );
            }

            return true;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}