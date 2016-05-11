using System;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Rappresenta un clienti dei percorsi.
    /// </summary>
    /// <remarks>
    /// La classe serve principalmente per determinare il biglietto, exception quindi è
    /// associata ad un prezzo base, allo sconto personale e allo sconto comitiva.
    /// Sconti non associati indicano che non viene applicato sconto.
    /// L'associazione con i briefing determina l'orario d'inizio del biglietto
    /// </remarks>
    public class Cliente : ICloneable, IEquatable<Cliente>, IComparable<Cliente>
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Il codice identificativo del cliente
        /// </summary>
        private int m_codice;
        /// <summary>
        /// Il briefing a cui è associato il cliente.
        /// </summary>
        private IBriefing m_briefing;
        /// <summary>
        /// Il prezzo di base del biglietto, senza sconti applicati
        /// </summary>
        private readonly PrezzoBase m_prezzo;
        /// <summary>
        /// Il tipo di sconto per persona singola applicato sul prezzo base del biglietto del cliente
        /// </summary>
        private ISconto m_sconto;
        /// <summary>
        /// Il tipo di sconto comitiva applicato sul prezzo base del biglietto del cliente. Può non essere presente
        /// </summary>
        private IScontoComitiva m_scontoComitiva;
        /// <summary>
        /// L'orario di ingresso del cliente
        /// </summary>
        private DateTime m_orarioIngresso;
        /// <summary>
        /// L'orario di uscita del cliente
        /// </summary>
        private DateTime m_orarioUscita;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Il briefing a cui è associato il cliente.
        /// </summary>
        public IBriefing Briefing
        {
            get { return this.m_briefing; }
        }

        /// <summary>
        /// Il codice identificativo del cliente
        /// </summary>
        public int Codice
        {
            get { return this.m_codice; }
        }

        /// <summary>
        /// L'orario di inizio validità del biglietto
        /// </summary>
        public DateTime OraIngresso
        {
            get { return this.m_orarioIngresso; }
        }

        /// <summary>
        /// L'orario di scadenza del biglietto
        /// </summary>
        public DateTime OraUscita
        {
            get { return this.m_orarioUscita; }
        }

        /// <summary>
        /// Il prezzo di base del biglietto, senza sconti applicati
        /// </summary>
        public decimal PrezzoBase
        {
            get { return this.m_prezzo.Prezzo; }
        }

        /// <summary>
        /// Il tipo di sconto per persona singola applicato sul prezzo base del biglietto del cliente. Può non essere presente
        /// </summary>
        public ISconto Sconto
        {
            get { return this.m_sconto; }
            set { this.m_sconto = value; }
        }

        /// <summary>
        /// Il tipo di sconto comitiva applicato sul prezzo base del biglietto del cliente. Può non essere presente
        /// </summary>
        public IScontoComitiva ScontoComitiva
        {
            get { return this.m_scontoComitiva; }
            set { this.m_scontoComitiva = value; }
        }

        /// <summary>
        /// Riferimento alla tipologia del cliente (adulto bambino ragazzo ecc..)
        /// </summary>
        public TipoCliente TipoCliente
        {
            get { return this.m_prezzo.TipoCliente; }
        }

        /// <summary>
        /// Indica se è stata effettuata la procedura di uscita per il cliente
        /// </summary>
        /// <value><c>true</c> se uscito; <c>false</c> altrimenti.</value>
        public bool Uscito { get; private set; }

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
        public static bool operator ==( Cliente leftOperand, Cliente rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( Cliente leftOperand, Cliente rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

		#endregion Operators 

		#region Constructors 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="codice">Il codice del cliente</param>
        /// <param name="prezzo">Il prezzo base del cliente</param>
        /// <param name="oraIngresso">L'orario di ingresso del cliente.</param>
        public Cliente( int codice, PrezzoBase prezzo, DateTime oraIngresso )
        {
            // Controllo dei parametri in ingresso
            if( prezzo == null )
            {
                throw new ArgumentNullException( "prezzo" );
            }

            this.m_codice = codice;
            this.m_prezzo = prezzo;
            this.m_orarioIngresso = oraIngresso;
            this.m_orarioUscita = oraIngresso + this.TipoCliente.DurataBiglietto;
            if( this.m_orarioUscita > Parco.GetParco().OrarioChiusura )
            {
                this.m_orarioUscita = Parco.GetParco().OrarioChiusura;
            }
            this.Uscito = false;
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="codice">Il codice del cliente</param>
        /// <param name="prezzo">Il prezzo base del cliente</param>
        public Cliente( int codice, PrezzoBase prezzo ) : this( codice, prezzo, new DateTime() )
        {
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Cliente"/> is reclaimed by garbage collection.
        /// </summary>
        ~Cliente()
        {
            // Quando il cliente viene eliminato devo assicurarmi di liberare il briefing che può occupare
            this.m_codice = -1;     // Note: questo viene fatto perchè è stato modificato Equals() exception ci possono essere dei Clone()
            this.UscitaCliente();
        }

        #endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Calcola il prezzo personale, ovvero il prezzo base cui è applicato un eventuale sconto personale
        /// </summary>
        /// <returns>Restituisce il prezzo personale</returns>
        public decimal GetPrezzoPersonale()
        {
            if( this.Sconto == null )
            {
                return PrezzoBase;
            }

            return Sconto.ScontaPrezzo( PrezzoBase );
        }

        /// <summary>
        /// Calcola il prezzo scontato, ovvero il prezzo base a cui sono applicati, se presenti, prima lo sconto
        /// personale e poi lo sconto comitiva
        /// </summary>
        /// <returns></returns>
        public decimal GetPrezzoScontato()
        {
            if( this.ScontoComitiva == null )
            {
                return this.GetPrezzoPersonale();
            }

            return this.ScontoComitiva.ScontaPrezzo( this.GetPrezzoPersonale() );
        }

        /// <summary>
        /// Crea un nuovo oggetto che è una copia dell'istanza corrente.
        /// </summary>
        /// <returns>Nuovo oggetto che è una copia dell'istanza corrente.</returns>
        object ICloneable.Clone()
        {
            return new Cliente( this.m_codice, this.m_prezzo ) { m_briefing = this.m_briefing, m_sconto = this.m_sconto, m_scontoComitiva = this.m_scontoComitiva, m_orarioIngresso = this.m_orarioIngresso, m_orarioUscita = this.m_orarioUscita, Uscito = this.Uscito };
        }

        /// <summary>
        /// Crea un nuovo oggetto che è una copia dell'istanza corrente.
        /// </summary>
        /// <returns>Nuovo oggetto che è una copia dell'istanza corrente.</returns>
        public Cliente Clone()
        {
            return (Cliente)(((ICloneable)this).Clone());
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
        public int CompareTo( Cliente other )
        {
            return this.TipoCliente.CompareTo( other.TipoCliente );
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format( "Cod={0}, Tipo={1}, Sconto={2}, P.Intero={4}, Pagato={5}, Briefing=({3})",
                        this.Codice, this.TipoCliente, this.Sconto ?? this.ScontoComitiva, this.Briefing, this.GetPrezzoPersonale(), this.GetPrezzoScontato() );
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
        /// Collega il cliente ad un briefing. In pratica viene memorizzato un riferimento a quel briefing
        /// </summary>
        /// <param name="briefing">Il briefing in cui inserire il cliente. Può essere <c>null</c></param>
        /// <remarks>
        /// Quando si assegna un briefing vengono automaticamente aggiornati l'orario di ingresso e di uscita del cliente.
        /// Se non viene assegnato un briefing, mediante il valore null, i valori degli orari rimangono invariati.
        /// </remarks>
        public void AssegnaBriefing( IBriefing briefing )
        {
            this.m_briefing = briefing;

            if( briefing != null )
            {
                this.m_orarioIngresso = briefing.Inizio;
                this.m_orarioUscita = briefing.Inizio + this.TipoCliente.DurataBiglietto;
                if( this.m_orarioUscita > Parco.GetParco().OrarioChiusura )
                {
                    this.m_orarioUscita = Parco.GetParco().OrarioChiusura;
                }
            }
        }

        /// <summary>
        /// Effettua l'uscita del cliente togliendolo dal briefing e cambiandone lo stato
        /// </summary>
        public void UscitaCliente()
        {
            if( this.m_briefing != null )
            {
                this.m_briefing.Libera( this );
            }

            this.Uscito = true;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se hanno tutti i campi dello stesso valore, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( Cliente other )
        {
            if( other == null )
            {
                return false;
            }

            return this.m_briefing == other.m_briefing &&
                   this.m_codice == other.m_codice &&
                   this.m_prezzo.Equals( other.m_prezzo ) &&
                   this.m_sconto == other.m_sconto &&
                   this.m_scontoComitiva == other.m_scontoComitiva;
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

            if( obj is Cliente )
            {
                return this.Equals( (Cliente)obj );
            }

            return false;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}