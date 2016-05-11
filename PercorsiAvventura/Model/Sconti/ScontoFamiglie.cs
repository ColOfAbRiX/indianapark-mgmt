using System;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model 
{
	/// <summary>
	/// Implementa il codice per lo sconto famiglie: se ci sono 2 adulti e 2 bambini
	/// viene applicato lo sconto.
	/// </summary>
    public class ScontoFamiglie : ScontoComitiva, IComparable<ScontoFamiglie>
    {
		#region Methods 

        #region Operators

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più piccolo del secondo</returns>
        public static bool operator <( ScontoFamiglie leftOperand, ScontoFamiglie rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) < 0;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più grande del secondo</returns>
        public static bool operator >( ScontoFamiglie leftOperand, ScontoFamiglie rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) > 0;
        }

        #endregion Operators
        
        #region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome">Il nome dello sconto</param>
        /// <param name="sconto">Valore dello sconto fisso, non negativo</param>
        public ScontoFamiglie( string nome, double sconto ) : base( nome, sconto )
        {
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Crea una lista di applicazione di sconti relativa alla lista clienti indicata.
        /// La funzione tratta indistintamente tutti i clienti, ovvero applica gli sconti considerando tutta
        /// la lista in ingresso.
        /// </summary>
        /// <param name="clienti">La lista clienti a cui applicare gli sconti. Non deve essere modificata</param>
        /// <returns>
        /// Una lista di sconti comitiva, delle stesse dimensioni della lista clienti in ingresso, in cui ogni
        /// elemento è associato all'elemento con lo stesso indice nella lista clienti. Questa lista contiene
        /// gli sconti comitiva effettivamente applicati (quindi un riferimento allo sconto oppure null).
        /// </returns>
        protected override IList<IScontoComitiva> ApplicaSconto( IList<Cliente> clienti )
        {
            throw new NotImplementedException();
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
		/// Il metodo applica lo sconto al prezzo e ne ritorna il risultato. La funzione
		/// non fa distinzione se è necessario o no applicare lo sconto: il risultato è
		/// sempre il prezzo scontato.
		/// </summary>
		/// <param name="prezzoBase">Il prezzo non scontato a cui applicare lo
		/// sconto</param>
        public override decimal ScontaPrezzo( decimal prezzoBase )
		{
		    if( prezzoBase < 0 )
		    {
		        throw new ArgumentOutOfRangeException( "prezzoBase", "The parameter must be non-negative" );
		    }

		    return Math.Max( 0, prezzoBase - (decimal)this.m_sconto );
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
            return new ScontoFamiglie( (string)this.m_nome.Clone(), this.m_sconto );
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo( ScontoFamiglie other )
        {
            return Math.Sign( this.Sconto - other.Sconto );
        }

		#endregion Public Methods 

		#endregion Methods 
	}
}