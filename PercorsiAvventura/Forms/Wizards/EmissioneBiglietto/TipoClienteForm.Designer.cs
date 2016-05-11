using IndianaPark.Tools;

namespace IndianaPark.PercorsiAvventura.Forms
{
    partial class TipoClienteForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TipoClienteForm ) );
            this.m_labelPrice = new System.Windows.Forms.Label();
            this.m_lableTitle = new System.Windows.Forms.Label();
            this.m_navigator = new IndianaPark.Tools.Controls.NavigationPanel();
            this.m_line1 = new IndianaPark.Tools.Controls.LineSeparator();
            this.m_repeaterTipoClienti = new IndianaPark.Tools.Controls.ControlRepeater();
            this.SuspendLayout();
            // 
            // m_labelPrice
            // 
            resources.ApplyResources( this.m_labelPrice, "m_labelPrice" );
            this.m_labelPrice.ForeColor = System.Drawing.Color.Red;
            this.m_labelPrice.Name = "m_labelPrice";
            // 
            // m_lableTitle
            // 
            resources.ApplyResources( this.m_lableTitle, "m_lableTitle" );
            this.m_lableTitle.ForeColor = System.Drawing.Color.Red;
            this.m_lableTitle.Name = "m_lableTitle";
            // 
            // m_navigator
            // 
            resources.ApplyResources( this.m_navigator, "m_navigator" );
            this.m_navigator.CancelEnabled = true;
            this.m_navigator.DialogResult = System.Windows.Forms.DialogResult.None;
            this.m_navigator.FinishEnabled = false;
            this.m_navigator.MaximumSize = new System.Drawing.Size( 282, 41 );
            this.m_navigator.MinimumSize = new System.Drawing.Size( 282, 41 );
            this.m_navigator.Name = "m_navigator";
            this.m_navigator.NextEnabled = true;
            this.m_navigator.PreviousEnabled = true;
            this.m_navigator.Navigate += new IndianaPark.Tools.Navigation.NavigationEvent( this.NavigateHandler );
            // 
            // m_line1
            // 
            resources.ApplyResources( this.m_line1, "m_line1" );
            this.m_line1.MaximumSize = new System.Drawing.Size( 2000, 2 );
            this.m_line1.MinimumSize = new System.Drawing.Size( 0, 2 );
            this.m_line1.Name = "m_line1";
            // 
            // m_repeaterTipoClienti
            // 
            resources.ApplyResources( this.m_repeaterTipoClienti, "m_repeaterTipoClienti" );
            this.m_repeaterTipoClienti.Creator = null;
            this.m_repeaterTipoClienti.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.m_repeaterTipoClienti.MaximumSize = new System.Drawing.Size( 650, 500 );
            this.m_repeaterTipoClienti.Name = "m_repeaterTipoClienti";
            this.m_repeaterTipoClienti.WrapContent = false;
            // 
            // TipoClienteForm
            // 
            this.AcceptButton = this.m_navigator;
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.m_repeaterTipoClienti );
            this.Controls.Add( this.m_line1 );
            this.Controls.Add( this.m_navigator );
            this.Controls.Add( this.m_lableTitle );
            this.Controls.Add( this.m_labelPrice );
            this.Name = "TipoClienteForm";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_labelPrice;
        private System.Windows.Forms.Label m_lableTitle;
        private Tools.Controls.NavigationPanel m_navigator;
        private Tools.Controls.LineSeparator m_line1;
        private Tools.Controls.ControlRepeater m_repeaterTipoClienti;
    }
}