using System;

namespace IndianaPark.Tools.Security.Licensing
{
    /// <summary>
    /// Richiesta di licenza
    /// </summary>
    [Serializable]
    public class LicenseRequest
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Fingerprint per cui si richiede la licenza
        /// </summary>
        public string Fingerprint { get; set; }

        /// <summary>
        /// Nome del Client
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Features richiede per il programma
        /// </summary>
        public SoftwareFeatures RequestedFeatures { get; set; }

		#endregion Public Fields 

		#endregion Fields 
    }
}