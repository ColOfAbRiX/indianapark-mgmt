using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IndianaPark.PercorsiAvventura.Model;

namespace IndianaPark.PercorsiAvventura.Pannelli
{
    /// <summary>
    /// Pannello che visualizza le statische veloci sul parco
    /// </summary>
    public partial class StatisticPanel : UserControl
    {
        private readonly Model.Parco m_parco = Model.Parco.GetParco();
        private List<Model.Cliente> m_clienti;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticPanel"/> class.
        /// </summary>
        public StatisticPanel()
        {
            InitializeComponent();

            var creator = new Tools.Controls.TypeControlCreator( typeof( Pannelli.DescriptionValueRow ) );
            this.m_rptWorking.Creator = creator;
            this.m_rptWholeDay.Creator = creator;
            this.m_rptNextReturns.Creator = creator;
            this.m_rptInfoBriefings.Creator = creator;

            this.m_timerUpdate.Start();
            this.UpdateStatistics();
        }

        /// <summary>
        /// Aggiorna le statistiche
        /// </summary>
        public void UpdateStatistics()
        {
            this.m_clienti = Model.Parco.GetFullRawList();

            this.BuildSituation();
            this.BuildTotalOfTheDay();
            this.BuildNextReturns();
            this.BuildBriefingStatistics();
            this.BuildMoneyStatistics();
        }

        /// <summary>
        /// Crea i totali della situazione corrente nel parco.
        /// </summary>
        /// <remarks>
        /// La situazione corrente del parco è il conteggio delle persone non ancora uscite e presenti
        /// dall'apertura del parco ad adesso.
        /// </remarks>
        private void BuildSituation()
        {
            this.BuildStatistic( this.m_rptWorking, this.m_clienti,
                item =>
                (
                    item.Uscito == false &&
                    item.OraIngresso <= DateTime.Now &&
                    item.OraIngresso >= m_parco.OrarioApertura
                )
            );
        }

        /// <summary>
        /// Crea i totali della giornata
        /// </summary>
        /// <remarks>
        /// Il totale del giorno è il conteggio dei clienti dall'apertura alla chiusura
        /// </remarks>
        private void BuildTotalOfTheDay()
        {
            this.BuildStatistic( this.m_rptWholeDay, this.m_clienti,
                item =>
                (
                    item.OraIngresso >= m_parco.OrarioApertura &&
                    item.OraUscita <= m_parco.OrarioChiusura
                )
            );
        }

        /// <summary>
        /// Crea i totali delle persone al rientro nei prossimi minuti
        /// </summary>
        /// <remarks>
        /// Le persone al rientro nei prossimi N minuti sono le persone nell'arco della giornata che sono entrate
        /// e non sono ancora uscite, e il cui orario di rientro è nei prossimi X minuti
        /// </remarks>
        private void BuildNextReturns()
        {
            try
            {
                var seeForwardTime = (TimeSpan)PluginPercorsi.GetGlobalParameter( "SeeForwardMinutes" ).Value;

                this.groupBox5.Text = String.Format( this.groupBox5.Text, seeForwardTime.TotalMinutes );
                this.BuildStatistic( this.m_rptNextReturns, this.m_clienti,
                     item =>
                     (
                         item.Uscito == false &&
                         item.OraIngresso <= DateTime.Now &&
                         item.OraUscita <= DateTime.Now + seeForwardTime
                     )
                 );
            }
            catch( Exception )
            {
            }
        }

        /// <summary>
        /// Crea delle statistiche sui briefing
        /// </summary>
        private void BuildBriefingStatistics()
        {
        }

        private void BuildMoneyStatistics()
        {
        }

        /// <summary>
        /// Crea o aggiorna un gruppo di statistiche conteggiando il numero di clienti che rispettano una condizione
        /// </summary>
        /// <param name="rpt">Il repeater a cui aggiungere/aggiornare i dati</param>
        /// <param name="list">La lista sulla quale lavorare</param>
        /// <param name="condition">La condizione per il conteggio delle statistiche</param>
        protected void BuildStatistic( Tools.Controls.LinkedRepeater<TipoCliente> rpt, IEnumerable<Model.Cliente> list, Func<Cliente, bool> condition )
        {
            if( m_parco.TipologieBiglietto.ContainsKey( "base" ) )
            {
                //foreach( var tipo in TipoCliente.GetAllTipologie() )
                foreach( var tipo in m_parco.TipologieBiglietto["base"].TipiCliente.Values )
                {
                    // Trovo il numero totale di clienti fino all'ultimo briefing incominciato
                    var listaTipo = list.Where( item=>item.TipoCliente == tipo );
                    var totale = listaTipo.ToList().Count( item=>condition( item ) );
                    var row = (DescriptionValueRow)rpt.Add( tipo );

                    row.Description = String.Format( Properties.Resources.TotaleBigliettiUntilNow, tipo.Nome );
                    row.Value = totale.ToString();
                }
            }
        }

        private void m_timerUpdate_Tick( object sender, EventArgs e )
        {
            this.UpdateStatistics();
        }
    }
}
