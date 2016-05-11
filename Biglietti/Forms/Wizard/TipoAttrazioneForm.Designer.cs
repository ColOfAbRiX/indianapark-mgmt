namespace IndianaPark.Biglietti.Forms.New
{
    partial class TipoAttrazioneForm
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TipoAttrazioneForm ) );
            this.m_EscapeButton = new System.Windows.Forms.Button();
            this.m_labelTitle = new System.Windows.Forms.Label();
            this.m_line1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.m_buttons = new IndianaPark.Tools.Controls.ControlRepeater();
            this.SuspendLayout();
            // 
            // m_EscapeButton
            // 
            resources.ApplyResources( this.m_EscapeButton, "m_EscapeButton" );
            this.m_EscapeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_EscapeButton.Name = "m_EscapeButton";
            this.m_EscapeButton.UseVisualStyleBackColor = true;
            this.m_EscapeButton.Click += new System.EventHandler( this.EscapeClickHandler );
            // 
            // m_labelTitle
            // 
            resources.ApplyResources( this.m_labelTitle, "m_labelTitle" );
            this.m_labelTitle.ForeColor = System.Drawing.Color.Red;
            this.m_labelTitle.Name = "m_labelTitle";
            // 
            // m_line1
            // 
            resources.ApplyResources( this.m_line1, "m_line1" );
            this.m_line1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.m_line1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.m_line1.Name = "m_line1";
            // 
            // m_buttons
            // 
            resources.ApplyResources( this.m_buttons, "m_buttons" );
            this.m_buttons.Creator = null;
            this.m_buttons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_buttons.Name = "m_buttons";
            this.m_buttons.WrapContent = false;
            this.m_buttons.Resize += new System.EventHandler( this.RepeaterResizeHandler );
            // 
            // TipoAttrazioneForm
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_EscapeButton;
            this.Controls.Add( this.m_buttons );
            this.Controls.Add( this.m_line1 );
            this.Controls.Add( this.m_labelTitle );
            this.Controls.Add( this.m_EscapeButton );
            this.Name = "TipoAttrazioneForm";
            this.Load += new System.EventHandler( this.TipoAttrazioneForm_Load );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_EscapeButton;
        private System.Windows.Forms.Label m_labelTitle;
        private Tools.Controls.LineSeparator m_line1;
        private IndianaPark.Tools.Controls.ControlRepeater m_buttons;
    }
}

