using System.Collections.Generic;
using System.Linq;
using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Rappresenta lo stato dove si sceglie la tipologia di biglietto
    /// </summary>
    public sealed class TipoBigliettoState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
		#region Fields 

        private Model.TipoBiglietto m_bigiettoScelto;
        private readonly bool m_autoForward = (bool)PluginPercorsi.GetGlobalParameter( "AutoForward" ).Value;
        private readonly Dictionary<string, Model.TipoBiglietto> m_tipologieBiglietto = Model.Parco.GetParco().TipologieBiglietto;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, NavigationEventArgs e )
        {
            this.NextState = null;

            if( e.Status == NavigationAction.Next )
            {
                // Recupero il tipo di biglietto scelto
                this.m_bigiettoScelto = WizardForm.ConvertUserData<Model.TipoBiglietto>( this.UserData );
                this.NextState = this.StatePool.GetUniqueType( new NominativoState( this.Wizard, this ) );

                // Se si tratta di un abbonamento devo passare a gestire l'abbonamento exception non i bigietti standard
                if( this.m_bigiettoScelto.IsAbbonamento )
                {
                    System.Windows.Forms.MessageBox.Show( "Abbonamenti ancora non disponibili", "Errore", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                    e.Cancel = true;
                    return;
                }
            }

            this.OnStatusChangeRequested( e.Status );
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoAttrazioneState"/> class.
        /// </summary>
        public TipoBigliettoState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
            if( this.m_tipologieBiglietto.Count > 1 )
            {
                // Alla form serve sapere quali sono le tipologie di biglietto del parco, per creare la lista pulsanti
                this.StateForm = new Forms.TipoBigliettoForm( Model.Parco.GetParco().TipologieBiglietto );
            }
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato
        /// </summary>
        public override void EnterState( BigliettoPercorsiBuilder builder )
        {
            // Controllo se devo fare l'auto-forward
            if( this.m_autoForward && this.m_tipologieBiglietto.Count == 1 )
            {
                this.m_bigiettoScelto = this.m_tipologieBiglietto.ToList()[0].Value;
                // Ricordarsi che in caso di NavigateAction.Back non si deve tornare qua...
                this.NextState = new TipoAbbonamentoState( this.Wizard, this.PreviousState );
                this.OnStatusChangeRequested( NavigationAction.Next );
            }
            else
            {
                base.EnterState( builder );
            }
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettoPercorsiBuilder builder )
        {
            // Memorizzo il tipo di biglietto scelto nel Builder
            builder.Storage.TipoBiglietto = this.m_bigiettoScelto;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}