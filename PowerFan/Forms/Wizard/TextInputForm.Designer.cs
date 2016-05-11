namespace IndianaPark.PowerFan.Forms.New
{
    partial class TextInputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TextInputForm ) );
            this.m_lableTitle = new System.Windows.Forms.Label();
            this.m_text = new System.Windows.Forms.TextBox();
            this.m_navigator = new IndianaPark.Tools.Controls.NavigationPanel();
            this.m_line1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.SuspendLayout();
            // 
            // m_lableTitle
            // 
            resources.ApplyResources( this.m_lableTitle, "m_lableTitle" );
            this.m_lableTitle.ForeColor = System.Drawing.Color.Red;
            this.m_lableTitle.Name = "m_lableTitle";
            // 
            // m_text
            // 
            resources.ApplyResources( this.m_text, "m_text" );
            this.m_text.Name = "m_text";
            // 
            // m_navigator
            // 
            this.m_navigator.CancelEnabled = true;
            this.m_navigator.DialogResult = System.Windows.Forms.DialogResult.None;
            this.m_navigator.FinishEnabled = false;
            resources.ApplyResources( this.m_navigator, "m_navigator" );
            this.m_navigator.MaximumSize = new System.Drawing.Size( 282, 41 );
            this.m_navigator.MinimumSize = new System.Drawing.Size( 282, 41 );
            this.m_navigator.Name = "m_navigator";
            this.m_navigator.NextEnabled = true;
            this.m_navigator.PreviousEnabled = true;
            // 
            // m_line1
            // 
            resources.ApplyResources( this.m_line1, "m_line1" );
            this.m_line1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.m_line1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.m_line1.Name = "m_line1";
            // 
            // TextInputForm
            // 
            this.AcceptButton = this.m_navigator;
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_line1 );
            this.Controls.Add( this.m_navigator );
            this.Controls.Add( this.m_text );
            this.Controls.Add( this.m_lableTitle );
            this.Name = "TextInputForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private Tools.Controls.LineSeparator m_line1;
        /// <summary>
        /// L'etichetta che rappresenta il titolo
        /// </summary>
        protected internal System.Windows.Forms.Label m_lableTitle;
        /// <summary>
        /// L'input box
        /// </summary>
        protected internal System.Windows.Forms.TextBox m_text;
        /// <summary>
        /// Il navigator per muoversi avanti ed indietro
        /// </summary>
        protected internal IndianaPark.Tools.Controls.NavigationPanel m_navigator;
    }
}