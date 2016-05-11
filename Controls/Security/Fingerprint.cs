/****************************************************************************
 *
 *  Fingerprint.cs
 *  --------------
 *  Reference:
 *    http://stackoverflow.com/questions/99880/generating-a-unique-machine-id
 *    
 *  Note that not all PC's have all items. exception.g. may not have network 
 *  card. Will not hang in this case as returns default values. Older 
 *  PC's may not have things like unique CPU or motherboard serial 
 *  numbers, so don't rely on this.
 *  
 *  Advantages: more secure than universally reuseable activation 
 *  code, which will soon be on the serialz sites. Cheaper than a 
 *  dongle and nothing to ship.
 *  
 *  Disadvantage: If legit user changes hardware then needs a new 
 *  code. May disuade/irritate some purchasers. (Best used with a 
 *  website where users can generate their own reactivation codes 
 *  to be sent only to their registered email address - and kill that 
 *  address if it generates an unreasonable number requests on the 
 *  same serial number. If app is guaranteed Internet access - exception.g. 
 *  it's a browser or an FTP tool - then this process can be invisible to 
 *  the customer - just do it in background.)
 *****************************************************************************/
#pragma warning disable 1591

using System;
using System.Security.Cryptography;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IndianaPark.Tools.Security
{
    /// <summary>
    /// Utilizzata per ottenere un identificativo univoco del computer su cui il programma è in esecuzione
    /// </summary>
    public class Fingerprint
    {
		#region Enumerations 

        /// <summary>
        /// Enumerazione dei possibili elementi informativi da includere nel fingerprint
        /// </summary>
        [Flags]
        public enum FingerprintElements
        {
            /// <summary>
            /// Identificativo della CPU
            /// </summary>
            CPU = 0x01,
            /// <summary>
            /// Identificativo della scheda madre
            /// </summary>
            Motherboard = 0x02,
            /// <summary>
            /// Identificativo del BIOS
            /// </summary>
            BIOS = 0x04,
            /// <summary>
            /// Identificativo dei dischi rigidi
            /// </summary>
            Disk = 0x08,
            /// <summary>
            /// Identificativo della scheda video
            /// </summary>
            Video = 0x10,
            /// <summary>
            /// Identificativo della scheda di rete
            /// </summary>
            Network = 0x20,
        }

		#endregion Enumerations 

		#region Fields 

		#region Internal Fields 

        private const int SLEEP_TIME = 5;
        private string m_cpuid;
        private string m_baseid;
        private string m_biosid;
        private string m_diskid;
        private string m_videoid;
        private string m_networkid;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="Fingerprint"/> class.
        /// </summary>
        public Fingerprint()
        {
            // Carico tutti i valori in multithreading, in modo da tentare di velocizzare il processo
            Task.Factory.StartNew( () => new Thread( () => this.m_baseid = this.GetBaseId() ).Start() );
            Task.Factory.StartNew( () => new Thread( () => this.m_biosid = this.GetBiosId() ).Start() );
            Task.Factory.StartNew( () => new Thread( () => this.m_cpuid = this.GetCpuId() ).Start() );
            Task.Factory.StartNew( () => new Thread( () => this.m_diskid = this.GetDiskId() ).Start() );
            Task.Factory.StartNew( () => new Thread( () => this.m_networkid = this.GetMacId() ).Start() );
            Task.Factory.StartNew( () => new Thread( () => this.m_videoid = this.GetVideoId() ).Start() );
        }

		#endregion Constructors 

		#region Internal Methods 

        //Return a hardware identifier
        private string GetIdentifier( string wmiClass, string wmiProperty )
        {
            return this.GetIdentifier( wmiClass, wmiProperty, null );
        }

        //CPU Identifier
        protected string GetCpuId()
        {
            // Uses first CPU identifier available in order of preference
            // Don't get all identifiers, as very time consuming
            string retVal = GetIdentifier( "Win32_Processor", "UniqueId" );

            if( String.Compare( retVal, "" ) == 0 ) //If no UniqueID, use ProcessorID
            {
                retVal = GetIdentifier( "Win32_Processor", "ProcessorId" );

                if( String.Compare( retVal, "" ) == 0 ) //If no ProcessorId, use Name
                {
                    retVal = GetIdentifier( "Win32_Processor", "Name" );

                    if( String.Compare( retVal, "" ) == 0 ) //If no Name, use Manufacturer
                    {
                        retVal = GetIdentifier( "Win32_Processor", "Manufacturer" );
                    }

                    //Add clock speed for extra security
                    retVal += " " + GetIdentifier( "Win32_Processor", "MaxClockSpeed" );
                }
            }

            return String.Format( "CPU: {0}", retVal );
        }

        //BIOS Identifier
        protected string GetBiosId() 
        {
            return String.Format( "Manufacturer: {0}; SMBIOS: {1}; Identification: {2}; Serial: {3}",
                   GetIdentifier( "Win32_BIOS", "Manufacturer" ),
                   GetIdentifier( "Win32_BIOS", "SMBIOSBIOSVersion" ),
                   GetIdentifier( "Win32_BIOS", "IdentificationCode" ),
                   GetIdentifier( "Win32_BIOS", "SerialNumber" ) );
        }

        //Main physical hard drive ID
        protected string GetDiskId() 
        {
            return String.Format( "Manufacturer: {0}; Model: {1}; Signature: {2}; Serial: {3}",
                   GetIdentifier( "Win32_DiskDrive", "Manufacturer" ),
                   GetIdentifier( "Win32_DiskDrive", "Model" ),
                   GetIdentifier( "Win32_DiskDrive", "Signature" ),
                   GetIdentifier( "Win32_DiskDrive", "SerialNumber" ) );
        }

        //Motherboard ID
        protected string GetBaseId() 
        {
            return String.Format( "Manufacturer: {0}; Model: {1}; Name: {2}, Serial: {3}",
                   GetIdentifier( "Win32_BaseBoard", "Manufacturer" ),
                   GetIdentifier( "Win32_BaseBoard", "Model" ),
                   GetIdentifier( "Win32_BaseBoard", "Name" ),
                   GetIdentifier( "Win32_BaseBoard", "SerialNumber" ) );
        }

        //Primary video controller ID
        protected string GetVideoId() 
        {
            return String.Format( "Name: {0}",
                GetIdentifier( "Win32_VideoController", "Name" ) );
        }

        //First enabled network card ID
        protected string GetMacId() 
        {
            return String.Format( "MAC Address: {0}",
                   GetIdentifier( "Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled" ) );
        }

        //Return a hardware identifier
        protected string GetIdentifier( string wmiClass, string wmiProperty, string wmiMustBeTrue )
        {
            string result = "";

            using( var mc = new ManagementClass( wmiClass ) )
            {
                var moc = mc.GetInstances();

                foreach( ManagementObject mo in moc )
                {
                    if( wmiMustBeTrue == null || bool.Parse( mo[wmiMustBeTrue].ToString() ) )
                    {
                        //Only get the first one
                        if( String.Compare( result, "" ) == 0 )
                        {
                            try
                            {
                                result = mo[wmiProperty].ToString();
                                break;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            return result;
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Restituisce un identificativo univoco del computer
        /// </summary>
        /// <returns>Una stringa contenente l'identificativo univoco del computer</returns>
        public string GetFingerprint()
        {
            return this.GetFingerprint(
                FingerprintElements.BIOS |
                FingerprintElements.CPU |
                FingerprintElements.Disk |
                FingerprintElements.Motherboard |
                FingerprintElements.Network |
                FingerprintElements.Video );
        }

        /// <summary>
        /// Restituisce un identificativo univoco del computer
        /// </summary>
        /// <param name="which">Quali elementi informativi includere nel fingerprint</param>
        /// <returns>Una stringa contenente l'identificativo univoco del computer</returns>
        public string GetFingerprint( FingerprintElements which )
        {
            string id = "";

            if( (which & FingerprintElements.CPU) == FingerprintElements.CPU )
            {
                while( this.m_cpuid == null )
                    Thread.Sleep( SLEEP_TIME );
                id += m_cpuid + "\n";
            }

            if( (which & FingerprintElements.Motherboard) == FingerprintElements.Motherboard )
            {
                while( this.m_baseid == null )
                    Thread.Sleep( SLEEP_TIME );
                id += this.m_baseid + "\n";
            }

            if( (which & FingerprintElements.BIOS) == FingerprintElements.BIOS )
            {
                while( this.m_biosid == null )
                    Thread.Sleep( SLEEP_TIME );
                id += this.m_biosid + "\n";
            }

            if( (which & FingerprintElements.Disk) == FingerprintElements.Disk )
            {
                while( this.m_diskid == null )
                    Thread.Sleep( SLEEP_TIME );
                id += this.m_diskid + "\n";
            }

            if( (which & FingerprintElements.Video) == FingerprintElements.Video )
            {
                while( this.m_videoid == null )
                    Thread.Sleep( SLEEP_TIME );
                id += this.m_videoid + "\n";
            }

            if( (which & FingerprintElements.Network) == FingerprintElements.Network )
            {
                while( this.m_networkid == null )
                    Thread.Sleep( SLEEP_TIME );
                id += this.m_networkid + "\n";
            }

            using( var sha512 = new SHA1CryptoServiceProvider() )
            {
                var hash = sha512.ComputeHash( Encoding.UTF8.GetBytes( id ) );
                return Convert.ToBase64String( hash );
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}