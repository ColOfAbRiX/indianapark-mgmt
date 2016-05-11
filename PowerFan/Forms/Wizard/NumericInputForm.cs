using System.Windows.Forms;
using IndianaPark.Tools.Wizard.New;

namespace IndianaPark.PowerFan.Forms.New
{
    /// <summary>
    /// Finestra di dialogo per l'inserimento 
    /// </summary>
    public class NumericInputForm : TextInputForm
    {
        private double m_numericResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericInputForm"/> class.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public NumericInputForm( string title, string text ) : base( title, text )
        {
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
            // Controllo che sia un dato convertibile in double
            if( !double.TryParse( this.m_text.Text, out this.m_numericResult ) )
            {
                MessageBox.Show(
                    "Il valore immesso non è un numero corretto!",
                    "Attenzione!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning );

                return false;
            }

            // Restituisco il controllo base
            return base.CheckData( value );
        }
    }
}
