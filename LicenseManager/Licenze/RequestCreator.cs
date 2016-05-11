using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using IndianaPark.Tools;
using IndianaPark.Tools.Security;
using IndianaPark.Tools.Security.Licensing;
using IndianaPark.Tools.Xml;
using NDesk.Options;
using IndianaPark.LicenseManager;

namespace IndianaPark.LicenseManager.Licenze
{
    class RequestCreator : SectionPrototype
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Opzioni per la creazione di licenze
        /// </summary>
        protected LicenseRequestOptions Options = new LicenseRequestOptions
        {
            ClientName = "",
            LicenseRequestFile = "request.lic",
            RequestedFeatures = SoftwareFeatures.Biglietti | SoftwareFeatures.Percorsi | SoftwareFeatures.Persistence | SoftwareFeatures.PowerFan
        };

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        protected RequestCreator( SectionPrototype parent ) : base( parent )
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
            // --lic --srvcert-file=server.pfx --clicert-file=client.pfx --request --name=Utente --features=Biglietti,Percorsi,PowerFan,Persistence
            Console.WriteLine( "Creazione di una richiesta di licenza" );

            var creator = new RequestCreator( parent );

            // Imposto le opzioni per le sezioni
            var p = new OptionSet()
            {
                { "name=",  v => creator.Options.ClientName = v ?? creator.Options.ClientName },
                { "features=", v => { SoftwareFeatures features; SoftwareFeatures.TryParse( v, out features ); creator.Options.RequestedFeatures = features; } },
                { "request-file=",  v => creator.Options.LicenseRequestFile = v ?? creator.Options.LicenseRequestFile }
            };

            try
            {
                // Controllo gli argomenti
                p.Parse( parametri );
                creator.CreaRichiesta();
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

		#region Public Methods 

        /// <summary>
        /// Crea una licenza d'uso del software utilizzando i parametri specificati
        /// </summary>
        public void CreaRichiesta()
        {
            // Creo il SecurityProvide relativo al client usando il certificato client dallo storage ed il certificato server da file
            var provider = new X509SecurityProvider
            (
                ((LicenseSection)this.Parent).ClientCertificate,
                ((LicenseSection)this.Parent).ServerCertificate
            );

            var request = new LicenseRequest
            {
                Name = this.Options.ClientName,
                RequestedFeatures = this.Options.RequestedFeatures,
                Fingerprint = new Fingerprint().GetFingerprint()
            };

            // Salvo la richiesta su file
            var sc = new SecureContainer<LicenseRequest>( request, provider, true, true );
            sc.MakeDataSecure();
            sc.SecuredData.SaveFormatted( this.Options.LicenseRequestFile );
        }

		#endregion Public Methods 

		#endregion Methods 

        #region Internal Classes 

        public struct LicenseRequestOptions
        {
            public string ClientName { get; set; }
            public string LicenseRequestFile { get; set; }
            public SoftwareFeatures RequestedFeatures { get; set; }
        }

        #endregion Internal Classes
    }
}
