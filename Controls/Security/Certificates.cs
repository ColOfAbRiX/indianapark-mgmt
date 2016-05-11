using System;
using System.Security.Cryptography.X509Certificates;

namespace IndianaPark.Tools.Security
{
    /// <summary>
    /// Classe per l'utilizzo dei certificati X509
    /// </summary>
    /// <remarks>
    /// Contiene i metodi per le operazioni più comuni per lavorare con i certificati, come il loro recupero, salvataggio
    /// installazione, ...
    /// </remarks>
    public static class Certificates
    {
        /// <summary>
        /// Recupera un certificato dallo storage locale
        /// </summary>
        /// <param name="storeName">Il nome dello storage riferito alla macchina locale</param>
        /// <param name="certName">Il nome del certificato</param>
        /// <returns>Un'istanza del certificato cercato, oppure <c>null</c> se il certificato non viene trovato</returns>
        public static X509Certificate2 ObtainCertificate( string storeName, string certName )
        {
            return ObtainCertificate( new X509Store( storeName, StoreLocation.LocalMachine ), certName );
        }

        /// <summary>
        /// Recupera un certificato dallo storage locale
        /// </summary>
        /// <param name="storeName">Il nome dello storage predefinito</param>
        /// <param name="storeLocation">La locazione dello storage</param>
        /// <param name="certName">Il nome del certificato</param>
        /// <returns>Un'istanza del certificato cercato, oppure <c>null</c> se il certificato non viene trovato</returns>
        public static X509Certificate2 ObtainCertificate( StoreName storeName, StoreLocation storeLocation, string certName )
        {
            return ObtainCertificate( new X509Store( storeName, storeLocation ), certName );
        }

        /// <summary>
        /// Recupera un certificato dallo storage locale
        /// </summary>
        /// <param name="store">Lo storage in cui cercare il certificato</param>
        /// <param name="certName">Il nome del certificato</param>
        /// <returns>Un'istanza del certificato cercato, oppure <c>null</c> se il certificato non viene trovato</returns>
        private static X509Certificate2 ObtainCertificate( X509Store store, string certName )
        {
            if( store == null )
            {
                return null;
            }

            try
            {
                // Apro lo storage per cercare il certificato
                store.Open( OpenFlags.ReadOnly );

                // Cerco tra tutti i c ertificati
                foreach( var cert in store.Certificates )
                {
                    if( String.Compare( cert.Subject, "CN=" + certName ) != 0 )
                    {
                        continue;
                    }

                    // Se trovo restituisco
                    store.Close();
                    return cert;
                }

                store.Close();
            }
            catch( Exception ex )
            {
                Logging.Logger.Default.Write( ex, "Exception while working with certificates store (retrieving)" );
            }

            // Se non trovo chiudo
            if( store != null )
            {
                store.Close();
            }

            return null;
        }

        /// <summary>
        /// Installa un certificato nello storage.
        /// </summary>
        /// <param name="storeName">Il nome dello storage predefinito</param>
        /// <param name="storeLocation">La locazione dello storage</param>
        /// <param name="cert">Il certificato da installare</param>
        /// <returns><c>true</c> se il certificato è stato installato con successo, <c>false</c> altrimenti.</returns>
        public static bool InstallCertificate( StoreName storeName, StoreLocation storeLocation, X509Certificate2 cert )
        {
            return InstallCertificate( new X509Store( storeName, storeLocation ), cert );
        }

        /// <summary>
        /// Installa un certificato nello storage della macchina locale.
        /// </summary>
        /// <param name="storeName">Il nome dello storage riferito alla macchina locale</param>
        /// <param name="cert">Il certificato da installare</param>
        /// <returns><c>true</c> se il certificato è stato installato con successo, <c>false</c> altrimenti.</returns>
        public static bool InstallCertificate( string storeName, X509Certificate2 cert )
        {
            return InstallCertificate( new X509Store( storeName, StoreLocation.LocalMachine ), cert );
        }

        /// <summary>
        /// Installa un certificato nello storage
        /// </summary>
        /// <param name="store">Lo storage in cui installare il certificato</param>
        /// <param name="cert">Il certificato da installare</param>
        /// <returns><c>true</c> se il certificato è stato installato con successo, <c>false</c> altrimenti.</returns>
        private static bool InstallCertificate( X509Store store, X509Certificate2 cert )
        {
            if( store == null )
            {
                return false;
            }

            try
            {
                store.Open( OpenFlags.ReadWrite );
                store.Add( cert );
                store.Close();

                return true;
            }
            catch( Exception ex )
            {
                Logging.Logger.Default.Write( ex, "Exception while working with certificates store (installing)" );
            }

            if( store != null )
            {
                store.Close();
            }

            return false;
        }
    }
}
