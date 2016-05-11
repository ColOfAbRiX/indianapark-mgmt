using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace IndianaPark.Tools.Logging
{
    /// <summary>
    /// Utilizzata per ottenere infomazioni formattate dalle eccezioni
    /// </summary>
    /// <remarks>
    /// Implementa il pattern Factory Method per la creazione delle istanze e il patter Template Method per la
    /// formattazione dell'output
    /// </remarks>
    public class ExceptionFormatter : IExceptionFormatter
    {
		#region Fields 

		#region Class-wise Fields 

        // Trova tutti i formatter nel namespace dei Formatters. Il metodo statico permette di caricare i dati una volta sola
        private static IList<Type> mc_AllFormatters = (
            from t in Assembly.GetExecutingAssembly().GetTypes()
            where t.IsClass
                && String.Equals(t.Namespace, "IndianaPark.Tools.Logging.Formatters")
                && t.ImplementsInterface( typeof(IExceptionFormatter) )
            select t).ToList();

		#endregion Class-wise Fields 

		#region Internal Fields 

        /// <summary>
        /// L'eccezione gestita dall'istanza di <see cref="ExceptionFormatter"/>
        /// </summary>
        protected Exception m_exception;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Eccezione gestita dalla specifica istanza di <see cref="ExceptionFormatter"/>
        /// </summary>
        public virtual Exception Exception
        {
            get
            {
                return m_exception;
            }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFormatter"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Se non si specifica una eccezione da gestire</exception>
        /// <param name="exception">The exception</param>
        protected ExceptionFormatter( Exception exception )
        {
            if( exception == null )
            {
                throw new ArgumentNullException( "exception,", "You must specify the Exception to be manipulated" );
            }

            this.m_exception = exception;
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Restituisce l'ExceptionFormatter specifico per il tipo di eccezione passata
        /// </summary>
        /// <param name="exception">L'eccezione in cui cercare le eccezioni annidate</param>
        /// <param name="getLast">Se <c>true</c> indica di recuperare l'ultima eccezione</param>
        /// <returns>
        /// Un'oggetto <see cref="IExceptionFormatter"/> inizializzato per gestire l'ultima eccezione annidata
        /// all'interno dell'eccezione specificata
        /// </returns>
        /// <exception cref="ArgumentNullException">Se non si specifica una eccezione da gestire</exception>
        /// <remarks>
        /// <para>Il metodo si occupa di cercare l'oggetto che gestisce il tipo di eccezione passata cercando tra tutti gli
        /// oggetti presenti nel namespace Formatters. Gli oggetti devono essere di tipo <see cref="IExceptionFormatter"/>
        /// e avere un costruttore che accetta il tipo di eccezione che deve gestire (e non un oggetto base).</para>
        /// <para>Per i dettagli sull'uso di <paramref name="getLast"/> vedere <see cref="GetLast"/>.</para>
        /// </remarks>
        public static IExceptionFormatter GetFormatter( Exception exception, bool getLast = true )
        {
            if( exception == null )
            {
                throw new ArgumentNullException( "exception,", "You must specify the Exception to be manipulated" );
            }

            // Cerca un costruttore che possa accettare l'eccezione come unico argomento
            var formatterType = (
                from efs in ExceptionFormatter.mc_AllFormatters
                where efs.GetConstructors( BindingFlags.Instance | BindingFlags.NonPublic ).Any( ctor =>
                    String.Compare( ctor.GetParameters()[0].ParameterType.Name, exception.GetType().Name, true ) == 0
                )
                select efs).FirstOrDefault();

            IExceptionFormatter formatter;

            // Cambio azione se trovo o non trovo Formatter specializzati
            if( formatterType == null || String.Compare( exception.GetType().Name, typeof( Exception ).Name, false ) == 0 )
            {
                formatter = new ExceptionFormatter( exception );
            }
            else
            {
                formatter = Activator.CreateInstance( formatterType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { exception }, null ) as IExceptionFormatter;
            }

            if( getLast )
            {
                formatter = formatter.GetLast();
            }

            return formatter;
        }

		#endregion Class-wise Methods 

		#region Public Methods 

        /// <summary>
        /// Crea un ExceptionFormatter per l'ultima Exception innestata che trova.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Le eccezioni possono contenere un riferimento ad una eccezione più specifica con molti livelli annidati.
        /// Il metodo restituisce l'ultima <see cref="System.Exception.InnerException"/> annidata.
        /// </remarks>
        public IExceptionFormatter GetLast()
        {
            var current = this.m_exception;

            while( current.InnerException != null )
            {
                current = current.InnerException;
            }

            var output = ExceptionFormatter.GetFormatter( current, false );
            return output;
        }

        /// <summary>
        /// Una stringa con l'output formattato
        /// </summary>
        /// <remarks>
        /// La formattazione deve essere generica e completa, se si ha bisogno di una formattazione diversa per il tipo
        /// di output è necessario fare uso degli altri metodi per recuperare i dati sull'eccezione.</remarks>
        /// <returns>
        /// Una stringa con l'output formattato con uno stile di default secondo la tipologia di eccezione
        /// </returns>
        public override string ToString()
        {
            // Scrivo tutte le informazioni che ritengo utili
            var text = String.Format(
                    "Exception: {0}\nSource: {1}\nMessage: {2}\nStack trace:\n{3}",
                    this.Exception.GetType().ToString(),
                    this.GetSource(),
                    this.GetMessage(),
                    this.GetStackTrace() );

            return text;
        }

        /// <summary>
        /// Il nome del file che ha causato l'eccezione
        /// </summary>
        /// <returns>
        /// Una stringa contente il nome del file che ha causato l'eccezione
        /// </returns>
        public virtual string GetSource()
        {
            return this.m_exception.Source;
        }

        /// <summary>
        /// Il messaggio contenuto nell'eccezione
        /// </summary>
        /// <returns>
        /// Una stringa contente il messaggio contenuto nell'eccezione
        /// </returns>
        public virtual string GetMessage()
        {
            return this.m_exception.Message;
        }

        /// <summary>
        /// La Stack Trace formattata
        /// </summary>
        /// <remarks>
        /// Per pulizia vengono accorciati i percorsi completi dei file nei soli nomi, viene tolta la specifica dei
        /// parametri per i metodi e vengono accorciati i namespace più comuni
        /// </remarks>
        /// <returns>
        /// Una stringa contente la Stack Trace formattata
        /// </returns>
        public virtual string GetStackTrace()
        {
            var text = this.m_exception.StackTrace;

            // Accorcio i nomi dei file
            Regex regex = new Regex( @"[A-Z]:\\([^\\\:\^\?]+\\)*[^\\\:\^\?]+(\.[^\\\:\^\?]+|)",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant |
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline );

            foreach( Match match in regex.Matches( text ) )
            {
                text = text.Replace( match.Value, System.IO.Path.GetFileName( match.Value ) );
            }

            // Tolgo la specifica dei parametri per i metodi
            regex = new Regex( @"\((\s*\w+\s*[\*&]{0,1}\s*\w+\s*\,)*\s*\w+\s*[\*&]{0,1}\s*\w+\s*\)",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant |
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline );
            text = regex.Replace( text, "()" );

            // Pulizia dai più comuni Namespace
            text = text.Replace( "System.Windows.Forms.", "frm." );
            text = text.Replace( "System.Windows.", "win." );
            text = text.Replace( "System.", "sys." );
            text = text.Replace( "IndianaPark.Tools.", "tools." );
            text = text.Replace( "IndianaPark.PercorsiAvventura.", "percorsi." );
            text = text.Replace( "IndianaPark.Biglietti.", "biglietti." );
            text = text.Replace( "IndianaPark.Persistence.", "data." );
            text = text.Replace( "IndianaPark.PluginSystem.", "plugin." );
            text = text.Replace( "IndianaPark.PowerFan.", "pf." );

            return text;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
