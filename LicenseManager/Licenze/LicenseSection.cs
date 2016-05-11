using System;
using System.Collections.Generic;
using NDesk.Options;
using IndianaPark.Tools.Security;
using System.Security.Cryptography.X509Certificates;

namespace IndianaPark.LicenseManager
{
    /// <summary>
    /// Sezione per l'elaborazione di tutte le richieste riguardanti le licenze
    /// </summary>
    public class LicenseSection : SectionPrototype
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Certificato del client
        /// </summary>
        /// <value>Un oggetto <see cref="X509Certificate2"/> relativo al certificato del client</value>
        public X509Certificate2 ClientCertificate { get; private set; }

        /// <summary>
        /// Certificato del server
        /// </summary>
        /// <value>Un oggetto <see cref="X509Certificate2"/> relativo al certificato del server</value>
        public X509Certificate2 ServerCertificate { get; private set; }

		#endregion Public Fields 

        #region Internal Fields 

        private string m_servCertFile;
        private string m_cliCertFile;
        private string m_servCertName;
        private string m_cliCertName;

        #endregion Internal Fields 
        
        #endregion Fields 

		#region Methods 

		#region Constructors 

        protected LicenseSection( SectionPrototype parent ) : base( parent )
        {
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Smista i comportamenti per la gestione delle licenze
        /// </summary>
        /// <param name="parametri">The parametri.</param>
        public static void Gestisci( List<string> parametri, SectionPrototype parent )
        {
            Console.WriteLine( "Elaborazione delle licenze" );

            var current = new LicenseSection( parent );

            // Imposto le opzioni per le sezioni
            Section handler = null;
            var p = new OptionSet()
            {
                { "srvcert-file=",  v => current.m_servCertFile = v },
                { "clicert-file=",  v => current.m_cliCertFile = v },
                { "srvcert-name=",  v => current.m_servCertName = v },
                { "clicert-name=",  v => current.m_cliCertName = v },
                { "reply|R",  v => handler = Licenze.LicenseCreator.Gestisci },
                { "request|r",  v => handler = Licenze.RequestCreator.Gestisci },
            };

            try
            {
                // Controllo gli argomenti
                List<string> extra = p.Parse( parametri );
                if( handler == null )
                {
                    // Se c'è qualcosa che non va, avviso l'utente di guardare le opzioni
                    throw new OptionException();
                }

                if( string.IsNullOrEmpty( current.m_cliCertFile ) && string.IsNullOrEmpty( current.m_cliCertName ) )
                {
                    throw new OptionException( "You must specify either --clicert-file or --clicert-name", "clicert-name,clicert-file" );
                }

                if( string.IsNullOrEmpty( current.m_servCertFile ) && string.IsNullOrEmpty( current.m_servCertName ) )
                {
                    throw new OptionException( "You must specify either --srvcert-file or --srvcert-name", "srvcert-name,srvcert-file" );
                }

                // Carico i certificati
                current.LoadCertificates();

                // Chiamo sezioni opportune
                handler( extra, current );
            }
            catch( OptionException oe )
            {
                // Utente, controlla un po' la sintassi...
                Console.WriteLine( oe );
                Console.WriteLine( "Try '{0} --help' for more information.", Program.ApplicationName );
                return;
            }

            return;
        }

		#endregion Class-wise Methods 

        #region Public Methods 

        /// <summary>
        /// Carica i certificati
        /// </summary>
        /// <remarks>
        /// Il programma tenta di caricare le licenze prima da file e poi dallo storage. Il certificato client viene
        /// poi cercato nello storage predefinito, il certificato server nello storage Root
        /// </remarks>
        protected void LoadCertificates()
        {
            // CERTIFICATO DEL CLIENT
            if( !string.IsNullOrEmpty( this.m_cliCertFile ) )
            {
                // Tento il caricamento del certificato da file
                this.ClientCertificate = new X509Certificate2( this.m_cliCertFile, Properties.Resources.DefaultPfxPassword, X509KeyStorageFlags.Exportable );
            }
            else if( !string.IsNullOrEmpty( this.m_cliCertName ) )
            {
                // Tento il caricamento del certificato dallo storage
                this.ClientCertificate = Certificates.ObtainCertificate( Properties.Resources.ClientKeyStorage, this.m_cliCertName );
            }

            // CERTIFICATO DEL SERVER
            if( !string.IsNullOrEmpty( this.m_servCertFile ) )
            {
                // Tento il caricamento del certificato da file
                this.ServerCertificate = new X509Certificate2( this.m_servCertFile, Properties.Resources.DefaultPfxPassword, X509KeyStorageFlags.Exportable );
            }
            else if( !string.IsNullOrEmpty( this.m_servCertName ) )
            {
                // Tento il caricamento del certificato dallo storage
                this.ServerCertificate = Certificates.ObtainCertificate( StoreName.Root, StoreLocation.LocalMachine, this.m_servCertName );
            }
        }

        #endregion Public Methods

        #endregion Methods
    }
}
