using System;
using System.Security.Cryptography;

namespace IndianaPark.Tools.Security
{
    /// <summary>
    /// Provider di chiavi per gli algoritmi di crittografia che utilizza <see cref="RSACryptoServiceProvider"/>
    /// </summary>
    public class RSASecurityProvider : ISecurityProvider
    {
        #region Fields 

        #region Internal Fields 

        private readonly RSACryptoServiceProvider m_localCsp;
        private readonly RSACryptoServiceProvider m_remoteCsp;

        #endregion Internal Fields 

        #endregion Fields 

        #region Methods 

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="RSASecurityProvider"/> class.
        /// </summary>
        /// <param name="localCsp">Chiave RSA locale. Deve essere presente anche la chiave privata.</param>
        /// <param name="remoteCsp">Chiave RSA remota.</param>
        /// <exception cref="ArgumentNullException">Se viene specificato un provider locale o remoto<c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Se il provider locale è sprovvisto di chiave privata</exception>
        public RSASecurityProvider( RSACryptoServiceProvider localCsp, RSACryptoServiceProvider remoteCsp )
        {
            if( localCsp == null )
            {
                throw new ArgumentNullException( "localCsp", "You must specify the local RSA encryption provider" );
            }
            if( remoteCsp == null )
            {
                throw new ArgumentNullException( "remoteCsp", "You must specify the remote RSA encryption provider" );
            }
            if( !localCsp.PossesPrivateKey() )
            {
                throw new ArgumentOutOfRangeException( "localCsp", "The local RSA provider must have a private key" );
            }

            this.m_localCsp = localCsp;
            this.m_remoteCsp = remoteCsp;
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
            return this.m_localCsp;
        }

        /// <summary>
        /// Recupera la chiave con cui si verificano le firme dei dati
        /// </summary>
        /// <returns>La chiave utilizzata per verificare le firme dei dati</returns>
        public RSA GetVerifyingKey()
        {
            return this.m_remoteCsp;
        }

        /// <summary>
        /// Recupera la chiave utilizzata per crittografare
        /// </summary>
        /// <returns>Un oggetto di tipo <see cref="RSA"/> utilizzato per crittografare i dati</returns>
        public RSA GetEncryptionKey()
        {
            return this.m_remoteCsp;
        }

        /// <summary>
        /// Recupera la chiave utilizzata per decrittografare
        /// </summary>
        /// <remarks>Deve essere presente anche la chiave privata</remarks>
        /// <returns>Un oggetto di tipo <see cref="RSA"/> utilizzato per decrittografare i dati</returns>
        public RSA GetDecryptionKey()
        {
            return this.m_localCsp;
        }

        #endregion Public Methods 

        #endregion Methods 
    }
}