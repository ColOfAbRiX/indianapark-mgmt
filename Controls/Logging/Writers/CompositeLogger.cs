using System.Collections.Generic;
using System;

namespace IndianaPark.Tools.Logging.Writers
{
    /// <summary>
    /// Scrive i messaggi di log su diversi output
    /// </summary>
    /// <remarks>
    /// <para>La classe utilizza un approccio intelligente filtrando i vari tipi di livelli <see cref="Verbosity"/>. In 
    /// particolare fà distinzione tra i messaggi da visualizzare all'utente e quelli da scrivere su log.</para>
    /// <para>Se le scritture hanno almeno un flag <see cref="ILogMessage.VerbosityLevel"/> compreso in <see cref="UserMask"/>
    /// l'output verrà effettuato su <see cref="UserLogger"/> e se ne hanno almeno uno in <see cref="TracingMask"/>
    /// verrà utilizzato <see cref="TracingLogger"/>. Notare che è possibile che le scritture vengano effettuati su
    /// entrambi gli oggetti o anche su nessuno.</para>
    /// <para>Le maschere qui impostate NON hanno la precedenza sulla maschera <see cref="Logger.VerbosityMask"/></para>
    /// </remarks>
    public class CompositeLogger : ILoggerWriter
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Oggetto per la scrittura di debug
        /// </summary>
        public ILoggerWriter TracingLogger { get; private set; }

        /// <summary>
        /// Maschera per la verifica dei flag per i messaggi da scrivere per il debug
        /// </summary>
        /// <remarks>
        /// I messaggi che avranno almeno un bit tra quelli impostati in questa maschera verranno scritti
        /// </remarks>
        public Verbosity TracingMask { get; set; }

        /// <summary>
        /// Oggetto per avvisare l'utente
        /// </summary>
        public ILoggerWriter UserLogger { get; private set; }

        /// <summary>
        /// Maschera per la verifica dei flag per i messaggi da visualizzare all'utente
        /// </summary>
        /// <remarks>
        /// I messaggi che avranno almeno un bit tra quelli impostati in questa maschera verranno scritti
        /// </remarks>
        public Verbosity UserMask { get; set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeLogger"/> class.
        /// </summary>
        /// <param name="tracing">Oggetto utilizzato per la scrittura di debug</param>
        /// <param name="user">Oggetto utilizzato per avvisare l'utente</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se viene incluso l'istanza corrente nella lista dei loggers. In questo caso si verificherebbe un riferimento
        /// circolare.
        /// </exception>
        public CompositeLogger( ILoggerWriter tracing, ILoggerWriter user )
        {
            if( tracing == this )
            {
                throw new ArgumentOutOfRangeException( "tracing", "You cannot include the current instance of this object in the chain of writers" );
            }

            if( user == this )
            {
                throw new ArgumentOutOfRangeException( "user", "You cannot include the current instance of this object in the chain of writers" );
            }

            this.TracingLogger = tracing;
            this.UserLogger = user;

            this.TracingMask = Verbosity.Debug;
            this.UserMask = Verbosity.User;
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Determina se la verbosità è rilevante per l'utente
        /// </summary>
        /// <param name="level">Il livello di verbosità da controllare</param>
        /// <returns>
        /// <c>true</c> se è un messaggio destinato all'utente, <c>false</c> altrimenti.
        /// </returns>
        protected bool IsUserRelevant( Verbosity level )
        {
            return (level & this.UserMask) != Verbosity.None;
        }

        /// <summary>
        /// Determina se la verbosità è rilevante per il debugging
        /// </summary>
        /// <param name="level">Il livello di verbosità da controllare</param>
        /// <returns>
        /// <c>true</c> se è un messaggio destinato al debug, <c>false</c> altrimenti.
        /// </returns>
        protected bool IsTracingRelevant( Verbosity level )
        {
            return (level & ~this.UserMask) != Verbosity.None;
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Scrive un testo sull'output
        /// </summary>
        /// <param name="message">Un oggetto <see cref="ILogMessage"/> contenente i dati da scrivere</param>
        public void Write( ILogMessage message )
        {
            if( this.IsUserRelevant( message.VerbosityLevel ) )
            {
                this.UserLogger.Write( message );
            }

            if( this.IsTracingRelevant(message.VerbosityLevel) )
            {
                this.TracingLogger.Write( message );
            }
        }

        /// <summary>
        /// Scrive il contenuto di una lista sull'output
        /// </summary>
        /// <typeparam name="T">Il tipo di dati contenuto nella lista</typeparam>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="list">La lista da visualizzare</param>
        public void Write<T>( ILogMessage message, IEnumerable<T> list )
        {
            if( this.IsUserRelevant( message.VerbosityLevel ) )
            {
                this.UserLogger.Write( message, list );
            }

            if( this.IsTracingRelevant( message.VerbosityLevel ) )
            {
                this.TracingLogger.Write( message, list );
            }
        }

        /// <summary>
        /// Scrive le informazioni di una eccezione sull'output
        /// </summary>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="formatter">Il <see cref="IExceptionFormatter"/> relativo all'eccezione da visualizzare</param>
        public void Write( ILogMessage message, IExceptionFormatter formatter )
        {
            if( this.IsUserRelevant(message.VerbosityLevel) )
            {
                this.UserLogger.Write( message, formatter );
            }

            if( this.IsTracingRelevant( message.VerbosityLevel ) )
            {
                this.TracingLogger.Write( message, formatter );
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
