using System.Drawing;
using System.Windows.Forms;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Linea separatrice.
    /// <para>Reference: http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/0d4b986e-3ed0-4933-a15d-4b42e02005a7</para>
    /// </summary>
    [ ToolboxBitmap( typeof(LineSeparator) , "LineSeparator" ) ]
    public sealed partial class LineSeparator : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineSeparator"/> class.
        /// </summary>
        public LineSeparator()
        {
            InitializeComponent();
            this.Paint += LineSeparatorPaint;
            this.MaximumSize = new Size( 2000, 2 );
            this.MinimumSize = new Size( 0, 2 );
            this.Width = 350;

        }

        /// <summary>
        /// Disegna la linea di separazione
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void LineSeparatorPaint( object sender, PaintEventArgs e )
        {
            Graphics g = e.Graphics;
            g.DrawLine( Pens.DarkGray, new Point( 0, 0 ), new Point( this.Width, 0 ) );
            g.DrawLine( Pens.White, new Point( 0, 1 ), new Point( this.Width, 1 ) );
        }
    }
}
