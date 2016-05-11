using System;
using System.Collections.Generic;

namespace IndianaPark.Tools.Logging
{
    /// <summary>
    /// Enumerazione per la visualizzazione dei messaggi di backup
    /// </summary>
    [Flags]
    public enum Verbosity : byte
    {
        /// <summary>
        /// Nessuna visualizzazione
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Livello informativo
        /// </summary>
        /// <remarks>
        /// Un qualsiasi messaggio che dà informazioni sull'esecuzione del programma
        /// </remarks>
        Information = 0x01,
        /// <summary>
        /// Messaggio di attenzione
        /// </summary>
        /// <remarks>
        /// In generale un errore ma non critico da compromettere l'esecuzione del programma
        /// </remarks>
        Warning = 0x02,
        /// <summary>
        /// Messaggio di errore o eccezione
        /// </summary>
        /// <remarks>
        /// In generale un errore critico, che può compromettere l'esecuzione del programma
        /// </remarks>
        Error = 0x04,
        /// <summary>
        /// Serie di dati
        /// </summary>
        /// <remarks>
        /// Qualsiasi dato utilizzato dal programma, codici, nomi, date, liste, ...
        /// </remarks>
        Data = 0x08,

        /// <summary>
        /// Indica che è una informazione da fornire obbligatoriamente all'utente
        /// </summary>
        User = 0x10,
        /// <summary>
        /// Indica che è una informazione da fornire obbligatoriamente al debug
        /// </summary>
        Debug = 0x20,

        /// <summary>
        /// Messaggi più generici da scrivere sempre su debug
        /// </summary>
        InformationDebug = Information | Debug,
        /// <summary>
        /// Dettagli dei messaggi più generici da scrivere sempre su debug
        /// </summary>
        WarningDebug = Warning | Debug,
        /// <summary>
        ///  Errore o eccezione da scrivere sempre su debug
        /// </summary>
        ErrorDebug = Error | Debug,
        /// <summary>
        /// Informazioni rilevanti per l'utente
        /// </summary>
        UserRelevants = Information | Error | User,
        /// <summary>
        /// Informazioni rilevanti
        /// </summary>
        Relevants = WarningDebug | ErrorDebug | Debug,
        /// <summary>
        /// Tutti i livelli
        /// </summary>
        All = Information | Warning | Error | Data | User | Debug
    }
    
    /// <summary>
    /// Un writer per messaggi di log
    /// </summary>
    /// <remarks>
    /// <para>Un writer si occupa di trasferire in un output i dati da visualizzare, quindi ci potrà essere un writer
    /// che fa uso di <see cref="System.Diagnostics.Debug"></see>, uno di <see cref="Console"/>, uno che avvisa
    /// l'utente con un <see cref="System.Windows.Forms.MessageBox"/> e così via.</para>
    /// <para>Ogni particolare implementazione utilizzerà i dati passati ai metodi nel modo che ritiene più utile, e non
    /// necessariamente tutti i dati.</para>
    /// </remarks>
    public interface ILoggerWriter
    {
        /// <summary>
        /// Scrive un testo sull'output
        /// </summary>
        /// <param name="message">Un oggetto <see cref="ILogMessage"/> contenente i dati da scrivere</param>
        void Write( ILogMessage message );

        /// <summary>
        /// Scrive il contenuto di una lista sull'output
        /// </summary>
        /// <typeparam name="T">Il tipo di dati contenuto nella lista</typeparam>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="list">La lista da visualizzare</param>
        void Write<T>( ILogMessage message, IEnumerable<T> list );

        /// <summary>
        /// Scrive le informazioni di una eccezione sull'output
        /// </summary>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="formatter">Il <see cref="IExceptionFormatter"/> relativo all'eccezione da visualizzare</param>
        void Write( ILogMessage message, IExceptionFormatter formatter );
    }

    /// <summary>
    /// Rappresenta un testo da visualizzare nel log
    /// </summary>
    public interface ILogMessage
    {
        /// <summary>
        /// Titolo assegnato al messaggio
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Il contenuto del messaggio
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Livello di verbosità assegnato al messaggio
        /// </summary>
        Verbosity VerbosityLevel { get; }
    }

    /// <summary>
    /// Interfaccia utilizzata per ottenere infomazioni formattate sulle eccezioni
    /// </summary>
    public interface IExceptionFormatter
    {
        /// <summary>
        /// L'eccezione gestita dall'ExceptionFormatter
        /// </summary>
        /// <value>The exception.</value>
        Exception Exception { get; }

        /// <summary>
        /// Crea un ExceptionFormatter per l'ultima Exception innestata che trova.
        /// </summary>
        /// <remarks>
        /// Le eccezioni possono contenere un riferimento ad una eccezione più specifica con molti livelli annidati.
        /// Il metodo restituisce l'ultima <see cref="System.Exception.InnerException"/> annidata.
        /// </remarks>
        IExceptionFormatter GetLast();

        /// <summary>
        /// Il nome del file che ha causato l'eccezione
        /// </summary>
        /// <returns>Una stringa contente il nome del file che ha causato l'eccezione</returns>
        string GetSource();

        /// <summary>
        /// Il messaggio contenuto nell'eccezione
        /// </summary>
        /// <returns>Una stringa contente il messaggio contenuto nell'eccezione</returns>
        string GetMessage();

        /// <summary>
        /// La Stack Trace formattata
        /// </summary>
        /// <returns>Una stringa contente la Stack Trace formattata</returns>
        string GetStackTrace();

        /// <summary>
        /// Una stringa con l'output formattato
        /// </summary>
        /// <remarks>
        /// La formattazione deve essere generica e completa, se si ha bisogno di una formattazione diversa per il tipo
        /// di output è necessario fare uso degli altri metodi per recuperare i dati sull'eccezione.</remarks>
        /// <returns>
        /// Una stringa con l'output formattato con uno stile di default secondo la tipologia di eccezione
        /// </returns>
        string ToString();
    }
}
