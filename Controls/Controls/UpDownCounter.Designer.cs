namespace IndianaPark.Tools.Controls
{
    partial class UpDownCounter
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

        #region Codice generato da Progettazione componenti

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpDownCounter));
            this.m_countInput = new System.Windows.Forms.TextBox();
            this.m_addButton = new System.Windows.Forms.Button();
            this.m_subtractButton = new System.Windows.Forms.Button();
            this.tmrRepeater = new System.Windows.Forms.Timer(this.components);
            this.tmrDelay = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // m_countInput
            // 
            resources.ApplyResources(this.m_countInput, "m_countInput");
            this.m_countInput.Name = "m_countInput";
            this.m_countInput.Leave += new System.EventHandler(this.CountClickHandler);
            // 
            // m_addButton
            // 
            resources.ApplyResources(this.m_addButton, "m_addButton");
            this.m_addButton.Name = "m_addButton";
            this.m_addButton.TabStop = false;
            this.m_addButton.UseVisualStyleBackColor = true;
            this.m_addButton.Click += new System.EventHandler(this.AddClickHandler);
            this.m_addButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownHandler);
            this.m_addButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpHandler);
            // 
            // m_subtractButton
            // 
            resources.ApplyResources(this.m_subtractButton, "m_subtractButton");
            this.m_subtractButton.Name = "m_subtractButton";
            this.m_subtractButton.UseVisualStyleBackColor = true;
            this.m_subtractButton.Click += new System.EventHandler(this.SubtractClickHandler);
            this.m_subtractButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownHandler);
            this.m_subtractButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpHandler);
            // 
            // tmrRepeater
            // 
            this.tmrRepeater.Interval = 500;
            this.tmrRepeater.Tick += new System.EventHandler(this.RepeaterTickHandler);
            // 
            // tmrDelay
            // 
            this.tmrDelay.Interval = 500;
            this.tmrDelay.Tick += new System.EventHandler(this.DelayTickHandler);
            // 
            // UpDownCounter
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.m_subtractButton);
            this.Controls.Add(this.m_addButton);
            this.Controls.Add(this.m_countInput);
            this.MaximumSize = new System.Drawing.Size(149, 46);
            this.MinimumSize = new System.Drawing.Size(149, 46);
            this.Name = "UpDownCounter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_countInput;
        private System.Windows.Forms.Button m_addButton;
        private System.Windows.Forms.Button m_subtractButton;
        private System.Windows.Forms.Timer tmrRepeater;
        private System.Windows.Forms.Timer tmrDelay;
    }
}
