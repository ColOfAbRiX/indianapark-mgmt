using System;
using System.Windows.Forms;
using IndianaPark.Biglietti.Wizard.New;
using System.Collections.Generic;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard.New;

namespace IndianaPark.Biglietti.Forms.New
{
    /// <summary>
    /// Finestra di dialogo per la selezione della tipologia di attrazione
    /// </summary>
    public partial class TipoAttrazioneForm : WizardForm
    {
		#region Fields 

        private int m_selection = -1;
        private readonly List<TipoAttrazioneState.AttrazioneData> m_listaAttrazioni;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Gestisce il click sul pulsante Esci
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EscapeClickHandler(object sender, EventArgs e)
        {
            this.m_selection = -1;
            this.RequireNavigationAction( NavigationAction.Cancel );
        }

        /// <summary>
        /// Gestisce le dimensioni del controllo ripetitore per aggiungere le barre di scorrimento laterali
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RepeaterResizeHandler( object sender, EventArgs e )
        {
            // Per ridimensionare disabilito il gestore
            this.m_buttons.Resize -= this.RepeaterResizeHandler;

            if( this.m_buttons.Size.Height == this.m_buttons.MaximumSize.Height )
            {
                // Se raggiungo l'altezza massima abilito le barre di scorrimento
                this.m_buttons.AutoScroll = true;
                this.m_buttons.Size += new System.Drawing.Size( 18, 0 );
                this.m_buttons.Location -= new System.Drawing.Size( 10, 0 );
                this.Size += new System.Drawing.Size( 18, 0 );
            }
            else
            {
                // Se sono sotto l'altezza massima disabilito le barre di scorrimento
                this.m_buttons.AutoScroll = false;
            }

            // Riabilito il gestore
            this.m_buttons.Resize += this.RepeaterResizeHandler;
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoAttrazioneForm"/> class.
        /// </summary>
        /// <param name="listaAttrazioni">La lista delle attrazioni da proporre</param>
        public TipoAttrazioneForm( IList<TipoAttrazioneState.AttrazioneData> listaAttrazioni )
        {
            InitializeComponent();

            // Inizializzo gli oggetti
            this.m_listaAttrazioni = (List<TipoAttrazioneState.AttrazioneData>)listaAttrazioni;
            this.m_buttons.Creator = new ChoiseButtonCreator( new System.Drawing.Size( 383, 78 ) );
            this.m_buttons.MaximumSize = new System.Drawing.Size( 0, 500 );

            // Creo la lista dei pulsanti per ogni attrazione
            for( int i = 0; i < this.m_listaAttrazioni.Count; i++ )
            {
                int selection = i;
                var btn = (Button)this.m_buttons.Add();

                btn.Name = String.Format( "Button_{0}", selection );
                btn.Text = this.m_listaAttrazioni[i].Nome;

                // Imposto cosa succede quando l'utente clicca sul pulsante di un'attrazione
                btn.Click +=
                    delegate
                    {
                        this.m_selection = selection;
                        this.RequireNavigationAction( NavigationAction.Next );
                    };
            }
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        /// <returns>Il valore gestito dalla form</returns>
        public override object GetData()
        {
            return this.m_listaAttrazioni[this.m_selection];
        }

        #endregion Public Methods 

        #endregion Methods 
    }
}
