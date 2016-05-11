namespace IndianaPark.Tools.Controls
{
    partial class ControlRepeater
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
            this.m_FlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // m_FlowPanel
            // 
            this.m_FlowPanel.AutoSize = true;
            this.m_FlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_FlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_FlowPanel.Location = new System.Drawing.Point( 0, 0 );
            this.m_FlowPanel.Margin = new System.Windows.Forms.Padding( 0 );
            this.m_FlowPanel.Name = "m_FlowPanel";
            this.m_FlowPanel.Size = new System.Drawing.Size( 304, 66 );
            this.m_FlowPanel.TabIndex = 0;
            this.m_FlowPanel.WrapContents = false;
            // 
            // NewRepeater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            base.Controls.Add( this.m_FlowPanel );
            this.Name = "NewRepeater";
            this.Size = new System.Drawing.Size( 304, 66 );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel m_FlowPanel;
    }
}
