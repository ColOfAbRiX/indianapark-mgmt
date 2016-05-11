using System;
using System.Collections;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Rappresenta una tipologia di briefing, contiene tutti i briefing schedulati per questa tipologia,
    /// exception le proprietà comuni.
    /// </summary>
    public interface ITipoBriefing : IEnumerable, ICloneable, IEquatable<ITipoBriefing>
    {
        /// <summary>
        /// La durata del briefing
        /// </summary>
        TimeSpan Durata { get; }

        /// <summary>
        /// Tempo che la persona ci mette a raggiungere l'area briefing.
        /// </summary>
        TimeSpan WalkTime { get; }

        /// <summary>
        /// Il nome del tipo briefing
        /// </summary>
        string Nome { get; }

        /// <summary>
        /// Il numero di posti totali in un briefing
        /// </summary>
        uint PostiTotali { get; }

        /// <summary>
        /// Indica se all'utente viene permesso di scegliere il briefing
        /// </summary>
        /// <remarks>
        /// Se il campo vale <c>true</c> viene richiesto il briefing più vicino all'ora attuale.
        /// </remarks>
        bool AskUser { get; }

        /// <summary>
        /// Recupera il primo briefing successivo all'orario indicato
        /// </summary>
        /// <param name="orario">Il primo briefing successivo all'orario indicato, oppure null se non trovato</param>
        IBriefing TrovaBriefing( DateTime orario );

        /// <summary>
        /// Permette di cambiare l'orario di inizio e fine dei briefing
        /// </summary>
        /// <remarks>
        /// Se un briefing viene eliminato, le persone che vi erano registrare risulteranno senza briefing.
        /// </remarks>
        /// <param name="inizio">L'orario di inizio</param>
        /// <param name="fine">L'orario di fine</param>
        void ReimpostaOrari( DateTime inizio, DateTime fine );
    }
}