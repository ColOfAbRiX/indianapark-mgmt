using System;
using System.Windows.Forms;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// Finestra di dialogo per scegliere tra gli sconti disponibili quello da applicare alla tipologia cliente
    /// </summary>
    public partial class SceltaScontoForm : WizardForm
    {
		#region Fields 

        private Model.ISconto m_scontoScelto;
        private bool m_customRequested;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Gestisce il click sul pulsante Esci
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EscapeClickHandler( object sender, EventArgs e )
        {
            this.Sail( NavigationAction.Back );
        }

        /// <summary>
        /// Gestisce le dimensioni del controllo ripetitore per aggiungere le barre di scorrimento laterali
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RepeaterResizeHandler( object sender, EventArgs e )
        {
            this.m_buttons.Resize -= this.RepeaterResizeHandler;

            if( this.m_buttons.Size.Height == this.m_buttons.MaximumSize.Height )
            {
                this.m_buttons.AutoScroll = true;
                this.m_buttons.Size += new System.Drawing.Size( 20, 0 );
                this.m_buttons.Location -= new System.Drawing.Size( 10, 0 );
                this.Size += new System.Drawing.Size( 20, 0 );
            }
            else
            {
                this.m_buttons.AutoScroll = false;
            }

            this.m_buttons.Resize += this.RepeaterResizeHandler;
        }

        /// <summary>
        /// Gestisce il click sul pulsante di sconto personalizzato
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CustomDiscountClickHandler( object sender, EventArgs e )
        {
            this.m_customRequested = true;
            this.Sail( NavigationAction.Next );
        }

        #endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SceltaScontoForm"/> class.
        /// </summary>
        public SceltaScontoForm()
        {
            InitializeComponent();

            this.m_buttons.Creator = new ChoiseButtonCreator( new System.Drawing.Size( 383, 78 ) );
            var listaSconti = Model.Parco.GetParco().ScontiPersonali;

            // Aggiunto i pulsanti relativi ai vari tipi di sconto.
            foreach( Model.ISconto sconto in listaSconti.Values )
            {
                var scelto = sconto;
                var btn = (Button)this.m_buttons.Add();

                btn.Text = sconto.Nome;
                btn.Click +=
                    delegate
                    {
                        this.m_scontoScelto = scelto;
                        this.Sail( NavigationAction.Next );
                    };
            }

            // Aggiungo un pulsante per lo sconto personalizzato
            var custom = (Button)this.m_buttons.Add();
            custom.Text = Properties.Resources.CustomDiscountButton;
            custom.Click += this.CustomDiscountClickHandler;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        /// <returns>Il valore gestito dalla form</returns>
        public override object GetData()
        {
            // Aggiorno il tipo di sconto che associato alle informazioni del cliente
            return new Wizard.SceltaScontiState.StateData { ScontoPersonale = this.m_scontoScelto, CustomRequested = this.m_customRequested };
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
