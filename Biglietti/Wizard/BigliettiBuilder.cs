using IndianaPark.Tools.Wizard;

namespace IndianaPark.Biglietti.Wizard
{
    /// <summary>
    /// Il costruttore dei dati per l'emissione generica di biglietti
    /// </summary>
    public class BigliettiBuilder : GenericWizardBuilderBase<BigliettiBuilder.AttrazioneStorage, IPrintableTickets>
    {
		#region Nested Classes 

        /// <summary>
        /// Il contenitore di dati per il Builder dei percorsi avventura
        /// </summary>
        public class AttrazioneStorage
        {
		    #region Fields 

            /// <summary>
            /// L'oggetto <see cref="IPrintableTickets"/> che è stato salvato dal wizard
            /// </summary>
            public IPrintableTickets TicketObject { get; set; }

    		#endregion Fields 
        }

		#endregion Nested Classes 

		#region Fields 

		#region Internal Fields 

        private AttrazioneStorage m_storage = new AttrazioneStorage();

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Il contenitore di dati per il Builder dei biglietti
        /// </summary>
        public override AttrazioneStorage Storage
        {
            get { return this.m_storage; }
            protected set { this.m_storage = value; }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Public Methods 

        /// <summary>
        /// Costruisce e restituisce il risultato della costruzione
        /// </summary>
        /// <returns><c>true</c> se la funziona ha costruito i dati con successo, <c>false</c> altrimenti.</returns>
        public override bool BuildResult()
        {
            return (this.m_storage.TicketObject != null);
        }

        /// <summary>
        /// Restituisce il risultato costruito con <see cref="IBuilder.BuildResult"/>
        /// </summary>
        /// <returns>
        /// L'oggetto costruito, oppure <c>null</c> se <see cref="IBuilder.BuildResult"/> non è mai stata
        /// chiamata o la sua chiamata non è riuscita
        /// </returns>
        public override IPrintableTickets GetResult()
        {
            return this.m_storage.TicketObject;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}