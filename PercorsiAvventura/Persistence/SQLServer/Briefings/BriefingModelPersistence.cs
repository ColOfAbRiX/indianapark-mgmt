using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IndianaPark.PercorsiAvventura.Persistence.SqlServer
{
    /// <summary>
    /// Classe per il caricamento e salvataggio di oggetti per il briefing
    /// </summary>
    internal sealed class BriefingModelPersistence
    {
        private readonly BriefingsDataContext m_dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BriefingModelPersistence"/> class.
        /// </summary>
        /// <param name="persistence">The persistence.</param>
        public BriefingModelPersistence( Plugin.IPersistence persistence )
        {
            // Controllo dei parametri in ingresso
            if( persistence == null )
            {
                throw new ArgumentNullException( "persistence" );
            }
            if( !persistence.IsConnectionInitialized )
            {
                throw new ArgumentNullException( "persistence", "The connection was not initialized" );
            }

            // Creo il contesto di lavoro per Cliente
            this.m_dataContext = new BriefingsDataContext( persistence.GetConnection() );
        }

        /// <summary>
        /// Carica i dati del modello relativo ai briefings
        /// </summary>
        /// <remarks>
        /// Quando non viene trovata una corrispondenza con un elemento del modello, per esempio quando viene cercato uno sconto
        /// che non è presente, quell'elemento viene ignorato (impostato a <c>null</c>. Se non viene trovato un elemento essenziale,
        /// come il TipoCliente, il caricamento viene interrotto.
        /// </remarks>
        /// <returns><c>true</c> se il caricamento è andato a buon fine, <c>false</c> altrimenti</returns>
        public bool LoadFromDatabase()
        {
            try
            {
            }
            catch( DbException dbex )
            {
                Debug.WriteLine( "Database exception while loading the model" );
                Debug.Indent();
                Debug.WriteLine( dbex.Source );
                Debug.WriteLine( dbex.Message );
                Debug.WriteLine( dbex.StackTrace );
                Debug.Unindent();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Salva i dati del modello relativo ai briefings
        /// </summary>
        /// <returns><c>true</c> se il salvataggio è andato a buon fine, <c>false</c> altrimenti</returns>
        public bool SaveToDatabase()
        {
            try
            {
            }
            catch( DbException dbex )
            {
                Debug.WriteLine( "Database exception while loading the model" );
                Debug.Indent();
                Debug.WriteLine( dbex.Source );
                Debug.WriteLine( dbex.Message );
                Debug.WriteLine( dbex.StackTrace );
                Debug.Unindent();

                return false;
            }

            return true;
        }
    }
}
