using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IndianaPark.Tools.Xml;
using IndianaPark.Tools.Logging;

namespace IndianaPark.Plugin.Persistence
{
    /// <summary>
    /// Oggetto che si occupa di salvare i parametri di configurazione dei plugin su un database
    /// </summary>
    public sealed class PluginConfigDBPersistence : IConfigPersistence
    {
		#region Fields 

		#region Internal Fields 

        private bool m_submitChanges = true;
        private readonly PluginParametersDataContext m_dataContext;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfigDBPersistence"/> class.
        /// </summary>
        /// <param name="persistence">The persistence.</param>
        public PluginConfigDBPersistence( Plugin.IPersistence persistence )
        {
            // Controllo dei parametri in ingresso
            if( persistence == null )
            {
                throw new ArgumentNullException( "persistence", "You must specify the persistence provider" );
            }
            if( !persistence.IsConnectionInitialized )
            {
                throw new ArgumentOutOfRangeException( "persistence", "The connection was not initialized" );
            }

            // Creo il contesto di lavoro per Cliente
            this.m_dataContext = new PluginParametersDataContext( persistence.GetConnection() );
        }

		#endregion Constructors 

		#region Class-wise Methods 

        private static IConfigValue ConvertFromEntity( TablePluginParameters input )
        {
            object paramValue;

            // Provo a creare un'istanza dell'oggetto a partire dal valore della sua stringa
            Type paramType = Type.GetType( input.Type, true );

            // Deserializzo il valore
            paramValue = input.Value.DeserializeXml( paramType );

            string description = input.Description ?? "";

            // Questa distinzione è richiesta da PluginConfigValue
            if( paramValue == null )
            {
                return new Plugin.PluginConfigValue( input.Name, paramType, description, input.IsPublic, input.IsPublic );
            }

            return new Plugin.PluginConfigValue( input.Name, paramValue, description, input.IsPublic, input.IsReadonly );
        }

        private static TablePluginParameters ConvertFromConfigValue( IConfigValue input, string owner )
        {
            try
            {
                // Assegno i valori
                var output = new TablePluginParameters
                {
                    Type = input.ValueType.AssemblyQualifiedName,
                    Description = input.Description ?? "",
                    Name = input.Name,
                    Owner = owner,
                    IsPublic = input.IsPublic,
                    IsReadonly = input.IsReadonly,
                    Value = input.Value.ToXml()
                };

                return output;
            }
            catch( Exception )
            {
                return null;
            }
        }

		#endregion Class-wise Methods 

		#region Public Methods 

        /// <summary>
        /// Carica tutti i parametri dei plugin
        /// </summary>
        /// <param name="owner">Il nome del plugin di cui caricare i parametri</param>
        /// <returns>La lista con i valori di configurazione per il plugin</returns>
        public IDictionary<string, IConfigValue> LoadAllParameters( string owner )
        {
            var output = new Dictionary<string, IConfigValue>();

            foreach( var parameter in m_dataContext.TablePluginParameters.Where( item=>item.Owner == owner ) )
            {
                var tmp = this.LoadParameter( parameter.Name, owner );
                if( tmp != null )
                {
                    output.Add( parameter.Name, tmp );
                }
            }

            return output;
        }

        /// <summary>
        /// Carica un parametro
        /// </summary>
        /// <param name="name">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui fa parte</param>
        /// <returns>Il valore di configurazione del plugin</returns>
        public IConfigValue LoadParameter( string name, string owner )
        {
            try
            {
                // Controllo se è da aggiungere o modificare
                var check = from p in m_dataContext.TablePluginParameters
                            where p.Name == name && p.Owner == owner
                            select p;

                if( check.Count() == 0 )
                {
                    return null;
                }

                var tmp = PluginConfigDBPersistence.ConvertFromEntity( check.Single() );
                Logger.Default.Write( String.Format( "Plugin parameter {0} of {1} loaded to cache with string value {2}", tmp.Name, owner, tmp.Value ?? "" ), Verbosity.InformationDebug | Verbosity.Data );
                return tmp;
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, String.Format( "Error while loading a parameter '{0}' for a plugin into the database", name ) );
                return null;
            }
        }

        /// <summary>
        /// Salva un parametro
        /// </summary>
        /// <param name="config">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui fa parte</param>
        /// <returns>Il valore di configurazione del plugin</returns>
        public bool SaveParameter( IConfigValue config, string owner )
        {
            try
            {
                // Controllo se è da aggiungere o modificare
                var check = from p in m_dataContext.TablePluginParameters
                            where p.Name == config.Name && p.Owner == owner
                            select p;

                if( check.Count() == 0 )
                {
                    // Aggiungo
                    m_dataContext.TablePluginParameters.InsertOnSubmit( PluginConfigDBPersistence.ConvertFromConfigValue( config, owner ) );
                }
                else
                {
                    // Modifico
                    var tblRow = check.Single();
                    var newRow = PluginConfigDBPersistence.ConvertFromConfigValue( config, owner );

                    tblRow.Description = newRow.Description;
                    tblRow.IsPublic = newRow.IsPublic;
                    tblRow.IsReadonly = newRow.IsReadonly;
                    tblRow.Type = newRow.Type;
                    tblRow.Value = newRow.Value;
                }

                if( m_submitChanges )
                {
                    m_dataContext.SubmitChanges();
                }
                Logger.Default.Write( String.Format( "Plugin parameter {0} of {1} stored in cache with string value {2}", config.Name, owner, config.Value ?? "" ), Verbosity.InformationDebug | Verbosity.Data );
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, String.Format( "Error while storing parameter {0}' for a plugin into the database", config.Name ) );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Salva tutti i parametri dei plugin
        /// </summary>
        /// <param name="configs">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui caricare i parametri</param>
        /// <returns>La lista con i valori di configurazione per il plugin</returns>
        public bool SaveAllParameters( IDictionary<string, IConfigValue> configs, string owner )
        {
            bool check = true;

            this.m_submitChanges = false;
            foreach( var parameter in configs.Values )
            {
                check &= this.SaveParameter( parameter, owner );
            }
            this.m_submitChanges = true;

            try
            {
                m_dataContext.SubmitChanges();
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Error while storing a parameter for a plugin into the database" );
                return false;
            }

            return check;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
