using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using IndianaPark.Tools.Logging;

namespace IndianaPark.PercorsiAvventura.Model 
{
	/// <summary>
	/// Classe base rappresentante il parco e le azioni base.
	/// </summary>
	public class Parco 
	{
		#region Fields

        #region Internals

        /// <summary>
        /// Numero di cifre per il codice del nominativo
        /// </summary>
        private static Parco ms_Instance;

        private readonly Dictionary<string, IScontoComitiva> m_scontiComitiva = new Dictionary<string, IScontoComitiva>( 4 );
        private readonly Dictionary<string, ISconto> m_listaSconti = new Dictionary<string, ISconto>();
        private readonly Dictionary<string, ISconto> m_scontiPersonalizzati = new Dictionary<string,ISconto>();
        private readonly Dictionary<string, ITipoBriefing> m_tipiBriefing = new Dictionary<string, ITipoBriefing>( 4 );
        private readonly Dictionary<string, TipoCliente> m_tipiCliente = new Dictionary<string, TipoCliente>( 6 );
        private readonly Dictionary<string, TipoBiglietto> m_tipiBiglietto = new Dictionary<string, TipoBiglietto>( 5 );
        private readonly Dictionary<string, PrezzoBase> m_listinoPrezzi = new Dictionary<string, PrezzoBase>( 12 );
        private readonly Dictionary<string, Nominativo> m_listaClienti = new Dictionary<string, Nominativo>();
        private readonly List<IAbbonamento> m_listaAbbonamenti = new List<IAbbonamento>();
        private DateTime m_orarioApertura;
        private DateTime m_orarioChiusura;

        #endregion

        #region Publics

        /// <summary>
		/// Orario di apertura del parco.
		/// </summary>
		public DateTime OrarioApertura
		{
			get { return this.m_orarioApertura; }
		}

		/// <summary>
		/// Orario di chiusura del parco.
		/// </summary>
		public DateTime OrarioChiusura
		{
			get { return this.m_orarioChiusura; }
		}

        /// <summary>
        /// Restituisce le tipologie di biglietti ammesse nel parco
        /// </summary>
		public Dictionary<string, TipoBiglietto> TipologieBiglietto
		{
            get { return this.m_tipiBiglietto; }
		}

        /// <summary>
        /// Restituisce la lista dei clienti nel parco
        /// </summary>
	    public Dictionary<string, Nominativo> ListaClienti
	    {
	        get { return this.m_listaClienti; }
	    }

        /// <summary>
        /// Restituisce la lista degli sconti personali di cui il parco dispone
        /// </summary>
	    public Dictionary<string, ISconto> ScontiPersonali
	    {
            get { return this.m_listaSconti; }
	    }

	    /// <summary>
        /// Restituisce la lista degli sconti personalizzati, ovvero quelli creati dall'utente a runtime e non preselezionati
        /// </summary>
        public Dictionary<string, ISconto> ScontiPersonalizzati
	    {
	        get { return m_scontiPersonalizzati; }
	    }

	    /// <summary>
        /// Restituisce la lista degli sconti comitiva di cui il parco dispone
        /// </summary>
        public Dictionary<string, IScontoComitiva> ScontiComitiva
        {
            get { return this.m_scontiComitiva; }
        }

        /// <summary>
        /// Il listino prezzi del parco.
        /// </summary>
        public Dictionary<string, PrezzoBase> ListinoPrezzi
        {
            get { return this.m_listinoPrezzi; }
        }

        /// <summary>
        /// La lista degli abbonamenti caricati del parco
        /// </summary>
        public List<IAbbonamento> Abbonamenti
        {
            get { return this.m_listaAbbonamenti; }
        }

        #endregion

        #endregion

		#region Methods

        #region Constructors

        /// <summary>
        /// Costruttore
        /// </summary>
        protected Parco()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Funzione provvisoria per l'inizializzazione dei dati del parco
        /// </summary>
        public void Init()
        {
            // Impostazione degli orari di apertura exception chiusura
            PluginPercorsi.GetGlobalParameter( "OpeningTime" ).Changed += this.UpdateTimings;
            PluginPercorsi.GetGlobalParameter( "ClosingTime" ).Changed += this.UpdateTimings;
            PluginPercorsi.GetGlobalParameter( "LastBriefingBefore" ).Changed += this.UpdateTimings;

            this.UpdateTimings( null, null );

            // Sconti Personali
            this.m_listaSconti.Add( "omaggio", new ScontoOmaggio( "Biglietto Omaggio" ) );
            this.m_listaSconti.Add( "fisso_2", new ScontoFisso( "Sconto di 2€", 2m ) );
            this.m_listaSconti.Add( "percento_20", new ScontoPercentuale( "Sconto del 20%", 0.2 ) );

            this.m_listaSconti.Add( "percento_20_adu", new ScontoFisso( "-20% Adulto", 4.5 ) );
            this.m_listaSconti.Add( "percento_20_rag", new ScontoFisso( "-20% Ragazzo", 3.5 ) );
            this.m_listaSconti.Add( "percento_20_bam", new ScontoFisso( "-20% Bambino", 2.0 ) );
            this.m_listaSconti.Add( "percento_20_bab", new ScontoFisso( "-20% Baby", 1.0 ) );

            // Sconti Comitiva
            this.m_scontiComitiva.Add( "comitiva", new ScontoConteggio( "Sconto Comitiva", 0, 10 ) );
            this.m_scontiComitiva.Add( "comitiva_organizzata", new ScontoConteggio( "Comitiva Organizzata", 0, 20 ) );

            // Tipi di Briefing
            this.m_tipiBriefing.Add( "adulti", new TipoBriefingGenerico( new TimeSpan( 0, 20, 0 ), "Briefing Adulti", 20, new TimeSpan( 0, 10, 0 ) ) );
            this.m_tipiBriefing.Add( "bambini", new TipoBriefingGenerico( new TimeSpan( 0, 20, 0 ), "Briefing Bambini", 20, new TimeSpan( 0, 10, 0 ) ) );
            this.m_tipiBriefing.Add( "baby", new TipoBriefingImmediato( "Briefing Baby", new TimeSpan( 0, 0, 0 ) ) );
            //this.m_tipiBriefing.Add( "notturna", new TipoBriefingNotturna( "Briefing Notturna" ) );

            // Tipi di clienti
            this.m_tipiCliente.Add( "adulti", new TipoCliente( 1, "Adulto", new TimeSpan( 3, 0, 0 ), m_tipiBriefing["adulti"] ) );
            this.m_tipiCliente.Add( "ragazzi", new TipoCliente( 1, "Ragazzi", new TimeSpan( 3, 0, 0 ), m_tipiBriefing["adulti"] ) );
            this.m_tipiCliente.Add( "bambini", new TipoCliente( 1, "Bambini", new TimeSpan( 1, 30, 0 ), m_tipiBriefing["bambini"] ) );
            this.m_tipiCliente.Add( "baby", new TipoCliente( 1, "Baby", new TimeSpan( 1, 0, 0 ), m_tipiBriefing["baby"] ) );
            //this.m_tipiCliente.Add( "adulti_notturna", new TipoCliente( 1, "Adulto", new TimeSpan( 3, 0, 0 ), m_tipiBriefing["notturna"] ) );

            // Tipi di biglietto
            this.m_tipiBiglietto.Add( "base", new TipoBiglietto( "Biglietto Base", m_scontiComitiva["comitiva"] ) );
            this.m_tipiBiglietto.Add( "gruppi", new TipoBiglietto( "Gruppi Organizzati", m_scontiComitiva["comitiva_organizzata"] ) );
            //this.m_tipiBiglietto.Add( "notturna", new TipoBiglietto( "Biglietto Notturna", m_scontiComitiva["comitiva"] ) );
            //this.m_tipiBiglietto.Add( "abbonamento", new TipoBiglietto( "Abbonamento", m_scontiComitiva["comitiva"] ) { IsAbbonamento = true } );

            // Tipi di clienti
            this.m_tipiBiglietto["base"].TipiCliente.Add( "adulti", m_tipiCliente["adulti"] );
            this.m_tipiBiglietto["base"].TipiCliente.Add( "ragazzi", m_tipiCliente["ragazzi"] );
            this.m_tipiBiglietto["base"].TipiCliente.Add( "bambini", m_tipiCliente["bambini"] );
            this.m_tipiBiglietto["base"].TipiCliente.Add( "baby", m_tipiCliente["baby"] );
            // ---
            this.m_tipiBiglietto["gruppi"].TipiCliente.Add( "adulti", m_tipiCliente["adulti"] );
            this.m_tipiBiglietto["gruppi"].TipiCliente.Add( "ragazzi", m_tipiCliente["ragazzi"] );
            this.m_tipiBiglietto["gruppi"].TipiCliente.Add( "bambini", m_tipiCliente["bambini"] );
            this.m_tipiBiglietto["gruppi"].TipiCliente.Add( "baby", m_tipiCliente["baby"] );
            // ---
            //this.m_tipiBiglietto["notturna"].TipiCliente.Add( "adulti", m_tipiCliente["adulti_notturna"] );
            
            // Listino prezzi
            this.m_listinoPrezzi.Add( "base_adulti", new PrezzoBase( "Base Adulti", m_tipiCliente["adulti"], m_tipiBiglietto["base"], 22 ) );
            this.m_listinoPrezzi.Add( "base_ragazzi", new PrezzoBase( "Base Ragazzi", m_tipiCliente["ragazzi"], m_tipiBiglietto["base"], 18 ) );
            this.m_listinoPrezzi.Add( "base_bambini", new PrezzoBase( "Base Bambini", m_tipiCliente["bambini"], m_tipiBiglietto["base"], 10 ) );
            this.m_listinoPrezzi.Add( "base_baby", new PrezzoBase( "Base Baby", m_tipiCliente["baby"], m_tipiBiglietto["base"], 5 ) );
            /*
            this.m_listinoPrezzi.Add( "gruppo_adulti", new PrezzoBase( "Gruppo Adulti", m_tipiCliente["adulti"], m_tipiBiglietto["gruppi"], 20 ) );
            this.m_listinoPrezzi.Add( "gruppo_ragazzi", new PrezzoBase( "Gruppo Ragazzi", m_tipiCliente["ragazzi"], m_tipiBiglietto["gruppi"], 16 ) );
            this.m_listinoPrezzi.Add( "gruppo_bambini", new PrezzoBase( "Gruppo Bambini", m_tipiCliente["bambini"], m_tipiBiglietto["gruppi"], 8 ) );
            this.m_listinoPrezzi.Add( "gruppo_baby", new PrezzoBase( "Gruppo Baby", m_tipiCliente["baby"], m_tipiBiglietto["gruppi"], 4 ) );

            this.m_listinoPrezzi.Add( "notturna_adulti", new PrezzoBase( "Notturna Adulti", m_tipiCliente["adulti_notturna"], m_tipiBiglietto["notturna"], 25 ) );
            */
        }

        /// <summary>
        /// Aggiorna il sistema con i nuovi orari di apertura e chiusura quando questi cambiano.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void UpdateTimings( object sender, EventArgs e )
        {
            var apertura = (TimeSpan)PluginPercorsi.GetGlobalParameter( "OpeningTime" ).Value;
            var chiusura = (TimeSpan)PluginPercorsi.GetGlobalParameter( "ClosingTime" ).Value;

            // Imposto gli orari
            this.m_orarioApertura = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, apertura.Hours, apertura.Minutes, apertura.Seconds );
            this.m_orarioChiusura = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, chiusura.Hours, chiusura.Minutes, chiusura.Seconds );

            // Devo creare o togliere i briefing rispettando i nuovi orari
            foreach( var b in this.TipologieBiglietto.Values )
            {
                foreach( var c in b.TipiCliente.Values )
                {
                    c.TipiBriefing.ReimpostaOrari( m_orarioApertura, m_orarioChiusura );
                }
            }
        }

        /// <summary>
        /// Recupera un nominativo a partire dal codice
        /// </summary>
        /// <param name="codice">Il codice del nominativo da trovare</param>
        /// <returns>L'istanza di <see cref="Nominativo"/> con il codice specificato, oppure <c>null</c> se il codice non ha corrispondenza</returns>
        public Nominativo TrovaNominativo( int codice )
        {
            Nominativo nominativo = null;

            try
            {
                nominativo = this.m_listaClienti.FirstOrDefault( item => item.Value.Codice == codice ).Value;
            }
            catch( NullReferenceException ex )
            {
                Logger.Default.Write( ex, String.Format( "No Nominativo found while searching for {0}", codice ) );
            }
            
            return nominativo;
        }

        /// <summary>
        /// Recupera un cliente a partire dai codici.
        /// </summary>
        /// <param name="codiceNominativo">Il codice nominativo.</param>
        /// <param name="codiceCliente">Il codice cliente.</param>
        /// <returns>Il cliente trovato, oppure <c>null</c> se non ha trovato il cliente.</returns>
        public Cliente TrovaCliente( int codiceNominativo, int codiceCliente )
        {
            var n = this.TrovaNominativo( codiceNominativo );

            if( n == null )
            {
                return null;
            }

            return n.TrovaCliente( codiceCliente );
        }

	    #endregion

        /// <summary>
		/// Utilizzata per creare un'unica istanza della classe
		/// </summary>
		public static Parco GetParco()
		{
            if( ms_Instance == null )
            {
                ms_Instance = new Parco();
            }
            return ms_Instance;
		}

        /// <summary>
        /// Recupera la lista di tutti i clienti caricati nel parco
        /// </summary>
        /// <returns>La lista completa dei clienti caricati nel parco</returns>
        public static List<Model.Cliente> GetFullRawList()
        {
            var clienti = new List<Model.Cliente>();

            foreach( var i in Model.Parco.GetParco().ListaClienti )
            {
                clienti.AddRange( i.Value.GetRawList() );
            }

            return clienti;
        }

        /// <summary>
        /// Crea un nuovo identificativo per i nominativi univoco all'interno delle liste nominativi caricati in memoria
        /// </summary>
        /// <returns>Un codice univoco nella lista nominativi caricata</returns>
        public static int CreateNominativoUid()
        {
            return CreateNominativoUid( Parco.GetParco().m_listaClienti.Values );
        }

        /// <summary>
        /// Crea un nuovo identificativo per i nominativi univoco all'interno delle lista di nominativi specificata
        /// </summary>
        /// <returns>Un codice univoco nella lista nominativi caricata</returns>
        public static int CreateNominativoUid( IEnumerable<Nominativo> set )
        {
            if( set == null )
            {
                throw new ArgumentNullException( "set" );
            }

            var size = (int)Math.Pow( 10, (int)PluginPercorsi.GetGlobalParameter( "NominativoCodeSize" ).Value );
            var rnd = new Tools.StartRandom();
            var codice = 0;

            // Creo un codice casuale che non sia gia stato usato
            do
            {
                codice = rnd.Next( size );
            }
            while( set.Any( item => item.Codice == codice ) );

            return codice;
        }

        /// <summary>
        /// Crea un nuovo identificativo per i clienti univoco all'interno del nominativo in cui sono inseriti
        /// </summary>
        /// <param name="set">Ambito di univocità del codice cliente</param>
        /// <param name="partial">Un eventuale <see cref="Inserimento"/> non ancora aggiunto a <paramref name="set"/>. Può essere <c>null</c></param>
        /// <returns>
        /// Un codice cliente univoco all'interno del nominativo indicato
        /// </returns>
        public static int CreateClienteUid( IEnumerable<Inserimento> set, IEnumerable<Cliente> partial )
        {
            if( set == null )
            {
                throw new ArgumentNullException( "set" );
            }

            var uniqueList = new List<Cliente>( partial );
            set.ToList().ForEach( uniqueList.AddRange );
            return CreateClienteUid( uniqueList );
        }

        /// <summary>
        /// Crea un nuovo identificativo per i clienti univoco all'interno dell'inserimento in cui sono inseriti
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>Un codice cliente univoco all'interno dell'inserimento indicato</returns>
        public static int CreateClienteUid( IEnumerable<Cliente> set )
        {
            if( set == null )
            {
                throw new ArgumentNullException( "set" );
            }

            var size = (int)Math.Pow( 10, (int)PluginPercorsi.GetGlobalParameter( "ClienteCodeSize" ).Value );
            var rnd = new Tools.StartRandom();
            int codice = 0;

            // Creo un codice casuale che non sia gia stato usato
            do
            {
                codice = rnd.Next( size );
            }
            while( set.Any( item => item.Codice == codice ) );

            return codice;
        }

	    #endregion
	}
}