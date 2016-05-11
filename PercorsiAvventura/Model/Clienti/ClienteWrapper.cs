using System;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Racchiude tutte le informazioni di un cliente
    /// </summary>
    public class ClienteWrapper : ICloneable
    {
		#region Fields 

		#region Internal Fields 

        private readonly Cliente m_cliente;
        private readonly Nominativo m_nominativo;
        private readonly Inserimento m_inserimento;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Il briefing a cui è associato il cliente.
        /// </summary>
        public IBriefing Briefing
        {
            get { return this.m_cliente.Briefing; }
        }

        /// <summary>
        /// Il codice identificativo del cliente
        /// </summary>
        public int CodiceCliente
        {
            get { return this.m_cliente.Codice; }
        }

        /// <summary>
        /// Codice identificativo del nominativo
        /// </summary>
        public int CodiceNominativo
        {
            get { return this.m_nominativo.Codice; }
        }

        /// <summary>
        /// Il codice completo che identifica il cliente
        /// </summary>
        public String CodiceCompleto
        {
            get
            {
                var nominativoFormat = new String( '0', (int)PluginPercorsi.GetGlobalParameter( "NominativoCodeSize" ).Value );
                var clienteFormat = new String( '0', (int)PluginPercorsi.GetGlobalParameter( "ClienteCodeSize" ).Value );

                return String.Format( "{0:" + nominativoFormat + "}{1:" + clienteFormat + "}", this.m_nominativo.Codice, this.m_cliente.Codice );
            }
        }

        /// <summary>
        /// L'orario di inizio validità del biglietto
        /// </summary>
        public DateTime OraIngresso
        {
            get { return this.m_cliente.OraIngresso; }
        }

        /// <summary>
        /// L'orario di scadenza del biglietto
        /// </summary>
        public DateTime OraUscita
        {
            get { return this.m_cliente.OraUscita; }
        }

        /// <summary>
        /// Il prezzo di base del biglietto, senza sconti applicati
        /// </summary>
        public decimal PrezzoBase
        {
            get { return this.m_cliente.PrezzoBase; }
        }

        /// <summary>
        /// Calcola il prezzo personale, ovvero il prezzo base cui è applicato un eventuale sconto personale
        /// </summary>
        public decimal PrezzoPersonale
        {
            get { return this.m_cliente.GetPrezzoPersonale(); }
        }

        /// <summary>
        /// Calcola il prezzo scontato, ovvero il prezzo base a cui sono applicati, se presenti, prima lo sconto
        /// personale e poi lo sconto comitiva
        /// </summary>
        public decimal PrezzoScontato
        {
            get { return this.m_cliente.GetPrezzoScontato(); }
        }

        /// <summary>
        /// Il tipo di sconto per persona singola applicato sul prezzo base del biglietto del cliente. Può non essere presente
        /// </summary>
        public ISconto Sconto
        {
            get { return this.m_cliente.Sconto; }
            set { this.m_cliente.Sconto = value; }
        }

        /// <summary>
        /// Il tipo di sconto comitiva applicato sul prezzo base del biglietto del cliente. Può non essere presente
        /// </summary>
        public IScontoComitiva ScontoComitiva
        {
            get { return this.m_cliente.ScontoComitiva; }
            set { this.m_cliente.ScontoComitiva = value; }
        }

        /// <summary>
        /// Il nome della tipologia cliente a cui appartiene il cliente
        /// </summary>
        public string NomeTipoCliente
        {
            get { return this.m_cliente.TipoCliente.Nome; }
        }

        /// <summary>
        /// Riferimento alla tipologia del cliente (adulto bambino ragazzo ecc..)
        /// </summary>
        public TipoCliente TipoCliente
        {
            get { return this.m_cliente.TipoCliente; }
        }

        /// <summary>
        /// Indica se è stata effettuata la procedura di uscita per il cliente
        /// </summary>
        /// <value><c>true</c> se uscito; <c>false</c> altrimenti.</value>
        public bool Uscito
        {
            get { return this.m_cliente.Uscito; }
        }

        /// <summary>
        /// Data in cui è stato fatto l'inserimento
        /// </summary>
        public DateTime DataInserimento
        {
            get { return this.m_inserimento.DataInserimento; }
        }

        /// <summary>
        /// Recupera il nominativo
        /// </summary>
        public Nominativo Nominativo
        {
            get { return this.m_nominativo; }
        }

        /// <summary>
        /// L'inserimento in cui è stato aggiunto il nominativo
        /// </summary>
        public Inserimento Inserimento
        {
            get { return this.m_inserimento; }
        }

        /// <summary>
        /// Il cliente a cui questa istanza fa riferimento
        /// </summary>
        public Cliente Cliente
        {
            get { return this.m_cliente; }
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
        public static bool operator ==( ClienteWrapper leftOperand, ClienteWrapper rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( ClienteWrapper leftOperand, ClienteWrapper rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        #endregion Operators
        
        #region Class-Wide Methods

        /// <summary>
        /// Crea una lista di <see cref="ClienteWrapper"/> dei clienti del parco
        /// </summary>
        /// <returns>Una lista di <see cref="ClienteWrapper"/> di tutti i clienti registrati nell'oggetto <see cref="Parco"/></returns>
        public static List<ClienteWrapper> FromParco()
        {
            var output = new List<ClienteWrapper>();

            foreach( var n in Parco.GetParco().ListaClienti.Values )
            {
                output.AddRange( FromNominativo( n ) );
            }

            return output;
        }

        /// <summary>
        /// Crea una lista di <see cref="ClienteWrapper"/> a partire da un nominativo
        /// </summary>
        /// <param name="n">Il nominativo da cui estrarre i dati</param>
        /// <returns>La lista dei <see cref="ClienteWrapper"/> creata utilizzando il nominativo indicato</returns>
        public static List<ClienteWrapper> FromNominativo( Nominativo n )
        {
            var output = new List<ClienteWrapper>();

            foreach( var i in n )
            {
                output.AddRange( FromInserimento( n, i ) );
            }

            return output;
        }

        /// <summary>
        /// Crea una lista di <see cref="ClienteWrapper"/> a partire da un inserimento
        /// </summary>
        /// <param name="i">L'inserimento da cui estrarre i dati</param>
        /// <param name="n">Il nominativo da associare ai Wrapper</param>
        /// <returns>La lista dei <see cref="ClienteWrapper"/> creata utilizzando il nominativo indicato</returns>
        public static List<ClienteWrapper> FromInserimento( Nominativo n, Inserimento i )
        {
            var output = new List<ClienteWrapper>();

            foreach( var c in i )
            {
                output.Add( new ClienteWrapper( c, i, n ) );
            }

            return output;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClienteWrapper"/> class.
        /// </summary>
        /// <param name="cliente">The cliente.</param>
        /// <param name="inserimento">The inserimento.</param>
        /// <param name="nominativo">The nominativo.</param>
        public ClienteWrapper( Cliente cliente, Inserimento inserimento, Nominativo nominativo )
        {
            // Controllo del formato dei parametri
            if( cliente == null || inserimento == null || nominativo == null )
            {
                throw new ArgumentNullException();
            }
            if( !nominativo.Contains( inserimento ) )
            {
                throw new ArgumentOutOfRangeException( "inserimento", "The object nominativo must contain the referred inserimento" );
            }
            if( !inserimento.Contains( cliente ) )
            {
                throw new ArgumentOutOfRangeException( "cliente", "The object inserimento must contain the referred cliente" );
            }

            this.m_cliente = cliente;
            this.m_inserimento = inserimento;
            this.m_nominativo = nominativo;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Crea un nuovo oggetto che è una copia dell'istanza corrente.
        /// </summary>
        /// <returns>Nuovo oggetto che è una copia dell'istanza corrente.</returns>
        object ICloneable.Clone()
        {
            return new ClienteWrapper( this.m_cliente, this.m_inserimento, this.m_nominativo );
        }

        /// <summary>
        /// Crea un nuovo oggetto che è una copia dell'istanza corrente.
        /// </summary>
        /// <returns>Nuovo oggetto che è una copia dell'istanza corrente.</returns>
        public ClienteWrapper Clone()
        {
            return (ClienteWrapper)(((ICloneable)this).Clone());
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.m_cliente.ToString();
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
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se hanno tutti i campi dello stesso valore, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( ClienteWrapper other )
        {
            if( other == null )
            {
                return false;
            }

            return base.Equals( other );
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

            if( obj is ClienteWrapper )
            {
                return this.Equals( (ClienteWrapper)obj );
            }

            return false;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
