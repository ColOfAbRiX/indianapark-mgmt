using System;
using System.Linq;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model 
{
    /// <summary>
    /// Raggruppa insieme le persone che sono entrate nello stesso momento permettendo di eseguire
    /// delle operazioni globalmente su queste persone
    /// </summary>
    public class Inserimento : ICollection<Cliente>, ICloneable, IEquatable<Inserimento>
	{
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Lista di clienti aggiunti in questo inserimento
        /// </summary>
        private List<Cliente> m_clienti;
	    /// <summary>
        /// Riferimento al tipo di biglietto base a cui viene applicato lo sconto comitiva
        /// </summary>
        private readonly TipoBiglietto m_tipoBiglietto;
        /// <summary>
        /// Data in cui è stato fatto l'inserimento. Corrisponde alla data e ora di istanziazione della classe
        /// </summary>
        private DateTime m_dataInserimento;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Ottiene il numero di elementi contenuti
        /// </summary>
        public int Count
        {
            get { return this.m_clienti.Count; }
        }

        /// <summary>
        /// Data in cui è stato fatto l'inserimento
        /// </summary>
        public DateTime DataInserimento
		{
			get { return this.m_dataInserimento; }
		}

        /// <summary>
        /// Ottiene un valore che indica se Inserimento è in sola lettura.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

	    /// <summary>
	    /// Lo sconto comitiva applicato alla comitiva rappresentata da tutto l'inserimento. Può non essere presente
	    /// </summary>
	    public IScontoComitiva ScontoComitiva { get; set; }

        /// <summary>
        /// Riferimento al tipo di biglietto base a cui viene applicato lo sconto comitiva
        /// </summary>
        public TipoBiglietto TipoBiglietto
        {
            get { return m_tipoBiglietto; }
        }

        /// <summary>
        /// L'insieme dei <see cref="Cliente"/> in questo inserimento
        /// </summary>
        public IEnumerable<Cliente> Clienti
        {
            get { return this.m_clienti; }
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
        public static bool operator ==( Inserimento leftOperand, Inserimento rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( Inserimento leftOperand, Inserimento rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        #endregion Operators
        
        #region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="scontoComitiva">Lo sconto comitiva applicato all'inserimento. Può anche essere null</param>
        /// <param name="tipoBiglietto">Il tipo di biglietto a cui applicare lo sconto comitiva. Può anche essere null</param>
        public Inserimento( IScontoComitiva scontoComitiva, TipoBiglietto tipoBiglietto ) : this( scontoComitiva, tipoBiglietto, DateTime.Now )
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="scontoComitiva">Lo sconto comitiva applicato all'inserimento. Può anche essere null</param>
        /// <param name="tipoBiglietto">Il tipo di biglietto a cui applicare lo sconto comitiva. Può anche essere null</param>
        /// <param name="dataInserimento">La data dell'inserimento</param>
        public Inserimento(IScontoComitiva scontoComitiva, TipoBiglietto tipoBiglietto, DateTime dataInserimento)
        {
            this.m_dataInserimento = dataInserimento;
            this.m_clienti = new List<Cliente>();
            this.ScontoComitiva = scontoComitiva;
            this.m_tipoBiglietto = tipoBiglietto;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Calcola il prezzo complessivo che tutti i clienti dell'inserimento devono pagare. Viene
        /// utilizzata per visualizzare in real-time questo dato nella GUI
        /// </summary>
        /// <returns>Il prezzo totale dei clienti dell'inserimento</returns>
        public decimal CalcolaPrezzo()
		{
            IList<Cliente> clienti = this.m_clienti;
            decimal totale = 0;

            // Applico lo sconto comitiva
            if( this.ScontoComitiva != null )
            {
                clienti = this.ScontoComitiva.PianoSconti( this.m_clienti.ToList() );
            }

            // Tiro le somme
            foreach( Cliente c in clienti )
            {
                totale += c.GetPrezzoScontato();
            }

            return totale;
		}

        /// <summary>
        /// Consente di rimuovere tutti gli elementi dal controllo
        /// </summary>
        public void Clear()
        {
            this.m_clienti.Clear();
        }

        /// <summary>
        /// Restituisce un enumeratore che consente di scorrere l'insieme
        /// </summary>
        /// <returns>Oggetto IEnumerator che può essere utilizzato per scorrere l'insieme.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.m_clienti.GetEnumerator();
        }

        /// <summary>
        /// Restituisce un enumeratore che scorre un insieme.
        /// </summary>
        /// <returns>Oggetto IEnumerator che può essere utilizzato per scorrere l'insieme.</returns>
        public IEnumerator<Cliente> GetEnumerator()
        {
            return this.m_clienti.GetEnumerator();
        }

        /// <summary>
        /// Aggiunge un elemento all'insieme 
        /// </summary>
        /// <param name="item">Oggetto da aggiungere</param>
        public void Add( Cliente item )
        {
            this.m_clienti.Add( item );
        }

        /// <summary>
        /// Consente di stabilire se Inserimento contiene un valore specifico.
        /// </summary>
        /// <param name="item">Oggetto da individuare</param>
        /// <returns>true se item si trova in Inserimento in caso contrario, false</returns>
        public bool Contains( Cliente item )
        {
            return this.m_clienti.Contains( item );
        }

        /// <summary>
        /// Rimuove la prima occorrenza di un oggetto specifico da Inserimento
        /// </summary>
        /// <param name="item">Oggetto da rimuovere</param>
        /// <returns>
        /// true, se item è stato correttamente rimosso da Inserimento in caso contrario, false.
        /// Questo metodo restituisce anche false se item non viene trovato in Inserimento
        /// </returns>
        public bool Remove( Cliente item )
        {
            return this.m_clienti.Remove( item );
        }

        /// <summary>
        /// Copia gli elementi di Inserimento in una classe Array, a partire da un particolare indice Array
        /// </summary>
        /// <param name="array">
        /// Classe Array unidimensionale che rappresenta la destinazione degli elementi
        /// copiati da Inserimento. L'indicizzazione di Array deve avere base zero.
        /// </param>
        /// <param name="arrayIndex">
        /// Indice in base zero in array in corrispondenza del quale ha inizio la copia.
        /// </param>
        public void CopyTo( Cliente[] array, int arrayIndex )
        {
            this.m_clienti.CopyTo( array, arrayIndex );
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
            var output = new Inserimento( this.ScontoComitiva, this.m_tipoBiglietto );

            output.m_dataInserimento = this.m_dataInserimento;
            output.m_clienti = this.m_clienti;
            //this.m_clienti.ForEach( item => output.m_clienti.Add( item.Clone() ) );

            return output;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public Inserimento Clone()
        {
            return (Inserimento)((ICloneable)this).Clone();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.m_dataInserimento + " " + this.m_tipoBiglietto;
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
            return this.m_clienti.GetHashCode() ^
                   this.m_dataInserimento.GetHashCode() ^
                   this.m_tipoBiglietto.GetHashCode();
            */
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se hanno tutti i campi dello stesso valore, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( Inserimento other )
        {
            if( other == null )
            {
                return false;
            }

            return this.m_clienti.Equals( other.m_clienti ) &&
                   this.m_dataInserimento.Equals( other.m_dataInserimento ) &&
                   this.m_tipoBiglietto == other.m_tipoBiglietto;
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

            if( obj is Inserimento )
            {
                return this.Equals( (Inserimento)obj );
            }

            return false;
        }
        
        #endregion Public Methods 

		#endregion Methods 
    }
}