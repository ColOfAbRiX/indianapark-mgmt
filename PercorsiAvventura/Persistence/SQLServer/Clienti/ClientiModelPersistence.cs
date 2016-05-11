using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using IndianaPark.Tools;
using IndianaPark.Tools.Logging;

namespace IndianaPark.PercorsiAvventura.Persistence.SqlServer
{
    /// <summary>
    /// Classe per il caricamento e salvataggio di oggetti <see cref="Model.Cliente"/>, <see cref="Model.Inserimento"/>
    /// exception <see cref="Model.Nominativo"/> nel modello dei dati.
    /// </summary>
    internal sealed class ClientiModelPersistence : IModelDataAccess
    {
		#region Fields 

		#region Internal Fields 

        private readonly ClientiDataContext m_dataContext;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientiModelPersistence"/> class.
        /// </summary>
        /// <param name="persistence">The persistence.</param>
        public ClientiModelPersistence( Plugin.IPersistence persistence )
        {
            // Controllo dei parametri in ingresso
            if( persistence == null )
            {
                throw new ArgumentNullException( "persistence" );
            }
            if( !persistence.IsConnectionInitialized )
            {
                throw new ArgumentNullException( "persistence", "The connection was not initialized" );
            }

            // Creo il contesto di lavoro per Cliente
            this.m_dataContext = new ClientiDataContext( persistence.GetConnection() );
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Carica i dati del modello relativo ai clienti
        /// </summary>
        /// <remarks>
        /// Quando non viene trovata una corrispondenza con un elemento del modello, per esempio quando viene cercato uno sconto
        /// che non è presente, quell'elemento viene ignorato (impostato a <c>null</c>. Se non viene trovato un elemento essenziale,
        /// come il TipoCliente, il caricamento viene interrotto.
        /// </remarks>
        /// <returns><c>true</c> se il caricamento è andato a buon fine, <c>false</c> altrimenti</returns>
        public bool LoadModel()
        {
            try
            {
                var parco = Model.Parco.GetParco();

                // Recupero dal DB i clienti di giornata exception tutte le informazioni collegate
                var clienti = from c in m_dataContext.TableClienti
                              join i in m_dataContext.TableInserimenti on c.TableInserimenti.Id equals i.Id
                              join n in m_dataContext.TableNominativi on i.TableNominativi.Id equals n.Id
                              where c.OraIngresso >= parco.OrarioApertura && c.OraIngresso <= parco.OrarioChiusura
                              select new { Nominativo = n, Inserimento = i, Cliente = c };
                var inserimenti = (from l in clienti select l.Inserimento).Distinct();
                var nominativi = (from l in clienti select l.Nominativo).Distinct();

                // Inserisco nominativo per nominativo
                foreach( var n in nominativi )
                {
                    // Controllo se aggiungere il nominativo
                    if( parco.ListaClienti.Values.FirstOrDefault( item => item.Nome == n.Nome && item.Codice == n.Codice ) == null )
                    {
                        var nuovo = new Model.Nominativo( n.Codice, n.Nome );
                        parco.ListaClienti.Add( Model.Nominativo.CreateKey( nuovo ), nuovo );
                        Logger.Default.Write( String.Format( "Nominativo {0} loaded in cache from database", n.Id ), Verbosity.InformationDebug | Verbosity.Data );
                    }
                }

                // Inserisco inserimento per inserimento
                foreach( var i in inserimenti )
                {
                    // Controllo che lo sconto comitiva sia corretto
                    Model.IScontoComitiva scontoComitiva = null;
                    if( i.ScontoComitiva != null && parco.ScontiComitiva.ContainsKey( i.ScontoComitiva ) )
                    {
                        scontoComitiva = parco.ScontiComitiva[i.ScontoComitiva];
                    }

                    // Controllo che il tipo biglietto sia corretto
                    Model.TipoBiglietto tipoBiglietto = null;
                    if( i.TipoBiglietto != null && parco.TipologieBiglietto.ContainsKey( i.TipoBiglietto ) )
                    {
                        tipoBiglietto = parco.TipologieBiglietto[i.TipoBiglietto];
                    }

                    // Controllo se aggiungere l'inserimento
                    var nuovoInserimento = new Model.Inserimento( scontoComitiva, tipoBiglietto, i.DataInserimento );

                    // Utilizzato ListaClienti[].First() è implicito che Inserimenti.CodNominativo == Nominativi.Id
                    var key = Model.Nominativo.CreateKey( i.TableNominativi.Codice, i.TableNominativi.Nome );
                    if( parco.ListaClienti[key].FirstOrDefault( item => item.DataInserimento == i.DataInserimento ) == null )
                    {
                        parco.ListaClienti[key].Add( nuovoInserimento );
                        Logger.Default.Write( String.Format( "Inserimento for nominativo {0} with date {1} loaded in cache from database", i.TableNominativi.Id, i.DataInserimento ), Verbosity.InformationDebug | Verbosity.Data );
                    }

                    i.Reference = nuovoInserimento;
                }

                // Inserisco cliente per cliente
                foreach( var c in clienti )
                {
                    // Controllo che il prezzo base sia corretto
                    if( !parco.ListinoPrezzi.ContainsKey( c.Cliente.PrezzoBase ) )
                    {
                        continue;
                    }
                    var prezzoBase = parco.ListinoPrezzi[c.Cliente.PrezzoBase];

                    // Controllo il tipo cliente. Viene memorizzato a parte per accedervi più facilmente
                    if( !prezzoBase.TipoBiglietto.TipiCliente.ContainsKey( c.Cliente.TipoCliente ) )
                    {
                        continue;
                    }
                    var tipoCliente = prezzoBase.TipoBiglietto.TipiCliente[c.Cliente.TipoCliente];

                    // Controllo che lo sconto sia corretto
                    Model.ISconto scontoPersonale = null;
                    if( c.Cliente.Sconto != null )
                    {
                        if( parco.ScontiPersonali.ContainsKey( c.Cliente.Sconto ) )
                        {
                            scontoPersonale = parco.ScontiPersonali[c.Cliente.Sconto];
                        }
                        else if( parco.ScontiPersonalizzati.ContainsKey( c.Cliente.Sconto ) )
                        {
                            scontoPersonale = parco.ScontiPersonalizzati[c.Cliente.Sconto];
                        }
                    }

                    // Controllo che lo sconto comitiva sia corretto
                    Model.IScontoComitiva scontoComitiva = null;
                    if( c.Cliente.ScontoComitiva != null && parco.ScontiComitiva.ContainsKey( c.Cliente.ScontoComitiva ) )
                    {
                        scontoComitiva = parco.ScontiComitiva[c.Cliente.ScontoComitiva];
                    }

                    // Creo il nuovo cliente
                    Model.Cliente nuovoCliente;
                    if( c.Cliente.OraIngresso != null )
                    {
                        nuovoCliente = new Model.Cliente( c.Cliente.Codice, prezzoBase, c.Cliente.OraIngresso.Value ) { Sconto = scontoPersonale, ScontoComitiva = scontoComitiva };
                    }
                    else
                    {
                        nuovoCliente = new Model.Cliente( c.Cliente.Codice, prezzoBase ) { Sconto = scontoPersonale, ScontoComitiva = scontoComitiva };
                    }

                    Logger.Default.Write( String.Format( "Cliente {0} for nominativo {1} loaded in cache from database", c.Cliente.Id, c.Nominativo.Id ), Verbosity.InformationDebug | Verbosity.Data );

                    // Lo faccio uscire se è uscito
                    if( c.Cliente.Uscito )
                    {
                        nuovoCliente.UscitaCliente();
                    }
                    else
                    {
                        // Gli assegno un briefing se serve
                        var briefing = tipoCliente.TipiBriefing.TrovaBriefing( nuovoCliente.OraIngresso );

                        if( briefing != null )
                        {
                            nuovoCliente.AssegnaBriefing( briefing );
                        }
                    }

                    // Lo metto nel suo inserimento
                    if( c.Inserimento.Reference.FirstOrDefault( item => item.Codice == nuovoCliente.Codice ) == null )
                    {
                        c.Inserimento.Reference.Add( nuovoCliente );
                    }
                }
            }
            catch( DbException dbex )
            {
                Logger.Default.Write( dbex,  "Database exception while loading the model" );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Salva i dati del modello relativo ai clienti
        /// </summary>
        /// <returns><c>true</c> se il salvataggio è andato a buon fine, <c>false</c> altrimenti</returns>
        public bool SaveModel()
        {
            var parco = Model.Parco.GetParco();
            //var dbRemainings = m_dataContext.TableClienti.ToList();
            var dbRemainings = (from c in m_dataContext.TableClienti
                                where c.OraIngresso > parco.OrarioApertura && c.OraIngresso < parco.OrarioChiusura
                                select c).ToList();

            try
            {
                // Aggiungo i nominativi
                foreach( var nom in parco.ListaClienti.Values )
                {
                    // Controllo se il nominativo è da aggiungere o da recuperare
                    var nomCheck = from n in m_dataContext.TableNominativi
                                   where n.Codice == nom.Codice && n.Nome == nom.Nome
                                   select n;

                    var nuovoNom = new TableNominativi();

                    // Controllo se il nominativo è da aggiungere o da recuperare
                    if( nomCheck.Count() == 0 )
                    {
                        m_dataContext.TableNominativi.InsertOnSubmit( nuovoNom );
                    }
                    else
                    {
                        nuovoNom = nomCheck.Single();
                    }

                    nuovoNom.Nome = nom.Nome;
                    nuovoNom.Codice = nom.Codice;

                    Logger.Default.Write( String.Format( "Nominativo ({0}, {1}) stored in cache from model", nuovoNom.Codice, nuovoNom.Nome ), Verbosity.InformationDebug | Verbosity.Data );

                    // Aggiungo gli inserimenti
                    foreach( var ins in nom )
                    {
                        // Creo una nuova istanza dell'entità Inserimento
                        var scontoComitiva = parco.ScontiComitiva.KeyOf( ins.ScontoComitiva );
                        var tipoBiglietto = parco.TipologieBiglietto.KeyOf( ins.TipoBiglietto );

                        // Controllo se l'inserimento è da aggiungere o da recuperare
                        var insCheck = from i in m_dataContext.TableInserimenti
                                       where i.TableNominativi == nuovoNom && i.DataInserimento == ins.DataInserimento
                                       select i;

                        var nuovoIns = new TableInserimenti();

                        if( insCheck.Count() == 0 )
                        {
                            m_dataContext.TableInserimenti.InsertOnSubmit( nuovoIns );
                        }
                        else
                        {
                            nuovoIns = insCheck.Single();
                        }

                        nuovoIns.DataInserimento = ins.DataInserimento;
                        nuovoIns.ScontoComitiva = scontoComitiva;
                        nuovoIns.TipoBiglietto = tipoBiglietto;
                        nuovoIns.PrezzoTotale = ins.CalcolaPrezzo();
                        nuovoIns.TableNominativi = nuovoNom;

                        Logger.Default.Write( 
                            String.Format( "Inserimento for nominativo {0} with date {1} stored in cache from model",
                                           nuovoNom.Codice, ins.DataInserimento ),
                            Verbosity.InformationDebug | Verbosity.Data );

                        // Aggiungo i clienti
                        foreach( var cli in ins )
                        {
                            var sconto = parco.ScontiPersonali.KeyOf( cli.Sconto );
                            if( cli.Sconto != null && sconto == null )
                            {
                                // Se non trovo lo sconto tra quelli Personali cerco in quelli personalizzati
                                sconto = parco.ScontiPersonalizzati.KeyOf( cli.Sconto );
                            }
                            var scontoComitivCli = parco.ScontiComitiva.KeyOf( cli.ScontoComitiva );
                            var tipoCliente = ins.TipoBiglietto.TipiCliente.KeyOf( cli.TipoCliente );
                            var prezzoBase = (from k in parco.ListinoPrezzi
                                              where
                                                  k.Value.TipoBiglietto == ins.TipoBiglietto &&
                                                  k.Value.TipoCliente == cli.TipoCliente
                                              select k.Key).FirstOrDefault();

                            // Controllo se il cliente è da aggiungere o da recuperare
                            var cliCheck = from c in m_dataContext.TableClienti
                                           where c.TableInserimenti == nuovoIns && c.Codice == cli.Codice
                                           select c;

                            var nuovoCli = new TableClienti();

                            if( cliCheck.Count() == 0 )
                            {
                                m_dataContext.TableInserimenti.InsertOnSubmit( nuovoIns );
                            }
                            else
                            {
                                nuovoCli = cliCheck.Single();
                                dbRemainings.Remove( nuovoCli );
                            }

                            nuovoCli.Codice = cli.Codice;
                            nuovoCli.OraIngresso = cli.OraIngresso;
                            nuovoCli.OraUscita = cli.OraUscita;
                            nuovoCli.Uscito = cli.Uscito;
                            nuovoCli.TableInserimenti = nuovoIns;
                            nuovoCli.PrezzoBase = prezzoBase;
                            nuovoCli.TipoCliente = tipoCliente;
                            nuovoCli.Sconto = sconto;
                            nuovoCli.ScontoComitiva = scontoComitivCli;
                            // Questo perchè al momento non vengono salvati gli sconti personalizzati
                            if( nuovoCli.PrezzoFinale == null )
                            {
                                nuovoCli.PrezzoFinale = cli.GetPrezzoScontato();
                            }

                            Logger.Default.Write( 
                                String.Format("Cliente {0} for nominativo {1} stored in cache from model", cli.Codice,
                                            nuovoIns.TableNominativi.Id ),
                                Verbosity.InformationDebug | Verbosity.Data );
                        }
                    }
                }

                // Elimino i clienti che non hanno una corrispondenza con il modello caricato in memoria
                foreach( var tc in dbRemainings )
                {
                    m_dataContext.TableClienti.DeleteOnSubmit( tc );
                    Logger.Default.Write( String.Format( "Removed from database Cliente {0}", tc.Codice ), Verbosity.InformationDebug | Verbosity.Data );
                }

                // Applico le modifiche
                m_dataContext.SubmitChanges();
                Logger.Default.Write( "Model cache succesfully dumped to database", Verbosity.InformationDebug );
            }
            catch( DbException dbex )
            {
                Logger.Default.Write( dbex, "Database exception while saving the model" );
                return false;
            }

            return true;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
