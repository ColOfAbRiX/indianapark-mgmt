using System.Windows.Forms;

namespace IndianaPark.Tools.Wizard
{
    /// <summary>
    /// Classe utilizzata per tipizzare tutti gli stati per l'emissione di biglietti
    /// </summary>
    /// <remarks>
    /// <para>Insieme ai membri di default della classe base <see cref="WizardStateBase"/> prevede tre proprietà protette
    /// basilari: una per la gestione della navigazione <see cref="WizardStateBase.NavigationAgent"/>, una per lo scambio
    /// di dati con l'utente <see cref="UserData"/> e una per la GUI visualizzata all'utente <see cref="StateForm"/>.</para>
    /// <para>Valorizzando la proprietà <see cref="StateForm"/> si valorizzando anche le altre due, in quanto un oggetto
    /// di tipo <see cref="WizardForm"/> implementa anche i metodi per le altre due proprietà.</para>
    /// <para>Il <see cref="IBuilder"/> viene tipizzato ed utilizzato per per gestire il salvataggio/recupero delle
    /// informazioni del wizard</para>
    /// <para>E' anche presente un oggetto per avere istanze univoche ti stati figlio, utilizzando un oggetto di tipo
    /// <see cref="TypedObjectPool"/>. Questo è utile per non creare ogni volta istanze nuove di stati a cui dirigersi
    /// ma riutilizzare quelli gia creati.</para>
    /// </remarks>
    public abstract class GenericWizardStateBase<TBuilder> : WizardStateBase where TBuilder : IBuilder
    {
		#region Fields 

		#region Internal Fields 

        private bool m_disposed;
        private WizardForm m_stateForm;
        /// <summary>
        /// Pool di stati creati per la navigazione.
        /// </summary>
        /// <remarks>
        /// Utilizzabile per non perdere stati configurati dall'utente durante la navigazione.
        /// </remarks>
        protected TypedObjectPool<GenericWizardStateBase<TBuilder>> StatePool { get; private set; }
        /// <summary>
        /// Form gestita dallo stato
        /// </summary>
        protected Form Gui { get; set; }

        /// <summary>
        /// Variabile che contiene il collegamento al <see cref="WizardForm"/> utilizzato dall'istanza della classe
        /// </summary>
        protected WizardForm StateForm
        {
            get { return this.m_stateForm; }
            set
            {
                if( this.NavigationAgent != null )
                {
                    this.NavigationAgent.Navigate -= this.DataReadyHandler;
                }

                this.Gui = value;
                this.UserData = value;
                this.NavigationAgent = value;

                if( value != null )
                {
                    this.NavigationAgent.Navigate += this.DataReadyHandler;
                }
            }
        }

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="EmissioneBaseState&lt;TBuilder&gt;"/> class.
        /// </summary>
        /// <param name="wizard">Il wizard parent dello stato</param>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <remarks>
        /// Ogni stato ricorda il suo precedente, che è lo stato che lo ha creato e non quello da cui si arriva
        /// </remarks>
        protected GenericWizardStateBase( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
            this.StatePool = new TypedObjectPool<GenericWizardStateBase<TBuilder>>();
        }

		#endregion Constructors 
		
        #region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato.
        /// </summary>
        /// <param name="builder">Oggetto per il recupero/salvataggio dei dati acquisiti durante il wizard.</param>
        /// <remarks>
        /// 	<para>Viene chiamato ogni volta che lo stato diventa lo stato corrente.</para>
        /// 	<para>La procedura standard prevede che questo metodo sia chiamato mentre è ancora attivo il gestore degli
        /// eventi dello stato precedente.</para>
        ///     <para>Ogni volta che viene chiamato si assicura che la form <see cref="Gui"/> venga visualizzata.</para>
        /// </remarks>
        public virtual void EnterState( TBuilder builder )
        {
            if( this.Gui != null )
            {
                this.Gui.Show();
            }
        }

        /// <summary>
        /// Termina l'esecuzione dello stato.
        /// </summary>
        /// <param name="builder">Oggetto per il recupero/salvataggio dei dati acquisiti durante il wizard.</param>
        /// <remarks>Viene chiamato ogni volta che lo stato termina di essere quello corrente</remarks>
        public virtual void ExitState( TBuilder builder )
        {
        }

        /// <summary>
        /// Avvia la gestione dello stato.
        /// </summary>
        /// <param name="builder">Oggetto per il recupero/salvataggio dei dati acquisiti durante il wizard.</param>
        /// <remarks>
        /// 	<para>Viene chiamato ogni volta che lo stato diventa lo stato corrente.</para>
        /// 	<para>La procedura standard prevede che questo metodo sia chiamato mentre è ancora attivo il gestore degli
        /// eventi dello stato precedente.</para>
        /// </remarks>
        public override sealed void EnterState( IBuilder builder )
        {
            this.EnterState( (TBuilder)builder );
        }

        /// <summary>
        /// Termina l'esecuzione dello stato.
        /// </summary>
        /// <param name="builder">Oggetto per il recupero/salvataggio dei dati acquisiti durante il wizard.</param>
        /// <remarks>Viene chiamato ogni volta che lo stato termina di essere quello corrente</remarks>
        public override sealed void ExitState( IBuilder builder )
        {
            this.ExitState( (TBuilder)builder );
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        /// <b>Nota:</b> questa funzione ipotizza che nessun nodo del grafo degli stati di un wizard sia condiviso da altri wizard, perchè
        /// attraversa tutto il grafo per finalizzarlo, exception se fosse condiviso finalizzerebbe stati utilizzati da altri wizard
        /// </remarks>
        /// <filterpriority>2</filterpriority>
        public override void Dispose()
        {
            // Evito riferimenti circolari
            if( this.m_disposed )
            {
                return;
            }
            this.m_disposed = true;

            // Finalizzo gli stati collegati
            if( this.PreviousState != null )
            {
                this.PreviousState.Dispose();
                this.PreviousState = null;
            }
            if( this.NextState != null )
            {
                this.NextState.Dispose();
                this.NextState = null;
            }

            // Finalizzo l'interfaccia grafica
            if( this.StateForm != null )
            {
                this.StateForm.Dispose();
                this.StateForm = null;
            }
            if( this.Gui != null )
            {
                this.Gui.Dispose();
                this.Gui = null;
            }

            // Completo la finalizzazione dalla classe genitore
            base.Dispose();
        }
        
        #endregion Public Methods 

		#endregion Methods 
    }
}