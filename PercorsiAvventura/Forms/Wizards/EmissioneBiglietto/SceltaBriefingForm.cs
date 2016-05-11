using System;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// Finestra di dialogo per la selezione del briefing di una tipologia di utente
    /// </summary>
    public partial class SceltaBriefingForm : WizardForm
    {
		#region Fields 

        private Model.IBriefing m_briefingScelto;
        private readonly Wizard.ClientiPartial m_infoClienti;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Gestisce la scelta o l'annullamento di un briefing da parte dell'utente
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BriefingSelectionChangedHandler( object sender, System.EventArgs e )
        {
            var briefing = (Pannelli.BriefingSelectRow)sender;

            if( briefing.Choosen )
            {
                int rimasti = briefing.Occupa( (uint)this.m_infoClienti.Quantita );

                if( rimasti < 0 )
                {
                    var result = System.Windows.Forms.MessageBox.Show(
                        Properties.Resources.PostiInsufficientiText,
                        Properties.Resources.PostiInsufficientiTitle,
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Warning,
                        System.Windows.Forms.MessageBoxDefaultButton.Button2 );

                    if( result == System.Windows.Forms.DialogResult.No )
                    {
                        briefing.Choosen = false;
                        return;
                    }
                }

                // Azzero tutti tranne quello corrente
                foreach( Pannelli.BriefingSelectRow b in this.m_repeater )
                {
                    if( b != sender )
                    {
                        b.Choosen = false;
                    }
                    else
                    {
                        this.m_briefingScelto = this.m_infoClienti.TipoCliente.TipiBriefing.TrovaBriefing( b.OrarioBriefing );
                    }
                }
            }
            else
            {
                briefing.Libera( (uint)this.m_infoClienti.Quantita );
                this.m_briefingScelto = null;
            }
        }

        /// <summary>
        /// Gestisce le richieste del controllo di navigazione della form
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void NavigationHandler( object source, IndianaPark.Tools.Navigation.NavigationEventArgs e )
        {
            // Controllo che l'operatore abbia scelto un briefing
            if( e.Status == IndianaPark.Tools.Navigation.NavigationAction.Next && this.m_briefingScelto == null )
            {
                System.Windows.Forms.MessageBox.Show(
                    Properties.Resources.BriefingNonSelezionatoText,
                    Properties.Resources.BriefingNonSelezionatoTitle,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning,
                    System.Windows.Forms.MessageBoxDefaultButton.Button2 );

                return;
            }

            this.Sail( e.Status );
        }

        /// <summary>
        /// Gestisce le dimensioni del controllo ripetitore per aggiungere le barre di scorrimento laterali
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RepeaterResizeHandler( object sender, System.EventArgs e )
        {
            this.m_repeater.Resize -= this.RepeaterResizeHandler;

            if( this.m_repeater.Size.Height == this.m_repeater.MaximumSize.Height )
            {
                this.m_repeater.AutoScroll = true;
                this.m_repeater.Size += new System.Drawing.Size( 18, 0 );
                this.m_repeater.Location -= new System.Drawing.Size( 10, 0 );
            }
            else
            {
                this.m_repeater.AutoScroll = false;
            }

            this.m_repeater.Resize += this.RepeaterResizeHandler;
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SceltaBriefingForm"/> class.
        /// </summary>
        /// <param name="infoCliente">The info cliente.</param>
        public SceltaBriefingForm( Wizard.ClientiPartial infoCliente )
        {
            if( infoCliente == null )
            {
                throw new ArgumentNullException( "infoCliente" );
            }

            InitializeComponent();

            this.m_infoClienti = infoCliente;
            var tipoBriefing = infoCliente.TipoCliente.TipiBriefing;

            // Testo visualizzato
            var firstBriefing = tipoBriefing.TrovaBriefing( DateTime.Now );
            this.m_labelTitolo.Text = string.Format( Properties.Resources.ChooseBriefingFor, m_infoClienti.TipoCliente.Nome );
            if( firstBriefing == null )
            {
                // Non ci sono briefing in orario!
                this.m_labelTitolo.Text = string.Format( Properties.Resources.NoBriefingAvailable, m_infoClienti.TipoCliente.Nome );
                return;
            }

            this.m_repeater.Creator = new Forms.BriefingInputCreator( tipoBriefing.PostiTotali );

            // Aggiungo un elemento per ogni briefing disponibile
            foreach( Model.IBriefing b in tipoBriefing )
            {
                // Aggiungo solo i briefing successivi ad adesso
                if( b.Inizio >= DateTime.Now + tipoBriefing.WalkTime )
                {
                    ((BriefingInputCreator)m_repeater.Creator).PostiOccupati = b.PostiOccupati;
                    var control = (Pannelli.BriefingSelectRow)this.m_repeater.Add();
                    control.ChoosenChanged += this.BriefingSelectionChangedHandler;
                    control.OrarioBriefing = b.Inizio;
                    if( m_infoClienti.Briefing != null && m_infoClienti.Briefing.Equals( b ) )
                    {
                        control.Choosen = true;
                        this.m_briefingScelto = b;
                    }
                }
            }
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        /// <returns></returns>
        public override object GetData()
        {
            return this.m_briefingScelto;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}