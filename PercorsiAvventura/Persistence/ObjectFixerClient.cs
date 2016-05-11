using System;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Persistence
{
    /// <summary>
    /// Indica che un oggetto può essere salvato e ricaricato in qualsiasi contesto di persistenza
    /// </summary>
    public interface IObjectFixable
    {
		#region Methods 

        /// <summary>
        /// Restituisce i valori con cui è possibile reistanziare l'oggetto
        /// </summary>
        /// <returns>Un oggetto <see cref="IList&lt;T&gt;"/> contenente i valori dei parametri con cui
        /// è stato istanziato l'oggetto a cui l'istanza fa riferimen</returns>
        IList<object> GetDefaultCtorParams( Model.ISconto instance );

        /// <summary>
        /// Crea un'istanza dell'oggetto utilizzando la lista di parametri specificata
        /// </summary>
        /// <param name="parameters">La lista di parametri</param>
        /// <returns>Un'istanza dello sconto costruita con i parametri indicati.</returns>
        Model.ISconto GetInstanceFromParams( IList<object> parameters );

		#endregion Methods 
    }

    /// <summary>
    /// Oggetto che si occupa di fornire i dati necessari per rendere persistente o per ricaricare da una persistenza
    /// un qualsiasi oggetto.
    /// </summary>
    public abstract class ObjectFixerClient
    {
		#region Methods 

		#region Internal Methods 

        /// <summary>
        /// Ottiene il corretto tipo di oggetto da utilizzare per estrarre le informazioni dell'oggetto da salvare/caricare
        /// </summary>
        /// <param name="tipo">Tipo dell'oggetto da salvare/caricare</param>
        /// <returns>Un'istanza di <see cref="IObjectFixable"/> relativo a <paramref name="tipo"/> indicato</returns>
        protected abstract IObjectFixable GetWrapper( Type tipo );

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Restituisce i valori con cui è possibile reistanziare l'oggetto
        /// </summary>
        /// <param name="sconto">L'istanza dell'oggetto di cui recuperare i parametri del costruttore</param>
        /// <returns>
        /// Un oggetto <see cref="IList&lt;T&gt;"/> contenente i valori dei parametri con cui
        /// è stato istanziato l'oggetto a cui l'istanza fa riferimento. <c>null</c> se l'oggetto <paramref name="sconto"/>
        /// non è presente nel gestore.
        /// </returns>
        public IList<object> GetParametersFromInstance( Model.ISconto sconto )
        {
            var wrapper = this.GetWrapper( sconto.GetType() );
            if( wrapper == null )
            {
                return null;
            }

            return wrapper.GetDefaultCtorParams( sconto );
        }

        /// <summary>
        /// Crea un'istanza dell'oggetto utilizzando la lista di parametri specificata
        /// </summary>
        /// <param name="tipo">Il tipo di oggetto da istanziare.</param>
        /// <param name="parameters">La lista di parametri da passare al costruttore dell'oggetto</param>
        /// <returns>
        /// Un'istanza dello sconto costruita con i parametri indicati.<c>null</c> se <paramref name="tipo"/>
        /// non è presente nel gestore.
        /// </returns>
        public Model.ISconto InstanceFromParameters( Type tipo, IList<object> parameters )
        {
            var wrapper = this.GetWrapper( tipo );
            if( wrapper == null )
            {
                return null;
            }

            return wrapper.GetInstanceFromParams( parameters );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
