using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IndianaPark.Tools.Logging.Writers
{
    /// <summary>
    /// Scrive i messaggi di log sul registro eventi di sistema
    /// </summary>
    /// <remarks>
    /// Questa classe utilizza il Registro Eventi di Sistema per registrare un evento
    /// </remarks>
    public class OSEventLogger : ILoggerWriter
    {
		#region Fields 

		#region Class-wise Fields 

        private static string ms_EventSource = Process.GetCurrentProcess().ProcessName;

		#endregion Class-wise Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="OSEventLogger"/> class.
        /// </summary>
        public OSEventLogger()
        {
            if( !EventLog.SourceExists( OSEventLogger.ms_EventSource ) )
            {
                EventLog.CreateEventSource( OSEventLogger.ms_EventSource, "Application" );
            }
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Trova l'icona da visualizzare nel <see cref="MessageBox"/>
        /// </summary>
        /// <param name="level">Il livello di verbosità di riferimento</param>
        /// <returns>Un enumerativo <see cref="MessageBoxIcon"/> corretto per il livello di verbosità indicato</returns>
        protected EventLogEntryType GetIcon( Verbosity level )
        {
            var icon = EventLogEntryType.Information;

            // Il flag di warning ha la precedenza su tutti eccetto su error
            if( (level & Verbosity.Warning) == Verbosity.Warning )
            {
                icon = EventLogEntryType.Warning;
            }

            // Se è presente il flag di errore ha la precedenza su tutti
            if( (level & Verbosity.Error) == Verbosity.Error )
            {
                icon = EventLogEntryType.Error;
            }

            return icon;
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Scrive un testo sull'output
        /// </summary>
        /// <param name="message">Un oggetto <see cref="ILogMessage"/> contenente i dati da scrivere</param>
        public void Write( ILogMessage message )
        {
            EventLog.WriteEntry(
                OSEventLogger.ms_EventSource,
                message.Description,
                this.GetIcon( message.VerbosityLevel ) );
        }

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
            return;
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

		#endregion Public Methods 

		#endregion Methods 
    }
}
