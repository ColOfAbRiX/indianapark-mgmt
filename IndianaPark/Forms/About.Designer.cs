namespace IndianaPark.Forms
{
    partial class About
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( About ) );
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = global::IndianaPark.Properties.Resources.LogoAbout;
            resources.ApplyResources( this.logoPictureBox, "logoPictureBox" );
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.TabStop = false;
            // 
            // labelProductName
            // 
            resources.ApplyResources( this.labelProductName, "labelProductName" );
            this.labelProductName.ForeColor = System.Drawing.Color.FromArgb( ((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))) );
            this.labelProductName.Name = "labelProductName";
            // 
            // labelVersion
            // 
            resources.ApplyResources( this.labelVersion, "labelVersion" );
            this.labelVersion.MaximumSize = new System.Drawing.Size( 0, 17 );
            this.labelVersion.Name = "labelVersion";
            // 
            // labelCopyright
            // 
            resources.ApplyResources( this.labelCopyright, "labelCopyright" );
            this.labelCopyright.Name = "labelCopyright";
            // 
            // textBoxDescription
            // 
            resources.ApplyResources( this.textBoxDescription, "textBoxDescription" );
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.TabStop = false;
            // 
            // labelCompanyName
            // 
            resources.ApplyResources( this.labelCompanyName, "labelCompanyName" );
            this.labelCompanyName.Name = "labelCompanyName";
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources( this.buttonOk, "buttonOk" );
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler( this.buttonOk_Click );
            // 
            // About
            // 
            this.AcceptButton = this.buttonOk;
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.Controls.Add( this.buttonOk );
            this.Controls.Add( this.labelCompanyName );
            this.Controls.Add( this.textBoxDescription );
            this.Controls.Add( this.labelCopyright );
            this.Controls.Add( this.labelVersion );
            this.Controls.Add( this.labelProductName );
            this.Controls.Add( this.logoPictureBox );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.Button buttonOk;
    }
}
