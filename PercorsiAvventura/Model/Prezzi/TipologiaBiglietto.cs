using System;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// La classe modella le tipologie di biglietti. Ogni istanza rappresenta una
    /// tipologia (Biglietto Base, Biglietto Gruppo Organizzato, Biglietto Notturna,
    /// Abbonamento) ognuna con i suoi prezzi base.
    /// Ad ogni tipologia biglietto può essere assegnato un tipo di Sconto
    /// </summary>
    public class TipoBiglietto : ICloneable, IEquatable<TipoBiglietto>, IComparable<TipoBiglietto>
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Nome della tipologia di biglietto
        /// </summary>
        private string m_tipologia;

        private readonly Dictionary<string, TipoCliente> m_clientiAssociati = new Dictionary<string, TipoCliente>();

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Indica se il tipo di biglietto è un abbonamento oppure no
        /// </summary>
        /// <value>
        /// 	<c>true</c> se è un abbonamento, <c>false</c> altrimenti.
        /// </value>
        public bool IsAbbonamento { get; set; }

        /// <summary>
        /// Lo sconto comitiva che può essere associato con questo tipo di biglietto
        /// </summary>
        public IScontoComitiva ScontoComitiva { get; private set; }

        /// <summary>
        /// Il nome testuale della tipologia di bigietto.
        /// </summary>
        public string Nome
        {
            get { return this.m_tipologia; }
            private set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( "value" );
                }
                this.m_tipologia = value;
            }
        }

        /// <summary>
        /// Le tipologie di cliente associate con questo tipo di biglietto
        /// </summary>
        public IDictionary<string, TipoCliente> TipiCliente
        {
            get { return this.m_clientiAssociati; }
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
        public static bool operator ==( TipoBiglietto leftOperand, TipoBiglietto rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( TipoBiglietto leftOperand, TipoBiglietto rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        #endregion Operators
        
        #region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="tipologia">Nome della tipologia del biglietto</param>
        /// <param name="scontoComitiva">Indica la presenza di uno sconto comitiva per la ripologia biglietto</param>
        public TipoBiglietto( string tipologia, IScontoComitiva scontoComitiva )
        {
            this.Nome = tipologia;
            this.ScontoComitiva = scontoComitiva;
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
            return new TipoBiglietto( this.m_tipologia, this.ScontoComitiva ) { IsAbbonamento = this.IsAbbonamento };
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public TipoBiglietto Clone()
        {
            return (TipoBiglietto)((ICloneable)this).Clone();
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
        public int CompareTo( TipoBiglietto other )
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
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
            /*
            return this.IsAbbonamento.GetHashCode() ^
                   this.Nome.ToLower().GetHashCode() ^
                   this.ScontoComitiva.GetHashCode();
            */
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se hanno tutti i campi dello stesso valore, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( TipoBiglietto other )
        {
            if( other == null )
            {
                return false;
            }

            return this.IsAbbonamento.Equals( other.IsAbbonamento ) &&
                   this.Nome.Equals( other.Nome, StringComparison.CurrentCultureIgnoreCase ) &&
                   this.ScontoComitiva == other.ScontoComitiva;
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

            if( obj is TipoBiglietto )
            {
                return this.Equals( (TipoBiglietto)obj );
            }

            return false;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}