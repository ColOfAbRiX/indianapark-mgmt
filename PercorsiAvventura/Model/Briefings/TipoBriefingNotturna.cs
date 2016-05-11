using System;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Tipologia di briefing fittiza che rappresenta l'assenza di briefing per una tipologia cliente. Quando
    /// richiesto un briefing ne viene creato uno con la data corrente
    /// </summary>
    public class TipoBriefingNotturna : TipoBriefingBase
    {
        /// <summary>
        /// Indica se all'utente viene permesso di scegliere il briefing
        /// </summary>
        /// <remarks>
        /// Se il campo vale <c>true</c> viene richiesto il briefing più vicino all'ora attuale.
        /// </remarks>
        public override bool AskUser
        {
            get { return false; }
        }

        /// <summary>
        /// Il costruttore per un TipoBriefing
        /// </summary>
        /// <param name="nome">Il nome dei briefing</param>
        public TipoBriefingNotturna( string nome ) : base( new TimeSpan( 0, 21, 30 ), nome, 40, new TimeSpan( 0, 0, 0 ) )
        {
            var tmp = (TimeSpan)PluginPercorsi.GetGlobalParameter( "NotturnaTime" ).Value;
            var notturna = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tmp.Hours, tmp.Milliseconds, tmp.Seconds );
            this.m_briefings.Add( Briefing.CreaBriefing( notturna, this ) );
        }

        /// <summary>
        /// Recupera il primo briefing successivo all'orario indicato
        /// </summary>
        /// <param name="orario">Il primo briefing successivo all'orario indicato, oppure null se non trovato</param>
        public override IBriefing TrovaBriefing( DateTime orario )
        {
            var notturna = m_briefings[0];

            if( orario > notturna.Inizio + notturna.Durata )
            {
                return null;
            }

            return notturna;
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
            var notturna = m_briefings[0];
            notturna.Inizio = inizio;
        }
    }
}
