using System;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Rappresenta un briefing generico con un'ora d'inizio.
    /// </summary>
    public interface IBriefing : ICloneable, IComparable<IBriefing>, IEquatable<IBriefing>
    {
        /// <summary>
        /// Elenco dei clienti inseriti in questo briefing
        /// </summary>
        List<Cliente> Clienti { get; set; }

        /// <summary>
        /// Durata del briefing, recuperato dalla tipologia di briefing
        /// </summary>
        TimeSpan Durata { get; }

        /// <summary>
        /// Ora esatta dell'inizio del briefing
        /// </summary>
        DateTime Inizio { get; set; }

        /// <summary>
        /// Numero di posti già occupati dai clienti nel briefing
        /// </summary>
        uint PostiOccupati { get; }

        /// <summary>
        /// Contiene un riferimento all'istanza che identifica il tipo di questo briefing
        /// </summary>
        ITipoBriefing TipoBriefing { get; }

        /// <summary>
        /// Controlla se ci sono posti liberi nel briefing confrontando il numero posti disponibili del tipo
        /// di briefing con i posti occupati nel briefing
        /// </summary>
        /// <returns>True se i posti occupati sono minori dei posti totali </returns>
        bool CheckPostiLiberi();

        /// <summary>
        /// Occupa aggiunge il cliente al briefing di cui si intende usufruire inoltre compila il campo
        /// del cliente col il briefing corretto. Questa funzione exception <see cref="Briefing.CheckPostiLiberi"/> non hanno
        /// nessun collegamento, quindi è possibile occupare posti anche <see cref="Briefing.CheckPostiLiberi"/> ritorna
        /// come valore <c>false</c>. La funzione si occupa anche di aggiornare il riferimenti del briefing del cliente
        /// indicato collegandolo a questa istanza di Briefing
        /// </summary>
        /// <param name="cliente">Il cliente da aggiungere al briefing</param>
        void Occupa( Cliente cliente );

        /// <summary>
        /// Toglie il cliente dalla lista clienti del briefing e avvisa l'istanza <see cref="Cliente"/> che non
        /// appartiene più a nessun briefing.
        /// </summary>
        /// <param name="cliente">Il cliente da rimuovere dal briefing</param>
        void Libera( Cliente cliente );

        /// <summary>
        /// Toglie tutti i clienti assegnati a questo briefing
        /// </summary>
        void Svuota();
    }
}