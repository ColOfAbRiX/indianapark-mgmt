using System;
namespace IndianaPark.Tools.Wizard
{
    /// <summary>
    /// Generico costruttore di dati per i wizard
    /// </summary>
    /// <remarks>
    /// Il <see cref="GenericWizardBuilderBase&lt;TStorage, TResult&gt;"/> hai il compito di costruire i dati finali utilizzando
    /// le informazioni memorizzate nello <typeparamref name="TStorage"/> di cui ha un riferimento e quindi di 
    /// permetterne l'accesso all'esterno.</remarks>
    /// <typeparam name="TStorage">Il tipo di oggetto utilizzato per contenere i dati provvisori</typeparam>
    /// <typeparam name="TResult">Il tipo di dato del risultato del costruttore</typeparam>
    public abstract class GenericWizardBuilderBase<TStorage, TResult> : IBuilder
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Il contenitore di dati per il Builder dei biglietti
        /// </summary>
        object IBuilder.Storage
        {
            get { return this.Storage; }
        }

        /// <summary>
        /// Il contenitore di dati per il Builder dei biglietti
        /// </summary>
        public abstract TStorage Storage { get; protected set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Public Methods 

        /// <summary>
        /// Costruisce e restituisce il risultato della costruzione
        /// </summary>
        /// <returns><c>true</c> se la funziona ha costruito i dati con successo, <c>false</c> altrimenti.</returns>
        public abstract bool BuildResult();

        /// <summary>
        /// Restituisce il risultato costruito con <see cref="IBuilder.BuildResult"/>
        /// </summary>
        /// <returns>
        /// L'oggetto costruito, oppure <c>null</c> se <see cref="IBuilder.BuildResult"/> non è mai stata
        /// chiamata o la sua chiamata non è riuscita
        /// </returns>
        object IBuilder.GetResult()
        {
            return this.GetResult();
        }

        /// <summary>
        /// Restituisce il risultato costruito con <see cref="IBuilder.BuildResult"/>
        /// </summary>
        /// <returns>
        /// L'oggetto costruito, oppure <c>null</c> se <see cref="IBuilder.BuildResult"/> non è mai stata
        /// chiamata o la sua chiamata non è riuscita
        /// </returns>
        public abstract TResult GetResult();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Dispose()
        {
            // Finalizzo lo storage
            if( this.Storage.ImplementsInterface( typeof(IDisposable) ) )
            {
                ((IDisposable)this.Storage).Dispose();
            }
            this.Storage = default( TStorage );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}