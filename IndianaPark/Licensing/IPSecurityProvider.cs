using IndianaPark.Tools.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using IndianaPark.Tools.Security;

namespace IndianaPark.Licensing
{
    /// <summary>
    /// Security Provider per il programma gestionale IndianaPark
    /// </summary>
    public sealed class IpSecurityProvider : ISecurityProvider
    {
		#region Fields 

        #region Internal Fields 

        private X509Certificate2 m_ClientCert;
        private X509Certificate2 m_ServerCert;
        private X509SecurityProvider m_provider;

        #endregion Interna Fields

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="IpSecurityProvider"/> class.
        /// </summary>
        public IpSecurityProvider()
        {
            // Carico il certificato del client, prima cerco nello storage poi nel file
            this.m_ClientCert = Certificates.ObtainCertificate( "IndianaPark", "Client" );
            if( this.m_ClientCert == null )
            {
                this.m_ClientCert = new X509Certificate2( "client.pfx", "aaa", X509KeyStorageFlags.Exportable );
            }

            // Carico il certificato del server, prima cerco nello storage poi nel file
            this.m_ServerCert = Certificates.ObtainCertificate( StoreName.Root, StoreLocation.LocalMachine, "Fabrizio Colonna" );
            if( this.m_ServerCert == null )
            {
                this.m_ServerCert = new X509Certificate2( "server.cer" );
            }

            // Creo il provider con i certificati caricati
            this.m_provider = new X509SecurityProvider( this.m_ClientCert, this.m_ServerCert );
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera la chiave con cui si firmano i dati.
        /// </summary>
        /// <remarks>Deve essere presente anche la chiave privata</remarks>
        /// <returns>La chiave utilizzata per firmare i dati</returns>
        public RSA GetSigningKey()
        {
            return this.m_provider.GetSigningKey();
        }

        /// <summary>
        /// Recupera la chiave con cui si verificano le firme dei dati
        /// </summary>
        /// <returns>La chiave utilizzata per verificare le firme dei dati</returns>
        public RSA GetVerifyingKey()
        {
            return this.m_provider.GetVerifyingKey();
        }

        /// <summary>
        /// Recupera la chiave utilizzata per crittografare
        /// </summary>
        /// <returns>Un oggetto di tipo <see cref="RSA"/> utilizzato per crittografare i dati</returns>
        public RSA GetEncryptionKey()
        {
            return this.m_provider.GetEncryptionKey();
        }

        /// <summary>
        /// Recupera la chiave utilizzata per decrittografare
        /// </summary>
        /// <remarks>Deve essere presente anche la chiave privata</remarks>
        /// <returns>Un oggetto di tipo <see cref="RSA"/> utilizzato per decrittografare i dati</returns>
        public RSA GetDecryptionKey()
        {
            return this.m_provider.GetDecryptionKey();
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
