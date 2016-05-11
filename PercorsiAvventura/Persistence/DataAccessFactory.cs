namespace IndianaPark.PercorsiAvventura.Persistence
{
    /// <summary>
    /// Rappresenta l'interfaccia per l'accesso ai dati persistenti, ovvero per il salvataggio ed il caricamento dei dati
    /// dalla persistenza
    /// </summary>
    public interface IModelDataAccess
    {
		#region Methods 

        /// <summary>
        /// Carica i dati del modello
        /// </summary>
        /// <remarks>
        /// Quando non viene trovata una corrispondenza con un elemento del modello, per esempio quando viene cercato uno sconto
        /// che non è presente, quell'elemento viene ignorato (impostato a <c>null</c>. Se non viene trovato un elemento essenziale,
        /// come il TipoCliente, il caricamento viene interrotto.
        /// </remarks>
        /// <returns><c>true</c> se il caricamento è andato a buon fine, <c>false</c> altrimenti</returns>
        bool LoadModel();

        /// <summary>
        /// Salva i dati del modello
        /// </summary>
        /// <returns><c>true</c> se il salvataggio è andato a buon fine, <c>false</c> altrimenti</returns>
        bool SaveModel();

		#endregion Methods 
    }

    /// <summary>
    /// Crea un oggetto IModelDataAccess relativo al tipo di persistenza scelta
    /// </summary>
    public abstract class ModelAccessFactory : IModelDataAccess
    {
		#region Methods 

		#region Class-wise Methods 

        /// <summary>
        /// Crea l'oggetto utilizzato per il caricamento/salvataggio dei dati del modello
        /// </summary>
        /// <param name="persistence">Il sistema utilizzato per la persistenza</param>
        /// <remarks>
        /// Se non viene specificata nessuna persistenza è obbligatorio restituire un oggetto anche per questo caso
        /// </remarks>
        /// <returns>L'oggetto per l'accesso alla persistenza dei dati. <c>null</c> se il tipo
        /// di persistenza non è supportato</returns>
        public static IModelDataAccess CreateDataAccessor( Plugin.IPersistence persistence )
        {
            // Nessuna persistenza -> new Null.NullModelAccess()
            if( persistence == null || !persistence.IsConnectionInitialized )
            {
                return new Null.NullModelAccess( persistence );
            }

            // SqlServer -> SqlServer.SqlModelAccess()
            if( persistence.GetConnection() is System.Data.SqlClient.SqlConnection )
            {
                return new SqlServer.SqlModelAccess( persistence );
            }

            return null;
        }

		#endregion Class-wise Methods 

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
        public abstract bool LoadModel();

        /// <summary>
        /// Salva i dati del modello
        /// </summary>
        /// <returns><c>true</c> se il salvataggio è andato a buon fine, <c>false</c> altrimenti</returns>
        public abstract bool SaveModel();

		#endregion Public Methods 

		#endregion Methods 
    }
}
