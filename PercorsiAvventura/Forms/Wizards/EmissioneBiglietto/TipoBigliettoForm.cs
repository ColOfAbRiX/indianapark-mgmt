using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IndianaPark.Tools.Navigation;
using System.Linq;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// Finestra di dialogo per la selezione della tipologia di attrazione
    /// </summary>
    public partial class TipoBigliettoForm : WizardForm
    {
		#region Fields 

        private Model.TipoBiglietto m_choosen;
        private readonly Dictionary<string, Model.TipoBiglietto> m_choises;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Gestisce il caricamento della form
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FormLoadHandler( object sender, EventArgs e )
        {
            if( (bool)PluginPercorsi.GetGlobalParameter( "AutoForward" ).Value )
            {
                // Se c'è solo un elemento abilito l'avanti automatico
                if( this.m_choises.Count == 1 )
                {
                    this.m_choosen = this.m_choises.Values.ToArray()[0];
                    this.Sail( NavigationAction.Next );
                    return;
                }
            }
        }

        /// <summary>
        /// Gestisce il click sul pulsante Indietro
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BackClickHandler(object sender, EventArgs e)
        {
            this.Sail( NavigationAction.Back );
        }

        /// <summary>
        /// Gestisce il click sul pulsante Esci
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EscapeClickHandler(object sender, EventArgs e)
        {
            this.Sail( NavigationAction.Cancel );
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
                this.m_buttons.Size += new System.Drawing.Size( 18, 0 );
                this.m_buttons.Location -= new System.Drawing.Size( 10, 0 );
                this.Size += new System.Drawing.Size( 18, 0 );
            }
            else
            {
                this.m_buttons.AutoScroll = false;
            }

            this.m_buttons.Resize += this.RepeaterResizeHandler;
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoBigliettoForm"/> class.
        /// </summary>
        /// <param name="listaTipologieBiglietti">The lista tipologie biglietti.</param>
        public TipoBigliettoForm( Dictionary<string, Model.TipoBiglietto> listaTipologieBiglietti )
        {
            if( listaTipologieBiglietti == null )
            {
                throw new ArgumentNullException( "listaTipologieBiglietti" );
            }

            InitializeComponent();
            this.m_choises = listaTipologieBiglietti;

            this.m_buttons.MaximumSize = new System.Drawing.Size( 0, 500 );
            this.m_buttons.Creator = new ChoiseButtonCreator( new System.Drawing.Size( 383, 78 ) );

            // Aggiungo tutti i pulsanti di scelta
            foreach( Model.TipoBiglietto tipo in this.m_choises.Values )
            {
                var btn = (Button)this.m_buttons.Add();

                btn.Name = string.Format( "Button_{0}", tipo.Nome.ToLower().Replace( " ", "_" ) );
                btn.Text = tipo.Nome;

                Model.TipoBiglietto biglietto = tipo;
                btn.Click +=
                    delegate
                    {
                        this.m_choosen = biglietto;
                        this.Sail( NavigationAction.Next );
                    };
            }

            this.Load += FormLoadHandler;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        /// <returns>Il valore gestito dalla form</returns>
        public override object GetData()
        {
            return this.m_choosen;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
