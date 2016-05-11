using System;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Rappresenta un (singolo) prezzo base, ovvero un prezzo a cui non è applicato nessuno
    /// sconto. Un prezzo è associato alla coppia (TipologiaCliente, TipologiaBiglietto)
    /// </summary>
    public class PrezzoBase : ICloneable, IEquatable<PrezzoBase>, IComparable<PrezzoBase>
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Riferimento alla tipologia di cliente
        /// </summary>
        private readonly TipoCliente m_tipoCliente;
        /// <summary>
        /// Riferimento alla tipologia di biglietto
        /// </summary>
        private readonly TipoBiglietto m_tipiBiglietto;

        private decimal m_prezzo;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Nome del prezzo
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Il valore del prezzo
        /// </summary>
        public decimal Prezzo
        {
            get { return this.m_prezzo; }
            set
            {
                if( value < 0 )
                {
                    throw new ArgumentOutOfRangeException( "value", "The parameter must be non-negative" );
                }
                this.m_prezzo = value;
            }
        }
        
        /// <summary>
        /// Riferimento alla tipologia di biglietto
        /// </summary>
        public TipoBiglietto TipoBiglietto
        {
            get { return m_tipiBiglietto; }
        }

        /// <summary>
        /// Riferimento alla tipologia di cliente
        /// </summary>
        public TipoCliente TipoCliente
        {
            get { return m_tipoCliente; }
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
        public static bool operator ==( PrezzoBase leftOperand, PrezzoBase rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( PrezzoBase leftOperand, PrezzoBase rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più piccolo del secondo</returns>
        public static bool operator <( PrezzoBase leftOperand, PrezzoBase rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) < 0;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più grande del secondo</returns>
        public static bool operator >( PrezzoBase leftOperand, PrezzoBase rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) > 0;
        }

        #endregion Operators
        
        #region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome">Il nome del prezzo.</param>
        /// <param name="tipoCliente">Riferimento al tipo di cliente associato</param>
        /// <param name="prezzoBase">Riferimento al tipo di biglietto associato</param>
        public PrezzoBase( string nome, TipoCliente tipoCliente, TipoBiglietto prezzoBase ) : this( nome, tipoCliente, prezzoBase, 0 )
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome">Il nome del prezzo.</param>
        /// <param name="tipoCliente">Riferimento al tipo di cliente associato</param>
        /// <param name="prezzoBase">Riferimento al tipo di biglietto associato</param>
        /// <param name="prezzo">Il valore del prezzo</param>
        public PrezzoBase( string nome, TipoCliente tipoCliente, TipoBiglietto prezzoBase, decimal prezzo )
        {
            // Controllo dei parametri
            if( String.IsNullOrEmpty( nome ) )
            {
                throw new ArgumentNullException( "nome" );
            }
            if( tipoCliente == null )
            {
                throw new ArgumentNullException( "tipoCliente" );
            }
            if( prezzoBase == null )
            {
                throw new ArgumentNullException( "PrezzoBase" );
            }

            this.Nome = nome;
            this.m_tipoCliente = tipoCliente;
            this.m_tipiBiglietto = prezzoBase;
            this.Prezzo = prezzo;
        }

		#endregion Constructors 

        #region Public Methods 

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        object ICloneable.Clone()
        {
            return new PrezzoBase( (string)this.Nome.Clone(), this.m_tipoCliente, this.m_tipiBiglietto ) { Prezzo = this.Prezzo };
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public PrezzoBase Clone()
        {
            return (PrezzoBase)((ICloneable)this).Clone();
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
            return this.m_tipiBiglietto.GetHashCode() ^
                   this.m_tipoCliente.GetHashCode() ^
                   this.Nome.ToLower().GetHashCode()^
                   this.Prezzo.GetHashCode();
            */
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se hanno tutti i campi dello stesso valore, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( PrezzoBase other )
        {
            if( other == null )
            {
                return false;
            }

            return this.m_tipiBiglietto == other.m_tipiBiglietto &&
                   this.m_tipoCliente == other.m_tipoCliente &&
                   this.Nome.Equals( other.Nome, StringComparison.CurrentCultureIgnoreCase ) &&
                   this.Prezzo.Equals( other.Prezzo );
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

            if( obj is PrezzoBase )
            {
                return this.Equals( (PrezzoBase)obj );
            }

            return false;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo( PrezzoBase other )
        {
            return Math.Sign( this.Prezzo - other.Prezzo );
        }

        #endregion Public Methods 

        #endregion Methods
    }
}