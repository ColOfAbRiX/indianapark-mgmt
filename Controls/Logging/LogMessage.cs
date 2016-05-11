using System;

namespace IndianaPark.Tools.Logging
{
    /// <summary>
    /// Un messaggio per la scrittura su log
    /// </summary>
    /// <remarks>
    /// Questo tipo di oggetto contiene tutte le informazioni necessarie per permettere una corretta visualizzazione
    /// del messaggio di log su più output, quindi contiene un eventuale titolo, un livello di verbosità associato, ...
    /// </remarks>
    public class LogMessage : ILogMessage
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Il contenuto del messaggio
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Titolo assegnato al messaggio
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Livello di verbosità assegnato al messaggio
        /// </summary>
        public Verbosity VerbosityLevel { get; private set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="level">The level.</param>
        public LogMessage( string description, Verbosity level ) : this( null, description, level )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="title">Titolo del messaggio</param>
        /// <param name="text">Testo del messaggio. Può essere <c>null</c></param>
        /// <param name="level">Livello di verbosità assegnato al messaggio</param>
        /// <exception cref="ArgumentNullException">Se viene specificato un <paramref name="text"/> <c>null</c></exception>
        public LogMessage( string title, string text, Verbosity level )
        {
            if( string.IsNullOrEmpty( text ) )
            {
                throw new ArgumentNullException( "text", "The message must have a text" );
            }

            this.Title = title;
            this.Description = text;
            this.VerbosityLevel = level;
        }

		#endregion Constructors 

		#endregion Methods 
    }
}
