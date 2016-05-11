using System;
using System.Collections.Generic;
using System.Linq;

namespace IndianaPark.Tools
{
    /// <summary>
    /// Memorizza di istanze univoche di un oggetto.
    /// </summary>
    /// <remarks>
    /// Gestisce una lista di oggetti di tipo <typeparamref name="TObject"/> in cui di una data istanza può essere 
    /// presente una sola copia.
    /// </remarks>
    /// <typeparam name="TObject">Il tipo di oggetti da memorizzare</typeparam>
    public class TypedObjectPool<TObject>
    {
		#region Fields 

		#region Internal Fields 

        private readonly Dictionary<Type, TObject> m_pool = new Dictionary<Type, TObject>();

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Public Methods 

        /// <summary>
        /// Resetta il pool delle istanze
        /// </summary>
        /// <remarks>
        /// Cancella il pool ma non esegue nessuna operazione di eliminazione sulle istanze stesse.
        /// </remarks>
        public void Clear()
        {
            this.m_pool.Clear();
        }

        /// <summary>
        /// Recupera dal pool l'istanza dell'oggetto indicato.
        /// </summary>
        /// <param name="obj">L'oggetto da recuperare dal pool</param>
        /// <returns>
        /// L'istanza passata come argomento se questa non è presente nel pool, altrimenti quella presente nel pool.
        /// Se viene passato il valore <c>null</c> il metodo restituisce il valore di default per l'oggetto
        /// <typeparamref name="TObject"/>.
        /// </returns>
        public TObject GetUniqueType( TObject obj )
        {
            // Il metodo funziona anche passando il valore null
            if( obj == null )
            {
                return default( TObject );
            }

            if( this.m_pool.Keys.Contains( obj.GetType() ) )
            {
                return this.m_pool[obj.GetType()];
            }

            this.m_pool.Add( obj.GetType(), obj );
            return obj;
        }

		#endregion Public Methods 

		#endregion Methods 
    }

}
