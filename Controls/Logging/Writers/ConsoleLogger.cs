using System;
using System.Collections.Generic;

namespace IndianaPark.Tools.Logging.Writers
{
    /// <summary>
    /// Scrive i messaggi di log su <see cref="Console"/>
    /// </summary>
    public class ConsoleLogger : ILoggerWriter
    {
        #region Methods 

        #region Public Methods 

        /// <summary>
        /// Scrive il contenuto di una lista sull'output
        /// </summary>
        /// <typeparam name="T">Il tipo di dati contenuto nella lista</typeparam>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="list">La lista da visualizzare</param>
        /// <remarks>
        /// Prima scrive il parametre <paramref name="message"/>, poi scrive ogni elemento della lista
        /// </remarks>
        public void Write<T>( ILogMessage message, IEnumerable<T> list )
        {
            this.Write( message );

            foreach( T item in list )
            {
                this.Write( new LogMessage( item.ToString(), message.VerbosityLevel ) );
            }
        }

        /// <summary>
        /// Scrive le informazioni di una eccezione sull'output
        /// </summary>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="formatter">Il <see cref="IExceptionFormatter"/> relativo all'eccezione da visualizzare</param>
        public void Write( ILogMessage message, IExceptionFormatter formatter )
        {
            this.Write( new LogMessage( message.Title, formatter.ToString(), message.VerbosityLevel ) );
        }

        /// <summary>
        /// Scrive un testo sull'output
        /// </summary>
        /// <param name="message">Un oggetto <see cref="ILogMessage"/> contenente i dati da scrivere</param>
        public void Write( ILogMessage message )
        {
            Console.WriteLine( message.Description );
        }

        #endregion Public Methods 

        #endregion Methods 
    }
}
