using System;
using System.Reflection;

namespace IndianaPark.Tools.Logging.Formatters
{
    /// <summary>
    /// Classe utilizzata per ottenere infomazioni formattate sulle eccezioni di tipo <see cref="ReflectionTypeLoadExceptionFormatter"/>
    /// </summary>
    public class ReflectionTypeLoadExceptionFormatter : ExceptionFormatter
    {
        #region Fields 

        #region Public Fields 

        /// <summary>
        /// L'eccezione gestita dall'ExceptionFormatter
        /// </summary>
        public new ReflectionTypeLoadException Exception
        {
            get { return (ReflectionTypeLoadException)base.m_exception; }
            set { base.m_exception = value; }
        }

        #endregion Public Fields 

        #endregion Fields 

        #region Methods 

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFormatter"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        internal ReflectionTypeLoadExceptionFormatter( ReflectionTypeLoadException exception ) : base( exception )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionTypeLoadExceptionFormatter"></see> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Se non si specifica una eccezione da gestire</exception>
        /// <param name="exception">The exception</param>
        internal ReflectionTypeLoadExceptionFormatter( Exception exception ) : this( (ReflectionTypeLoadException)exception )
        {
        }
         

        #endregion Constructors 

        #region Public Methods 

        /// <summary>
        /// Una stringa con l'output formattato
        /// </summary>
        /// <returns>
        /// Una stringa con l'output formattato con uno stile di default secondo la tipologia di eccezione
        /// </returns>
        public override string ToString()
        {
            var output = "\nInner Loader Exceptions:";

            foreach( var le in this.Exception.LoaderExceptions )
            {
                var lew = ExceptionFormatter.GetFormatter( le );

                output = String.Concat( output, "\n", lew );
                output = String.Concat( output, "\n------------------------------" );
            }

            output = String.Concat( base.ToString(), output );
            return output;
        }

        #endregion Public Methods 

        #endregion Methods 
    }
}