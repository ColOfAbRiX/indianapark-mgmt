using System;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Tipologia di briefing fittiza che rappresenta l'assenza di briefing per una tipologia cliente. Quando
    /// richiesto un briefing ne viene creato uno con la data corrente
    /// </summary>
    public class TipoBriefingImmediato : TipoBriefingBase
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
        /// <param name="walkTime">Tempo che viene dato al cliente per raggiungere l'area briefing</param>
        public TipoBriefingImmediato( string nome, TimeSpan walkTime ) : base( new TimeSpan( 0, 0, 1 ), nome, 1, walkTime )
        {
        }

        /// <summary>
        /// Recupera il primo briefing successivo all'orario indicato
        /// </summary>
        /// <param name="orario">Il primo briefing successivo all'orario indicato, oppure null se non trovato</param>
        public override IBriefing TrovaBriefing( DateTime orario )
        {
            // Se faccio il biglietto troppo presto lo posticipo
            if( orario < Parco.GetParco().OrarioApertura )
            {
                orario = Parco.GetParco().OrarioApertura;
            }

            // Se faccio il biglietto troppo tardi non posso entrare
            DateTime limiteChiusura = Parco.GetParco().OrarioChiusura - (TimeSpan)PluginPercorsi.GetGlobalParameter( "LastBriefingBefore" ).Value;
            if( orario > limiteChiusura )
            {
                return null;
            }

            return Briefing.CreaBriefing( orario, this );
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
            return;
        }
    }
}
