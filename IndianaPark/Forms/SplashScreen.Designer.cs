namespace IndianaPark.Forms
{
    /// <summary>
    /// Visualizza uno Splash Screen
    /// </summary>
    partial class SplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( SplashScreen ) );
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.AccessibleDescription = null;
            this.progressBar.AccessibleName = null;
            resources.ApplyResources( this.progressBar, "progressBar" );
            this.progressBar.BackgroundImage = null;
            this.progressBar.Font = null;
            this.progressBar.Name = "progressBar";
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.UseWaitCursor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleDescription = null;
            this.pictureBox1.AccessibleName = null;
            resources.ApplyResources( this.pictureBox1, "pictureBox1" );
            this.pictureBox1.BackgroundImage = null;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Font = null;
            this.pictureBox1.Image = global::IndianaPark.Properties.Resources.SplashScreen;
            this.pictureBox1.ImageLocation = null;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // labelMessage
            // 
            this.labelMessage.AccessibleDescription = null;
            this.labelMessage.AccessibleName = null;
            resources.ApplyResources( this.labelMessage, "labelMessage" );
            this.labelMessage.Font = null;
            this.labelMessage.Name = "labelMessage";
            // 
            // SplashScreen
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add( this.labelMessage );
            this.Controls.Add( this.progressBar );
            this.Controls.Add( this.pictureBox1 );
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = null;
            this.Name = "SplashScreen";
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelMessage;
    }
}