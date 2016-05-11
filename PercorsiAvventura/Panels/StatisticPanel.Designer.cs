namespace IndianaPark.PercorsiAvventura.Pannelli
{
    partial class StatisticPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( StatisticPanel ) );
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_rptWorking = new LocalRepeater();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_rptWholeDay = new LocalRepeater();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_rptInfoBriefings = new LocalRepeater();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.m_timerUpdate = new System.Windows.Forms.Timer( this.components );
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.m_rptNextReturns = new LocalRepeater();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources( this.groupBox1, "groupBox1" );
            this.groupBox1.Controls.Add( this.m_rptWorking );
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_rptWorking
            // 
            resources.ApplyResources( this.m_rptWorking, "m_rptWorking" );
            this.m_rptWorking.Creator = null;
            this.m_rptWorking.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_rptWorking.Name = "m_rptWorking";
            this.m_rptWorking.WrapContent = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources( this.groupBox2, "groupBox2" );
            this.groupBox2.Controls.Add( this.m_rptWholeDay );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_rptWholeDay
            // 
            resources.ApplyResources( this.m_rptWholeDay, "m_rptWholeDay" );
            this.m_rptWholeDay.Creator = null;
            this.m_rptWholeDay.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_rptWholeDay.Name = "m_rptWholeDay";
            this.m_rptWholeDay.WrapContent = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources( this.groupBox3, "groupBox3" );
            this.groupBox3.Controls.Add( this.m_rptInfoBriefings );
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // m_rptInfoBriefings
            // 
            resources.ApplyResources( this.m_rptInfoBriefings, "m_rptInfoBriefings" );
            this.m_rptInfoBriefings.Creator = null;
            this.m_rptInfoBriefings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_rptInfoBriefings.Name = "m_rptInfoBriefings";
            this.m_rptInfoBriefings.WrapContent = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add( this.tableLayoutPanel4 );
            resources.ApplyResources( this.groupBox4, "groupBox4" );
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources( this.tableLayoutPanel4, "tableLayoutPanel4" );
            this.tableLayoutPanel4.Controls.Add( this.label8, 0, 2 );
            this.tableLayoutPanel4.Controls.Add( this.textBox8, 1, 1 );
            this.tableLayoutPanel4.Controls.Add( this.textBox9, 1, 0 );
            this.tableLayoutPanel4.Controls.Add( this.label9, 0, 0 );
            this.tableLayoutPanel4.Controls.Add( this.label10, 0, 1 );
            this.tableLayoutPanel4.Controls.Add( this.textBox10, 1, 2 );
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // label8
            // 
            resources.ApplyResources( this.label8, "label8" );
            this.label8.Name = "label8";
            // 
            // textBox8
            // 
            resources.ApplyResources( this.textBox8, "textBox8" );
            this.textBox8.Name = "textBox8";
            // 
            // textBox9
            // 
            resources.ApplyResources( this.textBox9, "textBox9" );
            this.textBox9.Name = "textBox9";
            // 
            // label9
            // 
            resources.ApplyResources( this.label9, "label9" );
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources( this.label10, "label10" );
            this.label10.Name = "label10";
            // 
            // textBox10
            // 
            resources.ApplyResources( this.textBox10, "textBox10" );
            this.textBox10.Name = "textBox10";
            // 
            // m_timerUpdate
            // 
            this.m_timerUpdate.Interval = 2000;
            this.m_timerUpdate.Tick += new System.EventHandler( this.m_timerUpdate_Tick );
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources( this.tableLayoutPanel1, "tableLayoutPanel1" );
            this.tableLayoutPanel1.Controls.Add( this.flowLayoutPanel1, 1, 0 );
            this.tableLayoutPanel1.Controls.Add( this.flowLayoutPanel2, 0, 0 );
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources( this.flowLayoutPanel1, "flowLayoutPanel1" );
            this.flowLayoutPanel1.Controls.Add( this.groupBox3 );
            this.flowLayoutPanel1.Controls.Add( this.groupBox4 );
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // flowLayoutPanel2
            // 
            resources.ApplyResources( this.flowLayoutPanel2, "flowLayoutPanel2" );
            this.flowLayoutPanel2.Controls.Add( this.groupBox2 );
            this.flowLayoutPanel2.Controls.Add( this.groupBox1 );
            this.flowLayoutPanel2.Controls.Add( this.groupBox5 );
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // groupBox5
            // 
            resources.ApplyResources( this.groupBox5, "groupBox5" );
            this.groupBox5.Controls.Add( this.m_rptNextReturns );
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // m_rptNextReturns
            // 
            resources.ApplyResources( this.m_rptNextReturns, "m_rptNextReturns" );
            this.m_rptNextReturns.Creator = null;
            this.m_rptNextReturns.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_rptNextReturns.Name = "m_rptNextReturns";
            this.m_rptNextReturns.WrapContent = false;
            // 
            // StatisticPanel
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.tableLayoutPanel1 );
            this.Name = "StatisticPanel";
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout( false );
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout( false );
            this.tableLayoutPanel4.ResumeLayout( false );
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout( false );
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout( false );
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout( false );
            this.flowLayoutPanel2.PerformLayout();
            this.groupBox5.ResumeLayout( false );
            this.groupBox5.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private LocalRepeater m_rptWorking;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Timer m_timerUpdate;
        private LocalRepeater m_rptWholeDay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox5;
        private LocalRepeater m_rptNextReturns;
        private LocalRepeater m_rptInfoBriefings;
    }

    // Creata per facilitare l'uso all'interno del controllo.
    internal class LocalRepeater : IndianaPark.Tools.Controls.LinkedRepeater<Model.TipoCliente>
    {
    }
}
