using System;
using System.Collections.Generic;
using NDesk.Options;
using System.Security.Cryptography.X509Certificates;
using IndianaPark.Licensing;

namespace IndianaPark.LicenseManager
{
    /// <summary>
    /// Gestisce tutti gli aspetti che riguardano i certificati
    /// </summary>
    public class SezioneCertificati : SectionPrototype
    {
        protected SezioneCertificati( SectionPrototype parent ) : base( parent )
        {
        }

        public static void Certificati( List<string> parametri, SectionPrototype parent )
        {
        }

        /*
        private static bool mc_isServer;
        private static bool mc_isClient;
        private static string mc_certFile;

        public static void Certificati( List<string> parametri )
        {
            // Imposto le opzioni per le sezioni
            Section currentSection = null;
            var p = new OptionSet()
            {
                { "install|I", v => currentSection = SezioneCertificati.Installa },
                { "create|C",  v => currentSection = SezioneCertificati.Genera },
                { "server|s",  v => { mc_isClient = false; mc_isServer = true; } },
                { "client|c",  v => { mc_isClient = true; mc_isServer = false; } },
                { "file=|f",   v => mc_certFile = v },
            };

            try
            {
                // Controllo gli argomenti
                List<string> extra = p.Parse( parametri );
                if( currentSection == null )
                {
                    // Se c'è qualcosa che non va, avviso l'utente di guardare le opzioni
                    throw new OptionException();
                }

                // Chiamo sezioni opportune
                currentSection( extra );
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

        public static void Installa( List<string> parametri )
        {
            Console.WriteLine( "Installa certificato" );
            Console.WriteLine( "IsServer {0}", mc_isServer.ToString() );
            Console.WriteLine( "IsClient {0}", mc_isClient.ToString() );
            Console.WriteLine( "Certificato {0}", mc_certFile );
        }

        public static void Genera( List<string> parametri )
        {
            if( String.IsNullOrEmpty( mc_certFile ) )
            {
                throw new OptionException( "Valore non impostato", "file" );
            }
            Console.WriteLine( "Generazione del certificato" );

            string filename = mc_certFile;
            //string certName = Tools.SystemCommands.ReadLine( "Nome del certificato: " );
            string certName = "Fabrizio Colonna";

            if( mc_isServer )
            {
                // Comando per la creazione di un certificato Server
                // makecert -pe -r -cy authority -h 1 -sky signature -n "CN=Fabrizio Colonna" -sr localmachine -ss "IndianaPark Test" server.cer
                Console.WriteLine( Tools.SystemCommands.ExecuteCommand( String.Format(
                    "\"C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\makecert\" -pe -r -cy authority -h 1 -sky signature -n \"CN={0}\" -sr localmachine -ss root {1}",
                    certName, filename
                ) ) );
            }
            else if( mc_isClient )
            {
                string storeName = "IndianaPark";

                // Comando per la creazione di un certificato Client
                // makecert -pe -exception 12/31/2010 -sky signature  -n "CN=Client" -sr localmachine -ss "IndianaPark Test" -ir localmachine -is "IndianaPark Test" -in "Fabrizio Colonna" client.cer
                Console.WriteLine(
                    Tools.SystemCommands.ExecuteCommand( String.Format(
                        "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\Bin\\makecert -pe -exception 12/31/2010 -sky signature  -n \"CN={0}\" -sr localmachine -ss \"{1}\" -ir localmachine -is \"IndianaPark Test\" -in \"Fabrizio Colonna\" {2}",
                        certName, storeName, filename
                    ) )
                );
            }
        }
        */
    }
}
