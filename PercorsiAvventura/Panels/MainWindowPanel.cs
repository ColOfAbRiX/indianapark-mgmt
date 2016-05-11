using System;
using System.Windows.Forms;
using IndianaPark.Biglietti.Pannelli;

namespace IndianaPark.PercorsiAvventura.Pannelli
{
    /// <summary>
    /// Interfaccia principale del plugin
    /// </summary>
    public partial class MainWindowPanel : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowPanel"/> class.
        /// </summary>
        public MainWindowPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Recupera il pannello per l'uscita del cliente
        /// </summary>
        public ClienteEscape GetClienteEscape()
        {
            return this.clienteEscape1;
        }

        /// <summary>
        /// Recupera il pannello con le statistiche
        /// </summary>
        public StatisticPanel GetStatistiche()
        {
            return this.statistiche1;
        }
    }
}
