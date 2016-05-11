using System.Linq;
using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Rappresenta lo stato dove si inserisce il nominativo
    /// </summary> 
    public sealed class NominativoState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
		#region Fields 

        private string m_nominativo;

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
            if( e.Status == NavigationAction.Next )
            {
                // Recupero il testo del nominativo
                this.m_nominativo = WizardForm.ConvertUserData<string>( this.UserData ).Trim();

                this.NextState = this.StatePool.GetUniqueType( new TipoClienteState( this.Wizard, this ) );

                // Controllo se il nominativo è gia presente
                var trovati = from n in Model.Parco.GetParco().ListaClienti.Values
                              where n.Nome.Equals( m_nominativo, System.StringComparison.InvariantCultureIgnoreCase )
                              select n;

                if( trovati.Count() > 0 )
                {
                    var result = System.Windows.Forms.MessageBox.Show(
                        Properties.Resources.NominativoEsistenteDescription,
                        Properties.Resources.NominativoEsistenteTitle,
                        System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning
                    );

                    if( result == System.Windows.Forms.DialogResult.No )
                    {
                        this.NextState = this;
                    }
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
        public NominativoState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato
        /// </summary>
        /// <param name="builder"></param>
        /// <remarks>
        /// La procedura standard prevede che questo metodo sia chiamato mentre è ancora attivo il gestore degli eventi
        /// dello stato precedente.
        /// </remarks>
        public override void EnterState( BigliettoPercorsiBuilder builder )
        {
            this.StateForm = new Tools.Controls.TextInputForm( Properties.Resources.NominativoInputTitle, Properties.Resources.NominativoInputText, builder.Storage.Nominativo );
            base.EnterState( builder );
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettoPercorsiBuilder builder )
        {
            // Salvo il nominativo inserito nel builder
            builder.Storage.Nominativo = this.m_nominativo;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}