using System.Drawing;

namespace IndianaPark.PercorsiAvventura.Forms
{
    partial class ScontoPersonalizzatoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ScontoPersonalizzatoForm ) );
            this.m_line1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.m_BackButton = new System.Windows.Forms.Button();
            this.m_lablelTitle = new System.Windows.Forms.Label();
            this.m_buttonFisso = new System.Windows.Forms.Button();
            this.m_buttonPercentuale = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_line1
            // 
            resources.ApplyResources( this.m_line1, "m_line1" );
            this.m_line1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.m_line1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.m_line1.Name = "m_line1";
            // 
            // m_BackButton
            // 
            resources.ApplyResources( this.m_BackButton, "m_BackButton" );
            this.m_BackButton.Name = "m_BackButton";
            this.m_BackButton.UseVisualStyleBackColor = true;
            this.m_BackButton.Click += new System.EventHandler( this.BackClickHandler );
            // 
            // m_lablelTitle
            // 
            resources.ApplyResources( this.m_lablelTitle, "m_lablelTitle" );
            this.m_lablelTitle.ForeColor = System.Drawing.Color.Red;
            this.m_lablelTitle.Name = "m_lablelTitle";
            // 
            // m_buttonFisso
            // 
            resources.ApplyResources( this.m_buttonFisso, "m_buttonFisso" );
            this.m_buttonFisso.Name = "m_buttonFisso";
            this.m_buttonFisso.UseVisualStyleBackColor = true;
            this.m_buttonFisso.Click += new System.EventHandler( this.ScontoFissoClickHandler );
            // 
            // m_buttonPercentuale
            // 
            resources.ApplyResources( this.m_buttonPercentuale, "m_buttonPercentuale" );
            this.m_buttonPercentuale.Name = "m_buttonPercentuale";
            this.m_buttonPercentuale.UseVisualStyleBackColor = true;
            this.m_buttonPercentuale.Click += new System.EventHandler( this.ScontoPercentualeClickHandler );
            // 
            // ScontoPersonalizzatoForm
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_buttonPercentuale );
            this.Controls.Add( this.m_buttonFisso );
            this.Controls.Add( this.m_line1 );
            this.Controls.Add( this.m_BackButton );
            this.Controls.Add( this.m_lablelTitle );
            this.Name = "ScontoPersonalizzatoForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private IndianaPark.Tools.Controls.LineSeparator m_line1;
        private System.Windows.Forms.Button m_BackButton;
        private System.Windows.Forms.Label m_lablelTitle;
        private System.Windows.Forms.Button m_buttonFisso;
        private System.Windows.Forms.Button m_buttonPercentuale;
    }
}