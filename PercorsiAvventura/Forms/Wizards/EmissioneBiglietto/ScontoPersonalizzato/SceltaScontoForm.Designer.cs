namespace IndianaPark.PercorsiAvventura.Forms
{
    partial class SceltaScontoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( SceltaScontoForm ) );
            this.m_labelTitolo = new System.Windows.Forms.Label();
            this.m_buttons = new IndianaPark.Tools.Controls.ControlRepeater();
            this.m_line1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.m_EscapeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_labelTitolo
            // 
            resources.ApplyResources( this.m_labelTitolo, "m_labelTitolo" );
            this.m_labelTitolo.ForeColor = System.Drawing.Color.Red;
            this.m_labelTitolo.Name = "m_labelTitolo";
            // 
            // m_buttons
            // 
            resources.ApplyResources( this.m_buttons, "m_buttons" );
            this.m_buttons.Creator = null;
            this.m_buttons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_buttons.MaximumSize = new System.Drawing.Size( 410, 420 );
            this.m_buttons.Name = "m_buttons";
            this.m_buttons.WrapContent = false;
            this.m_buttons.Resize += new System.EventHandler( this.RepeaterResizeHandler );
            // 
            // m_line1
            // 
            resources.ApplyResources( this.m_line1, "m_line1" );
            this.m_line1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.m_line1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.m_line1.Name = "m_line1";
            // 
            // m_EscapeButton
            // 
            resources.ApplyResources( this.m_EscapeButton, "m_EscapeButton" );
            this.m_EscapeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_EscapeButton.Name = "m_EscapeButton";
            this.m_EscapeButton.UseVisualStyleBackColor = true;
            this.m_EscapeButton.Click += new System.EventHandler( this.EscapeClickHandler );
            // 
            // SceltaScontoForm
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_line1 );
            this.Controls.Add( this.m_EscapeButton );
            this.Controls.Add( this.m_buttons );
            this.Controls.Add( this.m_labelTitolo );
            this.Name = "SceltaScontoForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_labelTitolo;
        private IndianaPark.Tools.Controls.ControlRepeater m_buttons;
        private IndianaPark.Tools.Controls.LineSeparator m_line1;
        private System.Windows.Forms.Button m_EscapeButton;
    }
}