using System;

namespace IndianaPark.PercorsiAvventura.Model 
{
    /// <summary>
    /// E' una classe astratta che implementa i metodi comuni a tutti i tipi di
    /// abbonamento.
    /// </summary>
	public abstract class Abbonamento : IAbbonamento
	{
		#region Fields 

		#region Internal Fields 

        /// <summary>
		/// La data di emissione dell'abbonamento.
		/// </summary>
        protected readonly DateTime m_emissione;
		/// <summary>
		/// Il nominativo a cui è registrato l'abbonamento.
		/// </summary>
        protected readonly string m_nominativo;
        /// <summary>
        /// Il riferimento al prezzo base dell'abbonamento
        /// </summary>
		protected readonly PrezzoBase m_costo;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Il costo dell'abbonamento nuovo. In sola lettura
        /// </summary>
        public decimal CostoAbbonamento
        {
            get
            {
                return m_costo.Prezzo;
            }
        }

        /// <summary>
        /// La data di emissione dell'abbonamento. In sola lettura
        /// </summary>
        public DateTime Emissione
        {
            get
            {
                return this.m_emissione;
            }
        }

        /// <summary>
        /// Il nominativo a cui è registrato l'abbonamento
        /// </summary>
        public string Nominativo
        {
            get
            {
                return this.m_nominativo;
            }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

        #region Operators

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi rappresentano la stessa istanza</returns>
        public static bool operator ==( Abbonamento leftOperand, Abbonamento rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( Abbonamento leftOperand, Abbonamento rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        #endregion Operators
        
        #region Constructors 

        /// <summary>
        /// Il costruttore dell'oggetto
        /// </summary>
        /// <param name="nominativo">Nominativo a cui è registrato l'abbonamento</param>
        /// <param name="emissione">La data di emissione</param>
        /// <param name="prezzo">Il riferimento all'istanza del prezzo base dell'abbonamento</param>
        /// <exception cref="ArgumentException">Il nominativo non può essere nullo o vuoto</exception>
        /// <exception cref="ArgumentNullException">Il prezzo deve essere specificato</exception>
        public Abbonamento( string nominativo, DateTime emissione, PrezzoBase prezzo )
        {
            // Controllo dei parametri in ingresso
            if( string.IsNullOrEmpty( nominativo ) )
            {
                throw new ArgumentNullException( "nominativo" );
            }
            if( prezzo == null )
            {
                throw new ArgumentNullException( "prezzo" );
            }

            this.m_nominativo = nominativo;
            this.m_emissione = emissione;
            this.m_costo = prezzo;
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
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( Abbonamento other )
        {
            if( other == null )
            {
                return false;
            }

            return this.GetType() == other.GetType() &&
                   this.m_costo == other.m_costo &&
                   this.m_emissione.Equals( other.m_emissione ) &&
                   this.m_nominativo.Equals( other.m_nominativo, StringComparison.CurrentCultureIgnoreCase );
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals( object obj )
        {
            if( obj == null )
            {
                return base.Equals( obj );
            }

            if( obj is Abbonamento )
            {
                return this.Equals( (Abbonamento)obj );
            }

            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Nominativo;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
            /*
            return this.m_costo.GetHashCode() ^
                   this.m_emissione.GetHashCode() ^
                   this.m_nominativo.ToLower().GetHashCode();
            */
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract Abbonamento Clone();

        /// <summary>
        /// Controlla che l'abbonamento sia ancora valido. Ogni implementazione utilizza i
        /// propri criteri per determinare la validità
        /// </summary>
        /// <returns><c>true</c> se lo sconto è ancora valido</returns>
		public abstract bool ControllaValidita();

        /// <summary>
        /// Restituisce un'identificativo sul tipo di abbonamento.
        /// </summary>
        /// <returns>Il tipo derivato dell'abbonamento</returns>
		public abstract Type GetTipoAbbonamento();

        /// <summary>
        /// Utilizza un'ingresso dell'abbonamento e restituisce un valore che indica se
        /// l'abbonamento è ancora valido e quindi è stato usato (non è valido se per
        /// esempio è scaduto).
        /// </summary>
        /// <returns><c>true</c> se lo sconto è ancora valido. Vedere <see cref="Abbonamento.ControllaValidita"/></returns>
		public abstract bool UsaAbbonamento();

		#endregion Public Methods 

		#endregion Methods 
    }
}