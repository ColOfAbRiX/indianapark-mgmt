using System;
using System.Collections.Generic;
using System.Linq;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// La classe modella le tipologie clienti. Ogni istanza rappresenta una tipologia,
    /// come Adulti, Bambini, ...
    /// </summary>
    public class TipoCliente : IEquatable<TipoCliente>, ICloneable, IComparable<TipoCliente>
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Durata del biglietto
        /// </summary>
        private TimeSpan m_durataBiglietto;
        /// <summary>
        /// Nome della tipologia di cliente
        /// </summary>
        private string m_tipologia;
        /// <summary>
        /// Il tipo di imbrago della tipologia cliente
        /// </summary>
        private uint m_tipoImbrago;
        /// <summary>
        /// Riferimento al tipo di briefing del cliente
        /// </summary>
        private ITipoBriefing m_tipoBriefing;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// L'insieme dei briefing utilizzati dalla tipologia di cliente. Una tipologia
        /// lienti può anche non avere briefing
        /// </summary>
        public ITipoBriefing TipiBriefing
        {
            get { return this.m_tipoBriefing; }
            private set { this.m_tipoBriefing = value; }
        }

        /// <summary>
        /// La durata del biglietto per la tipologia cliente. Se per la tipologia cliente è
        /// previsto un briefing la durata del biglietto parte dall'inizio del briefing
        /// </summary>
        public TimeSpan DurataBiglietto
        {
            get { return this.m_durataBiglietto; }
            private set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( "value" );
                }
                this.m_durataBiglietto = value;
            }
        }

        /// <summary>
        /// Il tipo di imbrago della tipologia cliente
        /// </summary>
        public uint TipoImbrago
        {
            get { return this.m_tipoImbrago; }
            private set { this.m_tipoImbrago = value; }
        }

        /// <summary>
        /// Il nome testuale della tipologia di cliente.
        /// </summary>
        public string Nome
        {
            get { return this.m_tipologia; }
            private set
            {
                if( String.IsNullOrEmpty( value ) )
                {
                    throw new ArgumentNullException( "value" );
                }
                this.m_tipologia = value;
            }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

        #region Operators 

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi rappresentano la stessa istanza</returns>
        public static bool operator ==( TipoCliente leftOperand, TipoCliente rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( TipoCliente leftOperand, TipoCliente rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        #endregion Operators 

        #region Constructors

        /// <summary>
        /// Il costruttore del tipo cliente.
        /// </summary>
        /// <param name="tipoImbrago">L'intero che rappresenta il tipo di imbrago che il cliente usa</param>
        /// <param name="tipoCliente">Il nome usato per specificare la categoria del cliente</param>
        /// <param name="durataBiglietto">Il periodo di validità di un biglietto della categoria cliente</param>
        /// <param name="briefing">Il briefing che il cliente deve seguire prima di accedere alle attrazioni</param>
        public TipoCliente( uint tipoImbrago, string tipoCliente, TimeSpan durataBiglietto, ITipoBriefing briefing )
        {
            this.TipoImbrago = tipoImbrago;
            this.Nome = tipoCliente;
            this.DurataBiglietto = durataBiglietto;
            this.TipiBriefing = briefing;
        }

		#endregion Constructors 

        #region Class-Wise Methods 

        /// <summary>
        /// Recupera tutte le tipologie distinte di TipoCliente nel parco
        /// </summary>
        public static IList<TipoCliente> GetAllTipologie()
        {
            var tuttiTipi = new List<TipoCliente>();
            Parco.GetParco().TipologieBiglietto.Values.ToList().ForEach( item => tuttiTipi.AddRange( item.TipiCliente.Values.ToList() ) );
            return tuttiTipi.Distinct().ToList();
        }

        #endregion Class-Wise Methods

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
            return new TipoCliente( this.m_tipoImbrago, this.m_tipologia, new TimeSpan( this.m_durataBiglietto.Ticks ), this.m_tipoBriefing );
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public TipoCliente Clone()
        {
            return (TipoCliente)((ICloneable)this).Clone();
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This object is less than the <paramref name="other"/> parameter.
        ///                     Zero 
        ///                     This object is equal to <paramref name="other"/>. 
        ///                     Greater than zero 
        ///                     This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public int CompareTo( TipoCliente other )
        {
            return this.Nome.CompareTo( other.Nome );
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Nome;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se hanno tutti i campi dello stesso valore, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( TipoCliente other )
        {
            if( other == null )
            {
                return false;
            }

            return this.m_durataBiglietto.Equals( other.m_durataBiglietto ) &&
                   this.m_tipoBriefing == other.m_tipoBriefing &&
                   this.m_tipoImbrago.Equals( other.m_tipoImbrago ) &&
                   this.m_tipologia.Equals( other.m_tipologia, StringComparison.CurrentCultureIgnoreCase );
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

            if( obj is TipoCliente )
            {
                return this.Equals( (TipoCliente)obj );
            }

            return false;
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
            return m_durataBiglietto.GetHashCode() ^
                   m_tipoBriefing.GetHashCode() ^
                   m_tipoImbrago.GetHashCode() ^
                   m_tipologia.ToLower().GetHashCode();
            */
        }
        
        #endregion Public Methods 

		#endregion Methods 
    }
}