namespace IndianaPark.PowerFan.Forms.New
{
    partial class QuantityForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( QuantityForm ) );
            this.upDownCounter1 = new IndianaPark.Tools.Controls.UpDownCounter();
            this.m_lableTitle = new System.Windows.Forms.Label();
            this.m_NextButton = new System.Windows.Forms.Button();
            this.m_CancelButton = new System.Windows.Forms.Button();
            this.lineSeparator1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.SuspendLayout();
            // 
            // upDownCounter1
            // 
            this.upDownCounter1.Counter = 0;
            resources.ApplyResources( this.upDownCounter1, "upDownCounter1" );
            this.upDownCounter1.Max = 100;
            this.upDownCounter1.MaximumSize = new System.Drawing.Size( 149, 46 );
            this.upDownCounter1.Min = 0;
            this.upDownCounter1.MinimumSize = new System.Drawing.Size( 149, 46 );
            this.upDownCounter1.Name = "upDownCounter1";
            this.upDownCounter1.Step = ((uint)(1u));
            // 
            // m_lableTitle
            // 
            resources.ApplyResources( this.m_lableTitle, "m_lableTitle" );
            this.m_lableTitle.ForeColor = System.Drawing.Color.Red;
            this.m_lableTitle.Name = "m_lableTitle";
            // 
            // m_NextButton
            // 
            resources.ApplyResources( this.m_NextButton, "m_NextButton" );
            this.m_NextButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_NextButton.Name = "m_NextButton";
            this.m_NextButton.UseVisualStyleBackColor = true;
            this.m_NextButton.Click += new System.EventHandler( this.NextClickHandler );
            // 
            // m_CancelButton
            // 
            resources.ApplyResources( this.m_CancelButton, "m_CancelButton" );
            this.m_CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_CancelButton.Name = "m_CancelButton";
            this.m_CancelButton.UseVisualStyleBackColor = true;
            this.m_CancelButton.Click += new System.EventHandler( this.CancelClickHandler );
            // 
            // lineSeparator1
            // 
            resources.ApplyResources( this.lineSeparator1, "lineSeparator1" );
            this.lineSeparator1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.lineSeparator1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.lineSeparator1.Name = "lineSeparator1";
            // 
            // QuantityForm
            // 
            this.AcceptButton = this.m_NextButton;
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_CancelButton;
            this.Controls.Add( this.lineSeparator1 );
            this.Controls.Add( this.m_CancelButton );
            this.Controls.Add( this.m_NextButton );
            this.Controls.Add( this.m_lableTitle );
            this.Controls.Add( this.upDownCounter1 );
            this.Name = "QuantityForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.FormClosedHandler );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private IndianaPark.Tools.Controls.UpDownCounter upDownCounter1;
        private System.Windows.Forms.Label m_lableTitle;
        private System.Windows.Forms.Button m_NextButton;
        private System.Windows.Forms.Button m_CancelButton;
        private IndianaPark.Tools.Controls.LineSeparator lineSeparator1;
    }
}