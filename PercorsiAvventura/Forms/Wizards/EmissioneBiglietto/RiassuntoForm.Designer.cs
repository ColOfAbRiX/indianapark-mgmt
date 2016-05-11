namespace IndianaPark.PercorsiAvventura.Forms
{
    partial class RiassuntoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( RiassuntoForm ) );
            this.m_lableTitle = new System.Windows.Forms.Label();
            this.m_Navigator = new IndianaPark.Tools.Controls.NavigationPanel();
            this.m_line1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.m_tipoBiglietto = new System.Windows.Forms.Label();
            this.m_nominativo = new System.Windows.Forms.Label();
            this.m_totale = new System.Windows.Forms.Label();
            this.m_repeaterBiglietti = new IndianaPark.Tools.Controls.ControlRepeater();
            this.m_totalePersone = new System.Windows.Forms.Label();
            this.lineSeparator1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.m_repeaterBriefings = new IndianaPark.Tools.Controls.ControlRepeater();
            this.lineSeparator2 = new IndianaPark.Tools.Controls.LineSeparator();
            this.SuspendLayout();
            // 
            // m_lableTitle
            // 
            resources.ApplyResources( this.m_lableTitle, "m_lableTitle" );
            this.m_lableTitle.ForeColor = System.Drawing.Color.Red;
            this.m_lableTitle.Name = "m_lableTitle";
            // 
            // m_Navigator
            // 
            resources.ApplyResources( this.m_Navigator, "m_Navigator" );
            this.m_Navigator.CancelEnabled = true;
            this.m_Navigator.DialogResult = System.Windows.Forms.DialogResult.None;
            this.m_Navigator.FinishEnabled = true;
            this.m_Navigator.MaximumSize = new System.Drawing.Size( 282, 41 );
            this.m_Navigator.MinimumSize = new System.Drawing.Size( 282, 41 );
            this.m_Navigator.Name = "m_Navigator";
            this.m_Navigator.NextEnabled = false;
            this.m_Navigator.PreviousEnabled = true;
            this.m_Navigator.Navigate += new IndianaPark.Tools.Navigation.NavigationEvent( this.NavigationHandler );
            // 
            // m_line1
            // 
            resources.ApplyResources( this.m_line1, "m_line1" );
            this.m_line1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.m_line1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.m_line1.Name = "m_line1";
            // 
            // m_tipoBiglietto
            // 
            resources.ApplyResources( this.m_tipoBiglietto, "m_tipoBiglietto" );
            this.m_tipoBiglietto.Name = "m_tipoBiglietto";
            // 
            // m_nominativo
            // 
            resources.ApplyResources( this.m_nominativo, "m_nominativo" );
            this.m_nominativo.Name = "m_nominativo";
            // 
            // m_totale
            // 
            resources.ApplyResources( this.m_totale, "m_totale" );
            this.m_totale.Name = "m_totale";
            // 
            // m_repeaterBiglietti
            // 
            resources.ApplyResources( this.m_repeaterBiglietti, "m_repeaterBiglietti" );
            this.m_repeaterBiglietti.Creator = null;
            this.m_repeaterBiglietti.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_repeaterBiglietti.Name = "m_repeaterBiglietti";
            this.m_repeaterBiglietti.WrapContent = false;
            // 
            // m_totalePersone
            // 
            resources.ApplyResources( this.m_totalePersone, "m_totalePersone" );
            this.m_totalePersone.Name = "m_totalePersone";
            // 
            // lineSeparator1
            // 
            resources.ApplyResources( this.lineSeparator1, "lineSeparator1" );
            this.lineSeparator1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.lineSeparator1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.lineSeparator1.Name = "lineSeparator1";
            // 
            // m_repeaterBriefings
            // 
            resources.ApplyResources( this.m_repeaterBriefings, "m_repeaterBriefings" );
            this.m_repeaterBriefings.Creator = null;
            this.m_repeaterBriefings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_repeaterBriefings.Name = "m_repeaterBriefings";
            this.m_repeaterBriefings.WrapContent = false;
            // 
            // lineSeparator2
            // 
            resources.ApplyResources( this.lineSeparator2, "lineSeparator2" );
            this.lineSeparator2.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.lineSeparator2.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.lineSeparator2.Name = "lineSeparator2";
            // 
            // RiassuntoForm
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_line1 );
            this.Controls.Add( this.m_repeaterBriefings );
            this.Controls.Add( this.lineSeparator2 );
            this.Controls.Add( this.lineSeparator1 );
            this.Controls.Add( this.m_totalePersone );
            this.Controls.Add( this.m_repeaterBiglietti );
            this.Controls.Add( this.m_totale );
            this.Controls.Add( this.m_nominativo );
            this.Controls.Add( this.m_tipoBiglietto );
            this.Controls.Add( this.m_Navigator );
            this.Controls.Add( this.m_lableTitle );
            this.Name = "RiassuntoForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lableTitle;
        private IndianaPark.Tools.Controls.NavigationPanel m_Navigator;
        private IndianaPark.Tools.Controls.LineSeparator m_line1;
        private System.Windows.Forms.Label m_tipoBiglietto;
        private System.Windows.Forms.Label m_nominativo;
        private System.Windows.Forms.Label m_totale;
        private IndianaPark.Tools.Controls.ControlRepeater m_repeaterBiglietti;
        private System.Windows.Forms.Label m_totalePersone;
        private IndianaPark.Tools.Controls.LineSeparator lineSeparator1;
        private IndianaPark.Tools.Controls.ControlRepeater m_repeaterBriefings;
        private IndianaPark.Tools.Controls.LineSeparator lineSeparator2;
    }
}