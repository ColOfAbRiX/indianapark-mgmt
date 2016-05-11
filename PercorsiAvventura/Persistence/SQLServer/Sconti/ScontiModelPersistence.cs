using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using IndianaPark.PercorsiAvventura.Model;
using IndianaPark.Tools;
using IndianaPark.Tools.Xml;
using IndianaPark.Tools.Logging;

namespace IndianaPark.PercorsiAvventura.Persistence.SqlServer
{
    /// <summary>
    /// Classe per il caricamento e salvataggio di oggetti <see cref="Model.ISconto"/>
    /// </summary>
    internal sealed class ScontiModelPersistence : IModelDataAccess
    {
		#region Fields 

		#region Internal Fields 

        private readonly ScontiDataContext m_dataContext;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ScontiModelPersistence"/> class.
        /// </summary>
        /// <param name="persistence">The persistence.</param>
        public ScontiModelPersistence( Plugin.IPersistence persistence )
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
            this.m_dataContext = new ScontiDataContext( persistence.GetConnection() );
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Trasforma un record del database in un'istanza di <see cref="Model.ISconto"/>
        /// </summary>
        /// <param name="sconto">Un oggetto <see cref="TableSconti"/></param>
        /// <returns>Un'istanza di <see cref="Model.ISconto"/> costruita con i dati di <paramref name="sconto"/>,
        /// oppure <c>null</c> in caso di errore</returns>
        private Model.ISconto EntityToSconto( TableSconti sconto )
        {
            // Tipo di dato dello sconto
            Type scontoType;
            try
            {
                scontoType = Type.GetType( sconto.TableTipiSconto.FrameworkType, true );
            }
            catch( System.IO.FileLoadException )
            {
                return null;
            }

            // Controllo la correttezza delle interfacce
            if( !scontoType.ImplementsInterface( typeof( Model.ISconto ) ) )
            {
                return null;
            }
            if( sconto.IsComitiva && !scontoType.ImplementsInterface( typeof( Model.IScontoComitiva ) ) )
            {
                return null;
            }

            // Deserializzazione parametri del costruttore
            var ctorParameters = sconto.CtorParameters.DeserializeXml<List<object>>();

            // Istanzio
            return new SqlScontoClient().InstanceFromParameters( scontoType, ctorParameters );
        }

        /// <summary>
        /// Trasforma un'istanza di <see cref="Model.ISconto"/> in un record del database
        /// </summary>
        /// <param name="sconto">Un oggetto <see cref="Model.ISconto"/></param>
        /// <param name="personalizzato">Indica se è uno sconto personalizzato, creato a mano dall'utente</param>
        /// <returns>
        /// Un'entità <see cref="TableSconti"/> costruita a partire dei dati dello <paramref name="sconto"/>,
        /// oppure <c>null</c> in caso di errore
        /// </returns>
        private TableSconti ScontoToEntity( Model.ISconto sconto, bool personalizzato )
        {
            var parco = Model.Parco.GetParco();
            var output = new TableSconti();
            
            // CTORPARAMETERS
            var parameters = new SqlScontoClient().GetParametersFromInstance( sconto );
            output.CtorParameters = parameters.ToXml();
            if( output.CtorParameters == null )
            {
                output.CtorParameters = new List<object>().ToXml();
            }

            /* Note:
             * La struttura generale prevede che lo stesso sconto possa valere sia come ScontoPersonale che
             * come ScontoComitiva, in questa sezione impongo invece che vengano trattati come due insiemi
             * disgiunti. Dal punto di vista funzionale, al momento, è totalmente indifferente
             */
            // IsComitiva
            output.IsComitiva = sconto.ImplementsInterface( typeof( Model.IScontoComitiva ) );
            // IsPersonale
            output.IsPersonale = !output.IsComitiva;

            // KEY
            if( !output.IsComitiva )
            {
                output.Key = parco.ScontiPersonali.KeyOf( sconto );
            }
            else
            {
                output.Key = parco.ScontiComitiva.KeyOf( (Model.IScontoComitiva)sconto );
            }

            // ISCUSTOM
            output.IsCustom = personalizzato;
            if( personalizzato )
            {
                // Gli sconti personalizzati hanno un altro tipo di convenzione per le chiavi
                output.Key = Model.ScontoBase.CreateKey( sconto );
            }
            // NOME
            output.Nome = sconto.Nome;

            // VALORE
            output.Valore = sconto.Sconto;

            // TIPOSCONTO
            var type = sconto.GetType().FullName;
            var tmp = this.m_dataContext.TableTipiSconto.SingleOrDefault( ts => ts.FrameworkType == type );
            output.TipoSconto = tmp.Tipo;

            return output;
        }

        /// <summary>
        /// Aggiorna i dati del DataContext aggiungendo o modificando i dati
        /// </summary>
        /// <param name="sconti">La lista degli sconti da aggiungere</param>
        /// <param name="custom">Indica se la lista è di sconti personalizzati creati a mano</param>
        private void UpdateTables(IEnumerable<ISconto> sconti, bool custom )
        {
            if( sconti == null )
            {
                throw new ArgumentNullException( "sconti" );
            }

            // Salvo gli sconti personali
            foreach( var sconto in sconti )
            {
                var scontoEntity = this.ScontoToEntity( sconto, custom );
                scontoEntity.IsCustom = custom;

                // Recupero un riferimento al record nella tabella, se esiste
                var check = (from s in m_dataContext.TableSconti
                             where s.Key == scontoEntity.Key
                             select s).SingleOrDefault();

                if( check == null )
                {
                    // Il record non esiste, quindi aggiungo
                    this.m_dataContext.TableSconti.InsertOnSubmit( scontoEntity );
                    Logger.Default.Write( string.Format( "Added sconto {0} to the database", sconto.Nome ), Verbosity.InformationDebug | Verbosity.Data );
                }
                else
                {
                    // Il record esiste, quindi aggiorno
                    if( !scontoEntity.Equals( check ) )
                    {
                        scontoEntity.CopyTo( check );
                        Logger.Default.Write( string.Format( "Updated sconto {0} to the database", sconto.Nome ), Verbosity.InformationDebug | Verbosity.Data );
                    }
                }
            }
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Carica i dati del modello relativo agli sconti
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
                Logger.Default.Write( "Loading sconti from the persistence...", Verbosity.InformationDebug );

                var sconti = from s in m_dataContext.TableSconti
                             select s;

                // Aggiungo tutti gli sconti al parco.
                foreach( var s in sconti )
                {
                    var nuovo = this.EntityToSconto( s );
                    if( nuovo == null )
                    {
                        continue;
                    }

                    try
                    {
                        if( !s.IsCustom )
                        {
                            // Aggiungo come ScontoPersonale
                            if( s.IsPersonale )
                            {
                                Model.Parco.GetParco().ScontiPersonali.Add( s.Key, nuovo );
                                Logger.Default.Write( string.Format( "Sconto {0} added to the list ScontiPersonali", nuovo.Nome ), Verbosity.InformationDebug | Verbosity.Data );
                            }

                            // Aggiungo come ScontoComitiva
                            if( s.IsComitiva && nuovo.ImplementsInterface( typeof( Model.IScontoComitiva ) ) )
                            {
                                Model.Parco.GetParco().ScontiComitiva.Add( s.Key, (Model.IScontoComitiva)nuovo );
                                Logger.Default.Write( string.Format( "Sconto {0} added to the list ScontiComitiva", nuovo.Nome ), Verbosity.InformationDebug | Verbosity.Data );
                            }
                        }
                        else
                        {
                            // Gli sconti personalizzati sono a parte exception si trattato sempre come ScontiPersonali
                            Model.Parco.GetParco().ScontiPersonalizzati.Add( s.Key, nuovo );
                            Logger.Default.Write( string.Format( "Sconto {0} added to the list ScontiPersonalizzati", nuovo.Nome ), Verbosity.InformationDebug | Verbosity.Data );
                        }
                    }
                    catch( ArgumentException aex )
                    {
                        // Se per un qualche motivo la chiave c'è gia, vado avanti senza aggiungere
                        Logger.Default.Write( aex, "Exception while adding Sconti" );
                    }
                }
            }
            catch( DbException dbex )
            {
                Logger.Default.Write( dbex, "Database exception while loading the model" );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Salva i dati del modello relativo agli sconti
        /// </summary>
        /// <returns><c>true</c> se il salvataggio è andato a buon fine, <c>false</c> altrimenti</returns>
        public bool SaveModel()
        {
            try
            {
                Logger.Default.Write( "Saving sconti to the persistence...", Verbosity.InformationDebug );

                var sconti = Model.Parco.GetParco().ScontiPersonali.Values.ToList();
                Model.Parco.GetParco().ScontiComitiva.Values.ToList().ForEach( sconti.Add );

                // Salvo gli sconti personali exception comitiva
                this.UpdateTables( sconti, false );
                // Salvo gli sconti creati a mano
                this.UpdateTables( Model.Parco.GetParco().ScontiPersonalizzati.Values.ToList(), true );

                // Eseguo le modifiche
                this.m_dataContext.SubmitChanges();
            }
            catch( DbException dbex )
            {
                Logger.Default.Write( dbex, "Database exception while loading the model" );
                return false;
            }

            return true;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
