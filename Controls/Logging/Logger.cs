using System;
using System.Collections.Generic;
using IndianaPark.Tools.Logging.Writers;

namespace IndianaPark.Tools.Logging
{
    /// <summary>
    /// Scrive messaggi di log da varie sorgenti
    /// </summary>
    /// <remarks>
    /// <para>I messaggi di log sono scritti in base all'oggetto che viene passato alla creazione, mediante il pattern Strategy,
    /// si può quindi usare il Debug oppure visualizzare un MessageBox.</para>
    /// <para>Per ogni fonte principale di messaggi di log dovrebbero essere inseriti almeno due metodi: uno generico ed uno
    /// specifico per il caso d'uso più frequente.</para>
    /// <para>Per velocizzare l'accesso la classe è dotata di un membro statico che permette di accedere ad una istanza con
    /// una configurazione di default.</para>
    /// </remarks>
    public class Logger : ILoggerWriter
    {
		#region Fields 

		#region Class-wise Fields 

        private static Logger mc_default = new Logger(
            new CompositeLogger(
                new DebugLogger(),
                new MBoxLogger() )
        ) { VerbosityMask = Verbosity.All };

		#endregion Class-wise Fields 

		#region Internal Fields 

        /// <summary>
        /// <see cref="ILoggerWriter"/> da utilizzare per scrivere sull'output
        /// </summary>
        private ILoggerWriter m_strategy;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Restituisce un oggetto <see cref="ILoggerWriter"/> su cui viene effettuata la scrittura dei messaggi
        /// </summary>
        public ILoggerWriter Writer
        {
            get
            {
                return m_strategy;
            }
        }

        /// <summary>
        /// Livello di verbosità scelto per il debug
        /// </summary>
        /// <remarks>
        /// I messaggi la cui verbosità non è compresa in questo campo non verranno mai passati ai metodi
        /// di scrittura, si tratta in pratica di un filtro globale.
        /// </remarks>
        public Verbosity VerbosityMask { get; set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="strategy">La strategia implementativa per scrivere sull'output</param>
        /// <exception cref="ArgumentOutOfRangeException">Passando un oggetto <see cref="Logger"/> si crea un riferimento circolare.</exception>
        public Logger( ILoggerWriter strategy )
        {
            if( strategy is Logger == true )
            {
                throw new ArgumentOutOfRangeException( "strategy", "Specifying this kind of object as writer it results in a circular reference" );
            }

            this.m_strategy = strategy;
            this.VerbosityMask = Verbosity.All;
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Recupera l'istanza di default preconfigurata
        /// </summary>
        /// <remarks>
        /// <para>Di default viene utilizzato per la scrittura su output l'oggetto <see cref="Writers.CompositeLogger"/>
        /// inizializzato visualizzando tutti i messaggi che hanno il flag <see cref="Verbosity.User"/> mediante
        /// l'oggetto <see cref="Writers.MBoxLogger"/> e tutti quelli che hanno il flag <see cref="Verbosity.Debug"/>
        /// con <see cref="Writers.DebugLogger"/></para>
        /// <para>Di default non viene effettuato nessun filtraggio sui flag, ovvero tutti i messaggi vengono visualizzati</para>
        /// </remarks>
        /// <returns>L'istanza d idefault di <see cref="Logger"/></returns>
        public static Logger Default
        {
            get { return Logger.mc_default; }
        }

		#endregion Class-wise Methods 

		#region Public Methods 

        #region Messaggi Semplici 

        /// <summary>
        /// Scrive un testo sull'output
        /// </summary>
        /// <param name="message">Un oggetto <see cref="ILogMessage"/> contenente i dati da scrivere</param>
        /// <remarks>
        /// Il testo viene scritto solo se il livello corrente di verbosità di <paramref name="message"/>
        /// è incluso nel livello di verbosità scelto <see cref="VerbosityMask"/>
        /// </remarks>
        public void Write( ILogMessage message )
        {
            if( Convert.ToInt64( message.VerbosityLevel & this.VerbosityMask ) > 0 )
            {
                this.Writer.Write( message );
            }
        }

        /// <summary>
        /// Scrive un testo sull'output
        /// </summary>
        /// <param name="description">Il testo da visualizzare</param>
        /// <param name="level">Opzionale. Livello di verbosità associato al messaggio. Di default vale
        /// <see cref="Verbosity.InformationDebug"/></param>
        public void Write( string description, Verbosity level = Verbosity.InformationDebug )
        {
            var message = new LogMessage( description, level );
            this.Write( message );
        }

        #endregion Messaggi Semplici 

        #region Eccezioni 

        /// <summary>
        /// Scrive le informazioni di una eccezione sull'output
        /// </summary>
        /// <param name="description">Opzionale. La descrizione relativa al messaggio del log. Di default è una stringa vuota</param>
        /// <param name="exception">L'oggetto <see cref="Exception"/> di cui visualizzare le informazioni</param>
        /// <param name="level">Opzionale. Livello di verbosità associato al messaggio. Di default vale <see cref="Verbosity.ErrorDebug"/></param>
        public void Write( Exception exception, string description = "", Verbosity level = Verbosity.ErrorDebug )
        {
            var formatter = ExceptionFormatter.GetFormatter( exception );
            this.Write( new LogMessage( "Exception", description, level ), formatter );
        }

        /// <summary>
        /// Scrive le informazioni di una eccezione sull'output
        /// </summary>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="formatter">Il <see cref="IExceptionFormatter"/> relativo all'eccezione da visualizzare</param>
        /// <remarks>
        /// Il testo viene scritto solo se il livello corrente di verbosità di <see name="message"/>
        /// è incluso nel livello di verbosità scelto <see cref="VerbosityMask"/>
        /// </remarks>
        public void Write( ILogMessage message, IExceptionFormatter formatter )
        {
            if( Convert.ToInt64( message.VerbosityLevel & this.VerbosityMask ) > 0 )
            {
                this.Writer.Write( message, formatter );
            }
        }

        #endregion Eccezioni 

        #region Liste 

        /// <summary>
        /// Scrive il contenuto di una lista sull'output
        /// </summary>
        /// <typeparam name="T">Il tipo di dati contenuto nella lista</typeparam>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="list">La lista da visualizzare</param>
        public void Write<T>( ILogMessage message, IEnumerable<T> list )
        {
            if( Convert.ToInt64( message.VerbosityLevel & this.VerbosityMask ) > 0 )
            {
                this.Writer.Write<T>( message, list );
            }
        }

        /// <summary>
        /// Scrive il contenuto di una lista sull'output
        /// </summary>
        /// <typeparam name="T">Il tipo di dati contenuto nella lista</typeparam>
        /// <param name="list">La lista da visualizzare</param>
        /// <param name="description">La descrizione della lista</param>
        /// <param name="level">Opzionale. Livello di verbosità associato al messaggio. Di default vale
        /// <c><see cref="Verbosity.Data"/> | <see cref="Verbosity.InformationDebug"/></c></param>
        public void Write<T>( IEnumerable<T> list, string description = "", Verbosity level = Verbosity.Data | Verbosity.InformationDebug )
        {
            this.Write( new LogMessage( "Exception", description, level ), list );
        }

        #endregion Liste 

        /// <summary>
        /// Clona l'istanza di <see cref="Logger"/> utilizzando un nuovo <see cref="ILoggerWriter"/>
        /// </summary>
        /// <param name="nuovo">Il nuovo <see cref="ILoggerWriter"/> da usare nel clone</param>
        /// <returns>Un nuovo oggetto <see cref="Logger"/>, con le stesse impostazioni di quello corrente, ma in cui
        /// è stato sostituito un nuovo <see cref="ILoggerWriter"/></returns>
        public Logger Clone( ILoggerWriter nuovo )
        {
            return new Logger( nuovo ) { VerbosityMask = this.VerbosityMask };
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}