using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IndianaPark.Tools;
using IndianaPark.Tools.Security;
using IndianaPark.Tools.Security.Licensing;
using IndianaPark.Tools.Xml;
using IndianaPark.Tools.Logging;
using NDesk.Options;

namespace IndianaPark.LicenseManager.Licenze
{
    /// <summary>
    /// Oggetto utilizzato per creare licenze utente
    /// </summary>
    internal class LicenseCreator : SectionPrototype
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Opzioni per la creazione di licenze
        /// </summary>
        protected LicenseOptions Options = new LicenseOptions
        {
            CheckAssemblyChecksum = false,
            AssemblyChecksumPath = ".",
            CheckDuration = false,
            Duration = new TimeSpan( 365, 0, 0, 0 ),
            CheckFeatures = false,
            CheckFingerprint = false,
            LicenseFile = "license.lic",
            LicenseRequestFile = "request.lic"
        };

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        public LicenseCreator( SectionPrototype parent ) : base( parent )
        {
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Gestisce la creazione licenze smistanto i comportamenti e valorizzando le opzioni
        /// </summary>
        /// <param name="parametri">The parametri.</param>
        public static void Gestisci( List<string> parametri, SectionPrototype parent )
        {
            // TEST:
            // --lic --srvcert-file=server.pfx --clicert-file=client.pfx --reply --assembly-checksum --check-duration --check-features --check-fingerprint --checksum-path=. --duration=365 --license-file=license.lic --request-file=request.lic
            Console.WriteLine( "Creazione di una licenza" );

            var creator = new LicenseCreator( parent );
            
            // Imposto le opzioni per le sezioni
            var p = new OptionSet()
            {
                { "assembly-checksum",  v => creator.Options.CheckAssemblyChecksum = !string.IsNullOrEmpty( v ) },
                { "checksum-path=",  v => creator.Options.AssemblyChecksumPath = v ?? creator.Options.AssemblyChecksumPath },

                { "check-duration",  v => creator.Options.CheckDuration = !string.IsNullOrEmpty( v ) },
                { "duration=",  v => { int d = 0; if( int.TryParse( v, out d ) ) creator.Options.Duration = new TimeSpan( d, 0, 0, 0, 0); } },

                { "check-features",  v => creator.Options.CheckFeatures = !string.IsNullOrEmpty( v ) },
                { "check-fingerprint",  v => creator.Options.CheckFingerprint = !string.IsNullOrEmpty( v ) },

                { "license-file=",  v => creator.Options.LicenseFile = v ?? creator.Options.LicenseFile },
                { "request-file=",  v => creator.Options.LicenseRequestFile = v ?? creator.Options.LicenseRequestFile }
            };

            try
            {
                // Controllo gli argomenti
                p.Parse( parametri );
                creator.CreaLicenza();
            }
            catch( OptionException oe )
            {
                // Utente, controlla un po' la sintassi...
                Console.WriteLine( oe );
                Console.WriteLine( "Try '{0} --help' for more information.", Program.ApplicationName );
                return;
            }
        }

		#endregion Class-wise Methods 

		#region Internal Methods 

        /// <summary>
        /// Crea una tabella con i checksum di tutti gli assembly della directory indicata
        /// </summary>
        /// <param name="path">Il percorso in cui cercare gli assembly</param>
        /// <returns>La lista delle informazioni e dei checksum degli assembly</returns>
        private Dictionary<string, License.AssemblyInfo> ScanAssemblies( string path )
        {
            var abilitati = new Dictionary<string, License.AssemblyInfo>();

            foreach( var file in Directory.GetFiles( path ) )
            {
                if( Path.GetExtension( file ).ToLower() == ".dll" )
                {
                    try
                    {
                        var asm = Assembly.ReflectionOnlyLoadFrom( Path.GetFullPath( file ) );

                        abilitati.Add(
                            Path.GetFileName( file ),
                            new License.AssemblyInfo
                            {
                                FileName = Path.GetFileName( file ),
                                FullAssemblyName = asm.FullName,
                                Checksum = File.OpenRead( file ).Checksum( new System.Security.Cryptography.SHA1Managed() )
                            } );
                    }
                    catch
                    {
                        // Prossimo file...
                    }
                }
            }

            return abilitati;
        }

        /// <summary>
        /// Carica un file di richiesta
        /// </summary>
        /// <param name="fileName">Il nome del file</param>
        /// <returns>Un'istanza di <see cref="LicenseRequest"/> corrispondente al contenuto del file, oppure <c>null</c> in caso di problemi.</returns>
        private LicenseRequest LoadRequest( string fileName, X509SecurityProvider provider )
        {
            try
            {
                var doc = new System.Xml.XmlDocument();
                doc.Load( fileName );

                var sc = new SecureContainer<LicenseRequest>( doc, provider, true, true );
                sc.FreeSecuredData();
                return sc.Data;
            }
            catch( Exception ex )
            {
                new Logger( new IndianaPark.Tools.Logging.Writers.ConsoleLogger() ).Write( ex, "Error loading the license request" );
                return null;
            }
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Crea una licenza d'uso del software utilizzando i parametri specificati
        /// </summary>
        public void CreaLicenza()
        {
            // Creo il SecurityProvide relativo al server usando il certificato server dallo storage ed il certificato client da file
            var provider = new X509SecurityProvider
            (
                ((LicenseSection)this.Parent).ServerCertificate,
                ((LicenseSection)this.Parent).ClientCertificate
            );

            // Carico la richiesta
            var request = LoadRequest( this.Options.LicenseRequestFile, provider );
            
            if( request == null )
            {
                return;
            }

            // Creo la licenza
            var license = new License
            {
                Client = request.Name,
                EnabledFeatures = request.RequestedFeatures,
                Fingerprint = request.Fingerprint,
                ChecksumTable = ScanAssemblies( this.Options.AssemblyChecksumPath ),
                Duration = this.Options.Duration,
                EmissionDate = DateTime.Now,
                CheckAssemblyChecksum = this.Options.CheckAssemblyChecksum,
                CheckFeatures = this.Options.CheckFeatures,
                CheckFingerprint = this.Options.CheckFingerprint,
                CheckDuration = this.Options.CheckDuration
            };

            // Salvo la licenza su file
            var sc = new SecureContainer<License>( license, provider, true, true );
            sc.MakeDataSecure();
            sc.SecuredData.SaveFormatted( this.Options.LicenseFile );
        }

		#endregion Public Methods 

		#endregion Methods 

        #region Internal Classes 

        internal struct LicenseOptions
        {
            public string AssemblyChecksumPath { get; set; }
            public bool CheckAssemblyChecksum { get; set; }
            public bool CheckDuration { get; set; }
            public bool CheckFeatures { get; set; }
            public bool CheckFingerprint { get; set; }
            public TimeSpan Duration { get; set; }
            public string LicenseFile { get; set; }
            public string LicenseRequestFile { get; set; }
        }

        #endregion Internal Classes
    }
}
