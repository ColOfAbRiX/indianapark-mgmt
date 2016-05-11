using System;
using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    class InputScontoState : EmissioneBaseState<CustomDiscountBuilder>
    {
		#region Fields 

		#region Internal Fields 

        private string m_discountValue;
        private CustomDiscountBuilder m_builder;

		#endregion Internal Fields 

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, Tools.Navigation.NavigationEventArgs e )
        {
            if( e.Status == Tools.Navigation.NavigationAction.Next )
            {
                // Recupero valore da attribuire allo sconto
                this.m_discountValue = WizardForm.ConvertUserData<string>( this.UserData );
                this.m_builder.Storage.Valore = this.m_discountValue;

                try
                {
                    // Vedo se il valore dello sconto provoca eccezioni nella creazione dello sconto.
                    this.m_builder.Storage.CreateISconto();
                }
                catch( Exception )
                {
                    this.m_discountValue = "";
                    this.m_builder.Storage.Valore = "";

                    System.Windows.Forms.MessageBox.Show(
                        Properties.Resources.WrongNumericInputText, Properties.Resources.WrongNumericInputTitle,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Exclamation
                    );

                    // Ritorno su questo stesso stato in caso di errore
                    this.NextState = this.StatePool.GetUniqueType( new InputScontoState( this.Wizard, this ) );
                    this.OnStatusChangeRequested( Tools.Navigation.NavigationAction.Next );
                    return;
                }
            }

            this.OnStatusChangeRequested( Tools.Navigation.NavigationAction.Finish );
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="InputScontoState"/> class.
        /// </summary>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <remarks>
        /// Ogni stato ricorda il suo precedente, che è lo stato che lo ha creato e non quello da cui si arriva
        /// </remarks>
        public InputScontoState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
            this.StateForm = new Tools.Controls.NumericInputForm( Properties.Resources.ScontoValueTitle, Properties.Resources.ScontoValueText );
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
        public override void EnterState( CustomDiscountBuilder builder )
        {
            this.NextState = null;
            this.m_builder = builder;
            base.EnterState( builder );
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( CustomDiscountBuilder builder )
        {
            builder.Storage.Valore = this.m_discountValue;
            builder.BuildResult();
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
