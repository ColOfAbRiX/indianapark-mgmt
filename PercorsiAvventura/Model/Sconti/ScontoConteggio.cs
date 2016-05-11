using System;
using System.Linq;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Implementa il codice per gli sconti a N-Persone: ogni N persone la N+1 entra
    /// gratis.
    /// </summary>
    public class ScontoConteggio : ScontoComitiva, IComparable<ScontoConteggio>
    {
		#region Fields 

        /// <summary>
        /// Numero di persone dopo il quale si applica uno sconto comitiva
        /// </summary>
        private readonly uint m_conteggioPersone;

        /// <summary>
        /// Il numero di persone dopo il quale ce n'è una gratuita
        /// </summary>
        public uint Conteggio { get { return this.m_conteggioPersone; } }

        #endregion Fields 

		#region Methods 

        #region Operators

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più piccolo del secondo</returns>
        public static bool operator <( ScontoConteggio leftOperand, ScontoConteggio rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) < 0;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più grande del secondo</returns>
        public static bool operator >( ScontoConteggio leftOperand, ScontoConteggio rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) > 0;
        }

        #endregion Operators
        
        #region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome">Il nome dello sconto.</param>
        /// <param name="sconto">Valore dello sconto fisso, non negativo</param>
        /// <param name="conteggioPersone">Il numero di persone dopo il quale ce n'è una gratuita</param>
        public ScontoConteggio( string nome, double sconto, uint conteggioPersone ) : base( nome, sconto )
        {
            if( conteggioPersone < 0 )
            {
                throw new ArgumentOutOfRangeException( "conteggioPersone", "The parameter must be non-negative" );
            }

            this.m_conteggioPersone = conteggioPersone;
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Crea una lista di applicazione di sconti relativa alla lista clienti indicata.
        /// La funzione tratta indistintamente tutti i clienti, ovvero applica gli sconti considerando tutta
        /// la lista in ingresso.
        /// </summary>
        /// <param name="clienti">La lista clienti a cui applicare gli sconti.</param>
        /// <returns>
        /// Una lista di sconti comitiva, delle stesse dimensioni della lista clienti in ingresso, in cui ogni
        /// elemento è associato all'elemento con lo stesso indice nella lista clienti. Questa lista contiene
        /// gli sconti comitiva effettivamente applicati (quindi un riferimento allo sconto oppure null).
        /// </returns>
        protected override IList<IScontoComitiva> ApplicaSconto( IList<Cliente> clienti )
        {
            var outputList = new List<IScontoComitiva>( clienti.Count );

            // Trovo quanti clienti devo scontare
            var numeroScontati = Math.Floor( (double)clienti.Count / (double)(this.m_conteggioPersone + 1) );
            // Ordino i clienti in base al prezzo base
            // TODO: Usare la funzione nella classe base in modo da uniformare il funzionamento. Attenzione che viene usata anche per altri scopi
            var ordinati = from c in clienti
                           orderby c.PrezzoBase ascending
                           select c;
             
            // Inizializzo la lista in uscita
            for( int i = 0; i < clienti.Count; i++ )
            {
                outputList.Add( null );
            }

            // Applico lo sconto ai primi numeroScontati clienti della lista ordinata
            for( int j = 0; j < numeroScontati && j < ordinati.Count(); j++ )
            {
                outputList[clienti.IndexOf( ordinati.ElementAt( j ) )] = this;
            }

            return outputList;
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

            // Questo tipo di sconto regala il biglietto
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
            return new ScontoConteggio( (string)this.m_nome.Clone(), this.m_sconto, this.m_conteggioPersone );
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo( ScontoConteggio other )
        {
            return Math.Sign( this.Sconto - other.Sconto );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public override bool Equals( ISconto other )
        {
            return base.Equals( other ) &&
                   ((ScontoConteggio)other).Conteggio == this.Conteggio;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}