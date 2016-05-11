using System;
using System.Linq;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model 
{
    /// <summary>
    /// Contiene la lista dei clienti registrati sotto un nominativo unico.
    /// </summary>
    public class Nominativo : ICollection<Inserimento>, ICloneable, IEquatable<Nominativo>
	{
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Codice identificativo del nominativo
        /// </summary>
        private readonly int m_codice;
        /// <summary>
        /// Nominativo del responsabile dell'inserimento
        /// </summary>
        private readonly string m_nominativo;
        /// <summary>
        /// Lista di inserimenti fatti sotto questo nominativo
        /// </summary>
        private List<Inserimento> m_inserimento;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Codice identificativo del nominativo
        /// </summary>
        public int Codice
        {
            get { return this.m_codice; }
        }

        /// <summary>
        /// Ottiene il numero di elementi contenuti
        /// </summary>
        public int Count
        {
            get { return this.m_inserimento.Count; }
        }

        /// <summary>
        /// Ottiene un valore che indica se Nominativo è in sola lettura.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Nominativo del responsabile dell'inserimento
        /// </summary>
        public string Nome
        {
            get { return this.m_nominativo; }
        }

        /// <summary>
        /// L'insieme degli <see cref="Inserimento"/> in questo nominativo
        /// </summary>
        public IEnumerable<Inserimento> Inserimenti
        {
            get { return this.m_inserimento; }
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
        public static bool operator ==( Nominativo leftOperand, Nominativo rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( Nominativo leftOperand, Nominativo rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        #endregion Operators
        
        #region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="codice">Codice assegnato al nominativo. Univoco nella giornata</param>
        /// <param name="nome">Nome del nominativo</param>
        public Nominativo( int codice, string nome )
        {
            if( String.IsNullOrEmpty( nome ) )
            {
                throw new ArgumentNullException( "nome" );
            }

            this.m_codice = codice;
            this.m_inserimento = new List<Inserimento>();
            this.m_nominativo = nome;
        }

		#endregion Constructors 

        #region Class-Wise Methods 

        /// <summary>
        /// Crea una chiave per i dizionari a partire dal nominativo
        /// </summary>
        /// <param name="n">Il nominativo di riferimento.</param>
        /// <returns>Una chiave che identifica il nominativo</returns>
        public static string CreateKey( Nominativo n )
        {
            return CreateKey( n.Codice, n.Nome );
        }

        /// <summary>
        /// Crea una chiave per i dizionari a partire dal nominativo
        /// </summary>
        /// <param name="codice">Il codice del nominativo</param>
        /// <param name="nome">The nome del nominativo</param>
        /// <returns>Una chiave che identifica il nominativo</returns>
        public static string CreateKey( int codice, string nome )
        {
            string key = String.Format( "{0} {1}", nome, codice );
            //string key = String.Format( "{0}", nome );
            key = key.ToLower();
            key = key.Replace( " ", "_" );
            return key;
        }

	    #endregion

        #region Public Methods

        /// <summary>
        /// Consente di rimuovere tutti gli elementi dal controllo
        /// </summary>
        public void Clear()
        {
            this.m_inserimento.Clear();
        }

        /// <summary>
        /// Restituisce un enumeratore che scorre un insieme.
        /// </summary>
        /// <returns>Oggetto IEnumerator che può essere utilizzato per scorrere l'insieme.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.m_inserimento.GetEnumerator();
        }

        /// <summary>
        /// Restituisce un enumeratore che scorre un insieme.
        /// </summary>
        /// <returns>Oggetto IEnumerator che può essere utilizzato per scorrere l'insieme.</returns>
        public IEnumerator<Inserimento> GetEnumerator()
        {
            return this.m_inserimento.GetEnumerator();
        }

        /// <summary>
        /// Recupera la lista di tutti i clienti, senza suddividerli per inserimento
        /// </summary>
        /// <returns>La lista dei clienti completa di questo nominativo</returns>
        public IList<Cliente> GetRawList()
        {
            var clienti = new List<Cliente>();

            foreach( var i in this.m_inserimento )
            {
                clienti.AddRange( i );
            }

            return clienti;
        }

	    /// <summary>
        /// Recupera un cliente a partire dal codice cliente. Restituisce null se non ha trovato il cliente
        /// </summary>
        /// <param name="codice">Il codice del cliente da trovare</param>
        /// <returns>L'istanza di <see cref="Cliente"/> con il codice specificato</returns>
        public Cliente TrovaCliente( int codice )
        {
            Cliente cliente = null;

            foreach( var inserimento in this.m_inserimento )
            {
                cliente = inserimento.FirstOrDefault( item => item.Codice == codice );
                if( cliente != null )
                {
                    break;
                }
            }

            return cliente;
        }

	    /// <summary>
        /// Aggiunge un elemento all'insieme 
        /// </summary>
        /// <param name="item">Oggetto da aggiungere</param>
        public void Add( Inserimento item )
        {
            this.m_inserimento.Add( item );
        }

        /// <summary>
        /// Consente di stabilire se Nominativo contiene un valore specifico.
        /// </summary>
        /// <param name="item">Oggetto da individuare</param>
        /// <returns>true se item si trova in Nominativo in caso contrario, false</returns>
        public bool Contains( Inserimento item )
        {
            return this.m_inserimento.Contains( item );
        }

        /// <summary>
        /// Rimuove la prima occorrenza di un oggetto specifico da Nominativo
        /// </summary>
        /// <param name="item">Oggetto da rimuovere</param>
        /// <returns>
        /// true, se item è stato correttamente rimosso da Nominativo in caso contrario, false.
        /// Questo metodo restituisce anche false se item non viene trovato in Nominativo
        /// </returns>
        public bool Remove( Inserimento item )
        {
            return this.m_inserimento.Remove( item );
        }

        /// <summary>
        /// Copia gli elementi di Nominativo in una classe Array, a partire da un particolare indice Array
        /// </summary>
        /// <param name="array">
        /// Classe Array unidimensionale che rappresenta la destinazione degli elementi
        /// copiati da Nominativo. L'indicizzazione di Array deve avere base zero.
        /// </param>
        /// <param name="arrayIndex">
        /// Indice in base zero in array in corrispondenza del quale ha inizio la copia.
        /// </param>
        public void CopyTo( Inserimento[] array, int arrayIndex )
        {
            this.m_inserimento.CopyTo( array, arrayIndex );
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
            var output = new Nominativo( this.m_codice, (string)this.m_nominativo.Clone() );
            this.m_inserimento.ForEach( item => output.m_inserimento.Add( item.Clone() ) );
            return output;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public Nominativo Clone()
        {
            return (Nominativo)((ICloneable)this).Clone();
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
            return this.m_codice.GetHashCode() ^
                   this.m_inserimento.GetHashCode() ^
                   this.m_nominativo.GetHashCode();
            */
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se hanno tutti i campi dello stesso valore, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( Nominativo other )
        {
            if( other == null )
            {
                return false;
            }

            return this.m_codice.Equals( other.m_codice ) &&
                   this.m_inserimento.Equals( other.m_inserimento ) &&
                   this.m_nominativo.Equals( this.m_nominativo, StringComparison.CurrentCultureIgnoreCase );
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

            if( obj is Nominativo )
            {
                return this.Equals( (Nominativo)obj );
            }

            return false;
        }
        
        #endregion Public Methods 

		#endregion Methods 
    }
}