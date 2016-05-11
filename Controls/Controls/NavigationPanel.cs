using System;
using System.ComponentModel;
using System.Windows.Forms;
using IndianaPark.Tools.Navigation;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Pannello di navigazione che mostra i pulsanti Avanti, Indietro, Annulla ed Esci
    /// </summary>
    public sealed partial class NavigationPanel : NavigationControlBase, IButtonControl
    {
		#region Fields 

		#region Internal Fields 

        private DialogResult m_dialogResult;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Abilita il pulsante "Esci"
        /// </summary>
        [ Category( "Navigation" ),
          Description( "Abilita il pulsante Esci" )]
        public bool CancelEnabled
        {
            get { return this.btnCancel.Enabled; }
            set { this.btnCancel.Enabled = value; }
        }

        /// <summary>
        /// Ottiene o imposta il valore restituito al form padre quando si sceglie il pulsante.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Windows.Forms.DialogResult"/> values.
        /// </returns>
        public DialogResult DialogResult
        {
            get
            {
                return this.m_dialogResult;
            }
            set
            {
                if( Enum.IsDefined( typeof( DialogResult ), value ) )
                {
                    this.m_dialogResult = value;
                }
            }
        }

        /// <summary>
        /// Abilita il pulsante "Fine", disabilita e rende nascosto il pulsante "Avanti"
        /// </summary>
        [ Category( "Navigation" ),
          Description( "Abilita il pulsante Fine, disabilita exception rende nascosto il pulsante Avanti" ) ]
        public bool FinishEnabled
        {
            get { return this.btnFinish.Enabled; }
            set
            {
                if (value)
                {
                    this.btnNext.Visible = false;
                    this.btnNext.Enabled = false;
                    this.btnFinish.Visible = true;
                }

                this.btnFinish.Enabled = value;
            }
        }

        /// <summary>
        /// Abilita il pulsante "Avanti", disabilita e rende nascosto il pulsante "Fine"
        /// </summary>
        [ Category( "Navigation" ),
          Description( "Abilita il pulsante Avanti, disabilita exception rende nascosto il pulsante Fine" )]
        public bool NextEnabled
        {
            get { return this.btnNext.Enabled; }
            set
            {
                if (value)
                {
                    this.btnNext.Visible = true;
                    this.btnFinish.Enabled = false;
                    this.btnFinish.Visible = false;
                }

                this.btnNext.Enabled = value;
            }
        }

        /// <summary>
        /// Abilita il pulsante "Indietro"
        /// </summary>
        [ Category( "Navigation" ),
          Description( "Abilita il pulsante Indietro" )]
        public bool PreviousEnabled
        {
            get { return this.btnPrevious.Enabled; }
            set { this.btnPrevious.Enabled = value; }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Click sul pulsante Esci
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelClickHandler(object sender, EventArgs e)
        {
            this.Sail( NavigationAction.Cancel );
        }

        /// <summary>
        /// Click sul pulsante Fine
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void FinishClickHandler(object sender, EventArgs e)
        {
            this.Sail( NavigationAction.Finish );
        }

        /// <summary>
        /// Click sul pulsante avanti
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NextClickHandler(object sender, EventArgs e)
        {
            this.Sail( NavigationAction.Next );
        }

        /// <summary>
        /// Click sul pulsante indietro
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PreviousClickHandler(object sender, EventArgs e)
        {
            this.Sail( NavigationAction.Back );
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Costruttore standard
        /// </summary>
        public NavigationPanel()
        {
            InitializeComponent();

            this.FinishEnabled = false;
            this.NextEnabled = true;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Genera un evento Click per il controllo
        /// </summary>
        public void PerformClick()
        {
            this.NextClickHandler( this, new EventArgs() );
        }

        /// <summary>
        /// Notifica a un controllo di essere il pulsante predefinito, in modo che l'aspetto e il
        /// funzionamento del controllo vengano regolati di conseguenza.
        /// </summary>
        public void NotifyDefault( bool value ) { value = !(!(value));  }

        /// <summary>
        /// Utilizzato per abilitare o disabilitare nel controllo le operazioni di navigazione
        /// </summary>
        /// <param name="direction">Indica quale direzione di navigazione associata al controllo abilitare o disabilitare</param>
        /// <param name="status">Se <c>true</c> abilita il relativo controllo, se <c>false</c> lo disabilita</param>
        public override void EnableControl( NavigationAction direction, bool status )
        {
            switch( direction )
            {
                case NavigationAction.Back:
                    this.PreviousEnabled = status;
                    break;

                case NavigationAction.Next:
                    this.NextEnabled = status;
                    break;

                case NavigationAction.Finish:
                    this.FinishEnabled = status;
                    break;

                case NavigationAction.Cancel:
                    this.CancelEnabled = status;
                    break;

                case NavigationAction.Nothing:
                default:
                    break;
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
