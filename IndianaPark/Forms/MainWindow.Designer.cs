namespace IndianaPark.Forms
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.pbTitolo = new System.Windows.Forms.PictureBox();
            this.flpPlugins = new System.Windows.Forms.FlowLayoutPanel();
            this.mnuMainWindow = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.esciToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sezioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginCaricatiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aiutoStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informazioniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusMainWindow = new System.Windows.Forms.StatusStrip();
            this.userStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pluginStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.caricaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitolo)).BeginInit();
            this.mnuMainWindow.SuspendLayout();
            this.statusMainWindow.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbTitolo
            // 
            this.pbTitolo.Image = global::IndianaPark.Properties.Resources.BannerFinestra;
            resources.ApplyResources(this.pbTitolo, "pbTitolo");
            this.pbTitolo.Name = "pbTitolo";
            this.pbTitolo.TabStop = false;
            // 
            // flpPlugins
            // 
            resources.ApplyResources(this.flpPlugins, "flpPlugins");
            this.flpPlugins.Name = "flpPlugins";
            // 
            // mnuMainWindow
            // 
            this.mnuMainWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.sezioniToolStripMenuItem,
            this.aiutoStripMenuItem});
            resources.ApplyResources(this.mnuMainWindow, "mnuMainWindow");
            this.mnuMainWindow.Name = "mnuMainWindow";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginStripMenuItem,
            this.logoutStripMenuItem,
            this.toolStripMenuItem1,
            this.esciToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // loginStripMenuItem
            // 
            this.loginStripMenuItem.Name = "loginStripMenuItem";
            resources.ApplyResources(this.loginStripMenuItem, "loginStripMenuItem");
            // 
            // logoutStripMenuItem
            // 
            resources.ApplyResources(this.logoutStripMenuItem, "logoutStripMenuItem");
            this.logoutStripMenuItem.Name = "logoutStripMenuItem";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // esciToolStripMenuItem
            // 
            this.esciToolStripMenuItem.Name = "esciToolStripMenuItem";
            resources.ApplyResources(this.esciToolStripMenuItem, "esciToolStripMenuItem");
            this.esciToolStripMenuItem.Click += new System.EventHandler(this.esciToolStripMenuItem_Click);
            // 
            // sezioniToolStripMenuItem
            // 
            this.sezioniToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.pluginCaricatiToolStripMenuItem});
            this.sezioniToolStripMenuItem.Name = "sezioniToolStripMenuItem";
            resources.ApplyResources(this.sezioniToolStripMenuItem, "sezioniToolStripMenuItem");
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // pluginCaricatiToolStripMenuItem
            // 
            this.pluginCaricatiToolStripMenuItem.Name = "pluginCaricatiToolStripMenuItem";
            resources.ApplyResources(this.pluginCaricatiToolStripMenuItem, "pluginCaricatiToolStripMenuItem");
            this.pluginCaricatiToolStripMenuItem.Click += new System.EventHandler(this.pluginCaricatiToolStripMenuItem_Click);
            // 
            // aiutoStripMenuItem
            // 
            this.aiutoStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.aiutoStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.informazioniToolStripMenuItem});
            this.aiutoStripMenuItem.Name = "aiutoStripMenuItem";
            resources.ApplyResources(this.aiutoStripMenuItem, "aiutoStripMenuItem");
            // 
            // informazioniToolStripMenuItem
            // 
            this.informazioniToolStripMenuItem.Name = "informazioniToolStripMenuItem";
            resources.ApplyResources(this.informazioniToolStripMenuItem, "informazioniToolStripMenuItem");
            this.informazioniToolStripMenuItem.Click += new System.EventHandler(this.informazioniToolStripMenuItem_Click);
            // 
            // statusMainWindow
            // 
            this.statusMainWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userStripStatusLabel,
            this.pluginStripStatusLabel,
            this.toolStripStatusLabel1});
            resources.ApplyResources(this.statusMainWindow, "statusMainWindow");
            this.statusMainWindow.Name = "statusMainWindow";
            this.statusMainWindow.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            // 
            // userStripStatusLabel
            // 
            resources.ApplyResources(this.userStripStatusLabel, "userStripStatusLabel");
            this.userStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.userStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.userStripStatusLabel.Name = "userStripStatusLabel";
            // 
            // pluginStripStatusLabel
            // 
            resources.ApplyResources(this.pluginStripStatusLabel, "pluginStripStatusLabel");
            this.pluginStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.pluginStripStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.pluginStripStatusLabel.Name = "pluginStripStatusLabel";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Spring = true;
            // 
            // caricaToolStripMenuItem
            // 
            this.caricaToolStripMenuItem.Name = "caricaToolStripMenuItem";
            resources.ApplyResources(this.caricaToolStripMenuItem, "caricaToolStripMenuItem");
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusMainWindow);
            this.Controls.Add(this.pbTitolo);
            this.Controls.Add(this.mnuMainWindow);
            this.Controls.Add(this.flpPlugins);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.mnuMainWindow;
            this.Name = "MainWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pbTitolo)).EndInit();
            this.mnuMainWindow.ResumeLayout(false);
            this.mnuMainWindow.PerformLayout();
            this.statusMainWindow.ResumeLayout(false);
            this.statusMainWindow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpPlugins;
        private System.Windows.Forms.PictureBox pbTitolo;
        private System.Windows.Forms.MenuStrip mnuMainWindow;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem esciToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sezioniToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pluginCaricatiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aiutoStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informazioniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutStripMenuItem;
        private System.Windows.Forms.StatusStrip statusMainWindow;
        private System.Windows.Forms.ToolStripStatusLabel userStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel pluginStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem caricaToolStripMenuItem;

    }
}