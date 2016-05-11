namespace IndianaPark.PercorsiAvventura.Pannelli
{
    partial class MainWindowPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainWindowPanel ) );
            this.statistiche1 = new IndianaPark.PercorsiAvventura.Pannelli.StatisticPanel();
            this.clienteEscape1 = new IndianaPark.PercorsiAvventura.Pannelli.ClienteEscape();
            this.SuspendLayout();
            // 
            // statistiche1
            // 
            resources.ApplyResources( this.statistiche1, "statistiche1" );
            this.statistiche1.Name = "statistiche1";
            // 
            // clienteEscape1
            // 
            resources.ApplyResources( this.clienteEscape1, "clienteEscape1" );
            this.clienteEscape1.MaximumSize = new System.Drawing.Size( 228, 72 );
            this.clienteEscape1.MinimumSize = new System.Drawing.Size( 228, 72 );
            this.clienteEscape1.Name = "clienteEscape1";
            // 
            // MainWindowPanel
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.statistiche1 );
            this.Controls.Add( this.clienteEscape1 );
            this.Name = "MainWindowPanel";
            this.ResumeLayout( false );

        }

        #endregion

        private ClienteEscape clienteEscape1;
        private StatisticPanel statistiche1;
    }
}
