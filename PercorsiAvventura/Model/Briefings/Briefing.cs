using System;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Implementazione di base di un briefing generico con un'ora d'inizio.
    /// </summary>
    public class Briefing : IBriefing
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Ora esatta dell'inizio del briefing
        /// </summary>
        private DateTime m_inizio;
        /// <summary>
        /// Contiene un riferimento all'istanza che identifica il tipo di questo briefing
        /// </summary>
        private readonly ITipoBriefing m_tipoBriefing;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Elenco dei clienti inseriti in questo briefing
        /// </summary>
        public List<Cliente> Clienti { get; set; }

        /// <summary>
        /// Durata del briefing, recuperato dalla tipologia di briefing
        /// </summary>
        public TimeSpan Durata
        {
            get { return TipoBriefing.Durata; }
        }

        /// <summary>
        /// Ora esatta dell'inizio del briefing
        /// </summary>
        public DateTime Inizio
        {
            get { return m_inizio; }
            set
            {
                m_inizio = value;
            }
        }

        /// <summary>
        /// Numero di posti già occupati dai clienti nel briefing
        /// </summary>
        public uint PostiOccupati
        {
            get { return (uint)Clienti.Count; }
        }

        /// <summary>
        /// Contiene un riferimento all'istanza che identifica il tipo di questo briefing
        /// </summary>
        public ITipoBriefing TipoBriefing
        {
            get { return m_tipoBriefing; }
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
        public static bool operator ==( Briefing leftOperand, IBriefing rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( Briefing leftOperand, IBriefing rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più piccolo del secondo</returns>
        public static bool operator <( Briefing leftOperand, IBriefing rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) < 0;
        }
        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se il primo operando è più grande del secondo</returns>
        public static bool operator >( Briefing leftOperand, IBriefing rightOperand )
        {
            return leftOperand.CompareTo( rightOperand ) > 0;
        }
        
        #endregion Operators 

		#region Constructors 

        /// <summary>
        /// Costruttore privato perchè non si possono istanziare singoli briefing.
        /// bisogna passare attraverso <see cref="CreaListaBriefings"/>
        /// </summary>
        /// <param name="parent">il tipoBriefing a cui il briefing appartiene</param>
        /// <param name="inizio">l'ora di inizio del briefing</param>
        protected Briefing( ITipoBriefing parent, DateTime inizio )
        {
            // Controllo dei parametri
            if( parent == null )
            {
                throw new ArgumentNullException( "parent" );
            }
            m_tipoBriefing = parent;
            Inizio = inizio;
            Clienti = new List<Cliente>();
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Crea un'istanza di un unico briefing
        /// </summary>
        /// <param name="inizio">L'orario di inizio del briefing</param>
        /// <param name="tipoBriefing">La tipologia di briefing a cui i briefing creati appartiengono</param>
        /// <returns>Un'istanza di Briefing</returns>
        public static IBriefing CreaBriefing( DateTime inizio, ITipoBriefing tipoBriefing )
        {
            return new Briefing( tipoBriefing, inizio );
        }

        /// <summary>
        /// Crea una lista di oggetti Briefing, del tipo specificato, per coprire in difetto l'arco
        /// temporale indicato.
        /// </summary>
        /// <remarks>
        /// I briefing vengono creati dalla DateTime di inizio a quella di fine, anche se questi sono a cavallo di due giorni.
        /// L'intervallo <c></c><paramref name="fine"/> - <paramref name="inizio"/></c> non può essere superiore a 24h, nel qual caso
        /// l'intervallo viene troncato
        /// </remarks>
        /// <param name="inizio">L'orario di inizio del primo briefing.</param>
        /// <param name="fine">L'orario oltre cui non possono esserci altri briefing</param>
        /// <param name="tipoBriefing">La tipologia di briefing a cui i briefing creati appartiengono</param>
        public static List<IBriefing> CreaListaBriefings( DateTime inizio, DateTime fine, ITipoBriefing tipoBriefing )
        {
            if( inizio >= fine )
            {
                throw new ArgumentOutOfRangeException( "fine", "The parameter must respect the rule inizio < fine" );
            }

            var lista = new List<IBriefing>();

            // Converto in data corrente exception ora specificata
            //var tmp = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, inizio.Hour, inizio.Minute, inizio.Second );
            var tmp = new DateTime( inizio.Year, inizio.Month, inizio.Day, inizio.Hour, (int)(5 * Math.Round( inizio.Minute / 5.0 )), 0 );
            fine = tmp + (fine - inizio);
            inizio = tmp;

            // La lista dei briefing non può coprire più di 24 ore, ne essere negativa!
            if( fine - inizio >= new TimeSpan( 24, 0, 0 ) )
            {
                fine = inizio + new TimeSpan( 23, 59, 59 );
            }
            if( fine - inizio <= new TimeSpan( 0, 0, 0 ) )
            {
                fine = inizio;
            }

            /*
            // Se i briefing finiscono il giorno dopo, vuol dire che devono partire il giorno precedente
            if( fine.Day > inizio.Day && fine.Day == DateTime.Now.Day  )
            {
                inizio -= new TimeSpan( 24, 0, 0 );
                fine -= new TimeSpan( 24, 0, 0 );
            }
            */
            
            DateTime prossimo = inizio;
            TimeSpan durata = tipoBriefing.Durata;

            while( prossimo <= fine )
            {
                lista.Add( Briefing.CreaBriefing( prossimo, tipoBriefing ) );
                prossimo += durata;
            }

            return lista;
        }

		#endregion Class-wise Methods 

		#region Public Methods 

        /// <summary>
        /// Controlla se ci sono posti liberi nel briefing confrontando il numero posti disponibili del tipo
        /// di briefing con i posti occupati nel briefing
        /// </summary>
        /// <returns>True se i posti occupati sono minori dei posti totali </returns>
        public bool CheckPostiLiberi()
        {
            return ( TipoBriefing.PostiTotali > PostiOccupati );
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format( "Inizio={0}, Durata={1}, Iscritti={2}", this.Inizio.ToShortTimeString(), this.Durata.TotalMinutes, this.Clienti.Count );
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
            return this.m_inizio.GetHashCode() ^
                   this.m_tipoBriefing.GetHashCode();
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
            return new Briefing( this.m_tipoBriefing, new DateTime( m_inizio.Ticks ) ) { Clienti = this.Clienti };
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Briefing Clone()
        {
            return (Briefing)(((ICloneable)this).Clone());
        }

        /// <summary>
        /// Occupa aggiunge il cliente al briefing di cui si intende usufruire inoltre compila il campo
        /// del cliente col il briefing corretto. Questa funzione exception <see cref="CheckPostiLiberi"/> non hanno
        /// nessun collegamento, quindi è possibile occupare posti anche <see cref="CheckPostiLiberi"/> ritorna
        /// come valore <c>false</c>. La funzione si occupa anche di aggiornare il riferimenti del briefing del cliente
        /// indicato collegandolo a questa istanza di Briefing
        /// </summary>
        /// <param name="cliente">Il cliente da aggiungere al briefing</param>
        public void Occupa( Cliente cliente )
        {
            if( cliente == null )
            {
                throw new ArgumentNullException( "cliente" );
            }

            if( !Clienti.Contains( cliente ) )
            {
                Clienti.Add( cliente );
                cliente.AssegnaBriefing( this );
            }
        }

        /// <summary>
        /// Toglie il cliente dalla lista clienti del briefing e avvisa l'istanza <see cref="Cliente"/> che non
        /// appartiene più a nessun briefing.
        /// </summary>
        /// <param name="cliente">Il cliente da rimuovere dal briefing</param>
        public void Libera( Cliente cliente )
        {
            if( cliente == null )
            {
                throw new ArgumentNullException( "cliente" );
            }

            if( Clienti.Contains( cliente ) )
            {
                Clienti.Remove( cliente );
                cliente.AssegnaBriefing( null );
            }
        }

        /// <summary>
        /// Toglie tutti i clienti assegnati a questo briefing
        /// </summary>
        public void Svuota()
        {
            foreach( var c in this.Clienti )
            {
                this.Libera( c );
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se i due briefing hanno lo stesso inizio e appartengono alla stessa tipologia, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( IBriefing other )
        {
            if( other == null )
            {
                return false;
            }

            return this.m_inizio.Equals( other.Inizio ) &&
                   this.m_tipoBriefing.Equals( other.TipoBriefing );
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

            if( obj is Briefing )
            {
                return this.Equals( (Briefing)obj );
            }

            return false;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// Un numero negativo se il briefing corrente inizia prima di quello, specificato, un numero positivo se inizia dopo, zero altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object</param>
        public int CompareTo( IBriefing other )
        {
            return this.m_inizio.CompareTo( other.Inizio );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}