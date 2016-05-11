using System;

namespace IndianaPark.Tools.Wizard
{
    /// <summary>
    /// Crea un'istanza di un qualsiasi oggetto che implementa <see cref="IState"/> e che ha un
    /// costruttore con un unico parametro di tipo <see cref="IState"/> che accetta lo stato
    /// di provenienza
    /// </summary>
    /// <typeparam name="TState">L'oggetto che implementa <see cref="IState"/> che il Builder deve creare</typeparam>
    public class StateFactory<TState> : IStateBuilder where TState : IState
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Utilizzato per impostare lo stato collegato con il nuovo WizardStateBase
        /// </summary>
        public IState OriginState { get; set; }

        /// <summary>
        /// Riferimento all'oggetto <see cref="Wizard"/> che gestisce questo stato
        /// </summary>
        public Wizard Wizard { get; set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="StateFactory&lt;TState&gt;"/> class.
        /// </summary>
        public StateFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateFactory&lt;TState&gt;"/> class.
        /// </summary>
        /// <param name="origin">Stato di provenienza</param>
        /// <param name="wizard">Oggetto <see cref="Wizard"/> che gestisce lo stato da creare</param>
        public StateFactory( IState origin, Wizard wizard )
        {
            this.OriginState = origin;
            this.Wizard = wizard;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Crea il nuovo stato del wizard
        /// </summary>
        /// <returns>
        /// Il nuovo stato del wizard, oppure <c>null</c> se il tipo <typeparamref name="TState"/> non possiede un
        /// costruttore il cui unico argomento è <see cref="IState"/>
        /// </returns>
        public IState Create()
        {
            // Controllo che ci sia un costruttore del giusto tipo
            if( typeof(TState).GetConstructor( new[] { typeof(Wizard), typeof(IState) } ) != null )
            {
               return (IState)Activator.CreateInstance( typeof( TState ), this.Wizard, this.OriginState );
            }

            return null;
        }

#pragma warning disable 0693
        /// <summary>
        /// Crea un nuovo stato del tipo specificato
        /// </summary>
        /// <typeparam name="TState">Tipo di oggetto <see cref="IState"/> da creare</typeparam>
        /// <returns>
        /// Il nuovo stato del wizard, oppure <c>null</c> se il tipo <typeparamref name="TState"/> non possiede un
        /// costruttore il cui unico argomento è <see cref="IState"/>
        /// </returns>
        public TState Create<TState>() where TState : IState
        {
            return (TState)this.Create();
        }
#pragma warning restore 0693

        #endregion Public Methods

        #endregion Methods
    }
}