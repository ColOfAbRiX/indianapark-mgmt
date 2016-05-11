namespace IndianaPark.Biglietti.Pannelli
{
    partial class NewTicketPanel
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
            this.EmissioneBiglietti = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EmissioneBiglietti
            // 
            this.EmissioneBiglietti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmissioneBiglietti.Font = new System.Drawing.Font( "Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.EmissioneBiglietti.Location = new System.Drawing.Point( 8, 23 );
            this.EmissioneBiglietti.Margin = new System.Windows.Forms.Padding( 6 );
            this.EmissioneBiglietti.Name = "EmissioneBiglietti";
            this.EmissioneBiglietti.Size = new System.Drawing.Size( 206, 41 );
            this.EmissioneBiglietti.TabIndex = 2;
            this.EmissioneBiglietti.Text = "Nuovo Biglietto";
            this.EmissioneBiglietti.UseVisualStyleBackColor = true;
            this.EmissioneBiglietti.Click += new System.EventHandler( this.EmissioneBigliettiClickHandler );
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add( this.EmissioneBiglietti );
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.groupBox1.Location = new System.Drawing.Point( 6, 6 );
            this.groupBox1.Margin = new System.Windows.Forms.Padding( 6 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding( 8 );
            this.groupBox1.Size = new System.Drawing.Size( 222, 72 );
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Emissione Biglietti";
            // 
            // MainWindowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.groupBox1 );
            this.Name = "MainWindowPanel";
            this.Padding = new System.Windows.Forms.Padding( 6 );
            this.Size = new System.Drawing.Size( 234, 84 );
            this.groupBox1.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EmissioneBiglietti;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
