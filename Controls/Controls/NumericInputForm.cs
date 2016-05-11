using System.Windows.Forms;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Finestra di dialogo per l'inserimento di dati di tipo numerico
    /// </summary>
    public sealed class NumericInputForm : InputForm
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Non viene utilizzata per restituire il valore numerico
        /// </summary>>
        private double m_numericResult;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericInputForm"/> class.
        /// </summary>
        /// <param name="title">Titolo della finestra</param>
        /// <param name="text">Testo descrittivo per l'utente</param>
        public NumericInputForm( string title, string text ) : base( title, text )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericInputForm"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="value">The value.</param>
        public NumericInputForm( string title, string text, string value ) : base( title, text, value )
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
                IndianaPark.Tools.Properties.Resources.NumericInputFormWrongInputText,
                IndianaPark.Tools.Properties.Resources.NumericInputFormWrongInputTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
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
            if( string.IsNullOrWhiteSpace( value ) )
            {
                return false;
            }

            // Controllo che sia un dato convertibile in double
            if( !double.TryParse( value, out this.m_numericResult ) )
            {
                return false;
            }

            return true;
        }

		#endregion Internal Methods 

		#endregion Methods 
    }
}
