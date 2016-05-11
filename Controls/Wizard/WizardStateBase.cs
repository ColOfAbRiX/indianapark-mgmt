using IndianaPark.Tools.Navigation;
using System;

namespace IndianaPark.Tools.Wizard
{
    /// <summary>
    /// Realizzazione dell'interfaccia <see cref="IState"/> con l'aggiunta di alcune funzioni predefinite
    /// </summary>
    /// <remarks>
    /// <para>La classe prevede di avere un agente per la navigazione utente, memorizzato in <see cref="NavigationAgent"/>,
    /// un sorgente per i dati inseriti dall'utente tramite <see cref="UserData"/>.</para>
    /// <para>Sono creati dei metodi virtuali per la gestione di alcuni eventi dei membri e funzioni astratte per permettere
    /// la corretta configurazione del wizard.</para>
    /// </remarks>
    public abstract class WizardStateBase : IState
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// L'oggetto che gestisce la movimentazione tra stati
        /// </summary>
        /// <remarks>
        /// Lo stato può fare uso di questo oggetto per determinare lo stato successivo di navigazione
        /// </remarks>
        protected INavigator NavigationAgent { get; set; }

        /// <summary>
        /// L'oggetto da cui attingere i dati inseriti dall'utente
        /// </summary>
        /// <remarks>
        /// Se è necessario leggere dati inseriti in una GUI dall'utente questo campo fornisce un'interfaccia standard
        /// per questo scopo.
        /// </remarks>
        protected IUserData UserData { get; set; }

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Lo stato a cui si viene portati dopo questo
        /// </summary>
        public IState NextState { get; protected set; }

        /// <summary>
        /// Lo stato precedente a questo nel wizard
        /// </summary>
        public IState PreviousState { get; protected set; }

        /// <summary>
        /// Riferimento all'oggetto <see cref="Wizard"/> che gestisce questo stato
        /// </summary>
        /// <remarks>
        /// Ogni stato ha sempre e solo un proprietario determinato all'atto della creazione dell'istanza. In questa
        /// maniera uno stato può essere usato in diversi <see cref="Wizard"/> ma una specifica istanza solamente per
        /// un <see cref="Wizard"/> ben specifico.
        /// </remarks>
        public Wizard Wizard { get; protected set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Avvisa quando si è pronti per effettuare il cambio di stato
        /// </summary>
        public event StatusChangeRequestedHandler StatusChangeRequested;

		#region Raisers 

        /// <summary>
        /// Chiamato per notificare la disponibilità al cambio di stato
        /// </summary>
        protected void OnStatusChangeRequested( NavigationAction requestedChange )
        {
            if( this.StatusChangeRequested != null )
            {
                this.StatusChangeRequested(
                    this,
                    new StatusChangeRequestedEventArgs( requestedChange, this.PreviousState, this.NextState )
                );
            }
        }

		#endregion Raisers 

		#region Event Handlers 

        /// <summary>
        /// Gestore del recupero dei dati utente
        /// </summary>
        /// <param name="source">Oggetto che scatena l'evento</param>
        /// <param name="e">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// 	<para>Chiamata quando la finestra grafica ha disponibili i dati.</para>
        /// 	<para>Recupera i dati dalla WizardForm e notifica il cambio di stato.</para>
        /// </remarks>
        protected virtual void DataReadyHandler( object source, NavigationEventArgs e )
        {
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardStateBase"></see> class.
        /// </summary>
        /// <param name="wizard"></param>
        /// <param name="previous">The previous.</param>
        protected WizardStateBase(Wizard wizard, IState previous )
        {
            this.Wizard = wizard;
            this.PreviousState = previous;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato.
        /// </summary>
        /// <param name="builder">Oggetto per il recupero/salvataggio dei dati acquisiti durante il wizard.</param>
        /// <remarks>
        /// <para>Viene chiamato ogni volta che lo stato diventa lo stato corrente.</para>
        /// <para>La procedura standard prevede che questo metodo sia chiamato mentre è ancora attivo il gestore degli
        /// eventi dello stato precedente.</para>
        /// </remarks>
        public abstract void EnterState( IBuilder builder );

        /// <summary>
        /// Termina l'esecuzione dello stato.
        /// </summary>
        /// <param name="builder">Oggetto per il recupero/salvataggio dei dati acquisiti durante il wizard.</param>
        /// <remarks>Viene chiamato ogni volta che lo stato termina di essere quello corrente</remarks>
        public abstract void ExitState( IBuilder builder );

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Dispose()
        {
            // Chiudo tutte le risorse utilizzate

            if( this.NextState != null )
            {
                this.NextState.Dispose();
                this.NextState = null;
            }

            if( this.NextState != null )
            {
                this.PreviousState.Dispose();
                this.PreviousState = null;
            }

            this.NavigationAgent = null;
            this.UserData = null;

            this.StatusChangeRequested = null;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}