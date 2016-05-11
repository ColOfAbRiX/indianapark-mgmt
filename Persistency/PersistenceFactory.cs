using System.Data.Common;

// ReSharper disable InconsistentNaming
namespace IndianaPark.Persistence
{
    /// <summary>
    /// Factory astratta per ottenere i costruttori di connessioni
    /// </summary>
    public abstract class PersistenceFactory
    {
        /// <summary>
        /// Tipi di persistenza supportati
        /// </summary>
        public enum SupportedDatabase
        {
            /// <summary>
            /// Database non presente
            /// </summary>
            NoDatabase,
#if MSACCESS
            /// <summary>
            /// Database MS Access
            /// </summary>
            MSAccess,
#endif
#if SQLSERVER
            /// <summary>
            /// Database SQL Server 2005
            /// </summary>
            SQLServer200x,
#endif
#if SQLSERVERCE
            /// <summary>
            /// Database SQL Server Compact Edition 4.0
            /// </summary>
            SQLServerCE4
#endif
        }

        /// <summary>
        /// Crea le factory concrete per l'accesso ai dati
        /// </summary>
        /// <param name="whichDao">Il tipo di persistenza desiderato</param>
        /// <returns></returns>
        public static PersistenceFactory GetFactory( SupportedDatabase whichDao )
        {
            // Istanzio il tipo corretto di oggetto Factory
            switch( whichDao )
            {
#if MS_ACCESS
                case SupportedDatabase.MSAccess:
                    return new MSAccessDatabase();
#endif
#if SQLSERVER
                case SupportedDatabase.SQLServer200x:
                    return new SQLServer200xDatabase();
#endif
#if SQLSERVERCE
                case SupportedDatabase.SQLServerCE4:
                    return new SQLServerCE4Database();
#endif
                case SupportedDatabase.NoDatabase:
                    return null;
            }

            return null;
        }

        /// <summary>
        /// Crea ed apre la connessione indicata dalla connection string
        /// </summary>
        /// <param name="connectionString">La stringa di connessione al database</param>
        /// <returns>La connessione al database, oppure <c>null</c> se la creazione o l'apertura non sono andate a buon fine</returns>
        public abstract DbConnection CreateConnection( string connectionString );

        /// <summary>
        /// Ottiene la connessione aperta
        /// </summary>
        /// <returns>La connessione al database, oppure <c>null</c> se la connessione non è stata creata con successo tramite <see cref="CreateConnection"/></returns>
        public abstract DbConnection GetConnection();

        /// <summary>
        /// Costruisce la stringa di connessione adatta al tipo di database implementato
        /// </summary>
        /// <param name="host">Host in cui si trova il DBMS</param>
        /// <param name="user">Nome utente di accesso</param>
        /// <param name="password">La password di accesso</param>
        /// <param name="schema">Database iniziale</param>
        /// <returns>Restituisce la stringa di connessione per il DBMS o <c>null</c> in caso di problemi con i parametri</returns>
        public abstract string GetConnectionString( string host, string user, string password, string schema );

        /// <summary>
        /// Identifica il tipo di persistenza supportato dalla classe
        /// </summary>
        /// <returns>Un valore <see cref="SupportedDatabase"/> che indica il tipo di persistenza supportato</returns>
        public abstract SupportedDatabase PersistenceType();
    }
}
// ReSharper restore InconsistentNaming
