using System;
using CrystalDecisions.CrystalReports.Engine;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.Biglietti
{
    /// <summary>
    /// Implementato dagli oggetti che possono emettere biglietti.
    /// </summary>
    /// <remarks>
    /// Permette di recuperare le informazioni fondamentali sulle attrazioni e come poter inizializzare il
    /// wizard per l'emissione.
    /// </remarks>
    public interface IAttrazione
    {
        /// <summary>
        /// Recupera il nome visualizzato per l'attrazione
        /// </summary>
        /// <returns>Il nome dell'attrazione che verrà visualizzato all'utente</returns>
        string GetDisplayName();

        /// <summary>
        /// Recupera lo stato di partenza del wizard
        /// </summary>
        /// <returns>Lo stato di partenza del wizard per l'emissione dei biglietti</returns>
        Type GetFirstWizard();

        /// <summary>
        /// Recupera il costruttore dei dati utilizzato dall'attrazione
        /// </summary>
        /// <returns>Il costruttore dati</returns>
        IBuilder GetBuilder();
    }

    /// <summary>
    /// Rappresenta un ticket da stampare su una serie di dati
    /// </summary>
    public interface IPrintableTickets
    {
        /// <summary>
        /// Recupera il report del ticket
        /// </summary>
        /// <returns>Un'istanza del report del ticket</returns>
        /// <remarks>
        /// Al report è gia associato un insieme di dati.
        /// </remarks>
        ReportClass GetPrintableReport();
    }
}
