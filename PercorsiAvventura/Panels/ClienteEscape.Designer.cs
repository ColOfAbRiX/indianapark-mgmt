namespace IndianaPark.PercorsiAvventura.Pannelli
{
    partial class ClienteEscape
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ClienteEscape ) );
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_codiceInput = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources( this.groupBox1, "groupBox1" );
            this.groupBox1.Controls.Add( this.m_codiceInput );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_codiceInput
            // 
            resources.ApplyResources( this.m_codiceInput, "m_codiceInput" );
            this.m_codiceInput.Name = "m_codiceInput";
            this.m_codiceInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.KeyPressHandler );
            // 
            // ClienteEscape
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.groupBox1 );
            this.MaximumSize = new System.Drawing.Size( 228, 72 );
            this.MinimumSize = new System.Drawing.Size( 228, 72 );
            this.Name = "ClienteEscape";
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox m_codiceInput;
    }
}
