using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace IndianaPark.Tools.Security
{
    /// <summary>
    /// Provider per gli algoritmi di crittografia che utilizza i certificati X509
    /// </summary>
    /// <remarks>
    /// <para>Estrae le chiavi di crittografia da due certificati <see cref="X509Certificate2"/></para>
    /// <para>Utilizzo del pattern Decorator sull'oggetto <see cref="RSASecurityProvider"/></para>
    /// </remarks>
    public class X509SecurityProvider : ISecurityProvider
    {
        #region Fields 

        #region Internal Fields 

        private readonly RSASecurityProvider m_rsaProvider;

        #endregion Internal Fields 

        #endregion Fields 

        #region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="X509SecurityProvider"/> class.
        /// </summary>
        /// <remarks>
        /// Per il caricamento viene sempre effettuato il controllo di validità dei certificati, sulla scadenza
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se non viene specificato uno dei parametri</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se uno dei due certificati è scaduto oppure se il certificato locale è sprovvisto di chiave privata
        /// </exception>
        /// <param name="localCert">Certificato X509 locale. Deve essere presente anche la chiave privata.</param>
        /// <param name="remoteCert">Certificato X509 remoto.</param>
        public X509SecurityProvider( X509Certificate2 localCert, X509Certificate2 remoteCert )
        {
            // Presenza dei parametri
            if( localCert == null )
            {
                throw new ArgumentNullException( "localCert", "You must specify a local certificate" );
            }
            if( remoteCert == null )
            {
                throw new ArgumentNullException( "remoteCert", "You must specify a remote certificate" );
            }
            // Controllo periodo di validità dei certificati
            if( localCert.NotBefore > DateTime.Now || localCert.NotAfter < DateTime.Now )
            {
                throw new ArgumentOutOfRangeException( "localCert", "The local certificate is not valid or expired" );
            }
            if( remoteCert.NotBefore > DateTime.Now || remoteCert.NotAfter < DateTime.Now )
            {
                throw new ArgumentOutOfRangeException( "remoteCert", "The remote certificate is not valid or expired" );
            }
            // Controllo che il certificato locale abbia la chiave privata
            if( !localCert.HasPrivateKey )
            {
                throw new ArgumentOutOfRangeException( "localCert", "The local certificate hasn't got a private key" );
            }
            // Il provider di crittografia deve essere RSA
            if( !(localCert.PrivateKey is RSA) )
            {
                throw new ArgumentOutOfRangeException( "localCert", "The local certificate is not RSA" );
            }

            using( var local = new RSACryptoServiceProvider() )
            {
                local.ImportParameters( ((RSA)localCert.PrivateKey).ExportParameters( true ) );

                using( var remote = new RSACryptoServiceProvider() )
                {
                    remote.ImportParameters( ((RSA)remoteCert.PublicKey.Key).ExportParameters( false ) );

                    // Utilizzo un RSASecurityProvider per fornire i dati
                    this.m_rsaProvider = new RSASecurityProvider( local, remote );
                }
            }
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera la chiave con cui si firmano i dati.
        /// </summary>
        /// <returns>La chiave utilizzata per firmare i dati</returns>
        /// <remarks>Deve essere presente anche la chiave privata</remarks>
        public RSA GetSigningKey()
        {
            return this.m_rsaProvider.GetSigningKey();
        }

        /// <summary>
        /// Recupera la chiave con cui si verificano le firme dei dati
        /// </summary>
        /// <returns>
        /// La chiave utilizzata per verificare le firme dei dati
        /// </returns>
        public RSA GetVerifyingKey()
        {
            return this.m_rsaProvider.GetVerifyingKey();
        }

        /// <summary>
        /// Recupera la chiave utilizzata per crittografare
        /// </summary>
        /// <returns>
        /// Un oggetto di tipo <see cref="RSA"/> utilizzato per crittografare i dati
        /// </returns>
        public RSA GetEncryptionKey()
        {
            return this.m_rsaProvider.GetEncryptionKey();
        }

        /// <summary>
        /// Recupera la chiave utilizzata per decrittografare
        /// </summary>
        /// <returns>
        /// Un oggetto di tipo <see cref="RSA"/> utilizzato per decrittografare i dati
        /// </returns>
        /// <remarks>Deve essere presente anche la chiave privata</remarks>
        public RSA GetDecryptionKey()
        {
            return this.m_rsaProvider.GetDecryptionKey();
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}