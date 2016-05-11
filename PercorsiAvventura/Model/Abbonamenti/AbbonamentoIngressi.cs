using System;

namespace IndianaPark.PercorsiAvventura.Model 
{
	/// <summary>
	/// Abbonamento valido per N ingressi.
	/// </summary>
	public class AbbonamentoIngressi : Abbonamento
	{
		#region Fields

        #region Internals

        /// <summary>
        /// Il numero di ingressi totali per cui è progettato l'abbonamento
        /// </summary>
        private readonly uint m_ingressiTotali;
        /// <summary>
        /// Il numero di ingressi utilizzati di quelli totali disponibili
        /// </summary>
		private uint m_ingressiUtilizzati;

        #endregion

        #endregion

        #region Constructors
        
        /// <summary>
        /// Costruttore dell'oggetto
        /// </summary>
        /// <param name="nominativo">Nominativo a cui è registrato l'abbonamento</param>
        /// <param name="emissione">La data di emissione</param>
        /// <param name="prezzo">Il riferimento all'istanza del prezzo base dell'abbonamento</param>
        /// <param name="totali">Numero di ingressi totali permessi dall'abbonamento</param>
        /// <param name="usati">Numero di ingressi utilizzati</param>
        public AbbonamentoIngressi( string nominativo, DateTime emissione, PrezzoBase prezzo, uint totali, uint usati ) : base( nominativo, emissione, prezzo )
        {
            // Controllo la coerenza dei parametri
            if( usati > totali )
            {
                throw new ArgumentOutOfRangeException( "usati", "The parameter must respect the rule usati <= totali" );
            }

            this.m_ingressiTotali = totali;
            this.m_ingressiUtilizzati = usati;
        }

		#endregion

		#region Methods

	    /// <summary>
	    /// Creates a new object that is a copy of the current instance.
	    /// </summary>
	    /// <returns>
	    /// A new object that is a copy of this instance.
	    /// </returns>
	    /// <filterpriority>2</filterpriority>
	    public override Abbonamento Clone()
	    {
            return new AbbonamentoIngressi( this.m_nominativo, this.m_emissione, this.m_costo, this.m_ingressiTotali, this.m_ingressiUtilizzati );
	    }

	    /// <summary>
		/// Controlla che l'abbonamento sia ancora valido. Ogni implementazione utilizza i
		/// propri criteri per determinare la validità
		/// </summary>
		public override bool ControllaValidita()
		{
			return( m_ingressiTotali > m_ingressiUtilizzati );
		}

        /// <summary>
        /// GetTipoAbbonamento dovrebbe semplicemente dare in uscita il nome del tipo dell'abbonamento (a ingressi o a tempo)
        /// </summary>
        /// <remarks>DA DEFINIRE ESATTAMENTE NELL'AMBITO DELLA STRUTTURA DATI</remarks>
        /// <returns>Restituisce il tipo dell'oggetto base</returns>
        public override Type GetTipoAbbonamento()
        {
            return this.GetType();
        }

		/// <summary>
		/// Utilizza un'ingresso dell'abbonamento e restituisce un valore che indica se
		/// l'abbonamento è ancora valido e quindi è stato usato (non è valido se per
		/// esempio è scaduto).
		/// </summary>
		public override bool UsaAbbonamento()
		{
            if( this.ControllaValidita() )
            {
                this.m_ingressiUtilizzati++;
			    return true;
            }

            return false;
		}

		#endregion
	}
}