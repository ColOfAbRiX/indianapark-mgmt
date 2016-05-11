using System;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Implementazione dei metodi e proprietà comuni a tutti gli sconti
    /// </summary>
    public abstract class ScontoBase : ISconto
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Il nome dello sconto
        /// </summary>
        protected string m_nome;
        /// <summary>
        /// Il valore dello sconto
        /// </summary>
        protected double m_sconto;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Il nome dello sconto
        /// </summary>
        public string Nome
        {
            get { return this.m_nome; }
        }

        /// <summary>
        /// Il valore dello sconto applicato. Il significato di questo campo può cambiare da
        /// realizzazione a realizzazione, leggere la documentazione.
        /// </summary>
        public double Sconto
        {
            get { return this.m_sconto; }
        }

        /// <summary>
        /// Indica se lo sconto può essere tolto a favore di un piano sconti più favorevole
        /// </summary>
        /// <value></value>
        public bool CanOmit
        {
            get { return true; }
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
        public static bool operator ==( ScontoBase leftOperand, ScontoBase rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( ScontoBase leftOperand, ScontoBase rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

		#endregion Operators 

		#region Class-wise Methods 

        /// <summary>
        /// Crea una chiave per i dizionari a partire dallo sconto
        /// </summary>
        /// <param name="n">Lo sconto di riferimento.</param>
        /// <returns>Una chiave che identifica lo sconto</returns>
        public static string CreateKey( ISconto n )
        {
            if( n == null )
            {
                throw new ArgumentNullException( "n" );
            }

            var nome = string.Format( "{0}_{1}", n.GetType().Name.ToLower(), n.Sconto );
            nome = nome.Replace( " ", "_" );
            return nome;
        }

		#endregion Class-wise Methods 

		#region Public Methods 

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
            return this.GetType().GetHashCode() ^ this.m_sconto.GetHashCode();
            */
        }

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
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract ScontoBase Clone();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public virtual bool Equals( ISconto other )
        {
            if( other == null )
            {
                return false;
            }

            return this.GetType() == other.GetType() &&
                   this.Sconto.Equals( other.Sconto );
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

            if( obj is ScontoBase )
            {
                return this.Equals( (ScontoBase)obj );
            }

            return false;
        }

        /// <summary>
        /// Il metodo applica lo sconto al prezzo e ne ritorna il risultato. La funzione
        /// non fa distinzione se è necessario o no applicare lo sconto: il risultato è
        /// sempre il prezzo scontato.
        /// </summary>
        /// <param name="prezzoBase">Il prezzo non scontato a cui applicare lo
        /// sconto</param>
        public abstract decimal ScontaPrezzo( decimal prezzoBase );

		#endregion Public Methods 

		#endregion Methods 
    }
}
