using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;

namespace IndianaPark.Tools.Security.Licensing
{
    /// <summary>
    /// Enumerazione delle features che un client può chiedere/che gli possono venire licenziate
    /// </summary>
    /// <remarks>
    /// I client sono gli utenti finali a cui viene distribuito il prodotto; le features sono i componenti del programma
    /// che possono essere distribuite separatamente.
    /// </remarks>
    [Flags]
    [Serializable]
    public enum SoftwareFeatures
    {
        /// <summary>
        /// Nessuna funzionalità
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Emissione generica di biglietti
        /// </summary>
        Biglietti = 0x01,
        /// <summary>
        /// Gestione dei percorsi
        /// </summary>
        Percorsi = 0x02,
        /// <summary>
        /// Gestione del PowerFan
        /// </summary>
        PowerFan = 0x04,
        /// <summary>
        /// Sistema di persistenza
        /// </summary>
        Persistence = 0x8
    }

    /// <summary>
    /// Funzionalità abilitate in una classe
    /// </summary>
    /// <remarks>
    /// Per le funzionalità che si possono abilitare vedere <see cref="SoftwareFeatures"/>
    /// </remarks>
    public class EnabledFeaturesAttribute : Attribute
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Recupera le funzionalità abilitate per la classe
        /// </summary>
        public SoftwareFeatures Features { get; private set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="EnabledFeaturesAttribute"/> class.
        /// </summary>
        /// <param name="features">The features.</param>
        public EnabledFeaturesAttribute( SoftwareFeatures features )
        {
            Features = features;
        }

		#endregion Constructors 

		#endregion Methods 
    }

    /// <summary>
    /// Licenza che viene concessa ai Client
    /// </summary>
    [Serializable]
    public class License
    {
		#region Nested Classes 

        /// <summary>
        /// Raccoglie informazioni per la licenza sugli assembly
        /// </summary>
        /// <remarks>
        /// Vengono utilizzare per la creazione e la verifica dei dati sulle licenze
        /// </remarks>
        [Serializable]
        public class AssemblyInfo
        {
		    #region Fields 

		    #region Public Fields 

            /// <summary>
            /// File in cui è contenuto l'assembly
            /// </summary>
            public string FileName;
            /// <summary>
            /// Nome completo dell'assembly
            /// </summary>
            public string FullAssemblyName;
            /// <summary>
            /// Checksum del file in cui è contenuto l'assembly
            /// </summary>
            public string Checksum;

		    #endregion Public Fields 

		    #endregion Fields 
        }

		#endregion Nested Classes 

		#region Fields 

		#region Internal Fields 

        private TimeSpan m_duration;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Indica se controllare i checksum degli assembly
        /// </summary>
        /// <value><c>true</c> controlla i checksum, <c>false</c> non controllare.</value>
        public bool CheckAssemblyChecksum { get; set; }

        /// <summary>
        /// Indica se controllare o meno il tempo di validità della licenza
        /// </summary>
        /// <value><c>true</c> controlla il tempo di validità, <c>false</c> non controllare.</value>
        public bool CheckDuration { get; set; }

        /// <summary>
        /// Indica se eseguire o meno il controllo delle features abilitate
        /// </summary>
        /// <value><c>true</c> controlla l'abilitazione delle features, <c>false</c> non controllare.</value>
        public bool CheckFeatures { get; set; }

        /// <summary>
        /// Indica se eseguire o meno il controllo del fingerprint
        /// </summary>
        /// <remarks>
        /// Il fingerprint è l'impronta del computer, con le sue componenti installate
        /// </remarks>
        /// <value><c>true</c> controlla il fingerprint, <c>false</c> non controllare.</value>
        public bool CheckFingerprint { get; set; }

        /// <summary>
        /// Attributo per la serializzazione XML
        /// </summary>
        /// <remarks>
        /// Tabella contenente i checksum dei file di programma abilitati dalla licenza
        /// </remarks>
        [XmlIgnore]
        public Dictionary<string, AssemblyInfo> ChecksumTable { get; set; }

        /// <summary>
        /// Nome del client
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Durata di validità della licenza
        /// </summary>
        /// <remarks>
        /// La durata di validità è intesa a partire dall data specificata in <see cref="EmissionDate"/>
        /// </remarks>
        [XmlIgnore]
        public TimeSpan Duration
        {
            get { return m_duration; }
            set { m_duration = value; }
        }

        /// <summary>
        /// Data di emissione della licenza
        /// </summary>
        public DateTime EmissionDate { get; set; }

        /// <summary>
        /// Features rese disponibili al Client
        /// </summary>
        public SoftwareFeatures EnabledFeatures { get; set; }

        /// <summary>
        /// Fingerprint a cui è concessa la licenza
        /// </summary>
        public string Fingerprint { get; set; }

        /// <summary>
        /// Attributo per la serializzazione XML
        /// </summary>
        [XmlArray]
        public AssemblyInfo[] XmlChecksumTable
        {
            get
            {
                var list = this.ChecksumTable == null ? null : this.ChecksumTable.Values.ToArray();
                return list;
            }
            set
            {
                if( value != null )
                {
                    if( this.ChecksumTable == null )
                    {
                        this.ChecksumTable = new Dictionary<string, AssemblyInfo>();
                    }
                    foreach( var info in value )
                    {
                        this.ChecksumTable.Add( info.FileName, new AssemblyInfo { Checksum = info.Checksum, FileName = info.FileName, FullAssemblyName = info.FullAssemblyName } );
                    }
                }
            }
        }

        /// <summary>
        /// Attributo per la serializzazione XML
        /// </summary>
        [XmlAttribute( "Duration", DataType = "long" )]
        [EditorBrowsable( EditorBrowsableState.Never )]
        public long XmlDuration
        {
            get { return m_duration.Ticks; }
            set { m_duration = new TimeSpan( value ); }
        }

		#endregion Public Fields 

		#endregion Fields 
    }

    /// <summary>
    /// Richiesta di licenza
    /// </summary>
    /// <remarks>
    /// Struttura dati utilizzata per richiedere l'emissione di una licenza per un software
    /// </remarks>
    [Serializable]
    public class LicenseRequest
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Fingerprint per cui si richiede la licenza
        /// </summary>
        /// <remarks>
        /// Il fingerprint è l'impronta del computer, con le sue componenti installate
        /// </remarks>
        public string Fingerprint { get; set; }

        /// <summary>
        /// Nome del Client
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Features richieste per il programma
        /// </summary>
        public SoftwareFeatures RequestedFeatures { get; set; }

		#endregion Public Fields 

		#endregion Fields 
    }
}
