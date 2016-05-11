namespace IndianaPark.PercorsiAvventura.Forms
{
    partial class ClientiView
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientiView));
            this.m_grid = new System.Windows.Forms.DataGridView();
            this.m_clientiBinding = new System.Windows.Forms.BindingSource(this.components);
            this.briefingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codiceClienteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codiceNominativoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codiceCompletoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oraIngressoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oraUscitaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prezzoBaseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prezzoPersonaleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prezzoScontatoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scontoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scontoComitivaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipoClienteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uscitoDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataInserimentoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nominativoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inserimentoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clienteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_remove = new System.Windows.Forms.Button();
            this.m_escape = new System.Windows.Forms.Button();
            this.m_print = new System.Windows.Forms.Button();
            this.m_close = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_lableTitle = new System.Windows.Forms.Label();
            this.Codice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nominativo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sconto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScontoComitiva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prezzo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ingresso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uscita = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uscito = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.briefingDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_clientiBinding)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_grid
            // 
            this.m_grid.AllowUserToAddRows = false;
            this.m_grid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.m_grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.m_grid, "m_grid");
            this.m_grid.AutoGenerateColumns = false;
            this.m_grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.m_grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Codice,
            this.Nominativo,
            this.Cliente,
            this.Sconto,
            this.ScontoComitiva,
            this.Prezzo,
            this.Ingresso,
            this.Uscita,
            this.Uscito,
            this.briefingDataGridViewTextBoxColumn1});
            this.m_grid.DataSource = this.m_clientiBinding;
            this.m_grid.Name = "m_grid";
            this.m_grid.ReadOnly = true;
            this.m_grid.RowTemplate.Height = 20;
            this.m_grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_grid.ShowEditingIcon = false;
            this.m_grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.GridCellFormattingHandler);
            this.m_grid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_grid_ColumnHeaderMouseClick);
            this.m_grid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridRowEnterHandler);
            // 
            // m_clientiBinding
            // 
            this.m_clientiBinding.DataSource = typeof(IndianaPark.PercorsiAvventura.Model.ClienteWrapper);
            // 
            // briefingDataGridViewTextBoxColumn
            // 
            this.briefingDataGridViewTextBoxColumn.DataPropertyName = "Briefing";
            resources.ApplyResources(this.briefingDataGridViewTextBoxColumn, "briefingDataGridViewTextBoxColumn");
            this.briefingDataGridViewTextBoxColumn.Name = "briefingDataGridViewTextBoxColumn";
            this.briefingDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // codiceClienteDataGridViewTextBoxColumn
            // 
            this.codiceClienteDataGridViewTextBoxColumn.DataPropertyName = "CodiceCliente";
            resources.ApplyResources(this.codiceClienteDataGridViewTextBoxColumn, "codiceClienteDataGridViewTextBoxColumn");
            this.codiceClienteDataGridViewTextBoxColumn.Name = "codiceClienteDataGridViewTextBoxColumn";
            this.codiceClienteDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // codiceNominativoDataGridViewTextBoxColumn
            // 
            this.codiceNominativoDataGridViewTextBoxColumn.DataPropertyName = "CodiceNominativo";
            resources.ApplyResources(this.codiceNominativoDataGridViewTextBoxColumn, "codiceNominativoDataGridViewTextBoxColumn");
            this.codiceNominativoDataGridViewTextBoxColumn.Name = "codiceNominativoDataGridViewTextBoxColumn";
            this.codiceNominativoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // codiceCompletoDataGridViewTextBoxColumn
            // 
            this.codiceCompletoDataGridViewTextBoxColumn.DataPropertyName = "CodiceCompleto";
            resources.ApplyResources(this.codiceCompletoDataGridViewTextBoxColumn, "codiceCompletoDataGridViewTextBoxColumn");
            this.codiceCompletoDataGridViewTextBoxColumn.Name = "codiceCompletoDataGridViewTextBoxColumn";
            this.codiceCompletoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oraIngressoDataGridViewTextBoxColumn
            // 
            this.oraIngressoDataGridViewTextBoxColumn.DataPropertyName = "OraIngresso";
            resources.ApplyResources(this.oraIngressoDataGridViewTextBoxColumn, "oraIngressoDataGridViewTextBoxColumn");
            this.oraIngressoDataGridViewTextBoxColumn.Name = "oraIngressoDataGridViewTextBoxColumn";
            this.oraIngressoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oraUscitaDataGridViewTextBoxColumn
            // 
            this.oraUscitaDataGridViewTextBoxColumn.DataPropertyName = "OraUscita";
            resources.ApplyResources(this.oraUscitaDataGridViewTextBoxColumn, "oraUscitaDataGridViewTextBoxColumn");
            this.oraUscitaDataGridViewTextBoxColumn.Name = "oraUscitaDataGridViewTextBoxColumn";
            this.oraUscitaDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // prezzoBaseDataGridViewTextBoxColumn
            // 
            this.prezzoBaseDataGridViewTextBoxColumn.DataPropertyName = "PrezzoBase";
            resources.ApplyResources(this.prezzoBaseDataGridViewTextBoxColumn, "prezzoBaseDataGridViewTextBoxColumn");
            this.prezzoBaseDataGridViewTextBoxColumn.Name = "prezzoBaseDataGridViewTextBoxColumn";
            this.prezzoBaseDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // prezzoPersonaleDataGridViewTextBoxColumn
            // 
            this.prezzoPersonaleDataGridViewTextBoxColumn.DataPropertyName = "PrezzoPersonale";
            resources.ApplyResources(this.prezzoPersonaleDataGridViewTextBoxColumn, "prezzoPersonaleDataGridViewTextBoxColumn");
            this.prezzoPersonaleDataGridViewTextBoxColumn.Name = "prezzoPersonaleDataGridViewTextBoxColumn";
            this.prezzoPersonaleDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // prezzoScontatoDataGridViewTextBoxColumn
            // 
            this.prezzoScontatoDataGridViewTextBoxColumn.DataPropertyName = "PrezzoScontato";
            resources.ApplyResources(this.prezzoScontatoDataGridViewTextBoxColumn, "prezzoScontatoDataGridViewTextBoxColumn");
            this.prezzoScontatoDataGridViewTextBoxColumn.Name = "prezzoScontatoDataGridViewTextBoxColumn";
            this.prezzoScontatoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // scontoDataGridViewTextBoxColumn
            // 
            this.scontoDataGridViewTextBoxColumn.DataPropertyName = "Sconto";
            resources.ApplyResources(this.scontoDataGridViewTextBoxColumn, "scontoDataGridViewTextBoxColumn");
            this.scontoDataGridViewTextBoxColumn.Name = "scontoDataGridViewTextBoxColumn";
            this.scontoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // scontoComitivaDataGridViewTextBoxColumn
            // 
            this.scontoComitivaDataGridViewTextBoxColumn.DataPropertyName = "ScontoComitiva";
            resources.ApplyResources(this.scontoComitivaDataGridViewTextBoxColumn, "scontoComitivaDataGridViewTextBoxColumn");
            this.scontoComitivaDataGridViewTextBoxColumn.Name = "scontoComitivaDataGridViewTextBoxColumn";
            this.scontoComitivaDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tipoClienteDataGridViewTextBoxColumn
            // 
            this.tipoClienteDataGridViewTextBoxColumn.DataPropertyName = "TipoCliente";
            resources.ApplyResources(this.tipoClienteDataGridViewTextBoxColumn, "tipoClienteDataGridViewTextBoxColumn");
            this.tipoClienteDataGridViewTextBoxColumn.Name = "tipoClienteDataGridViewTextBoxColumn";
            this.tipoClienteDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uscitoDataGridViewCheckBoxColumn
            // 
            this.uscitoDataGridViewCheckBoxColumn.DataPropertyName = "Uscito";
            resources.ApplyResources(this.uscitoDataGridViewCheckBoxColumn, "uscitoDataGridViewCheckBoxColumn");
            this.uscitoDataGridViewCheckBoxColumn.Name = "uscitoDataGridViewCheckBoxColumn";
            this.uscitoDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // dataInserimentoDataGridViewTextBoxColumn
            // 
            this.dataInserimentoDataGridViewTextBoxColumn.DataPropertyName = "DataInserimento";
            resources.ApplyResources(this.dataInserimentoDataGridViewTextBoxColumn, "dataInserimentoDataGridViewTextBoxColumn");
            this.dataInserimentoDataGridViewTextBoxColumn.Name = "dataInserimentoDataGridViewTextBoxColumn";
            this.dataInserimentoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nominativoDataGridViewTextBoxColumn
            // 
            this.nominativoDataGridViewTextBoxColumn.DataPropertyName = "Nominativo";
            resources.ApplyResources(this.nominativoDataGridViewTextBoxColumn, "nominativoDataGridViewTextBoxColumn");
            this.nominativoDataGridViewTextBoxColumn.Name = "nominativoDataGridViewTextBoxColumn";
            this.nominativoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // inserimentoDataGridViewTextBoxColumn
            // 
            this.inserimentoDataGridViewTextBoxColumn.DataPropertyName = "Inserimento";
            resources.ApplyResources(this.inserimentoDataGridViewTextBoxColumn, "inserimentoDataGridViewTextBoxColumn");
            this.inserimentoDataGridViewTextBoxColumn.Name = "inserimentoDataGridViewTextBoxColumn";
            this.inserimentoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // clienteDataGridViewTextBoxColumn
            // 
            this.clienteDataGridViewTextBoxColumn.DataPropertyName = "Cliente";
            resources.ApplyResources(this.clienteDataGridViewTextBoxColumn, "clienteDataGridViewTextBoxColumn");
            this.clienteDataGridViewTextBoxColumn.Name = "clienteDataGridViewTextBoxColumn";
            this.clienteDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.m_remove);
            this.groupBox1.Controls.Add(this.m_escape);
            this.groupBox1.Controls.Add(this.m_print);
            this.groupBox1.MinimumSize = new System.Drawing.Size(204, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_remove
            // 
            resources.ApplyResources(this.m_remove, "m_remove");
            this.m_remove.Name = "m_remove";
            this.m_remove.UseVisualStyleBackColor = true;
            this.m_remove.Click += new System.EventHandler(this.RemoveClickHandler);
            // 
            // m_escape
            // 
            resources.ApplyResources(this.m_escape, "m_escape");
            this.m_escape.Name = "m_escape";
            this.m_escape.UseVisualStyleBackColor = true;
            this.m_escape.Click += new System.EventHandler(this.EscapeClickHandler);
            // 
            // m_print
            // 
            resources.ApplyResources(this.m_print, "m_print");
            this.m_print.Name = "m_print";
            this.m_print.UseVisualStyleBackColor = true;
            this.m_print.Click += new System.EventHandler(this.PrintClickHandler);
            // 
            // m_close
            // 
            resources.ApplyResources(this.m_close, "m_close");
            this.m_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_close.Name = "m_close";
            this.m_close.UseVisualStyleBackColor = true;
            this.m_close.Click += new System.EventHandler(this.CloseClickHandler);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Nominativo";
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TipoCliente";
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Sconto";
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ScontoComitiva";
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Nominativo";
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "TipoCliente";
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Sconto";
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "ScontoComitiva";
            resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Nominativo";
            resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "TipoCliente";
            resources.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "Sconto";
            resources.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "ScontoComitiva";
            resources.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // m_lableTitle
            // 
            resources.ApplyResources(this.m_lableTitle, "m_lableTitle");
            this.m_lableTitle.ForeColor = System.Drawing.Color.Black;
            this.m_lableTitle.Name = "m_lableTitle";
            // 
            // Codice
            // 
            this.Codice.DataPropertyName = "CodiceCompleto";
            resources.ApplyResources(this.Codice, "Codice");
            this.Codice.Name = "Codice";
            this.Codice.ReadOnly = true;
            // 
            // Nominativo
            // 
            this.Nominativo.DataPropertyName = "Nominativo";
            resources.ApplyResources(this.Nominativo, "Nominativo");
            this.Nominativo.Name = "Nominativo";
            this.Nominativo.ReadOnly = true;
            // 
            // Cliente
            // 
            this.Cliente.DataPropertyName = "TipoCliente";
            resources.ApplyResources(this.Cliente, "Cliente");
            this.Cliente.Name = "Cliente";
            this.Cliente.ReadOnly = true;
            // 
            // Sconto
            // 
            this.Sconto.DataPropertyName = "Sconto";
            resources.ApplyResources(this.Sconto, "Sconto");
            this.Sconto.Name = "Sconto";
            this.Sconto.ReadOnly = true;
            // 
            // ScontoComitiva
            // 
            this.ScontoComitiva.DataPropertyName = "ScontoComitiva";
            resources.ApplyResources(this.ScontoComitiva, "ScontoComitiva");
            this.ScontoComitiva.Name = "ScontoComitiva";
            this.ScontoComitiva.ReadOnly = true;
            // 
            // Prezzo
            // 
            this.Prezzo.DataPropertyName = "PrezzoScontato";
            resources.ApplyResources(this.Prezzo, "Prezzo");
            this.Prezzo.Name = "Prezzo";
            this.Prezzo.ReadOnly = true;
            // 
            // Ingresso
            // 
            this.Ingresso.DataPropertyName = "OraIngresso";
            resources.ApplyResources(this.Ingresso, "Ingresso");
            this.Ingresso.Name = "Ingresso";
            this.Ingresso.ReadOnly = true;
            // 
            // Uscita
            // 
            this.Uscita.DataPropertyName = "OraUscita";
            resources.ApplyResources(this.Uscita, "Uscita");
            this.Uscita.Name = "Uscita";
            this.Uscita.ReadOnly = true;
            // 
            // Uscito
            // 
            this.Uscito.DataPropertyName = "Uscito";
            resources.ApplyResources(this.Uscito, "Uscito");
            this.Uscito.Name = "Uscito";
            this.Uscito.ReadOnly = true;
            this.Uscito.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Uscito.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // briefingDataGridViewTextBoxColumn1
            // 
            this.briefingDataGridViewTextBoxColumn1.DataPropertyName = "Briefing";
            resources.ApplyResources(this.briefingDataGridViewTextBoxColumn1, "briefingDataGridViewTextBoxColumn1");
            this.briefingDataGridViewTextBoxColumn1.Name = "briefingDataGridViewTextBoxColumn1";
            this.briefingDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // ClientiView
            // 
            this.AcceptButton = this.m_close;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_close;
            this.Controls.Add(this.m_lableTitle);
            this.Controls.Add(this.m_close);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_grid);
            this.Name = "ClientiView";
            ((System.ComponentModel.ISupportInitialize)(this.m_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_clientiBinding)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView m_grid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button m_remove;
        private System.Windows.Forms.Button m_escape;
        private System.Windows.Forms.Button m_print;
        private System.Windows.Forms.Button m_close;
        private System.Windows.Forms.BindingSource m_clientiBinding;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn briefingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codiceClienteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codiceNominativoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codiceCompletoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oraIngressoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oraUscitaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prezzoBaseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prezzoPersonaleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn prezzoScontatoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scontoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scontoComitivaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipoClienteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn uscitoDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataInserimentoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nominativoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn inserimentoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clienteDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label m_lableTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Codice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nominativo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sconto;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScontoComitiva;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prezzo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ingresso;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uscita;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Uscito;
        private System.Windows.Forms.DataGridViewTextBoxColumn briefingDataGridViewTextBoxColumn1;
    }
}