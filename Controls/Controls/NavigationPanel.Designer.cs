namespace IndianaPark.Tools.Controls
{
    sealed partial class NavigationPanel
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private readonly System.ComponentModel.IContainer m_components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (m_components != null))
            {
                m_components.Dispose();
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
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font( "Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.btnNext.Location = new System.Drawing.Point( 195, 3 );
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size( 84, 35 );
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Avanti";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler( this.NextClickHandler );
            // 
            // btnPrevious
            // 
            this.btnPrevious.Font = new System.Drawing.Font( "Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.btnPrevious.Location = new System.Drawing.Point( 105, 3 );
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size( 84, 35 );
            this.btnPrevious.TabIndex = 2;
            this.btnPrevious.Text = "Indietro";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler( this.PreviousClickHandler );
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font( "Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.btnCancel.Location = new System.Drawing.Point( 3, 3 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 84, 35 );
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Esci";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler( this.CancelClickHandler );
            // 
            // btnFinish
            // 
            this.btnFinish.Font = new System.Drawing.Font( "Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.btnFinish.Location = new System.Drawing.Point( 195, 3 );
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size( 84, 35 );
            this.btnFinish.TabIndex = 0;
            this.btnFinish.Text = "Fine";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Visible = false;
            this.btnFinish.Click += new System.EventHandler( this.FinishClickHandler );
            // 
            // NavigationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.btnFinish );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnPrevious );
            this.Controls.Add( this.btnNext );
            this.MaximumSize = new System.Drawing.Size( 282, 41 );
            this.MinimumSize = new System.Drawing.Size( 282, 41 );
            this.Name = "NavigationPanel";
            this.Size = new System.Drawing.Size( 282, 41 );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFinish;
    }
}
