using System;
using System.Collections.Generic;

namespace IndianaPark.Tools.Logging.Writers
{
    /// <summary>
    /// Scrive il log su più output, sequenzialmente
    /// </summary>
    /// <remarks>
    /// <para>La classe contiene una lista di <see cref="ILoggerWriter"/> che richiama uno alla volta passando lo stesso
    /// messaggio. E' quindi possibile scrivere su più destinazioni lo stesso messaggio.</para>
    /// <para>Utilizza il pattern Decorator per aggiungere funzionalità a <see cref="ILoggerWriter"/> gia esistenti</para>
    /// </remarks>
    public class ChainLogger : ILoggerWriter
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Lista di <see cref="ILoggerWriter"/> utilizzati per scrivere in output i messaggi di log.
        /// </summary>
        public IList<ILoggerWriter> Loggers { get; protected set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainLogger"/> class.
        /// </summary>
        public ChainLogger()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainLogger"/> class.
        /// </summary>
        /// <param name="loggers">La lista di <see cref="ILoggerWriter"/> da gestire</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se viene incluso l'istanza corrente nella lista dei loggers. In questo caso si verificherebbe un riferimento
        /// circolare.
        /// </exception>
        /// <exception cref="ArgumentNullException">Se viene passato una lista di loggers impostata a <c>null</c></exception>
        public ChainLogger( IList<ILoggerWriter> loggers )
        {
            if( loggers == null )
            {
                throw new ArgumentNullException( "loggers", "You must specify a chain of loggers or use another constructor" );
            }

            if( loggers.Contains( this ) )
            {
                throw new ArgumentOutOfRangeException( "loggers", "You cannot include the current instance of this object in the chain of writers" );
            }

            this.Loggers = loggers;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Scrive un testo sull'output
        /// </summary>
        /// <param name="message">Un oggetto <see cref="ILogMessage"/> contenente i dati da scrivere</param>
        public void Write( ILogMessage message )
        {
            foreach( var logger in this.Loggers )
            {
                logger.Write( message );
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
            foreach( var logger in this.Loggers )
            {
                logger.Write<T>( message, list );
            }
        }

        /// <summary>
        /// Scrive le informazioni di una eccezione sull'output
        /// </summary>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="formatter">Il <see cref="IExceptionFormatter"/> relativo all'eccezione da visualizzare</param>
        public void Write( ILogMessage message, IExceptionFormatter formatter )
        {
            foreach( var logger in this.Loggers )
            {
                logger.Write( message, formatter );
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
