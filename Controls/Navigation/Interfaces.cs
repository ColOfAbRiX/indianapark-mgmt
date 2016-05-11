namespace IndianaPark.Tools.Navigation
{
    /// <summary>
    /// Le possibili operazione di navigazione
    /// </summary>
    /// <remarks>
    /// Per operazioni di navigazione si intendente un'azione come "Avanti", "Prosegui", "Annulla", ...
    /// </remarks>
    public enum NavigationAction
    {
        /// <summary>
        /// Azione di annullamento della navigazione
        /// </summary>
        Cancel,
        /// <summary>
        /// Azione di movimento al passo successivo
        /// </summary>
        Next,
        /// <summary>
        /// Azione di movimento al passo precedente
        /// </summary>
        Back,
        /// <summary>
        /// Azione di termine per fine della navigazione
        /// </summary>
        Finish,
        /// <summary>
        /// Nessuna azione richiesta
        /// </summary>
        Nothing
    }

    /// <summary>
    /// Viene utilizzata per dialogare con i gestori degli eventi di navigazione
    /// </summary>
    public class NavigationEventArgs : System.EventArgs
    {
		#region Fields 

		#region Internal Fields 

        // Mantiene lo stato di navigazione
        private readonly NavigationAction m_nsStatus = NavigationAction.Nothing;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Questa proprietà permette di annullare l'operazione di navigazione che è stata richiesta.
        /// </summary>
        /// <value>
        /// <c>true</c> indica la richiesta di annullare l'azione di navigazione.
        /// </value>
        public bool Cancel { get; set; }

        /// <summary>
        /// Lo stato di navigazione in cui ci si trova o che è stato richiesto all'evento
        /// </summary>
        public NavigationAction Status
        {
            get { return m_nsStatus; }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Costruttore standard
        /// </summary>
        /// <param name="ns">Lo stato di navigazione dal quale parte il Navigatore</param>
        public NavigationEventArgs( NavigationAction ns )
        {
            this.m_nsStatus = ns;
        }

        /// <summary>
        /// Costruttore standard
        /// </summary>
        /// <remarks>
        /// Lo stato di navigazione iniziale è <see cref="NavigationAction.Nothing"/>
        /// </remarks>
        public NavigationEventArgs() : this( NavigationAction.Nothing )
        {
        }

		#endregion Constructors 

		#endregion Methods 
    }

    /// <summary>
    /// Delegato per i gestori degli eventi di navigazione
    /// </summary>
    /// <param name="source">Oggetto da cui parte l'evento</param>
    /// <param name="e">Informazioni sull'azione di navigazione richiesta</param>
    public delegate void NavigationEvent( object source, NavigationEventArgs e );

    /// <summary>
    /// Rappresenta un oggetto che può navigare.
    /// </summary>
    /// <remarks>
    /// Con il termine navigare si intendono le comuni azioni di andare avanti, indietro, annullare l'azione, ...
    /// Qualunque oggetto che permetta di eseguire queste operazioni deve realizzare questa interfaccia
    /// </remarks>
    public interface INavigator
    {
		#region Fields 

        /// <summary>
        /// Lo stato  corrente dell'oggetto navigatore
        /// </summary>
        NavigationAction NavigationStatus { get; }

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Generato quando viene richiesta un'azione di navigazione
        /// </summary>
        event NavigationEvent Navigate;

		#endregion Events 

		#region Methods 

        /// <summary>
        /// Utilizzato per richiedere al navigatore un'azione di navigazione. Per informazioni su cosa e quali sono le azioni
        /// di navigazione vedere <see cref="NavigationAction"/>
        /// </summary>
        /// <remarks>Ad ogni chiamata di questo metodo deve corrispondere lo scatenarsi dell'evento <see cref="Navigate"/></remarks>
        /// <param name="action">Tipo di azione richiesta</param>
        void Sail( NavigationAction action );

		#endregion Methods 
    }
}