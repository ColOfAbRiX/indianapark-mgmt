using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using IndianaPark.Tools.Security.Licensing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using IndianaPark.Tools;
using IndianaPark.Tools.Security;
using License = IndianaPark.Tools.Security.Licensing.License;
using IndianaPark.Tools.Logging;

namespace IndianaPark.Licensing
{
    /// <summary>
    /// Controlla la validità della licenza fornita all'utente e rilascia le licenze per i vari componenti
    /// del progetto
    /// </summary>
    public class IpLicenseProvider : LicenseProvider
    {
		#region Fields 

        #region Internal Fields 

        /// <summary>
        /// File in cui è contenuta la licenza del programma
        /// </summary>
        public const string LicenseFile = "license.lic";

        #endregion Internal Fields 

        #region Class-wise Fields

        private static SecureContainer<License> ms_License;
        private static readonly string ms_Fingerprint = new Fingerprint().GetFingerprint();     // Carico il fingerprint solo una volta per tutto il programma

		#endregion Class-wise Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="IpLicenseProvider"/> class.
        /// </summary>
        public IpLicenseProvider()
        {
            if( IpLicenseProvider.ms_License != null )
            {
                return;
            }

            if( !File.Exists( LicenseFile ) )
            {
                return;
            }

            // Carico la licenza dal file
            var license = new System.Xml.XmlDocument();
            license.Load( new System.Xml.XmlTextReader( LicenseFile ) );

            // Metto la licenza nel contenitore sicuro
            ms_License = new SecureContainer<License>( license, new IpSecurityProvider(), true, true );
        }

        #endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Controlla che il cecksum del file contenente l'assembly del tipo specificato sia corretto
        /// </summary>
        /// <param name="tipo">Il tipo dell'oggetto da controllare</param>
        /// <param name="info">La collezione di informazioni su tutti gli assembly</param>
        /// <returns><c>true</c> se l'assembly è valido, <c>false</c> se non lo è.</returns>
        private bool AssemblyChecksum( Type tipo, Dictionary<string, License.AssemblyInfo> info )
        {
            var asmTipo = Path.GetFileName( tipo.Assembly.CodeBase );
            var asmInfo = (from i in info.Keys
                           where String.Compare( i.ToLower(), asmTipo.ToLower(), true ) == 0
                           select info[i]).FirstOrDefault();

            if( asmInfo == null || String.Compare( asmInfo.FullAssemblyName, tipo.Assembly.FullName ) != 0 )
            {
                return false;
            }

            var chechsum = File.OpenRead( Path.GetFullPath( asmTipo ) ).Checksum( new SHA1Managed() );
            if( asmInfo.Checksum != chechsum )
            {
                return false;
            }

            return true;
        }

        #endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// When overridden in a derived class, gets a license for an instance or type of component, when given a context and whether the denial of a license throws an exception.
        /// </summary>
        /// <param name="context">A <see cref="T:System.ComponentModel.LicenseContext"/> that specifies where you can use the licensed object.</param>
        /// <param name="type">A <see cref="T:System.Type"/> that represents the component requesting the license.</param>
        /// <param name="instance">An object that is requesting the license.</param>
        /// <param name="allowExceptions">true if a <see cref="T:System.ComponentModel.LicenseException"/> should be thrown when the component cannot be granted a license; otherwise, false.</param>
        /// <returns>
        /// A valid <see cref="T:System.ComponentModel.License"/>.
        /// </returns>
        public override System.ComponentModel.License GetLicense( LicenseContext context, Type type, object instance, bool allowExceptions )
        {
            Logger.Default.Write( String.Format( "Requested license for component {0}", type.Name ), Verbosity.InformationDebug | Verbosity.Data );

            // Controllo che il file di licenza sia presente
            if( ms_License == null )
            {
                Logger.Default.Write( "Cannot verify the license. I was not able to load the license. Check previous log." );
                return null;
            }

            // Decrittografo la licenza
            if( !ms_License.FreeSecuredData() )
            {
                Logger.Default.Write( "Cannot verify the license. I'm unable to decrypt the license." );
                ms_License.Data = null;
                return null;
            }

            // Controllo che la licenza sia firmata correttamente
            if( !ms_License.IsVerified )
            {
                Logger.Default.Write( "Cannot verify the license. The license is not correctly signed!" );
                ms_License.Data = null;
                return null;
            }

            var licenza = ms_License.Data;

            // Controllo che la licenza non sia scaduta
            if( licenza.CheckDuration && licenza.EmissionDate + licenza.Duration < DateTime.Now )
            {
                Logger.Default.Write( "Cannot verify the license. The license is no more valid." );
                ms_License.Data = null;
                return null;
            }

            // Controllo che la licenza sia valida per questo componente
            var feature = SoftwareFeatures.None;
            var attributes = type.GetCustomAttributes( typeof( EnabledFeaturesAttribute ), true );
            if( attributes.Length > 0 )
            {
                feature = ((EnabledFeaturesAttribute)attributes[0]).Features;
            }
            if( licenza.CheckFeatures && ( licenza.EnabledFeatures & feature ) != feature )
            {
                Logger.Default.Write( "Cannot verify the license. The component requested is not enabled in your license." );
                ms_License.Data = null;
                return null;
            }

            // Controllo che l'assembly del componente abbia il checksum corretto
            if( !this.AssemblyChecksum( type, licenza.ChecksumTable ) )
            {
                Logger.Default.Write( "Cannot verify the license. The license was not emitted for the current assembly." );
                ms_License.Data = null;
                return null;
            }

            // Controllo che il fingerprint corrisponda al computer corrente
            if( licenza.CheckFingerprint && (licenza.Fingerprint != ms_Fingerprint) )
            {
                Logger.Default.Write( "Cannot verify the license. The license was not emitted for this computer." );
                ms_License.Data = null;
                return null;
            }

            Logger.Default.Write( "Valid license found for the component." );
            
            var codice = ms_License.Data.Client;
            ms_License.Data = null;
            return new RunningLicense( codice );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
