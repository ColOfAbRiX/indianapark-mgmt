namespace IndianaPark.PercorsiAvventura.Forms
{
    partial class SceltaBriefingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( SceltaBriefingForm ) );
            this.m_repeater = new IndianaPark.Tools.Controls.ControlRepeater();
            this.navigationPanel1 = new IndianaPark.Tools.Controls.NavigationPanel();
            this.lineSeparator1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.m_labelTitolo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_repeater
            // 
            resources.ApplyResources( this.m_repeater, "m_repeater" );
            this.m_repeater.Creator = null;
            this.m_repeater.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_repeater.MaximumSize = new System.Drawing.Size( 0, 290 );
            this.m_repeater.MinimumSize = new System.Drawing.Size( 230, 0 );
            this.m_repeater.Name = "m_repeater";
            this.m_repeater.WrapContent = false;
            this.m_repeater.Resize += new System.EventHandler( this.RepeaterResizeHandler );
            // 
            // navigationPanel1
            // 
            resources.ApplyResources( this.navigationPanel1, "navigationPanel1" );
            this.navigationPanel1.CancelEnabled = true;
            this.navigationPanel1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.navigationPanel1.FinishEnabled = false;
            this.navigationPanel1.MaximumSize = new System.Drawing.Size( 282, 41 );
            this.navigationPanel1.MinimumSize = new System.Drawing.Size( 282, 41 );
            this.navigationPanel1.Name = "navigationPanel1";
            this.navigationPanel1.NextEnabled = true;
            this.navigationPanel1.PreviousEnabled = true;
            this.navigationPanel1.Navigate += new IndianaPark.Tools.Navigation.NavigationEvent( this.NavigationHandler );
            // 
            // lineSeparator1
            // 
            resources.ApplyResources( this.lineSeparator1, "lineSeparator1" );
            this.lineSeparator1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.lineSeparator1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.lineSeparator1.Name = "lineSeparator1";
            // 
            // m_labelTitolo
            // 
            resources.ApplyResources( this.m_labelTitolo, "m_labelTitolo" );
            this.m_labelTitolo.ForeColor = System.Drawing.Color.Red;
            this.m_labelTitolo.Name = "m_labelTitolo";
            // 
            // SceltaBriefingForm
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_labelTitolo );
            this.Controls.Add( this.lineSeparator1 );
            this.Controls.Add( this.navigationPanel1 );
            this.Controls.Add( this.m_repeater );
            this.Name = "SceltaBriefingForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private IndianaPark.Tools.Controls.ControlRepeater m_repeater;
        private IndianaPark.Tools.Controls.NavigationPanel navigationPanel1;
        private IndianaPark.Tools.Controls.LineSeparator lineSeparator1;
        private System.Windows.Forms.Label m_labelTitolo;
    }
}