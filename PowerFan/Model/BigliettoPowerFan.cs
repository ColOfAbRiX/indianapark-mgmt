namespace IndianaPark.PowerFan.Model
{
    /// <summary>
    /// Un biglietto per il PowerFan
    /// </summary>
    public class BigliettoPowerFan : Biglietto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BigliettoPowerFan"/> class.
        /// </summary>
        /// <param name="prezzo">Il prezzo del biglietto</param>
        public BigliettoPowerFan( decimal prezzo ) : base( prezzo )
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
            return "PowerFan";
        }
    }
}
