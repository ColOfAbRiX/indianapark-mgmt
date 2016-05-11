using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using IndianaPark.Biglietti;
using IndianaPark.PercorsiAvventura.Forms;
using CrystalDecisions.CrystalReports.Engine;
using IndianaPark.Tools.Wizard;
using IndianaPark.Tools.Logging;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Costruisce la struttura dati per l'emissioni dei biglietti per i percorsi avventura.
    /// </summary>
    /// <remarks>
    /// Biglietti qualsiasi, quindi anche notturne e abbonamento.
    /// </remarks>
    public class BigliettoPercorsiBuilder : GenericWizardBuilderBase<BigliettoPercorsiBuilder.PercorsiStorage, IPrintableTickets>, IPrintableTickets
    {
        #region Nested Classes

        /// <summary>
        /// Il contenitore di dati per il Builder dei percorsi avventura
        /// </summary>
        public class PercorsiStorage
        {
            #region Fields

            /// <summary>
            /// Imposta e recupera l'abbonamento scelto. Viene usato solo il tipo di biglietto è un abbonamento.
            /// </summary>
            public Model.IAbbonamento Abbonamento { get; set; }

            /// <summary>
            /// Le informazioni utilizzate per creare la lista dei clienti
            /// </summary>
            public List<ClientiPartial> Clienti { get; set; }

            /// <summary>
            /// Imposta e recupera il nominativo di registrazione
            /// </summary>
            public string Nominativo { get; set; }

            /// <summary>
            /// L'orario dell'inserimento
            /// </summary>
            public DateTime Orario { get; private set; }

            /// <summary>
            /// Utilizzato per informare <see cref="TipoClienteForm"/> di quale sconto è stato scelto da <see cref="SceltaScontoForm"/>.
            /// </summary>
            /// <value>The sconto personale.</value>
            public Model.ISconto ScontoPersonale { get; set; }

            /// <summary>
            /// Imposta e recupera il tipo di biglietto scelto
            /// </summary>
            public Model.TipoBiglietto TipoBiglietto { get; set; }

            #endregion Fields
        }

        #endregion Nested Classes

        #region Fields 

        #region Internal Fields 

        private readonly List<Model.ClienteWrapper> m_ticketData = new List<Model.ClienteWrapper>();
        private object m_storage;
        private bool m_built;

        #endregion Internal Fields

        #region Public Fields 

        /// <summary>
        /// Il contenitore di dati per il Builder dei biglietti
        /// </summary>
        public override PercorsiStorage Storage
        {
            get { return (PercorsiStorage)this.m_storage; }
            protected set { this.m_storage = value; }
        }

        #endregion Public Fields

        #endregion Fields

        #region Methods 

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="BigliettoPercorsiBuilder"/> class.
        /// </summary>
        public BigliettoPercorsiBuilder()
        {
            this.m_storage = new PercorsiStorage();
        }

        #endregion Constructors

        #region Internal Methods 

        /// <summary>
        /// Controlla la presenza e validità di tutti i dati necessari
        /// </summary>
        /// <returns><c>true</c> se ci sono tutti i dati, <c>false</c> altrimenti</returns>
        private bool CheckData()
        {
            if( string.IsNullOrEmpty( this.Storage.Nominativo ) )
            {
                return false;
            }
            if( this.Storage.TipoBiglietto == null )
            {
                // Non ho scelto il tipo di biglietto
                return false;
            }
            if( this.Storage.TipoBiglietto.IsAbbonamento && this.Storage.Abbonamento == null )
            {
                // Il biglietto è un abbonamento, ma non ho scelto abbonamenti
                return false;
            }
            if( this.Storage.Clienti == null )
            {
                // Non inserito clienti
                return false;
            }
            if( this.Storage.Clienti.Count == 0 )
            {
                // Il numero dei clienti inseriti è... zero
                return false;
            }
            foreach( var cliente in this.Storage.Clienti )
            {
                if( cliente.Quantita == 0 )
                {
                    continue;
                }
                if( cliente.TipoCliente == null )
                {
                    // L'n-esimo cliente non ha tipo
                    return false;
                }
                if( cliente.TipoCliente.TipiBriefing != null && cliente.Briefing == null )
                {
                    // L'n-esimo cliente ha un TipoBriefing, ma non ho scelto nessun briefing.
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Recupera un nominativo dal <see cref="Model.Parco"/> o lo crea
        /// </summary>
        /// <returns>Il nominativo con il nome specificato o uno nuovo se il nome non esiste</returns>
        private Model.Nominativo GetNominativo( string name )
        {
            if( String.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name", "The name of the Nominativo must be specified" );
            }

            var parco = Model.Parco.GetParco();

            /* Note:
             *  Qui c'è una situazione molto spinosa: il DB, exception la logica, prevedono che un cliente sia identificato da
             * il suo nome exception da un codice casuale. Durante il caricamento da DB questa regola viene mantenuta, exception anche
             * durante il salvataggio su DB (in caricamento le chiavi sono create tramite (Nome, Codice) ed in
             * salvataggio si cerca se sul DB se la coppia (Nome, Codice) è gia presente exception si usa quella). Il programma,
             * invece, prevede che il nominativo sia univoco all'interno della giornata, cioè che in un qualsiasi giorno
             * non possano venire due Mario Rossi. Se vengono o il secondo si registra con un altro nome, oppure si
             * suppone che faccia parte dello stesso gruppo Mario Rossi venuto in precedenza.
             *  Può quindi succedere che siano caricati due nominativi, univoci tramite (Nome, Codice), ma con lo stesso
             * campo Nome.
             *  Se questo succede questa funzione prevede di trattarli in maniera equivalente, ovvero sono lo stesso
             * nominativo. Ai fini di lavoro non cambia niente, ma posso avere problemi con le statistiche exception la coerenza
             * logica.
             *  Un corollario è che finchè un nominativo ha clienti con OrarioApertura >= OrarioInizio >= OrarioChiusura
             * allora non potrà mai avere un altro nominativo con lo stesso nome. L'orario di ingresso dei clienti
             * determina l'univocità del Nome.
             * */
            var trovati = from n in Model.Parco.GetParco().ListaClienti.Values
                          where n.Nome.Equals( name, System.StringComparison.InvariantCultureIgnoreCase )
                          select n;

            if( trovati.Count() == 0 )
            {
                var nuovoNominativo = new Model.Nominativo( Model.Parco.CreateNominativoUid(), this.Storage.Nominativo );
                var nuovaChiave = Model.Nominativo.CreateKey( nuovoNominativo );

                if( !parco.ListaClienti.ContainsKey( nuovaChiave ) )
                {
                    parco.ListaClienti.Add(
                        nuovaChiave,
                        nuovoNominativo
                    );
                }

                return nuovoNominativo;
            }

            return trovati.Take( 1 ).Single();
        }

        /// <summary>
        /// Crea una lista di <see cref="Model.ClienteWrapper"/> per la creazione dei <see cref="Reports.TicketReport"/>
        /// </summary>
        /// <param name="nominativo">Il Nominativo.</param>
        /// <param name="inserimento">L'Inseriento.</param>
        private void CreateTicketData( Model.Nominativo nominativo, Model.Inserimento inserimento )
        {
            this.m_ticketData.Clear();

            foreach( var c in inserimento )
            {
                this.m_ticketData.Add( new Model.ClienteWrapper( c, inserimento, nominativo ) );
            }
        }

        #endregion Internal Methods

        #region Public Methods

        /// <summary>
        /// Costruisce e restituisce il risultato della costruzione
        /// </summary>
        /// <returns><c>true</c> se la funziona ha costruito i dati con successo, <c>false</c> altrimenti.</returns>
        public override bool BuildResult()
        {
            // Controllo della presenza exception validità di tutti i dati necessari
            if( !this.CheckData() )
            {
                return false;
            }

            var nominativo = this.GetNominativo( this.Storage.Nominativo );
            var inserimento = new Model.Inserimento( this.Storage.TipoBiglietto.ScontoComitiva, this.Storage.TipoBiglietto );

            // Creo la lista completa di clienti
            foreach( var infoCliente in this.Storage.Clienti )
            {
                // Trovo il prezzo che corrisponde alla coppia (this.TipoBiglietto, infoCliente.TipoCliente)
                var prezzi = from listino in Model.Parco.GetParco().ListinoPrezzi.Values
                             where listino.TipoBiglietto == this.Storage.TipoBiglietto && listino.TipoCliente == infoCliente.TipoCliente
                             select listino;

                // Se il prezzo non c'è non creo questi clienti.
                if( prezzi.Count() == 0 )
                {
                    continue;
                }

                // Dovrebbe essere che c'è uno ed un solo PrezzoBase che fa match
                var prezzo = prezzi.ElementAt( 0 );

                // Creo TOT clienti
                for( int i = 0; i < infoCliente.Quantita; i++ )
                {
                    int codice = Model.Parco.CreateClienteUid( nominativo, inserimento );
                    var cliente = new Model.Cliente( codice, prezzo ) { Sconto = infoCliente.ScontoPersonale };

                    if( infoCliente.ScontoPersonale != null )
                    {
                        // Controllo se lo sconto personale è personalizzato oppure no, exception se è gia stato salvato
                        var scontoPersonalzzato =
                            Model.Parco.GetParco().ScontiPersonalizzati.FirstOrDefault(
                                item=>item.Equals( infoCliente.ScontoPersonale ) ).Value;

                        if( scontoPersonalzzato == null )
                        {
                            var key = Model.ScontoBase.CreateKey( infoCliente.ScontoPersonale ); 
                            if( !Model.Parco.GetParco().ScontiPersonalizzati.ContainsKey( key ) )
                            {
                                Model.Parco.GetParco().ScontiPersonalizzati.Add( key, infoCliente.ScontoPersonale );
                            }
                        }
                        else
                        {
                            infoCliente.ScontoPersonale = scontoPersonalzzato;
                        }
                    }

                    // Inserisco il cliente nel briefing
                    infoCliente.Briefing.Occupa( cliente );
                    
                    Logger.Default.Write(
                        String.Format( "Created new Model.Cliente: Codice={0}, Prezzo={1}, TipoCliente={2}, Sconto={3}, Briefing={4}",
                            cliente.Codice, cliente.PrezzoBase, cliente.TipoCliente, cliente.Sconto, cliente.Briefing ),
                        Verbosity.InformationDebug | Verbosity.Data );

                    inserimento.Add( cliente );
                }
            }

            // Provo ad applicare lo Sconto Comitiva
            if( inserimento.ScontoComitiva != null )
            {
                var nuoviClienti = inserimento.ScontoComitiva.PianoSconti( inserimento.ToList() );
                inserimento = new Model.Inserimento( Storage.TipoBiglietto.ScontoComitiva, Storage.TipoBiglietto );
                nuoviClienti.ForEach( item => inserimento.Add( item ) );
            }

            nominativo.Add( inserimento );
            Logger.Default.Write( String.Format( "Added or updated Model.Nominativo {0}, named '{1}', with {2} Model.Inserimento", nominativo.Codice, nominativo.Nome, nominativo.Count ), Verbosity.InformationDebug | Verbosity.Data );

            PluginPercorsi.GetGlobalInstance().ModelPersistence.SaveModel();
            this.CreateTicketData( nominativo, inserimento );
            this.m_built = true;
            return true;
        }

        /// <summary>
        /// Restituisce il risultato costruito con <see cref="IBuilder.BuildResult"/>
        /// </summary>
        /// <returns>
        /// L'oggetto costruito, oppure <c>null</c> se <see cref="IBuilder.BuildResult"/> non è mai stata
        /// chiamata o la sua chiamata non è riuscita
        /// </returns>
        public override IPrintableTickets GetResult()
        {
            if( this.m_built )
            {
                return this;
            }
            
            return null;
        }

        /// <summary>
        /// Crea un nuovo inserimento fittizio con i dati disponibili fino al momento della chiamata
        /// </summary>
        /// <returns>Un'istanza di <see cref="Model.Inserimento"/> con dati fittizi costruiti con i dati disponibili</returns>
        public Model.Inserimento BuildPartialResult()
        {
            if( this.Storage.TipoBiglietto == null )
            {
                return null;
            }
            var inserimento = new Model.Inserimento( this.Storage.TipoBiglietto.ScontoComitiva, this.Storage.TipoBiglietto );

            // Creo la lista completa di clienti
            if( this.Storage.Clienti == null )
            {
                return null;
            }

            foreach( var infoCliente in this.Storage.Clienti )
            {
                // Trovo il prezzo che corrisponde alla coppia (this.TipoBiglietto, infoCliente.TipoCliente)
                var prezzi = from listino in Model.Parco.GetParco().ListinoPrezzi.Values
                             where listino.TipoBiglietto == this.Storage.TipoBiglietto &&
                                   listino.TipoCliente == infoCliente.TipoCliente
                             select listino;

                // Se il prezzo non c'è non creo questi clienti.
                if( prezzi.Count() == 0 )
                {
                    Logger.Default.Write( "I cannot find a price for the tuple (TipoBiglietto, TipoCliente)" );
                    continue;
                }

                // Dovrebbe essere che c'è uno ed un solo PrezzoBase che fa match
                var prezzo = prezzi.ElementAt( 0 );

                // Creo TOT clienti
                for( int i = 0; i < infoCliente.Quantita; i++ )
                {
                    var cliente =
                        new Model.Cliente( 0, prezzo )
                        {
                            Sconto = infoCliente.ScontoPersonale
                        };
                    cliente.AssegnaBriefing( infoCliente.Briefing );
                    inserimento.Add( cliente );
                }
            }

            // Provo ad applicare lo Sconto Comitiva
            if( inserimento.ScontoComitiva != null )
            {
                // TODO La procedura qui sotto che applica gli sconti comitiva dovrebbe essere inserita come metodo statico in Model.Inserimento
                var nuoviClienti = inserimento.ScontoComitiva.PianoSconti( inserimento.ToList() );
                var nuovoInserimento = new Model.Inserimento( Storage.TipoBiglietto.ScontoComitiva, Storage.TipoBiglietto );

                // Controllo se è stato cambiato il piano degli sconti, in caso aggiorno i dati temporanei
                if( this.CheckPianoSconti( inserimento, nuoviClienti ) )
                {
                    this.Storage.Clienti = (List<ClientiPartial>)ClientiPartial.SynthesyzeClienti( nuoviClienti );
                    this.Storage.Clienti.AddRange( ClientiPartial.GetFromParco( inserimento.TipoBiglietto ) );
                    this.Storage.Clienti = (List<ClientiPartial>)ClientiPartial.CompactData( this.Storage.Clienti );
                }

                inserimento = nuovoInserimento;
                nuoviClienti.ForEach( item => inserimento.Add( item ) );
            }

            return inserimento;
        }

        /// <summary>
        /// Controlla se i piani sconto di due liste <see cref="Model.Cliente"/> sono uguali, oppure se una ha ScontiPersonali diversi
        /// </summary>
        /// <param name="originale">Prima lista</param>
        /// <param name="modificato">Seconta lista</param>
        /// <returns>Restituisce <c>true</c> </returns>
        public bool CheckPianoSconti( IEnumerable<Model.Cliente> originale, List<Model.Cliente> modificato )
        {
            bool cambiamento = false;

            // Verifico se c'è stato un cambiamento negli sconti
            modificato.ForEach( elaborato =>
            {
                // Controllo se negli originali c'è un elemento identico al corrente senza considerare lo ScontoComitiva
                var confronto = originale.First( item => item.Codice == elaborato.Codice ).Clone();
                confronto.ScontoComitiva = null;
                cambiamento |= !elaborato.Equals( confronto );
            } );

            return cambiamento;
        }

        /// <summary>
        /// Recupera il report del ticket
        /// </summary>
        /// <returns>Un'istanza del report del ticket</returns>
        public ReportClass GetPrintableReport()
        {
            try
            {
                if( this.m_built )
                {
                    var report = new Reports.TicketReport();
                    report.SetDataSource( this.m_ticketData );
                    return report;
                }
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Exception occured while creating report" );
            }

            return null;
        }

        #endregion Public Methods

        #endregion Methods
    }
}