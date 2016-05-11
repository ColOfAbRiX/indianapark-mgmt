using IndianaPark.Plugin;
namespace IndianaPark.PercorsiAvventura.Persistence.Null
{
    /// <summary>
    /// Accesso al modello se non è specificata nessuna persistenza. A conti fatti questa classe non fa nulla
    /// </summary>
    public class NullModelAccess : IndianaPark.PercorsiAvventura.Persistence.ModelAccessFactory
    {
		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlModelAccess"/> class.
        /// </summary>
        /// <param name="persistence">The persistence.</param>
        public NullModelAccess( IPersistence persistence )
        {
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Carica i dati del modello
        /// </summary>
        /// <remarks>
        /// Quando non viene trovata una corrispondenza con un elemento del modello, per esempio quando viene cercato uno sconto
        /// che non è presente, quell'elemento viene ignorato (impostato a <c>null</c>. Se non viene trovato un elemento essenziale,
        /// come il TipoCliente, il caricamento viene interrotto.
        /// </remarks>
        /// <returns><c>true</c> se il caricamento è andato a buon fine, <c>false</c> altrimenti</returns>
        public override bool LoadModel()
        {
            return true;
        }

        /// <summary>
        /// Salva i dati del modello
        /// </summary>
        /// <returns><c>true</c> se il salvataggio è andato a buon fine, <c>false</c> altrimenti</returns>
        public override bool SaveModel()
        {
            return true;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
