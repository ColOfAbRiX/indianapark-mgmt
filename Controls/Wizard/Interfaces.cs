using IndianaPark.Tools.Navigation;

namespace IndianaPark.Tools.Wizard
{
    /// <summary>
    /// Classe per passare informazioni all'evento <see cref="IState.StatusChangeRequested"/>
    /// </summary>
    public class StatusChangeRequestedEventArgs : System.EventArgs
    {
        /// <summary>
        /// L'azione che viene richiesto di effettuare
        /// </summary>
        public NavigationAction RequestedChange { get; private set; }

        /// <summary>
        /// Lo stato precedente a quello corrente.
        /// </summary>
        /// <remarks>
        /// Ovverosia lo stato che ha richiamato lo stato corrente
        /// </remarks>
        public IState PreviousState { get; private set; }

        /// <summary>
        /// Lo stato successivo a quello corrente.
        /// </summary>
        /// <remarks>
        /// Può essere lo stato al quale si richiede di navigare o anche uno stato in cui si è passati ma poi l'utente
        /// ha richiesto di tornare indietro.
        /// </remarks>
        public IState NextState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusChangeRequestedEventArgs"/> class.
        /// </summary>
        /// <param name="requestedChange">L'azione che viene richiesto di effettuare</param>
        /// <param name="previousState">Lo stato precedente a quello corrente.</param>
        /// <param name="nextState">Lo stato successivo a quello corrente.</param>
        public StatusChangeRequestedEventArgs( NavigationAction requestedChange, IState previousState, IState nextState )
        {
            this.RequestedChange = requestedChange;
            this.PreviousState = previousState;
            this.NextState = nextState;
        }
    }

    /// <summary>
    /// Delegato di gestore di evento della richiesta di cambiamento di stato
    /// </summary>
    public delegate void StatusChangeRequestedHandler(object sender, StatusChangeRequestedEventArgs e);
    
    /// <summary>
    /// Interfaccia di un costruttore di dati per i wizard
    /// </summary>
    /// <remarks>
    /// Il costruttore ha due compiti: deve memorizzare tutti i dati necessari per costruire exception, ovviamente, costruire
    /// ciò per cui viene scritto. Per memorizzare i dati si affida ad un oggetto esterno; la costruzione avviene in
    /// due fasi: la costruzione vera e propria e il recupero del risultato.
    /// </remarks>
    public interface IBuilder : System.IDisposable
    {
        /// <summary>
        /// Il contenitore di dati per il Builder dei giblietti
        /// </summary>
        object Storage { get; }

        /// <summary>
        /// Costruisce e restituisce il risultato della costruzione
        /// </summary>
        /// <returns><c>true</c> se la funziona ha costruito i dati con successo, <c>false</c> altrimenti.</returns>
        bool BuildResult();

        /// <summary>
        /// Restituisce il risultato costruito con <see cref="BuildResult"/>
        /// </summary>
        /// <returns>
        /// L'oggetto costruito, oppure <c>null</c> se <see cref="BuildResult"/> non è mai stata
        /// chiamata o la sua chiamata ha dato come risultato <c>false</c>.
        /// </returns>
        object GetResult();
    }

    /// <summary>
    /// Rappresenta uno stato dell'automa di Mealy del Wizard
    /// </summary>
    /// <remarks>
    /// <para>
    /// E' compito di ogni stato permettere di recuperare lo stato che lo precede e quello che lo segue. Generalmente
    /// lo stato precedente è memorizzato in una variabile interna alla creazione, mentre lo stato successivo è
    /// determinato con un algoritmo ben specifico per ogni stato. Viene così a crearsi una doppia lista linkata, e
    /// quindi la storia complessiva dell'automa può essere determinata solamente avendo a disposizione tutti gli stati
    /// attivati.</para>
    /// <para>
    /// Ogni stato ricorda il suo precedente, ovvero lo stato che lo ha creato e non quello da cui si arriva.
    /// </para>
    /// </remarks>
    public interface IState : System.IDisposable
    {
        /// <summary>
        /// Riferimento all'oggetto <see cref="Wizard"/> che gestisce questo stato
        /// </summary>
        /// <remarks>
        /// Ogni stato ha sempre e solo un proprietario determinato all'atto della creazione dell'istanza. In questa
        /// maniera uno stato può essere usato in diversi <see cref="Wizard"/> ma una specifica istanza solamente per
        /// un <see cref="Wizard"/> ben specifico.
        /// </remarks>
        Wizard Wizard { get; }

        /// <summary>
        /// Avvia la gestione dello stato. Viene chiamato ogni volta che lo stato diventa quello corrente.
        /// </summary>
        /// <remarks>
        /// La procedura standard prevede che questo metodo sia chiamato mentre è ancora attivo il gestore degli eventi
        /// <see cref="StatusChangeRequested"/> dello stato precedente.
        /// </remarks>
        void EnterState( IBuilder builder );

        /// <summary>
        /// Termina l'esecuzione dello stato. Viene chiamato ogni volta che lo stato termina di essere quello corrente
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        void ExitState( IBuilder builder );

        /// <summary>
        /// Avvisa quando lo stato corrente richiede un cambiamento di stato, per esempio a causa di un input utente.
        /// </summary>
        event StatusChangeRequestedHandler StatusChangeRequested;
    }

    /// <summary>Rappresenta una sorgente di dati utente</summary>
    /// <remarks>
    /// Può essere applicata a qualsiasi oggetto dal quale è necessario recuperare dei dati che inserisce un utente.
    /// Un utilizzo tipico è l'implementazione in una Form nella quale l'utente inserisce dei dati, la form si aggiorna
    /// utilizzando i dati inseriti e quando termina i dati possono essere recuperati.
    /// </remarks>
    public interface IUserData
    {
        /// <summary>
        /// Permette di recuperare i dati utente
        /// </summary>
        /// <remarks>
        /// Solitamente i dati utente sono quelli che inserisce l'utente a mano in un qualche oggetto di input.
        /// </remarks>
        object GetData();

        /// <summary>
        /// Indica se i dati sono validi
        /// </summary>
        /// <remarks>
        /// La validità dei dati dipende strettamente dal contesto in cui l'interfaccia si applica. Se i dati hanno
        /// una qualche validità exception possono quindi essere recuperati con <see cref="GetData"/> allora questo campo
        /// dovrebbe valere <c>true</c>.
        /// </remarks>
        /// <value><c>true</c> se i dati sono validi, <c>false</c> altrimenti</value>
        bool IsDataValid { get; }

        /// <summary>
        /// Fornisce le informazioni necessare per aggiornare il suo stato
        /// </summary>
        /// <remarks>
        /// Chi implementa l'interfaccia può avere necessitià di dati esterni per effettuare le proprie elaborazioni,
        /// come ad esempio dati di inizializzazione
        /// </remarks>
        /// <param name="info">Le informazioni di aggiornamento</param>
        void SetInformations( object info );

        /// <summary>
        /// Indica la richiesta da parte del proprietario dei dati di fornirgli dati aggiornati
        /// </summary>
        /// <remarks>
        /// A seguito di azioni dell'utente può essere necessario aggiornare alcuni dati che chi implementa l'interfaccia
        /// utilizza per le sue elaborazioni interne. Questo evento indica la richiesta di fornire dati aggiornati.
        /// </remarks>
        event System.EventHandler UpdateInformations;
    }

    /// <summary>
    /// Interfaccia che specifica i costruttori di <see cref="IState"/>
    /// </summary>
    /// <remarks>
    /// Per la costruzione di oggetti <see cref="IState"/> viene utilizzato il pattern Builder.
    /// </remarks>
    public interface IStateBuilder
    {
        /// <summary>
        /// Utilizzato per impostare lo stato collegato con il nuovo WizardStateBase
        /// </summary>
        IState OriginState { get; set; }

        /// <summary>
        /// Riferimento all'oggetto <see cref="Wizard"/> che gestisce lo stato
        /// </summary>
        Wizard Wizard { get; set; }

        /// <summary>
        /// Crea il nuovo stato del wizard
        /// </summary>
        /// <returns>Il nuovo stato del wizard</returns>
        IState Create();
    }
}
