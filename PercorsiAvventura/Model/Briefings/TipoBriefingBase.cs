using System;
using System.Collections;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Implementazione astratta dei metodi di gestione comuni delle tipologie di briefing
    /// </summary>
    public abstract class TipoBriefingBase : ITipoBriefing
    {
        #region Fields 

        #region Internal Fields 

        protected DateTime m_orarioInizio;
        protected DateTime m_orarioChiusura;
        protected TimeSpan m_bufferTime;
        protected TimeSpan m_walkTime;
        /// <summary>
        /// Il nome del tipo briefing
        /// </summary>
        protected string m_nome;
        /// <summary>
        /// La lista dei briefing
        /// </summary>
        protected List<IBriefing> m_briefings;
        /// <summary>
        /// Il numero di posti totali in un briefing
        /// </summary>
        protected uint m_postiTotali;

        #endregion Internal Fields 

        #region Public Fields 

        /// <summary>
        /// Indica se all'utente viene permesso di scegliere il briefing
        /// </summary>
        /// <remarks>
        /// Se il campo vale <c>true</c> viene richiesto il briefing più vicino all'ora attuale.
        /// </remarks>
        public virtual bool AskUser
        {
            get { return true; }
        }

        /// <summary>
        /// La durata del briefing
        /// </summary>
        public TimeSpan Durata { get; protected set; }

        /// <summary>
        /// Tempo che la persona ci mette a raggiungere l'area briefing.
        /// </summary>
        public TimeSpan WalkTime
        {
            get { return this.m_walkTime; }
        }

        /// <summary>
        /// Il nome del tipo briefing
        /// </summary>
        public string Nome
        {
            get { return this.m_nome; }
        }

        /// <summary>
        /// Il numero di posti totali in un briefing
        /// </summary>
        public uint PostiTotali
        {
            get { return this.m_postiTotali; }
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
        public static bool operator ==( TipoBriefingBase leftOperand, TipoBriefingBase rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>Restituisce <c>true</c> se i due operandi non rappresentano la stessa istanza</returns>
        public static bool operator !=( TipoBriefingBase leftOperand, TipoBriefingBase rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        #endregion Operators 

        #region Constructors 

        /// <summary>
        /// Il costruttore per un TipoBriefing
        /// </summary>
        /// <param name="durata">La durata di ogni briefing</param>
        /// <param name="nome">Il nome dei briefing</param>
        /// <param name="postiTotali">Il numero di posti disponibili in un briefing</param>
        /// <param name="walkTime">Il tempo che il cliente ci mette ad arrivare all'area briefing</param>
        protected TipoBriefingBase( TimeSpan durata, string nome, uint postiTotali, TimeSpan walkTime )
        {
            this.m_orarioInizio = Parco.GetParco().OrarioApertura;
            this.m_orarioChiusura = Parco.GetParco().OrarioChiusura;
            this.m_bufferTime = (TimeSpan)PluginPercorsi.GetGlobalParameter( "LastBriefingBefore" ).Value;

            this.Durata = durata;
            this.m_walkTime = walkTime;
            this.m_nome = nome;
            this.m_postiTotali = postiTotali;
            this.m_briefings = Briefing.CreaListaBriefings( m_orarioInizio, m_orarioChiusura - m_bufferTime, this );
            this.m_briefings.Sort();
        }

        #endregion Constructors 

        #region Public Methods 

        /// <summary>
        /// Restituisce un enumeratore che scorre un insieme.
        /// </summary>
        /// <returns>Oggetto IEnumerator che può essere utilizzato per scorrere l'insieme.</returns>
        public IEnumerator GetEnumerator()
        {
            return this.m_briefings.GetEnumerator();
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
            return new TipoBriefingGenerico( new TimeSpan( this.Durata.Ticks ), (string)this.m_nome.Clone(), this.m_postiTotali, this.m_walkTime );
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public TipoBriefingBase Clone()
        {
            return (TipoBriefingBase)((ICloneable)this).Clone();
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
        }

        /// <summary>
        /// Recupera il primo briefing successivo all'orario indicato
        /// </summary>
        /// <param name="orario">Il primo briefing successivo all'orario indicato, oppure null se non trovato</param>
        public abstract IBriefing TrovaBriefing( DateTime orario );

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se hanno tutti i campi dello stesso valore, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( ITipoBriefing other )
        {
            if( other == null )
            {
                return false;
            }

            return this.Durata.Equals( other.Durata ) &&
                   this.m_nome.Equals( other.Nome, StringComparison.CurrentCultureIgnoreCase ) &&
                   this.m_postiTotali.Equals( other.PostiTotali );
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

            if( obj is ITipoBriefing )
            {
                return Equals( (ITipoBriefing)obj );
            }

            return false;
        }

        /// <summary>
        /// Permette di cambiare l'orario di inizio e fine dei briefing
        /// </summary>
        /// <remarks>
        /// Se un briefing viene eliminato, le persone che vi erano registrare risulteranno senza briefing.
        /// </remarks>
        /// <param name="inizio">L'orario di inizio</param>
        /// <param name="fine">L'orario di fine</param>
        public abstract void ReimpostaOrari( DateTime inizio, DateTime fine );

        #endregion Public Methods 

        #endregion Methods 
    }
}