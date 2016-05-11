namespace IndianaPark.Tools.Navigation
{
    partial class NavigationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( NavigationForm ) );
            this.tmrOpacity = new System.Windows.Forms.Timer( this.components );
            this.SuspendLayout();
            // 
            // tmrOpacity
            // 
            this.tmrOpacity.Interval = 20;
            this.tmrOpacity.Tick += new System.EventHandler( this.OpacityTickHandler );
            // 
            // NavigationForm
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NavigationForm";
            this.VisibleChanged += new System.EventHandler( this.VisibleChangedHandler );
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.KeyPressHandler );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.FormClosingHandler );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Timer tmrOpacity;
    }
}