using System;
using System.Windows.Forms;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Finestra di dialogo per l'inserimento di dati di tipo testuale
    /// </summary>
    public sealed class TextInputForm : InputForm
    {
		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputForm"/> class.
        /// </summary>
        /// <param name="title">Titolo della finestra</param>
        /// <param name="text">Testo descrittivo per l'utente</param>
        public TextInputForm( string title, string text ) : this( title, text, "" )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputForm"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="value">The value.</param>
        public TextInputForm( string title, string text, string value ) : base( title, text, value )
        {
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Metodo richiamato quando si richiede un'azione di navigazione e i dati non sono validi
        /// </summary>
        protected override void WrongData()
        {
            MessageBox.Show(
                IndianaPark.Tools.Properties.Resources.TextInputFormWrongInputText,
                IndianaPark.Tools.Properties.Resources.TextInputFormWrongInputTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Warning
            );
        }

        /// <summary>
        /// Controlla il formato dei dati dell'input
        /// </summary>
        /// <param name="value">Il valore da controllare</param>
        /// <returns>
        /// 	<c>true</c> se il dato è nel formato corretto e la procedura può proseguire, <c>false</c> altrimenti.
        /// </returns>
        protected override bool CheckData( string value )
        {
            // Controllo se è stato inserito del testo
            if( String.IsNullOrWhiteSpace( value ) )
            {
                return false;
            }

            return true;
        }

		#endregion Internal Methods 

		#endregion Methods 
    }
}
