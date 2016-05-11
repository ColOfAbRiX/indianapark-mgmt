using System;
using IndianaPark.Tools.Navigation;
using System.ComponentModel;
using System.Threading;

namespace IndianaPark.Tools.Wizard
{
    /// <summary>
    /// Gestore dei wizard.
    /// </summary>
    /// <remarks>
    /// L'oggetto si occupa dell'inizializzazione del wizard, di segnalare mediante eventi la sua terminazione,
    /// inizializzare e terminare il wizard, exception soprattutto si occupa di cambiare il proprio stato interno. Ogni
    /// stato rappresenta un passo del wizard.
    /// </remarks>
    public class Wizard : IDisposable
    {
		#region Delegates 

        /// <summary>
        /// Delegato per il gestore dell'evento <see cref="WizardCompleted"/>
        /// </summary>
        public delegate void WizardCompletedHandler( object sender, NavigationEventArgs e );

		#endregion Delegates 

		#region Fields 

		#region Internal Fields 

        private IState m_state;
        private readonly IBuilder m_builder;
        private bool m_cancelling;
        private BackgroundWorker m_worker;

        private IState State
        {
            get { return this.m_state; }
            set
            {
                // Gli stati non possono essere condivisi tra più wizard per mantenere un buon livello di coerenza
                if( value != null && value.Wizard != this )
                {
                    throw new ApplicationException( "The specified IState doesn't belong to this instance of Wizard" );
                }

                this.m_state = value;
            }
        }

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Il <see cref="IBuilder"/> per la costruzione delle strutture dati finali
        /// </summary>
        public IBuilder Builder
        {
            get { return this.m_builder; }
        }

        /// <summary>
        /// Lo stato a cui passare quando si richiede l'annullamento del Wizard. Se viene 
        /// impostato a <c>null</c> non viene utilizzato ed il Wizard termina normalmente
        /// </summary>
        /// <value>The state of the cancel.</value>
        public IStateBuilder CancelState { get; set; }

        /// <summary>
        /// Imposta lo stato di partenza del Wizard
        /// </summary>
        public IState InitialState { private get; set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Avvisa quando il Wizard è terminato
        /// </summary>
        public event WizardCompletedHandler WizardCompleted;

		#region Raisers 

        /// <summary>
        /// Genera l'evento per notificare il completamento del Wizard
        /// </summary>
        protected void OnWizardCompleted( NavigationAction requestedChange )
        {
            if( this.WizardCompleted != null )
            {
                this.WizardCompleted(
                    this,
                    new NavigationEventArgs(requestedChange)
                );
            }
        }

		#endregion Raisers 

		#region Event Handlers 

        private void StateChangeRequestedHandler( object sender, StatusChangeRequestedEventArgs e )
        {
            // Tratto lo stato in uscita
            this.State.StatusChangeRequested -= this.StateChangeRequestedHandler;
            this.State.ExitState( this.m_builder );

            // Decido qual'è lo stato successivo in base a dove devo muovermi
            switch( e.RequestedChange )
            {
                case NavigationAction.Next:
                    this.m_cancelling = false;
                    this.State = e.NextState;
                    break;

                case NavigationAction.Back:
                    this.m_cancelling = false;
                    this.State = e.PreviousState;
                    break;

                case NavigationAction.Finish:
                    this.m_cancelling = false;
                    this.State = null;
                    break;

                case NavigationAction.Cancel:
                    // Solo se è stato abilitato l'uso di uno stato di annullamento
                    if( this.CancelState != null )
                    {
                        // Lo stato CancelWizardState emette ancora un NavigationAction.Cancel che devo ignorare
                        if( !this.m_cancelling )
                        {
                            this.m_cancelling = true;
                            this.CancelState.OriginState = this.State;
                            this.CancelState.Wizard = this.CancelState.Wizard ?? this;
                            this.State = this.CancelState.Create();
                        }
                        else
                        {
                            this.m_cancelling = false;
                            this.State = null;
                        }
                    }
                    else
                    {
                        // Stato di annullamento non abilitato, richiedo l'uscita
                        this.State = null;
                    }

                    break;

                case NavigationAction.Nothing:
                    this.m_cancelling = false;
                    break;
            }

            // Il Wizard termina quando non ci sono stati successivi, che avviene con <c>e.Status = NavigationAction.Finish</c>
            // oppure dalla convalida di <c>e.Status = NavigationAction.Cancel</c>
            if( this.State == null )
            {
                if( e.RequestedChange == NavigationAction.Finish && this.m_builder != null )
                {
                    this.m_builder.BuildResult();
                }

                this.OnWizardCompleted( e.RequestedChange );
                return;
            }

            Logging.Logger.Default.Write( String.Format( "Wizard is changing state to {0}", this.State.GetType() ) );

            // Inizializzo il nuovo stato
            this.State.StatusChangeRequested += this.StateChangeRequestedHandler;
            this.State.EnterState( this.m_builder );
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="Wizard"/> class.
        /// </summary>
        public Wizard() : this( null, null )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wizard"/> class.
        /// </summary>
        /// <param name="startState">Lo stato di partenza</param>
        public Wizard( IState startState ) : this( startState, null )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wizard"/> class.
        /// </summary>
        /// <param name="builder">Il <see cref="IBuilder"/> per la costruzione delle strutture dati finali</param>
        public Wizard( IBuilder builder ) : this( null, builder )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wizard"/> class.
        /// </summary>
        /// <param name="startState">Lo stato di partenza</param>
        /// <param name="builder">Il <see cref="IBuilder"/> per la costruzione delle strutture dati finali</param>
        public Wizard( IState startState, IBuilder builder )
        {
            this.m_builder = builder;
            this.InitialState = startState;
        }

        #endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Avvia il Wizard
        /// </summary>
        /// <remarks>Avvia il wizard utilizzando il valore di <see cref="Wizard.InitialState"/></remarks>
        public void StartWizard()
        {
            this.StartWizard( this.InitialState );
        }

        /// <summary>
        /// Avvia il Wizard
        /// </summary>
        /// <param name="initialState">Lo stato di partenza.</param>
        /// <remarks>Oltre ad avviare il wizard viene reimpostato il valore di <see cref="Wizard.InitialState"/></remarks>
        public void StartWizard( IState initialState )
        {
            if( initialState == null )
            {
                throw new ArgumentNullException( "initialState" );
            }
            this.InitialState = initialState;

            Logging.Logger.Default.Write( String.Format( "Starting wizard from state {0}", this.InitialState.GetType() ) );

            // Imposto lo stato corrente a quello iniziale
            this.State = this.InitialState;

            // Mi assicuro che il gestore sia sempre associato una e solo una volta
            this.InitialState.StatusChangeRequested -= this.StateChangeRequestedHandler;
            this.InitialState.StatusChangeRequested += this.StateChangeRequestedHandler;

            /*
            var tState = new Thread(
                new ThreadStart(
                    delegate()
                    {
                        this.InitialState.EnterState( this.m_builder );
                    }
                )
            );
            tState.Name = "Wizard State Thread";
            tState.Start();
            */

            /*
            this.m_worker = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false,
            };

            this.m_worker.DoWork += delegate( object o, DoWorkEventArgs e )
            {
                this.InitialState.EnterState( this.m_builder );
            };

            this.m_worker.RunWorkerAsync();
            */

            // this.InitialState.EnterState( this.m_builder );
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if( this.m_builder != null )
            {
                this.m_builder.Dispose();
            }

            if( this.InitialState != null )
            {
                this.InitialState.Dispose();
M                this.InitialState = null;
            }

            this.WizardCompleted = null;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
