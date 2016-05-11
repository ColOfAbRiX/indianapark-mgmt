namespace IndianaPark.PercorsiAvventura.Forms
{
    partial class TotaleCassa
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
            this.m_lableTitle = new System.Windows.Forms.Label();
            this.m_clientiList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.m_totaleLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_exitButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_lablePrezzoSelezionato = new System.Windows.Forms.Label();
            this.m_lableQuantitàSelezionata = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lableTitle
            // 
            this.m_lableTitle.AutoSize = true;
            this.m_lableTitle.Font = new System.Drawing.Font( "Tahoma", 24F, System.Drawing.FontStyle.Bold );
            this.m_lableTitle.ForeColor = System.Drawing.Color.Black;
            this.m_lableTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_lableTitle.Location = new System.Drawing.Point( 12, 9 );
            this.m_lableTitle.Name = "m_lableTitle";
            this.m_lableTitle.Size = new System.Drawing.Size( 301, 39 );
            this.m_lableTitle.TabIndex = 26;
            this.m_lableTitle.Text = "Totale del giorno:";
            // 
            // m_clientiList
            // 
            this.m_clientiList.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4} );
            this.m_clientiList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_clientiList.FullRowSelect = true;
            this.m_clientiList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.m_clientiList.Location = new System.Drawing.Point( 10, 25 );
            this.m_clientiList.Name = "m_clientiList";
            this.m_clientiList.Size = new System.Drawing.Size( 540, 249 );
            this.m_clientiList.TabIndex = 27;
            this.m_clientiList.UseCompatibleStateImageBehavior = false;
            this.m_clientiList.View = System.Windows.Forms.View.Details;
            this.m_clientiList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler( this.m_clientiList_ItemSelectionChanged );
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Categoria";
            this.columnHeader1.Width = 260;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Prezzo Unitario";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Quantità";
            this.columnHeader3.Width = 70;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Totale";
            this.columnHeader4.Width = 85;
            // 
            // m_totaleLabel
            // 
            this.m_totaleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_totaleLabel.Font = new System.Drawing.Font( "Tahoma", 24F, System.Drawing.FontStyle.Bold );
            this.m_totaleLabel.ForeColor = System.Drawing.Color.Red;
            this.m_totaleLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_totaleLabel.Location = new System.Drawing.Point( 319, 9 );
            this.m_totaleLabel.Name = "m_totaleLabel";
            this.m_totaleLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.m_totaleLabel.Size = new System.Drawing.Size( 253, 39 );
            this.m_totaleLabel.TabIndex = 28;
            this.m_totaleLabel.Text = "{0,-10:C}";
            this.m_totaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add( this.m_clientiList );
            this.groupBox1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.groupBox1.Location = new System.Drawing.Point( 12, 51 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding( 10 );
            this.groupBox1.Size = new System.Drawing.Size( 560, 284 );
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Totali per categoria";
            // 
            // m_exitButton
            // 
            this.m_exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_exitButton.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.m_exitButton.Location = new System.Drawing.Point( 487, 349 );
            this.m_exitButton.Name = "m_exitButton";
            this.m_exitButton.Size = new System.Drawing.Size( 75, 30 );
            this.m_exitButton.TabIndex = 30;
            this.m_exitButton.Text = "&Esci";
            this.m_exitButton.UseVisualStyleBackColor = true;
            this.m_exitButton.Click += new System.EventHandler( this.button1_Click );
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add( this.label2 );
            this.groupBox2.Controls.Add( this.label1 );
            this.groupBox2.Controls.Add( this.m_lableQuantitàSelezionata );
            this.groupBox2.Controls.Add( this.m_lablePrezzoSelezionato );
            this.groupBox2.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.groupBox2.Location = new System.Drawing.Point( 12, 341 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 459, 38 );
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selezione";
            // 
            // m_lablePrezzoSelezionato
            // 
            this.m_lablePrezzoSelezionato.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lablePrezzoSelezionato.Location = new System.Drawing.Point( 328, 16 );
            this.m_lablePrezzoSelezionato.Name = "m_lablePrezzoSelezionato";
            this.m_lablePrezzoSelezionato.Size = new System.Drawing.Size( 125, 15 );
            this.m_lablePrezzoSelezionato.TabIndex = 0;
            this.m_lablePrezzoSelezionato.Text = "0";
            this.m_lablePrezzoSelezionato.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // m_lableQuantitàSelezionata
            // 
            this.m_lableQuantitàSelezionata.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lableQuantitàSelezionata.Location = new System.Drawing.Point( 121, 16 );
            this.m_lableQuantitàSelezionata.Name = "m_lableQuantitàSelezionata";
            this.m_lableQuantitàSelezionata.Size = new System.Drawing.Size( 54, 15 );
            this.m_lableQuantitàSelezionata.TabIndex = 1;
            this.m_lableQuantitàSelezionata.Text = "0";
            this.m_lableQuantitàSelezionata.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 198, 17 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 124, 15 );
            this.label1.TabIndex = 2;
            this.label1.Text = "Quantità Selezionata:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 7, 16 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 108, 15 );
            this.label2.TabIndex = 3;
            this.label2.Text = "Clienti Selezionati:";
            // 
            // TotaleCassa
            // 
            this.AcceptButton = this.m_exitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_exitButton;
            this.ClientSize = new System.Drawing.Size( 584, 391 );
            this.Controls.Add( this.groupBox2 );
            this.Controls.Add( this.m_exitButton );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.m_totaleLabel );
            this.Controls.Add( this.m_lableTitle );
            this.MaximumSize = new System.Drawing.Size( 600, 550 );
            this.MinimumSize = new System.Drawing.Size( 600, 240 );
            this.Name = "TotaleCassa";
            this.Text = "Totale di Cassa";
            this.groupBox1.ResumeLayout( false );
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lableTitle;
        private System.Windows.Forms.ListView m_clientiList;
        private System.Windows.Forms.Label m_totaleLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button m_exitButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label m_lablePrezzoSelezionato;
        private System.Windows.Forms.Label m_lableQuantitàSelezionata;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}