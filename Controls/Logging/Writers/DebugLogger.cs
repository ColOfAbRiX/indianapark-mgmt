using SysDebug = System.Diagnostics.Debug;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using System.IO;

namespace IndianaPark.Tools.Logging.Writers
{
    /// <summary>
    /// Scrive i messaggi di log su <see cref="System.Diagnostics.Debug"/>
    /// </summary>
    public class DebugLogger : ILoggerWriter, IDisposable
    {
		#region Fields 

		#region Class-wise Fields 

        private static int ms_instanceReferences;
        private static TextWriterTraceListener ms_debugTextListener;

		#endregion Class-wise Fields 

		#region Internal Fields 

        /// <summary>
        /// Il numero massimo di file di log che si possono creare
        /// </summary>
        protected const int MaxCount = 10;
        /// <summary>
        /// Pattern per i nomi dei file di log.
        /// </summary>
        /// <remarks>
        /// Il parametro "{0}" viene sostituito con il numero sequenziale, "{1}" viene sostituito con la data corrente,
        /// "{2}" con l'ora corrente.
        /// </remarks>
        protected const string FileName = @".\debug_{0}.log";
        /// <summary>
        /// Marcatore dell'ora in cui viene scritto un messaggio
        /// </summary>
        /// <remarks>
        /// Il parametro "{0}" viene sostituito con la data corrente, "{1}" con l'ora corrente.
        /// </remarks>
        protected const string LogTimeStamp = "[{1}]: ";

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        public DebugLogger()
        {
            // Quando si crea la prima istanza inizializzo la sessione
            if( DebugLogger.ms_instanceReferences == 0 )
            {
                DebugLogger.SessionInitialize();
            }
            DebugLogger.ms_instanceReferences++;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Quando si arriva a distruggere l'ultima istanza finalizzo la sessione
            DebugLogger.ms_instanceReferences--;
            if( DebugLogger.ms_instanceReferences == 0 )
            {
                DebugLogger.SessionDispose();
            }
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Inizializza la sessione di output su debug
        /// </summary>
        /// <remarks>
        /// Viene aperto lo stream per la scrittura del log su file. I file di log vengono creati in maniera ciclica
        /// con un massimo di <see cref="DebugLogger.MaxCount"/> file. I nome dei file seguono il pattern indicato in
        /// <see cref="DebugLogger.FileName"/>
        /// </remarks>
        protected static void SessionInitialize()
        {
            int find = -1;
            string fileName = "";
            var newest = new DateTime( 0 );

            // Procedura per trovare il primo file mancante o quello utilizzato più di recente.
            // Il nuovo file di debug sarà quello successivo (effettuando anche il wrap around)
            for( int i = 0; i < DebugLogger.MaxCount; i++ )
            {
                fileName = String.Format( DebugLogger.FileName, i, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString() );

                if( File.Exists( fileName ) )
                {
                    // Trovo il file utilizzato più di recente
                    if( File.GetLastWriteTime( fileName ) > newest )
                    {
                        newest = File.GetLastWriteTime( fileName );
                        find = i;
                    }
                }
            }

            find = (find + 1) % DebugLogger.MaxCount;
            fileName = String.Format( DebugLogger.FileName, find, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString() );

            // Impostazione degli stream per la scrittura del log
            Stream debugLog = null;

            try
            {
                debugLog = File.Create( fileName, 256, FileOptions.WriteThrough );
                DebugLogger.ms_debugTextListener = new TextWriterTraceListener( debugLog );
                DebugLogger.ms_debugTextListener.TraceOutputOptions |= TraceOptions.DateTime | TraceOptions.Timestamp;
                Debug.Listeners.Add( DebugLogger.ms_debugTextListener );
                Debug.AutoFlush = true;
            }
            catch( Exception ex )
            {
                // Qui non posso fare altrimenti, visto che DebugLogger non è ancora stato inizializzato!
                Debug.WriteLine( "Error initializing logging listeners: " + ex.Message );
            }
        }

        /// <summary>
        /// Termina la sessione di output su Debug
        /// </summary>
        /// <remarks>
        /// Termina tutti gli oggetti creati durante l'inizializzazione e chiude tutte le risorse.
        /// </remarks>
        protected static void SessionDispose()
        {
            // Chiusura degli stream per il log dei debug.
            try
            {
                if( DebugLogger.ms_debugTextListener == null )
                {
                    return;
                }

                Debug.Flush();
                Debug.Listeners.Remove( DebugLogger.ms_debugTextListener );
                DebugLogger.ms_debugTextListener.Close();
                DebugLogger.ms_debugTextListener.Dispose();
            }
            catch( Exception ex )
            {
                // Qui non posso fare altrimenti, visto che la terminazione di DebugLogger non ha avuto buon fine!
                Debug.WriteLine( "Error closing logging listeners: " + ex.Message );
            }
        }

		#endregion Class-wise Methods 

		#region Public Methods 

        /// <summary>
        /// Scrive un testo sull'output
        /// </summary>
        /// <param name="message">Un oggetto <see cref="ILogMessage"/> contenente i dati da scrivere</param>
        public void Write( ILogMessage message )
        {
            // Viene scritta l'ora in cui è stampato il messaggio
            var time = string.Format(
                DebugLogger.LogTimeStamp,
                DateTime.Now.ToShortDateString(),
                DateTime.Now.ToLongTimeString()
            );

            // Inserisco un'indentatura pari alla lunghezza dell'orario in tutte le nuove righe, tranne la prima
            var spacing = new string( ' ', time.Length );
            var indented = new Regex( "\n", RegexOptions.Multiline ).Replace( message.Description, "\n" + spacing );

            if( !string.IsNullOrWhiteSpace( message.Title ) )
            {
                // Formattazione con il titolo
                SysDebug.WriteLine( time + message.Title );
                SysDebug.WriteLine( spacing + indented );
            }
            else
            {
                // Formattazione senza titolo
                SysDebug.WriteLine( time + indented );
            }

            SysDebug.WriteLine( "" );

            // Forzo la scrittura, per cercare di poter leggere il file anche se è ancora in uso
            SysDebug.Flush();
            foreach( TraceListener tl in SysDebug.Listeners )
            {
                tl.Flush();
            }
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
            message = new LogMessage( message.Description, formatter.ToString(), message.VerbosityLevel );
            this.Write( message );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
