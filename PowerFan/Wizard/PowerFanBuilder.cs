using System;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using IndianaPark.Biglietti;

namespace IndianaPark.PowerFan.Wizard
{
    /// <summary>
    /// Il Builder per costruire i dati finali del modello PowerFan
    /// </summary>
    public class PowerFanBuilder : Tools.Wizard.GenericWizardBuilderBase<PowerFanStorage, IPrintableTickets>, IPrintableTickets
    {
        private PowerFanStorage m_storage = new PowerFanStorage { TipoBiglietto = typeof( Model.BigliettoPowerFan ) };
        private readonly List<Model.Biglietto> m_aggiunti = new List<PowerFan.Model.Biglietto>();

        /// <summary>
        /// Il contenitore di dati per il Builder dei biglietti
        /// </summary>
        public override PowerFanStorage Storage
        {
            get { return this.m_storage; }
            protected set { this.m_storage = value; }
        }

        /// <summary>
        /// Costruisce e restituisce il risultato della costruzione
        /// </summary>
        /// <returns><c>true</c> se la funziona ha costruito i dati con successo, <c>false</c> altrimenti.</returns>
        public override bool BuildResult()
        {
            if( this.m_storage.Quantity == 0 )
            {
                return false;
            }

            m_aggiunti.Clear();

            for( int i = 0; i < this.m_storage.Quantity; i++ )
            {
                this.m_aggiunti.Add( (Model.Biglietto)Activator.CreateInstance( this.m_storage.TipoBiglietto, (decimal)this.m_storage.Prezzo ) );
            }

            Model.Torre.GetTorre().BigliettiEmessi.AddRange( m_aggiunti );

            return true;
        }

        /// <summary>
        /// Restituisce il risultato costruito con <see cref="IWizardBuilder.BuildResult"/>
        /// </summary>
        /// <returns>
        /// L'oggetto costruito, oppure <c>null</c> se <see cref="IWizardBuilder.BuildResult"/> non è mai stata
        /// chiamata o la sua chiamata non è riuscita
        /// </returns>
        public override IPrintableTickets GetResult()
        {
            if( this.m_storage.Quantity == 0 )
            {
                return null;
            }

            return this;
        }

        /// <summary>
        /// Recupera il report del ticket
        /// </summary>
        /// <returns>Un'istanza del report del ticket</returns>
        /// <remarks>
        /// Al report è gia associato un insieme di dati.
        /// </remarks>
        public ReportClass GetPrintableReport()
        {
            if( this.m_storage.Quantity > 0 )
            {
                var report = new Reports.TicketReport();
                report.SetDataSource( this.m_aggiunti );
                return report;
            }

            return null;
        }
    }

    /// <summary>
    /// Immagazzina i dati temporanei per il Builder
    /// </summary>
    public class PowerFanStorage
    {
        private Type m_tipoBiglietto;

        /// <summary>
        /// Il numero di biglietti da stampare
        /// </summary>
        public uint Quantity { get; set; }

        /// <summary>
        /// La tipologia di biglietti da stampare
        /// </summary>
        public Type TipoBiglietto
        {
            get { return this.m_tipoBiglietto; }
            set
            {
                if( !value.IsSubclassOf( typeof( Model.Biglietto ) ) )
                {
                    throw new ArgumentException();
                }

                this.m_tipoBiglietto = value;
            }
        }

        /// <summary>
        /// Il prezzo di un biglietto
        /// </summary>
        public double Prezzo { get; set; }
    }
}
