namespace IndianaPark.PercorsiAvventura.Pannelli
{
    partial class DescriptionValueRow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( DescriptionValueRow ) );
            this.m_description = new System.Windows.Forms.Label();
            this.m_value = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_description
            // 
            resources.ApplyResources( this.m_description, "m_description" );
            this.m_description.Name = "m_description";
            // 
            // m_value
            // 
            resources.ApplyResources( this.m_value, "m_value" );
            this.m_value.Name = "m_value";
            // 
            // DescriptionValueRow
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_value );
            this.Controls.Add( this.m_description );
            this.MinimumSize = new System.Drawing.Size( 0, 20 );
            this.Name = "DescriptionValueRow";
            this.Resize += new System.EventHandler( this.RowResizeHandler );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_description;
        private System.Windows.Forms.TextBox m_value;
    }
}
