using System;
using System.Collections.Generic;
using System.IO;
using IndianaPark.Tools.Xml;
using NDesk.Options;

namespace IndianaPark.LicenseManager
{
    public delegate void Section( List<string> parametri, SectionPrototype parent );

    /// <summary>
    /// Programma di gestione delle licenze
    /// </summary>
    public class Program : SectionPrototype
    {
		#region Fields 

		#region Class-wise Fields 

        /// <summary>
        /// Il nome dell'eseguibile in esecuzione
        /// </summary>
        public static string ApplicationName = Path.GetFileName( System.Reflection.Assembly.GetExecutingAssembly().Location );

		#endregion Class-wise Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        public Program() : base( null )
        {
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Visualizza l'Help del programma
        /// </summary>
        /// <param name="parametri">I parametri aggiuntivi</param>
        private static void Help()
        {
            Console.Write( IndianaPark.LicenseManager.Properties.Resources.Syntax );
        }

        static void Main( string[] args )
        {
            // Imposto le opzioni per le sezioni
            Section currentSection = null;
            var p = new OptionSet()
            {
                { "cert",       v => currentSection += SezioneCertificati.Certificati },
                { "lic",        v => currentSection += LicenseSection.Gestisci },
                { "users",      v => {} },
                { "h|?|help",   v => { currentSection = null; Program.Help(); } },
            };

            try
            {
                // Controllo gli argomenti
                List<string> extra = p.Parse( args );
                if( currentSection == null )
                {
                    // Se c'è qualcosa che non va, avviso l'utente di guardare le opzioni
                    throw new OptionException();
                }

                // Chiamo sezioni opportune
                currentSection( extra, new Program() );
            }
            catch( OptionException )
            {
                // Utente, controlla un po' la sintassi...
                Console.WriteLine( "Try '{0} --help' for more information.", Program.ApplicationName );
                return;
            }

            return;
        }

		#endregion Class-wise Methods 

		#endregion Methods 
    }
}
