namespace IndianaPark.PercorsiAvventura.Pannelli
{
    partial class BriefingSelectRow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( BriefingSelectRow ) );
            this.m_posti = new System.Windows.Forms.TextBox();
            this.m_orario = new System.Windows.Forms.TextBox();
            this.m_selected = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // m_posti
            // 
            resources.ApplyResources( this.m_posti, "m_posti" );
            this.m_posti.Name = "m_posti";
            // 
            // m_orario
            // 
            resources.ApplyResources( this.m_orario, "m_orario" );
            this.m_orario.Name = "m_orario";
            // 
            // m_selected
            // 
            resources.ApplyResources( this.m_selected, "m_selected" );
            this.m_selected.Name = "m_selected";
            this.m_selected.UseVisualStyleBackColor = true;
            this.m_selected.CheckedChanged += new System.EventHandler( this.CheckedChangedHandler );
            // 
            // BriefingSelectRow
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_selected );
            this.Controls.Add( this.m_posti );
            this.Controls.Add( this.m_orario );
            this.Name = "BriefingSelectRow";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_posti;
        private System.Windows.Forms.TextBox m_orario;
        private System.Windows.Forms.CheckBox m_selected;
    }
}
