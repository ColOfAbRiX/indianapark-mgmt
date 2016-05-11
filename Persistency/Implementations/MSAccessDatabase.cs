#if MSACCESS
using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using IndianaPark.Tools.Logging;

// ReSharper disable InconsistentNaming
namespace IndianaPark.Persistence
{
    /// <summary>
    /// Factory per la creazione di connessione al database MS Access
    /// </summary>
    public class MSAccessDatabase : PersistenceFactory
    {
        private OleDbConnection m_connection;

        /// <summary>
        /// Crea ed apre la connessione indicata dalla connection string
        /// </summary>
        /// <param name="connectionString">La stringa di connessione al database</param>
        /// <returns>
        /// La connessione al database, oppure <c>null</c> se la creazione o l'apertura non sono andate a buon fine
        /// </returns>
        public override DbConnection CreateConnection( string connectionString )
        {
            try
            {
                if( this.m_connection == null )
                {
                    m_connection = new OleDbConnection( connectionString );

                    Logger.Default.Write(
                        String.Format( "{0}\n\t{1}", "Create connection to database MS Access with connection string: ", connectionString ),
                        Verbosity.InformationDebug | Verbosity.Data );
                }

                // Non gestisco le eccezioni perchè voglio che siano rese visibili a livello utente da PluginManager
                if( this.m_connection.State != System.Data.ConnectionState.Open )
                {
                    m_connection.Open();
                    Logger.Default.Write( "Connection to the database succesfully opened." ,Verbosity.InformationDebug | Verbosity.Data );
                }

                return m_connection;
            }
            catch( DbException dbex )
            {
                Logger.Default.Write( dbex, "Database exception while connecting" );
            }

            return null;
        }

        /// <summary>
        /// Ottiene la connessione aperta
        /// </summary>
        /// <returns>
        /// La connessione al database, oppure <c>null</c> se la connessione non è stata creata con successo tramite <see cref="CreateConnection"/>
        /// </returns>
        public override DbConnection GetConnection()
        {
            return m_connection;
        }

        /// <summary>
        /// Costruisce la stringa di connessione adatta al tipo di database implementato
        /// </summary>
        /// <param name="host">Host in cui si trova il DBMS</param>
        /// <param name="user">Nome utente di accesso</param>
        /// <param name="password">La password di accesso</param>
        /// <param name="schema">Database iniziale</param>
        /// <returns>
        /// Restituisce la stringa di connessione per il DBMS o <c>null</c> in caso di problemi con i parametri
        /// </returns>
        public override string GetConnectionString( string host, string user, string password, string schema )
        {
            if( schema == "" )
            {
                return null;
            }
            return String.Format(
                @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False", schema
            );
        }

        /// <summary>
        /// Identifica il tipo di persistenza supportato dalla classe
        /// </summary>
        /// <returns>Un valore <see cref="PersistenceFactory.SupportedDatabase"/> che indica il tipo di persistenza supportato</returns>
        public override SupportedDatabase PersistenceType()
        {
            return SupportedDatabase.MSAccess;
        }
    }
}
// ReSharper restore InconsistentNaming
#endif
