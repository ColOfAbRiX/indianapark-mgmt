using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IndianaPark.Plugin.Persistence;

namespace IndianaPark.Plugin
{
    /// <summary>
    ///   Delegato che rappresenta un evento generato dai plugin
    /// </summary>
    /// <param name="sender">L'istanza del plugin che ha sollevato l'evento</param>
    /// <param name="exception">I parametri dell'evento</param>
    public delegate void PluginEventHandler( object sender, EventArgs e );

    /// <summary>
    /// Interfaccia che deve essere implementata da tutti i plugin
    /// </summary>
    public interface IPlugin : IDisposable
    {
        #region Campi

        /// <summary>
        /// Nome del plugin. Deve essere la stringa che contiene il nome della classe del plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Il percorso assoluto della DLL contenente il plugin.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Oggetto contenitore del plugin. Utilizzato per fare interagire il plugin con l'ambiente esterno
        /// </summary>
        IPluginHost Host { get; set; }

        /// <summary>
        /// Insieme dei parametri di configurazione del plugin
        /// </summary>
        Dictionary<string, IConfigValue> Parameters { get; }

        #endregion

        #region Metodi

        /// <summary>
        /// Funzione utilizzata per l'inizializzazione del plugin.
        /// </summary>
        /// <remarks>
        /// Viene chiamata una volta quando il programma si avvia ed il plugin viene caricato, subito dopo il costruttore.
        /// Viene utilizzata per istanziare le variabili, creare le configurazioni, ed operazioni simili.
        /// </remarks>
        /// <returns><c>true</c> se il caricamento ha avuto esito positivo, <c>false</c> altrimenti</returns>
        bool Load();

        /// <summary>
        /// Funzione utilizzata per la chiusura del plugin.
        /// </summary>
        new void Dispose();

        #endregion

        #region Eventi

        /// <summary>
        /// Evento che si scatena quando il plugin ha terminato di caricarsi
        /// </summary>
        event PluginEventHandler PluginLoaded;

        /// <summary>
        /// Evento che si scatena quando il plugin è stato finalizzato per la terminazione
        /// </summary>
        event PluginEventHandler PluginDisposed;

        /// <summary>
        /// Evento che si scatena quando viene cambiato l'host del plugin
        /// </summary>
        event PluginEventHandler HostChanged;

        /// <summary>
        /// Scatenato ogni volta che un valore di configurazione cambia
        /// </summary>
        event PluginEventHandler ConfigChanged;

        #endregion
    }

    /// <summary>
    /// Rappresenta un valore di configurazione
    /// </summary>
    public interface IConfigValue : IEquatable<IConfigValue>
    {
        /// <summary>
        /// Nome del valore di configurazione
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Valore di configurazione
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Il tipo di dato che l'istanza gestisce
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Descrizione del dato di configurazione
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// Indica se la configurazione è di sola lettura
        /// </summary>
        bool IsReadonly { get; }

        /// <summary>
        /// Indica se la configurazione è pubblica, cioè se può essere modificata dagli utente
        /// </summary>
        bool IsPublic { get; }

        /// <summary>
        /// Richiamato quando c'è un accesso in lettura al valore di configurazione
        /// </summary>
        /// <remarks>
        /// L'evento è generato prima che il valore venga restituito
        /// </remarks>
        event EventHandler Reading;

        /// <summary>
        /// Richiamato quando c'è un accesso in scrittura al valore di configurazione.
        /// </summary>
        /// <remarks>
        /// L'evento è generato prima che il vecchio valore venga sovrascritto
        /// </remarks>
        event EventHandler<PluginConfigWriteAccessArgument> Changing;

        /// <summary>
        /// Richiamato quando il valore del parametro è stato modificato
        /// </summary>
        event EventHandler Changed;
    }

    /// <summary>
    /// Indica che un valore di configurazione può essere reso persistente
    /// </summary>
    public interface IConfigPersistent
    {
        /// <summary>
        /// L'oggetto che serve per mantenere la persistenza
        /// </summary>
        IConfigPersistence Persistence { get; set; }

        /// <summary>
        /// Permette di impostare e recuperare il nome del proprietario del valore
        /// </summary>
        string OwnerName { get; set; }

        /// <summary>
        /// Tenta di salvare il parametro dal database
        /// </summary>
        void SaveParameter();
    }

    /// <summary>
    /// Rappresenta un plugin che permette di interagire con l'utente. Più precisamente questa interfaccia permette
    /// ad un oggetto <see cref="IPluginGraphicHost"/> di estrarre dal plugin le sue interfacce grafiche.
    /// </summary>
    public interface IPluginViewable : IPlugin
    {
        /// <summary>
        /// Utilizzata per recuperare un'interfaccia grafica per l'interazione con l'utente. Il controllo utente
        /// recuperato viene, di solito, aggiunto alla finestra principale del programma
        /// </summary>
        /// <returns>Il pannello da aggiungere all'interfaccia grafica. Null in caso non si debba aggiungere niente</returns>
        Control GetCommandPanel();

        /// <summary>
        /// Utilizzata per recuperare un elemento di un menù a tendina da aggiungere al menu principale della GUI.
        /// </summary>
        /// <returns>Il menu da aggiungere. Null in caso non si debba aggiungere niente</returns>
        ToolStripItem GetDropdownMenu();
    }

    /// <summary>
    /// Rappresenta un plugin che dispone di esecuzione differita
    /// </summary>
    public interface IPluginRunnable : IPlugin
    {
        /// <summary>
        /// Avvia le routine di gestione del plugin.
        /// </summary>
        /// <param name="owner">L'oggetto che richiede l'esecuzione del plugin</param>
        /// <remarks>
        /// Il suo scopo è quello di differire l'esecuzione delle routine del plugin dalle procedure di caricamento;
        /// un esempio d'uso è l'impostazione di configurazioni prima dell'avvio del plugin
        /// </remarks>
        void Run( IPlugin owner );

        /// <summary>
        /// Evento che si scatena quando viene chiamata la routine Run() del plugin
        /// </summary>
        event PluginEventHandler PluginRun;
    }
}
