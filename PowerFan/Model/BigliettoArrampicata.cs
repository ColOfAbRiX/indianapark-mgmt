namespace IndianaPark.PowerFan.Model
{
    /// <summary>
    /// Un biglietto per l'arrampicata
    /// </summary>
    public class BigliettoArrampicata : Biglietto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BigliettoArrampicata"/> class.
        /// </summary>
        /// <param name="prezzo">Il prezzo del biglietto</param>
        public BigliettoArrampicata( decimal prezzo ) : base( prezzo )
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Arrampicata";
        }
    }
}
