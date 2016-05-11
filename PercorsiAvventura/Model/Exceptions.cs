using System;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Classe che rappresenta un'eccezione del modello di dati
    /// </summary>
    public class ModelException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelException"/> class.
        /// </summary>
        public ModelException() : base() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ModelException( string message ) : base( message ) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ModelException( string message, Exception innerException ) : base( message, innerException ) { }
    }
}
