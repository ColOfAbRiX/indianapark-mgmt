using System;

namespace IndianaPark.PercorsiAvventura.Model
{
	/// <summary>
	/// Implementa il codice per lo sconto fisso.
	/// </summary>
    public class ScontoOmaggio : ScontoBase, IComparable<ScontoOmaggio>
	{
		#region Methods 

        #region Operators 

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più piccolo del secondo</returns>
        public static bool operator <( ScontoOmaggio leftOperand, ScontoOmaggio rightOperand )
        {
            return false;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più grande del secondo</returns>
        public static bool operator >( ScontoOmaggio leftOperand, ScontoOmaggio rightOperand )
        {
            return false;
        }

        #endregion Operators
        
        #region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome">Il nome dello sconto</param>
        /// <exception cref="ArgumentNullException">Il nome dello sconto è obbligatorio</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lo sconto deve essere positivo.</exception>
        public ScontoOmaggio( string nome )
        {
            if( String.IsNullOrEmpty( nome ) )
            {
                throw new ArgumentNullException( "nome" );
            }

            this.m_nome = nome;
            this.m_sconto = 0;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
		/// Il metodo applica lo sconto al prezzo e ne ritorna il risultato. La funzione
		/// non fa distinzione se è necessario o no applicare lo sconto: il risultato è
		/// sempre il prezzo scontato.
		/// </summary>
		/// <param name="prezzoBase">
        /// Il prezzo non scontato a cui applicare lo sconto
        /// </param>
        /// <returns>Restituisce sempre 0 (cioè un biglietto omaggio)</returns>
		public override decimal ScontaPrezzo(decimal prezzoBase)
		{
            if( prezzoBase < 0 )
            {
                throw new ArgumentOutOfRangeException( "prezzoBase" );
            }

            return 0;
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
            return new ScontoOmaggio( (string)this.m_nome.Clone() );
        }

	    /// <summary>
	    /// Compares the current object with another object of the same type.
	    /// </summary>
	    /// <returns>
	    /// A 32-bit signed integer that indicates the relative order of the objects being compared.
	    /// </returns>
	    /// <param name="other">An object to compare with this object.</param>
	    public int CompareTo( ScontoOmaggio other )
	    {
            return 0;
	    }
        
        #endregion Public Methods 

		#endregion Methods 
	}
}