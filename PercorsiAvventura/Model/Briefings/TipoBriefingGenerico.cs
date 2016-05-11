using System;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Contiene una lista di tutti i briefing appartenenti ad una tipologia cliente exception
    /// ne rappresenta le proprietà comuni. Es. Briefing Adulti, Briefing Bambini,
    /// Briefing Notturna
    /// </summary>
    public class TipoBriefingGenerico : TipoBriefingBase
    {        
        #region Methods 

		#region Constructors 

        /// <summary>
        /// Il costruttore per un TipoBriefing
        /// </summary>
        /// <param name="durata">La durata di ogni briefing</param>
        /// <param name="nome">Il nome dei briefing</param>
        /// <param name="postiTotali">Il numero di posti disponibili in un briefing</param>
        /// <param name="walkTime">Il tempo che il cliente ci mette ad arrivare all'area briefing</param>
        public TipoBriefingGenerico( TimeSpan durata, string nome, uint postiTotali, TimeSpan walkTime ) : base( durata, nome, postiTotali, walkTime )
        {
        }

        #endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera il primo briefing successivo all'orario indicato
        /// </summary>
        /// <param name="orario">Il primo briefing successivo all'orario indicato, oppure null se non trovato</param>
        public override IBriefing TrovaBriefing( DateTime orario )
        {
            IBriefing selectedBriefing = null;

            // Controllo tutti i briefings
            foreach( IBriefing briefing in this.m_briefings )
            {
                // Controllo se il briefing è dopo l'orario specificato
                if( briefing.Inizio.CompareTo( orario ) >= 0 )
                {
                    // Controllo che questo briefing sia il PRIMO dopo l'orario di inizio
                    if( selectedBriefing == null || 
                      ( selectedBriefing != null && selectedBriefing.Inizio > briefing.Inizio ) )
                    {
                        selectedBriefing = briefing;
                    }
                }
            }

            return selectedBriefing;
        }

        /// <summary>
        /// Permette di cambiare l'orario di inizio e fine dei briefing
        /// </summary>
        /// <remarks>
        /// Se un briefing viene eliminato, le persone che vi erano registrare risulteranno senza briefing.
        /// </remarks>
        /// <param name="inizio">L'orario di inizio</param>
        /// <param name="fine">L'orario di fine</param>
        public override void ReimpostaOrari( DateTime inizio, DateTime fine )
        {
            var bufferTime = (TimeSpan)PluginPercorsi.GetGlobalParameter( "LastBriefingBefore" ).Value;

            // Risparimio energie
            if( inizio == m_orarioInizio && fine == m_orarioChiusura && bufferTime == m_bufferTime )
            {
                return;
            }
            m_orarioInizio = inizio;
            m_orarioChiusura = fine;

            var vecchi = this.m_briefings;

            m_briefings = Briefing.CreaListaBriefings( inizio, fine - bufferTime, this );
            m_briefings.Sort();

            foreach( var ob in vecchi )
            {
                if( ob.Inizio >= inizio && ob.Inizio + ob.TipoBriefing.Durata <= fine )
                {
                    // I briefing che sono dentro l'intervallo vengono trasferiti
                    ob.Clienti.ForEach( item => item.AssegnaBriefing( this.TrovaBriefing( ob.Inizio ) ) );
                }
                else
                {
                    // I briefing che sono fuori dal nuovo intervallo vengono svuotati
                    ob.Svuota();
                }
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}