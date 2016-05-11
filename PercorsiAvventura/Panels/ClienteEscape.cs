using System;
using System.Windows.Forms;

namespace IndianaPark.PercorsiAvventura.Pannelli
{
    /// <summary>
    /// Pannello per l'inserimento del codice per l'uscita dei clienti
    /// </summary>
    public partial class ClienteEscape : UserControl
    {
        private int m_codiceNominativo;
        private int m_codiceCliente;

        /// <summary>
        /// Avvisa quando è disponibile un codice
        /// </summary>
        public event EventHandler CodiceReady;

        /// <summary>
        /// Il codice completo come inserito dall'utente in formato stringa
        /// </summary>
        public string Codice
        {
            get { return this.m_codiceInput.Text; }
        }

        /// <summary>
        /// Recupera la parte del codice inserito dall'utente relativa al nominativo
        /// </summary>
        public int CodiceNominativo
        {
            get { return this.m_codiceNominativo; }
        }

        /// <summary>
        /// Recupera la parte del codice inserito dall'utente relativa al cliente
        /// </summary>
        public int CodiceCliente
        {
            get { return this.m_codiceCliente; }
        }

        /// <summary>
        /// Resetta il codice
        /// </summary>
        public void ClearCodice()
        {
            this.m_codiceInput.Text = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowPanel"/> class.
        /// </summary>
        public ClienteEscape()
        {
            InitializeComponent();
        }

        private void KeyPressHandler( object sender, KeyPressEventArgs e )
        {
            if( e.KeyChar == (char)Keys.Return )
            {
                e.Handled = true;

                var codNomSize = (int)PluginPercorsi.GetGlobalParameter( "NominativoCodeSize" ).Value;
                var codCliSize = (int)PluginPercorsi.GetGlobalParameter( "ClienteCodeSize" ).Value;
                var codiceInput = this.m_codiceInput.Text.Trim();

                // Controllo che siano delle dimensioni giuste
                if( codiceInput.Length != codNomSize + codCliSize )
                {
                    MessageBox.Show(
                        Properties.Resources.WrongCodeLengthText,
                        Properties.Resources.WrongCodeLengthTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    this.ClearCodice();
                    return;
                }

                // Divido la stringa nelle due componenti
                var codiceNominativo = codiceInput.Substring( 0, codNomSize );
                var codiceCliente = codiceInput.Substring( codCliSize, codNomSize );

                // Controllo che siano dei numeri
                if( !int.TryParse( codiceNominativo, out this.m_codiceNominativo ) || !int.TryParse( codiceCliente, out this.m_codiceCliente ) )
                {
                    MessageBox.Show(
                        Properties.Resources.WrongCodeFormatText,
                        Properties.Resources.WrongCodeFormatTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    this.ClearCodice();
                    return;
                }

                if( this.CodiceReady != null )
                {
                    this.CodiceReady( this, new EventArgs() );
                }
            }
        }
    }
}
