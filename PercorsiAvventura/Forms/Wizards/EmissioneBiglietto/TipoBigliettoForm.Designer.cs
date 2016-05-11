using System;
using IndianaPark.Tools;
using IndianaPark.Tools.Navigation;

namespace IndianaPark.PercorsiAvventura.Forms
{
    partial class TipoBigliettoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TipoBigliettoForm ) );
            this.m_lablelTitle = new System.Windows.Forms.Label();
            this.m_EscapeButton = new System.Windows.Forms.Button();
            this.m_BackButton = new System.Windows.Forms.Button();
            this.m_line1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.m_buttons = new IndianaPark.Tools.Controls.ControlRepeater();
            this.SuspendLayout();
            // 
            // m_lablelTitle
            // 
            resources.ApplyResources( this.m_lablelTitle, "m_lablelTitle" );
            this.m_lablelTitle.ForeColor = System.Drawing.Color.Red;
            this.m_lablelTitle.Name = "m_lablelTitle";
            // 
            // m_EscapeButton
            // 
            resources.ApplyResources( this.m_EscapeButton, "m_EscapeButton" );
            this.m_EscapeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_EscapeButton.Name = "m_EscapeButton";
            this.m_EscapeButton.UseVisualStyleBackColor = true;
            this.m_EscapeButton.Click += new System.EventHandler( this.EscapeClickHandler );
            // 
            // m_BackButton
            // 
            resources.ApplyResources( this.m_BackButton, "m_BackButton" );
            this.m_BackButton.Name = "m_BackButton";
            this.m_BackButton.UseVisualStyleBackColor = true;
            this.m_BackButton.Click += new System.EventHandler( this.BackClickHandler );
            // 
            // m_line1
            // 
            resources.ApplyResources( this.m_line1, "m_line1" );
            this.m_line1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.m_line1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.m_line1.Name = "m_line1";
            // 
            // m_buttons
            // 
            resources.ApplyResources( this.m_buttons, "m_buttons" );
            this.m_buttons.Creator = null;
            this.m_buttons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_buttons.Name = "m_buttons";
            this.m_buttons.WrapContent = false;
            this.m_buttons.Resize += new System.EventHandler( this.RepeaterResizeHandler );
            // 
            // TipoBigliettoForm
            // 
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_EscapeButton;
            this.Controls.Add( this.m_buttons );
            this.Controls.Add( this.m_line1 );
            this.Controls.Add( this.m_BackButton );
            this.Controls.Add( this.m_lablelTitle );
            this.Controls.Add( this.m_EscapeButton );
            this.Name = "TipoBigliettoForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lablelTitle;
        private System.Windows.Forms.Button m_EscapeButton;
        private System.Windows.Forms.Button m_BackButton;
        private Tools.Controls.LineSeparator m_line1;
        private Tools.Controls.ControlRepeater m_buttons;
    }
}