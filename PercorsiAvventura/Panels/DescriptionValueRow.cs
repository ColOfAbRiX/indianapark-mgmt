using System;
using System.Drawing;
using System.Windows.Forms;

namespace IndianaPark.PercorsiAvventura.Pannelli
{
    /// <summary>
    /// Riga di visualizzazione di una coppia Descrizione-Valora
    /// </summary>
    public partial class DescriptionValueRow : UserControl
    {
        private double m_descSize = 0.65238095238095238095238095238095;
        private double m_valueSize = 0.31904761904761904761904761904762;

        /// <summary>
        /// Testo di Descrizione
        /// </summary>
        public string Description
        {
            get { return this.m_description.Text; }
            set { this.m_description.Text = value; }
        }

        /// <summary>
        /// Valore da visualizzare
        /// </summary>
        public string Value
        {
            get { return this.m_value.Text; }
            set { this.m_value.Text = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionValueRow"/> class.
        /// </summary>
        public DescriptionValueRow()
        {
            InitializeComponent();
        }

        private void RowResizeHandler( object sender, System.EventArgs e )
        {
            this.m_description.Size = new Size( (int)Math.Round( this.Size.Width * m_descSize ), m_description.Size.Height );
            this.m_value.Size = new Size( (int)Math.Round( this.Size.Width * m_valueSize ), m_value.Size.Height );
        }
    }
}
