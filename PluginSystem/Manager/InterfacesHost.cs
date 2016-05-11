using System;
using System.IO;
using System.Data.Common;
using System.Collections.Generic;

namespace IndianaPark.Plugin
{
    /// <summary>
    /// Interfaccia per permettere la persistenza dei dati
    /// </summary>
    public interface IPersistence
    {
        /// <summary>
        /// Indica se la connessione è stata inizializzata
        /// </summary>
        /// <value>
        /// 	<c>true</c> se la connessione è stata inizializzata <c>false</c>, altrimenti.
        /// </value>
        bool IsConnectionInitialized { get; }

        /// <summary>
        /// Recupera la connessione al database
        /// </summary>
        /// <returns>Un oggetto rappresentante la connessione al database</returns>
        DbConnection GetConnection();

        /// <summary>
        /// Avviene quando viene creata una nuova connessione.
        /// </summary>
        event EventHandler ConnectionChanged;
    }

    /// <summary>
    ///   Interfaccia dei gestori della GUI preposti a presentare l'interazione con l'utente dei plugin
    /// </summary>
    public interface IPluginGraphicHost
    {
        /// <summary>
        ///   Utilizzato per collegare la grafica del plugin specificato all'host grafico del plugin
        /// </summary>
        /// <param name="plugin">Il plugin da aggiungere nella grafica</param>
        void AttachInterface( IPluginViewable plugin );

        /// <summary>
        ///   Utilizzato per rimuovere la grafica del plugin specificato all'host grafico del plugin
        /// </summary>
        /// <param name="plugin">Il plugin da aggiungere nella grafica</param>
        void DetachInterface( IPluginViewable plugin );

        /// <summary>
        /// Permette di ricavare la finestra principale
        /// </summary>
        System.Windows.Forms.Form GetMainForm();
    }

    /// <summary>
    ///   Interfaccia che rappresenta un contenitore di plugin
    /// </summary>
    public interface IPluginHost
    {
        /// <summary>
        ///   Oggetto gestore della GUI deoi plugin. Utilizzato per fare interagire il plugin con l'ambiente esterno.
        /// </summary>
        /// <value>L'host grafico</value>
        IPluginGraphicHost GraphicHost { get; set; }

        /// <summary>
        ///   Oggetto che si occupa della gestione di tutti i plugin, come il cariamento e scaricamento, l'esecuzione, ...
        /// </summary>
        /// <value>Il gestore dei plugin</value>
        IPluginManager Manager { get; set; }

        /// <summary>
        ///   Oggetto che si occupa delle operazioni di base per la persistenza dei dati, come la connessione ad un database
        /// </summary>
        /// <value>Il gestore della persistenza</value>
        IPersistence Persistence { get; set; }

        /// <summary>
        /// Avviene quando il gestore della GUI viene cambiato
        /// </summary>
        event EventHandler GraphicHostChanged;

        /// <summary>
        /// Avviene quando il gestore della persistenza viene cambiato
        /// </summary>
        event EventHandler PersistenceChanged;
    }

    /// <summary>
    ///   Interfaccia dei contenitori dei plugin.
    /// </summary>
    /// <remarks>
    ///   In questa implementazione un gestore di plugin è anche un contenitore di plugin, cosa non ovvia.
    ///   Per questo viene anche implementato IEnumerable, per dare comodità d'uso ai client
    /// </remarks>
    public interface IPluginManager : IPluginHost, IEnumerable<KeyValuePair<string, IPlugin>>
    {
        #region Campi

        /// <summary>
        /// Recupera l'istanza del plugin con il nome specificato
        /// </summary>
        /// <value>Nome del plugin da recuperare</value>
        /// <returns>L'istanza del plugin scelto</returns>
        IPlugin this[string name] { get; }

        /// <summary>
        /// Indica se cercare i plugin in maniera ricorsiva oppure no dalla directory di avvio
        /// </summary>
        SearchOption ScanRecursively { get; set; }

        #endregion

        #region Metodi

#pragma warning disable 1591
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
        int LoadAll( string subPath );

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
        int LoadAll();

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
        bool LoadFromPath( string fileSubpath );

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
        bool LoadFromPath( string fileSubpath, string name );

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
        bool LoadFromName( string name );

        /// <summary>
        /// Scarica tutti i plugin avviati
        /// </summary>
        /// <remarks>
        /// Prima di scaricare il plugin viene chiamato il metodo <see cref="IPlugin.Dispose"/> di ogni plugin.
        /// </remarks>
        /// <returns>
        /// <c>true</c> se tutti i plugin sono stati scaricato, <c>false</c> altrimenti
        /// </returns>
        bool UnloadAll();

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
        bool UnloadFromPath( string fileSubpath );

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
        bool Unload( string name );

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
        bool Unload( IPlugin plugin );

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
        bool RunAll( IPlugin owner );

        /// <summary>
        /// Esegue la procedura principale di tutti i plugin non dipendono da nessun altro plugin
        /// </summary>
        /// <remarks>
        /// Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/>
        /// </remarks>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo su tutti i plugin, <c>false</c> altrimenti
        /// </returns>
        bool RunIndipendents();

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
        bool RunOwneds( IPlugin owner );

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
        bool RunFromPath( string fileSubpath, IPlugin owner, bool strict );

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
        bool RunFromPath( string fileSubpath );

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
        bool Run( string name, IPlugin owner, bool strict );

        /// <summary>
        /// Esegue la procedura principale di un plugin.
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificato il nome del plugin <paramref name="name"/></exception>
        /// <remarks>Il plugin deve implementare l'interfaccia <see cref="IPluginRunnable"/></remarks>
        /// <param name="name">Nome del plugin da avviare</param>
        /// <returns>
        /// <c>true</c> se la procedura <see cref="IPluginRunnable.Run"/> è stata chiamata con successo, <c>false</c> altrimenti
        /// </returns>
        bool Run( string name );

        /// <summary>
        ///   Controlla se il plugin specificato è stato caricato
        /// </summary>
        /// <param name="name">Il nome del plugin da cercare comprensivo del suo Namespace</param>
        /// <returns><c>true</c> se il plugin indicato è stato caricato, <c>false</c> altrimenti</returns>
        bool IsLoaded( string name );

        /// <summary>
        /// Recupera l'istanza del plugin con il nome specificato
        /// </summary>
        /// <param name="name">Il nome del plugin completo di namespace</param>
        /// <returns>
        /// L'istanza del plugin cercato o null se non presente
        /// </returns>
        IPlugin GetFromName( string name );
        
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
        IList<IPlugin> GetFromPath( string fileSubpath );

        /// <summary>
        /// Restituisce la lista dei plugin che dipendono dall'oggeto indicato
        /// </summary>
        /// <param name="owner">Il proprietario</param>
        /// <returns>La lista dei plugin che dipendono dall'oggetto indicato</returns>
        IList<IPlugin> GetOwned( IPlugin owner );

#pragma warning restore

        #endregion
    }
}
