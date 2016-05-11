using System;

namespace IndianaPark.PercorsiAvventura.Model 
{
	/// <summary>
	/// Abbonamento a scadenza temporale
	/// </summary>
	public class AbbonamentoTempo : Abbonamento 
	{
		#region Fields 

		private readonly DateTime m_scadenza;

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Costruttore dell'oggetto
        /// </summary>
        /// <param name="nominativo">Nominativo a cui è registrato l'abbonamento</param>
        /// <param name="emissione">La data di emissione</param>
        /// <param name="prezzo">Il riferimento all'istanza del prezzo base dell'abbonamento</param>
        /// <param name="scadenza">Data di scadenza dell'abbonamento</param>
        public AbbonamentoTempo( string nominativo, DateTime emissione, PrezzoBase prezzo, DateTime scadenza ) : base( nominativo, emissione, prezzo )
        {
            // Controllo la coerenza dei parametri
            if( emissione > scadenza )
            {
                throw new ArgumentOutOfRangeException( "emissione", "The parameter must respect the rule emissione <= scadenza" );
            }

            this.m_scadenza = scadenza;
        }

		#endregion Constructors 

		#region Public Methods 

	    /// <summary>
	    /// Creates a new object that is a copy of the current instance.
	    /// </summary>
	    /// <returns>
	    /// A new object that is a copy of this instance.
	    /// </returns>
	    /// <filterpriority>2</filterpriority>
	    public override Abbonamento Clone()
	    {
            return new AbbonamentoTempo( this.m_nominativo, this.m_emissione, this.m_costo, this.m_scadenza );
	    }

	    /// <summary>
		/// Controlla che l'abbonamento sia ancora valido. Ogni implementazione utilizza i
		/// propri criteri per determinare la validità
		/// </summary>
		public override bool ControllaValidita()
		{
			return( DateTime.Now > m_scadenza );
		}

        /// <summary>
        /// gettipoabbonamento dovrebbe semplicemente dare in uscita il nome del tipo dell'abbonamento (a ingressi o a tempo)
        /// </summary>
        /// <returns></returns>
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
            return ControllaValidita();
		}

		#endregion Public Methods 

		#endregion Methods 
	}
}