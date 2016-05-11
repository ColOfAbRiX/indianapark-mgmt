using System;

namespace IndianaPark.PercorsiAvventura.Model 
{
	/// <summary>
	/// Implementa il codice per lo sconto percentuale.
	/// </summary>
    public class ScontoPercentuale : ScontoBase, IComparable<ScontoPercentuale>
    {
		#region Methods 

        #region Operators 

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più piccolo del secondo</returns>
        public static bool operator <( ScontoPercentuale leftOperand, ScontoPercentuale rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) < 0;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più grande del secondo</returns>
        public static bool operator >( ScontoPercentuale leftOperand, ScontoPercentuale rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) > 0;
        }
        
        #endregion Operators

        #region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome">Il nome dello sconto</param>
        /// <param name="sconto">Valore dello sconto percentuale, compreso tra 0 e 1</param>
        /// <exception cref="ArgumentNullException">Il nome dello sconto è obbligatorio</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lo sconto deve essere compreso tra 0 e 1.</exception>
        public ScontoPercentuale( string nome, double sconto )
        {
            if( String.IsNullOrEmpty( nome ) )
            {
                throw new ArgumentNullException( "nome" );
            }
            if( sconto < 0 || sconto > 1 )
            {
                throw new ArgumentOutOfRangeException( "sconto" );
            }

            this.m_sconto = sconto;
            this.m_nome = nome;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
		/// Il metodo applica lo sconto al prezzo e ne ritorna il risultato. La funzione
		/// non fa distinzione se è necessario o no applicare lo sconto: il risultato è
		/// sempre il prezzo scontato.
		/// </summary>
		/// <param name="prezzoBase">Il prezzo non scontato a cui applicare lo
		/// sconto</param>
		public override decimal ScontaPrezzo(decimal prezzoBase)
		{
            if( prezzoBase < 0 )
            {
                throw new ArgumentOutOfRangeException( "prezzoBase" );
            }

            return (decimal)((double)(prezzoBase) - (double)(prezzoBase) * this.m_sconto);
		}

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override ScontoBase Clone()
        {
            return new ScontoPercentuale( (string)this.m_nome.Clone(), this.m_sconto );
        }

	    /// <summary>
	    /// Compares the current object with another object of the same type.
	    /// </summary>
	    /// <returns>
	    /// A 32-bit signed integer that indicates the relative order of the objects being compared.
	    /// </returns>
	    /// <param name="other">An object to compare with this object.</param>
	    public int CompareTo( ScontoPercentuale other )
	    {
            return Math.Sign(this.m_sconto - other.m_sconto);
	    }
        
        #endregion Public Methods 

		#endregion Methods 
    }
}