namespace IndianaPark.Forms
{
    /// <summary>
    /// Finestra per la visualizzazione dei plugin caricati e per la loro configurazione
    /// </summary>
    partial class Plugins
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Plugins ) );
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonParametri = new System.Windows.Forms.Button();
            this.listPlugins = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnLocation = new System.Windows.Forms.ColumnHeader();
            this.columnExecutable = new System.Windows.Forms.ColumnHeader();
            this.columnOwners = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.AccessibleDescription = null;
            this.buttonOk.AccessibleName = null;
            resources.ApplyResources( this.buttonOk, "buttonOk" );
            this.buttonOk.BackgroundImage = null;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonParametri
            // 
            this.buttonParametri.AccessibleDescription = null;
            this.buttonParametri.AccessibleName = null;
            resources.ApplyResources( this.buttonParametri, "buttonParametri" );
            this.buttonParametri.BackgroundImage = null;
            this.buttonParametri.Name = "buttonParametri";
            this.buttonParametri.UseVisualStyleBackColor = true;
            this.buttonParametri.Click += new System.EventHandler( this.buttonParametri_Click );
            // 
            // listPlugins
            // 
            this.listPlugins.AccessibleDescription = null;
            this.listPlugins.AccessibleName = null;
            resources.ApplyResources( this.listPlugins, "listPlugins" );
            this.listPlugins.BackgroundImage = null;
            this.listPlugins.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnLocation,
            this.columnExecutable,
            this.columnOwners} );
            this.listPlugins.Font = null;
            this.listPlugins.FullRowSelect = true;
            this.listPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listPlugins.MultiSelect = false;
            this.listPlugins.Name = "listPlugins";
            this.listPlugins.ShowGroups = false;
            this.listPlugins.UseCompatibleStateImageBehavior = false;
            this.listPlugins.View = System.Windows.Forms.View.Details;
            this.listPlugins.DoubleClick += new System.EventHandler( this.listPlugins_DoubleClick );
            this.listPlugins.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler( this.listPlugins_ItemSelectionChanged );
            // 
            // columnName
            // 
            resources.ApplyResources( this.columnName, "columnName" );
            // 
            // columnLocation
            // 
            resources.ApplyResources( this.columnLocation, "columnLocation" );
            // 
            // columnExecutable
            // 
            resources.ApplyResources( this.columnExecutable, "columnExecutable" );
            // 
            // columnOwners
            // 
            resources.ApplyResources( this.columnOwners, "columnOwners" );
            // 
            // Plugins
            // 
            this.AcceptButton = this.buttonOk;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources( this, "$this" );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.buttonOk;
            this.Controls.Add( this.listPlugins );
            this.Controls.Add( this.buttonParametri );
            this.Controls.Add( this.buttonOk );
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Plugins";
            this.ShowInTaskbar = false;
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonParametri;
        private System.Windows.Forms.ListView listPlugins;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnLocation;
        private System.Windows.Forms.ColumnHeader columnExecutable;
        private System.Windows.Forms.ColumnHeader columnOwners;
    }
}