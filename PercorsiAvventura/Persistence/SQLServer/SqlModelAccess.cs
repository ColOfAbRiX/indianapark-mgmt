namespace IndianaPark.PercorsiAvventura.Persistence.SqlServer
{
    /// <summary>
    /// Permette il caricamento ed il salvataggio del modello dei dati dei PercorsiAvventura su database di tipo
    /// SqlServer2005
    /// </summary>
    public class SqlModelAccess : IndianaPark.PercorsiAvventura.Persistence.ModelAccessFactory
    {
		#region Fields 

		#region Internal Fields 

        readonly IModelDataAccess m_prezziLoader;
        readonly IModelDataAccess m_scontiLoader;
        readonly IModelDataAccess m_clientiLoader;
        readonly IModelDataAccess m_briefingsLoader;
        readonly IModelDataAccess m_abbonamentiLoader;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlModelAccess"/> class.
        /// </summary>
        /// <param name="persistence">The persistence.</param>
        public SqlModelAccess( Plugin.IPersistence persistence )
        {
            if( persistence == null || !persistence.IsConnectionInitialized  )
            {
                return;
            }

            this.m_abbonamentiLoader = null;
            this.m_briefingsLoader = null;
            this.m_clientiLoader = new ClientiModelPersistence( persistence );
            this.m_prezziLoader = null;
            this.m_scontiLoader = new ScontiModelPersistence( persistence );
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
            bool output = true;

            // Rispettare l'ordine di caricamento !!!

            if( this.m_scontiLoader != null )
            {
                output &= this.m_scontiLoader.LoadModel();
            }

            if( this.m_briefingsLoader != null )
            {
                output &= this.m_briefingsLoader.LoadModel();
            }

            if( this.m_prezziLoader != null )
            {
                output &= this.m_prezziLoader.LoadModel();
            }

            if( this.m_clientiLoader != null )
            {
                output &= this.m_clientiLoader.LoadModel();
            }

            if( this.m_abbonamentiLoader != null )
            {
                output &= this.m_abbonamentiLoader.LoadModel();
            }

            return output;
        }

        /// <summary>
        /// Salva i dati del modello
        /// </summary>
        /// <returns><c>true</c> se il salvataggio è andato a buon fine, <c>false</c> altrimenti</returns>
        public override bool SaveModel()
        {
            bool output = true;

            // Rispettare l'ordine di salvataggio !!!

            if( this.m_abbonamentiLoader != null )
                output &= this.m_abbonamentiLoader.SaveModel();

            if( this.m_briefingsLoader != null )
                output &= this.m_briefingsLoader.SaveModel();

            if( this.m_clientiLoader != null )
                output &= this.m_clientiLoader.SaveModel();

            if( this.m_prezziLoader != null )
                output &= this.m_prezziLoader.SaveModel();

            if( this.m_scontiLoader != null )
                output &= this.m_scontiLoader.SaveModel();

            return output;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
