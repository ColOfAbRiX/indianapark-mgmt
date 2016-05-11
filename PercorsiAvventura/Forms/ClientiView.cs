using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using IndianaPark.Tools.Controls;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// Form per la visualizzazione della lista cliente e delle azioni da svolgere
    /// </summary>
    public partial class ClientiView : BaseDialogForm
    {
		#region Fields 

		#region Internal Fields 

        private readonly List<Model.ClienteWrapper> m_selected = new List<Model.ClienteWrapper>();

		#endregion Internal Fields 

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Pulsante per la chiusura della form
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CloseClickHandler( object sender, System.EventArgs e )
        {
            this.Close();
        }

        /// <summary>
        /// Stampa i biglietti delle righe selezionate
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PrintClickHandler( object sender, System.EventArgs e )
        {
            if( this.m_selected.Count == 0 )
            {
                return;
            }

            // Conferma dell'utente
            var dr = MessageBox.Show(
                string.Format( Properties.Resources.PrintTicketText, this.m_selected.Count ),
                Properties.Resources.PrintTicketTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
            if( dr == DialogResult.No )
            {
                return;
            }

            var report = new Reports.TicketReport();
            report.SetDataSource( this.m_selected );
            Biglietti.PluginBiglietti.Print( report );
        }

        /// <summary>
        /// Esegue la procedura di uscita per i clienti delle righe selezionate
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EscapeClickHandler( object sender, EventArgs e )
        {
            if( this.m_selected.Count == 0 )
            {
                return;
            }

            // Conferma dell'utente
            var dr = MessageBox.Show(
                string.Format( Properties.Resources.ClienteEscapeText, this.m_selected.Count ),
                Properties.Resources.ClienteEscapeTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
            if( dr == DialogResult.No )
            {
                return;
            }

            this.m_selected.ForEach( item => item.Cliente.UscitaCliente() );
            this.UpdateData();
            // PluginPercorsi.GetGlobalInstance().ModelPersistence.SaveModel();
        }

        /// <summary>
        /// Elimina i clienti delle righe selezionate
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RemoveClickHandler( object sender, EventArgs e )
        {
            if( this.m_selected.Count == 0 )
            {
                return;
            }

            // Conferma dell'utente
            var dr = MessageBox.Show(
                string.Format( Properties.Resources.ClienteRemoveText, this.m_selected.Count ),
                Properties.Resources.ClienteRemoveTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
            if( dr == DialogResult.No )
            {
                return;
            }

            var selected = this.m_selected;
            this.m_selected.ForEach(
                item =>
                {
                    item.Cliente.UscitaCliente();
                    item.Inserimento.Remove( item.Cliente );
                }
            );

            // Solo successivamente elimino i clienti dalla sorgente
            selected.ForEach( item =>this.m_clientiBinding.Remove( item ) );

            this.UpdateData();
            //PluginPercorsi.GetGlobalInstance().ModelPersistence.SaveModel();
        }

        private void GridCellFormattingHandler( object sender, DataGridViewCellFormattingEventArgs e )
        {
            var dgv = (DataGridView)sender;

            // Formattazione colonna del Sconto
            if( dgv.Columns[e.ColumnIndex].Name.Equals( "Sconto" ) )
            {
                var sconto = (Model.ISconto)e.Value;

                if( sconto == null )
                {
                    e.Value = "-";
                    return;
                }

                e.Value = sconto.ToString();
            }

            // Formattazione colonna del ScontoComitiva
            if( dgv.Columns[e.ColumnIndex].Name.Equals( "ScontoComitiva" ) )
            {
                var sconto = (Model.IScontoComitiva)e.Value;

                if( sconto == null )
                {
                    e.Value = "-";
                    return;
                }

                e.Value = sconto.ToString();
            }

            // Formattazione colonna del Prezzo
            if( dgv.Columns[e.ColumnIndex].Name.Equals( "Prezzo" ) )
            {
                e.Value = string.Format( "{0,-10:C}", e.Value );
            }

            // Formattazione colonna del Ingresso
            if( dgv.Columns[e.ColumnIndex].Name.Equals( "Ingresso" ) )
            {
                e.Value = DateTime.Parse( e.Value.ToString() ).ToShortTimeString();
            }

            // Formattazione colonna del Uscita
            if( dgv.Columns[e.ColumnIndex].Name.Equals( "Uscita" ) )
            {
                e.Value = DateTime.Parse( e.Value.ToString() ).ToShortTimeString();
            }
        }

        private void GridRowEnterHandler( object sender, DataGridViewCellEventArgs e )
        {
            this.m_selected.Clear();
            var dgv = (DataGridView)sender;

            foreach( DataGridViewRow row in dgv.SelectedRows )
            {
                var cw = from b in (IEnumerable<Model.ClienteWrapper>)this.m_clientiBinding.DataSource
                         where b.CodiceCompleto == (string)(row.Cells["Codice"].Value)
                         select b;
                if( cw.Count() > 0 )
                {
                    this.m_selected.Add( cw.ElementAt( 0 ) );
                }
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientiView"/> class.
        /// </summary>
        public ClientiView()
        {
            InitializeComponent();

            this.UpdateData();

            // Selezione della riga iniziale
            if( this.m_grid.Rows.Count > 0 )
            {
                this.m_grid.Rows[0].Selected = true;
            }
        }

		#endregion Constructors 

		#region Internal Methods 

        private void UpdateData()
        {
            var tutto = Model.ClienteWrapper.FromParco();

            this.m_clientiBinding.DataSource = tutto;

            this.m_grid.DataSource = this.m_clientiBinding;
            this.m_grid.Update();
        }

		#endregion Internal Methods 

        private void m_grid_ColumnHeaderMouseClick( object sender, DataGridViewCellMouseEventArgs e )
        {
            //this.m_grid.Sort( this.m_grid.Columns[exception.ColumnIndex], System.ComponentModel.ListSortDirection.Ascending );
        }

		#endregion Methods 
    }
}
