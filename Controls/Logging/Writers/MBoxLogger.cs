using System.Windows.Forms;
using System.Collections.Generic;

namespace IndianaPark.Tools.Logging.Writers
{
    /// <summary>
    /// Visualizza i messaggi di log tramite <see cref="MessageBox"/>
    /// </summary>
    /// <remarks>
    /// La visualizzazione dei <see cref="MessageBox"/> è personalizzata in base al livello di verbosità dei messaggi
    /// visualizzando il box di informazione, di warning o di errore.
    /// </remarks>
    public class MBoxLogger : ILoggerWriter
    {
		#region Methods 

		#region Internal Methods 

        /// <summary>
        /// Trova l'icona da visualizzare nel <see cref="MessageBox"/>
        /// </summary>
        /// <param name="level">Il livello di verbosità di riferimento</param>
        /// <returns>Un enumerativo <see cref="MessageBoxIcon"/> corretto per il livello di verbosità indicato</returns>
        protected MessageBoxIcon GetIcon( Verbosity level )
        {
            var icon = MessageBoxIcon.Information;

            // Il flag di warning ha la precedenza su tutti eccetto su error
            if( (level & Verbosity.Warning) == Verbosity.Warning )
            {
                icon = MessageBoxIcon.Warning;
            }

            // Se è presente il flag di errore ha la precedenza su tutti
            if( (level & Verbosity.Error) == Verbosity.Error )
            {
                icon = MessageBoxIcon.Error;
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
            MessageBox.Show( message.Description, message.Title,
                MessageBoxButtons.OK,
                this.GetIcon( message.VerbosityLevel ) );
        }

        /// <summary>
        /// Scrive il contenuto di una lista sull'output
        /// </summary>
        /// <remarks>
        /// L'implementazione in questa classe non visualizza niente, perchè l'utente NON DEVE MAI essere a conoscenza
        /// dei dati interni del programma, e stampare una lista è palesemente visualizzare dati interni
        /// </remarks>
        /// <typeparam name="T">Il tipo di dati contenuto nella lista</typeparam>
        /// <param name="message">La descrizione relativa all'operazione di output</param>
        /// <param name="list">La lista da visualizzare</param>
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
            MessageBox.Show( formatter.ToString(), message.Title,
                MessageBoxButtons.OK,
                this.GetIcon( message.VerbosityLevel ),
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.ServiceNotification );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
