using System;
using System.ComponentModel;
using System.Diagnostics;

namespace IndianaPark.Tools.Logging.Formatters
{
    /// <summary>
    /// Classe utilizzata per ottenere infomazioni formattate sulle eccezioni di tipo <see cref="LicenseException"/>
    /// </summary>
    public class LicenseExceptionFormatter : ExceptionFormatter
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// L'eccezione gestita dall'ExceptionFormatter
        /// </summary>
        public new LicenseException Exception
        {
            get { return (LicenseException)base.m_exception; }
            set { base.m_exception = value; }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseExceptionFormatter"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        internal LicenseExceptionFormatter( LicenseException exception ) : base( exception )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseExceptionFormatter"></see> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Se non si specifica una eccezione da gestire</exception>
        /// <param name="exception">The exception</param>
        internal LicenseExceptionFormatter( Exception exception ) : this( (LicenseException)exception )
        {
        }
         

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Il nome del file che ha causato l'eccezione
        /// </summary>
        /// <returns>
        /// Una stringa contente il nome del file che ha causato l'eccezione
        /// </returns>
        public override string GetSource()
        {
            // Informazioni solo sul punto in cui è stata sollevata l'eccezione
            var st = new StackTrace( this.m_exception, true );
            var frame = st.GetFrame( 0 );
            return frame.GetFileName();
        }

        /// <summary>
        /// La Stack Trace formattata
        /// </summary>
        /// <returns>
        /// Una stringa contente la Stack Trace formattata
        /// </returns>
        public override string GetStackTrace()
        {
            // Non serve far vedere lo stack trace per questo tipo di eccezione
            return "";
        }

        /// <summary>
        /// Il messaggio contenuto nell'eccezione
        /// </summary>
        /// <returns>
        /// Una stringa contente il messaggio contenuto nell'eccezione
        /// </returns>
        public override string GetMessage()
        {
            return "There are some errors loading, verifying or validating your licence";
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format( "{0}\n\n{1}", this.GetMessage(), this.m_exception.Message );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}