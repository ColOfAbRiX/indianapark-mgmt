using IndianaPark.Tools;

namespace IndianaPark.PercorsiAvventura.Pannelli
{
    partial class TipoClienteRow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TipoClienteRow ) );
            this.lblTipo = new System.Windows.Forms.Label();
            this.udcContatore = new IndianaPark.Tools.Controls.UpDownCounter();
            this.m_buttonSconto = new System.Windows.Forms.Button();
            this.m_buttonDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTipo
            // 
            resources.ApplyResources( this.lblTipo, "lblTipo" );
            this.lblTipo.Name = "lblTipo";
            // 
            // udcContatore
            // 
            resources.ApplyResources( this.udcContatore, "udcContatore" );
            this.udcContatore.Counter = 0;
            this.udcContatore.Max = 100;
            this.udcContatore.MaximumSize = new System.Drawing.Size( 149, 46 );
            this.udcContatore.Min = 0;
            this.udcContatore.MinimumSize = new System.Drawing.Size( 149, 46 );
            this.udcContatore.Name = "udcContatore";
            this.udcContatore.Step = ((uint)(1u));
            this.udcContatore.Change += new System.EventHandler( this.CounterChangeHandler );
            // 
            // m_buttonSconto
            // 
            resources.ApplyResources( this.m_buttonSconto, "m_buttonSconto" );
            this.m_buttonSconto.Name = "m_buttonSconto";
            this.m_buttonSconto.UseVisualStyleBackColor = true;
            this.m_buttonSconto.Click += new System.EventHandler( this.AddScontoClickHandler );
            // 
            // m_buttonDelete
            // 
            resources.ApplyResources( this.m_buttonDelete, "m_buttonDelete" );
            this.m_buttonDelete.Name = "m_buttonDelete";
            this.m_buttonDelete.UseVisualStyleBackColor = true;
            this.m_buttonDelete.Click += new System.EventHandler( this.RemoveScontoClickHandler );
            // 
            // TipoClienteInputRow
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_buttonDelete );
            this.Controls.Add( this.udcContatore );
            this.Controls.Add( this.m_buttonSconto );
            this.Controls.Add( this.lblTipo );
            this.Name = "TipoClienteInputRow";
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Label lblTipo;
        private Tools.Controls.UpDownCounter udcContatore;
        private System.Windows.Forms.Button m_buttonSconto;
        private System.Windows.Forms.Button m_buttonDelete;
    }
}
