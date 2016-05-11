using System.Collections.Generic;

namespace IndianaPark.PowerFan.Model
{
    /// <summary>
    /// Rappresenta e gestisce la Torre
    /// </summary>
    public class Torre
    {
        private static Torre ms_Instance;
        private readonly List<Biglietto> m_biglietti = new List<Biglietto>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Torre"/> class.
        /// </summary>
        private Torre()
        {
        }

        /// <summary>
        /// Permette di accedere globalmente a questo oggetto
        /// </summary>
        /// <returns>L'istanza globale di Torre</returns>
        public static Torre GetTorre()
        {
            if( ms_Instance == null )
            {
                ms_Instance = new Torre();
            }

            return Torre.ms_Instance;
        }

        /// <summary>
        /// I biglietti emessi per la torre
        /// </summary>
        public List<Biglietto> BigliettiEmessi
        {
            get { return this.m_biglietti; }
        }
    }
}
