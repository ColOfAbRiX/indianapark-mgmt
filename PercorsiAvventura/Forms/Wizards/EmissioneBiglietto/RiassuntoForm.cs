using System;
using System.Linq;
using System.Windows.Forms;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// Finestra di dialogo riassunti con tutti i dati selezionati dall'operatore
    /// </summary>
    public partial class RiassuntoForm : WizardForm
    {
		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Gestisce le richieste del controllo di navigazione della form
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void NavigationHandler( object source, Tools.Navigation.NavigationEventArgs e )
        {
            this.Sail( e.Status );
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="RiassuntoForm"/> class.
        /// </summary>
        public RiassuntoForm( Wizard.BigliettoPercorsiBuilder.PercorsiStorage dataStorage, Model.Inserimento dataInserimento )
        {
            if( dataStorage == null )
            {
                throw new ArgumentNullException( "dataStorage" );
            }
            if( dataInserimento == null )
            {
                throw new ArgumentNullException( "dataInserimento" );
            }

            InitializeComponent();

            // Visualizzo le informazioni
            this.FixedTexts( dataStorage, dataInserimento );
            this.TicketInformations( dataInserimento );
            this.BriefingInformations( dataStorage );
        }

		#endregion Constructors 

        private void FixedTexts( Wizard.BigliettoPercorsiBuilder.PercorsiStorage storage, Model.Inserimento insert )
        {
            // Imposto i testi fissi
            this.m_tipoBiglietto.Text = String.Format( this.m_tipoBiglietto.Text, storage.TipoBiglietto.Nome );
            this.m_nominativo.Text = String.Format( this.m_nominativo.Text, storage.Nominativo );
            this.m_totale.Text = String.Format( this.m_totale.Text, insert.CalcolaPrezzo() );
            this.m_totalePersone.Text = String.Format( this.m_totalePersone.Text, insert.Count );
        }

        private void TicketInformations( Model.Inserimento insert )
        {
            // Aggiungo le informazioni suddivise per cliente

            var crTickets = new BigliettiLabelCreator( Properties.Resources.RiassuntoBiglietti );
            this.m_repeaterBiglietti.Creator = crTickets;

            this.m_repeaterBriefings.Anchor = AnchorStyles.Bottom;
            this.m_line1.Anchor = AnchorStyles.Bottom;

            // Recupero le informazioni dai dati dell'inserimento
            var distincion = 
                from c in insert
                group c by new { c.TipoCliente, c.Sconto, c.ScontoComitiva } into gr
                select new
                {
                    gr.Key.TipoCliente,
                    gr.Key.Sconto,
                    gr.Key.ScontoComitiva,
                    Prezzo = (from c in insert
                              where c.TipoCliente == gr.Key.TipoCliente &&
                                    c.Sconto == gr.Key.Sconto &&
                                    c.ScontoComitiva == gr.Key.ScontoComitiva
                              select c.GetPrezzoScontato()).ElementAt( 0 ),
                    Quantita = gr.Count()
                };

            foreach( var kind in distincion )
            {
                crTickets.TipoCliente = kind.TipoCliente;
                crTickets.Quantita = kind.Quantita;
                crTickets.Sconto = kind.Sconto ?? kind.ScontoComitiva;
                crTickets.Prezzo = kind.Prezzo;

                this.m_repeaterBiglietti.Add();
            }
        }

        private void BriefingInformations( Wizard.BigliettoPercorsiBuilder.PercorsiStorage storage )
        {
            // Inizializzo i repeaters
            var crBriefs = new BriefingLabelCreator( Properties.Resources.RiassuntoBriefings );
            this.m_repeaterBriefings.Creator = crBriefs;

            // Aggiungo gli orari di inizio dei briefings
            this.m_repeaterBriefings.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom);
            this.m_line1.Anchor = AnchorStyles.Top;
            foreach( var a in storage.Clienti )
            {
                if( a.Quantita > 0 && a.ScontoPersonale == null )
                {
                    crBriefs.Briefing = a.Briefing;
                    crBriefs.TipoCliente = a.TipoCliente;

                    this.m_repeaterBriefings.Add();
                }
            }
        }

        #endregion Methods 
    }
}