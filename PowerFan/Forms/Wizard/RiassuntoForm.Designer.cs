namespace IndianaPark.PowerFan.Forms.New
{
    partial class RiassuntoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( RiassuntoForm ) );
            this.label1 = new System.Windows.Forms.Label();
            this.navigationPanel1 = new IndianaPark.Tools.Controls.NavigationPanel();
            this.lineSeparator1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 10, 11 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 359, 58 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Verranno stampati {0} biglietto/i\r\nper {1}";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // navigationPanel1
            // 
            this.navigationPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.navigationPanel1.CancelEnabled = true;
            this.navigationPanel1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.navigationPanel1.FinishEnabled = true;
            this.navigationPanel1.Location = new System.Drawing.Point( 87, 98 );
            this.navigationPanel1.MaximumSize = new System.Drawing.Size( 282, 41 );
            this.navigationPanel1.MinimumSize = new System.Drawing.Size( 282, 41 );
            this.navigationPanel1.Name = "navigationPanel1";
            this.navigationPanel1.NextEnabled = false;
            this.navigationPanel1.PreviousEnabled = true;
            this.navigationPanel1.Size = new System.Drawing.Size( 282, 41 );
            this.navigationPanel1.TabIndex = 1;
            this.navigationPanel1.Navigate += new IndianaPark.Tools.Navigation.NavigationEvent( this.NavigationHandler );
            // 
            // lineSeparator1
            // 
            this.lineSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lineSeparator1.Location = new System.Drawing.Point( 10, 86 );
            this.lineSeparator1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.lineSeparator1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.lineSeparator1.Name = "lineSeparator1";
            this.lineSeparator1.Size = new System.Drawing.Size( 359, 2 );
            this.lineSeparator1.TabIndex = 2;
            // 
            // RiassuntoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 381, 151 );
            this.Controls.Add( this.lineSeparator1 );
            this.Controls.Add( this.navigationPanel1 );
            this.Controls.Add( this.label1 );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MinimumSize = new System.Drawing.Size( 397, 160 );
            this.Name = "RiassuntoForm";
            this.Text = "RiassuntoForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private IndianaPark.Tools.Controls.NavigationPanel navigationPanel1;
        private IndianaPark.Tools.Controls.LineSeparator lineSeparator1;
    }
}