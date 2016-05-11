using System;

namespace IndianaPark.Tools.Security.Licensing
{
    /// <summary>
    /// Enumerazione delle features che un client può chiedere/che gli possono venire licenziate
    /// </summary>
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
}