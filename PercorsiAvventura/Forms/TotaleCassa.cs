using System;
using System.Linq;
using IndianaPark.Tools.Controls;

namespace IndianaPark.PercorsiAvventura.Forms
{
    public partial class TotaleCassa : BaseDialogForm
    {
        public TotaleCassa()
        {
            InitializeComponent();
            CalcolaTotale();
            CategorizzaClienti();
        }

        private void CalcolaTotale()
        {
            decimal totale = 0;
            var parco = Model.Parco.GetParco();
            foreach( var nominativo in parco.ListaClienti )
            {
                foreach( var inserimento in nominativo.Value )
                {
                    totale += inserimento.CalcolaPrezzo();
                }
            }

            this.m_totaleLabel.Text = string.Format( this.m_totaleLabel.Text, totale );
        }

        private void CategorizzaClienti()
        {
            var parco = Model.Parco.GetParco();
            var clienti = Model.Parco.GetFullRawList();

            var categorie = from cliente in clienti
                            where cliente.OraIngresso >= parco.OrarioApertura && cliente.OraUscita <= parco.OrarioChiusura
                            group cliente by new { cliente.TipoCliente, cliente.Sconto, cliente.ScontoComitiva, Prezzo = cliente.GetPrezzoScontato() } into categoria
                            select new
                            {
                                Nome = string.Format( "{0} {1}", categoria.Key.TipoCliente.Nome, categoria.Key.ScontoComitiva ?? categoria.Key.Sconto ),
                                PrezzoUnitario = categoria.Key.Prezzo,
                                Quantità = categoria.Count(),
                                Totale = categoria.Sum( c => c.GetPrezzoScontato() )
                            };

            categorie = categorie.OrderBy( k => k.Nome );
            foreach( var c in categorie )
            {
                var nuovo = this.m_clientiList.Items.Add( new System.Windows.Forms.ListViewItem( new[] {
                    c.Nome,
                    string.Format( "{0,-10:C}", c.PrezzoUnitario ),
                    c.Quantità.ToString(),
                    string.Format( "{0,-10:C}", c.Totale ),
                } ) );

                nuovo.Tag = new { c.Nome, c.PrezzoUnitario, c.Quantità, c.Totale };
            }
        }

        private void button1_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void m_clientiList_ItemSelectionChanged( object sender, System.Windows.Forms.ListViewItemSelectionChangedEventArgs e )
        {
            decimal prezzoTotale = 0;
            uint quantitàTotale = 0;

            foreach( int i in this.m_clientiList.SelectedIndices )
            {
                prezzoTotale += decimal.Parse( this.m_clientiList.Items[i].SubItems[3].Text, System.Globalization.NumberStyles.Currency );
                quantitàTotale += uint.Parse( this.m_clientiList.Items[i].SubItems[2].Text );
            }

            this.m_lablePrezzoSelezionato.Text = string.Format( "{0,-10:C}", prezzoTotale );
            this.m_lableQuantitàSelezionata.Text = quantitàTotale.ToString();
        }
    }
}
