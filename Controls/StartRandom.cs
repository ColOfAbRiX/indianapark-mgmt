using System;

namespace IndianaPark.Tools
{
    /// <summary>
    /// Oggetto per ottenere numeri casuali in cui l'inizializzazione viene effettuata
    /// in automatico al momento della creazione dell'istanza
    /// </summary>
    public class StartRandom : Random
    {
		#region Fields 

		#region Class-wise Fields 

        private static int ms_Seed;

		#endregion Class-wise Fields 

		#region Internal Fields 

        private readonly Random m_random;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="StartRandom"/> class.
        /// </summary>
        public StartRandom()
        {
            lock( new object() )
            {
                var rnd = new Random( (int)DateTime.Now.Ticks );
                ms_Seed = rnd.Next();

                this.m_random = new Random( ms_Seed );
            }
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero and less than <see cref="F:System.Int32.MaxValue"/>.
        /// </returns>
        public override int Next()
        {
            return this.m_random.Next();
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>
        /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
        /// </returns>
        public override double NextDouble()
        {
            return this.m_random.NextDouble();
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to zero.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily includes zero but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="maxValue"/> is less than zero. </exception>
        public override int Next( int maxValue )
        {
            return this.m_random.Next( maxValue );
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null. </exception>
        public override void NextBytes( byte[] buffer )
        {
            this.m_random.NextBytes( buffer );
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        public override int Next( int minValue, int maxValue )
        {
            return this.m_random.Next( minValue, maxValue );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
