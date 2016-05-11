using System;

namespace IndianaPark.PowerFan.Model
{
    /// <summary>
    /// Rappresenta un biglietto
    /// </summary>
    public abstract class Biglietto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Biglietto"/> class.
        /// </summary>
        public Biglietto( decimal prezzo )
        {
            this.Emissione = DateTime.Now;
            this.Prezzo = prezzo;
        }

        /// <summary>
        /// Il nome del biglietto
        /// </summary>
        public string Nome
        {
            get { return this.ToString(); }
        }

        /// <summary>
        /// Prezzo del biglietto
        /// </summary>
        public decimal Prezzo { get; private set; }

        /// <summary>
        /// Data di emissione
        /// </summary>
        public DateTime Emissione { get; private set; }

        /// <summary>
        /// Numero di volte che può essere usato il biglietto
        /// </summary>
        public int NumeroUsi { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Arrampicata/PowerFan";
        }
    }
}
