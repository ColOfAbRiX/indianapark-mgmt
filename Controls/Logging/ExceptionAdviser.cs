using System;
using SystemDebug = System.Diagnostics.Debug;
using System.Windows.Forms;

namespace IndianaPark.Tools.Debug
{
    /// <summary>
    /// Viene utilizzato per creare presentare in output il risultato delle chiamate di debug
    /// </summary>
    public class ExceptionAdviser
    {
        /// <summary>
        /// Il formatter 
        /// </summary>
        /// <value>The formatter.</value>
        public IExceptionFormatter Formatter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionAdviser"/> class.
        /// </summary>
        /// <param name="exform">The exform.</param>
        public ExceptionAdviser( IExceptionFormatter exform )
        {
            if( exform == null )
            {
                throw new ArgumentNullException( "exform" );
            }

            this.Formatter = exform;
        }

        /// <summary>
        /// Avvisa l'utente di una eccezione mediante un message box
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public DialogResult WarnUser( string message, string title )
        {
            message = message + this.Formatter.Exception.Message;
            return MessageBox.Show( message, title, MessageBoxButtons.OK, MessageBoxIcon.Error );
        }

        /// <summary>
        /// Scrive sull'output il risultato dell'eccezione
        /// </summary>
        /// <param name="text">Un testo aggiuntivo da aggiungere prima dei dati sull'eccezione</param>
        public void WriteDebug( string text )
        {
            SystemDebug.WriteLine( "" );
            SystemDebug.WriteLine( ">> BEGIN <<" );
            
            if( !string.IsNullOrEmpty( text ) )
            {
                SystemDebug.WriteLine( text );
            }

            SystemDebug.WriteLine( this.Formatter.ToString().Replace( "\n", Environment.NewLine ) );
            SystemDebug.WriteLine( ">> END <<" );
            SystemDebug.WriteLine( "" );
        }
    }
}
