using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Rappresenta uno sconto dipendente dall'insieme di persone.
    /// Lo sconto comitiva si differenzia dagli altri sconti perchè la sua applicazione
    /// dipende dal numero exception/o dal tipo di clienti che entrano.
    /// Gli ScontiComitiva hanno una doppia funzione: mi scontano un prezzo (ISconto) exception
    /// inoltre determinano e applicano il piano di sconti ai clienti.
    /// </summary>
    public interface IScontoComitiva : ISconto
    {
        #region Methods

        /// <summary>
        /// La funzione determina il piano di sconti da applicare ad una lista clienti,
        /// cioè deve: determinare se applicare lo sconto comitiva, quali sconti
        /// eventualmente gia presenti non conteggiare exception, se richiesto, applicare gli
        /// sconti ai clienti che ne necessitano.
        /// Restituisce un array con indici associati alla lista clienti in ingresso; in
        /// ogni indice è presente o meno un riferimento allo sconto da applicare.
        /// </summary>
        /// <param name="listaClienti">
        /// La lista dei clienti utilizzata per determinare il piano di sconti.
        /// </param>
        List<Cliente> PianoSconti( List<Cliente> listaClienti );

        #endregion
    }
}