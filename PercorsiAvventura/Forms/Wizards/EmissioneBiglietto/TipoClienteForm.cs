using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;
using IndianaPark.PercorsiAvventura.Pannelli;
using IndianaPark.PercorsiAvventura.Wizard;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// Finestra di dialogo per indicare le quatità delle varie tipologie cliente e l'aggiunta di eventuali sconti.
    /// </summary>
    public partial class TipoClienteForm : WizardForm
    {
		#region Fields 

		#region Internal Fields 

        private int m_clienteSelezionato;
        private List<ClientiPartial> m_listaTipologie;
        private TipoClienteState.TipoClienteAction m_action;
        /// <summary>
        /// Imposta il prezzo parziale visualizzato come riferimento per l'utente
        /// </summary>
        private decimal PrezzoParziale
        {
            set
            {
                this.m_labelPrice.Text = String.Format( Properties.Resources.TipoClientePartialPrice, value );
            }
        }

		#endregion Internal Fields 

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Gestisce le richieste del controllo di navigazione della form
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        private void NavigateHandler( object source, NavigationEventArgs e )
        {
            switch( e.Status )
            {
                case NavigationAction.Next:
                    if( this.CheckSum() == 0 )
                    {
                        System.Windows.Forms.MessageBox.Show(
                            Properties.Resources.NoClienteAddedText,
                            Properties.Resources.NoClienteAddedTitle,
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning,
                            System.Windows.Forms.MessageBoxDefaultButton.Button2 );
                        return;
                    }
                    this.m_action = TipoClienteState.TipoClienteAction.Avanti;
                    this.Sail( e.Status );
                    break;

                case NavigationAction.Cancel:
                case NavigationAction.Back:
                    // Passa in modo trasparente l'evento al livello superiore
                    this.m_action = TipoClienteState.TipoClienteAction.Avanti;
                    this.Sail( e.Status );
                    break;

                case NavigationAction.Finish:
                case NavigationAction.Nothing:
                    // Non avvengono mai in questa form
                    break;
            }
        }

        /// <summary>
        /// Gestore della richiesta di aggiunta di uno sconto
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddScontoHandler( object source, EventArgs e )
        {
            this.m_action = TipoClienteState.TipoClienteAction.Sconta;
            this.m_clienteSelezionato = ((Pannelli.TipoClienteRow)source).Id;
            this.Sail( NavigationAction.Next );
        }

        /// <summary>
        /// Gestore della richiesta di rimozione di uno sconto
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RemoveScontoHandler( object source, EventArgs e )
        {
            this.m_action = TipoClienteState.TipoClienteAction.Elimina;
            this.m_clienteSelezionato = ((Pannelli.TipoClienteRow)source).Id;
            this.Sail( NavigationAction.Next );
        }

        /// <summary>
        /// Gestisce le dimensioni del controllo ripetitore per aggiungere le barre di scorrimento laterali
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RepeaterResizeHandler( object sender, EventArgs e )
        {
            this.m_repeaterTipoClienti.Resize -= this.RepeaterResizeHandler;

            if( this.m_repeaterTipoClienti.Size.Height == this.m_repeaterTipoClienti.MaximumSize.Height )
            {
                this.m_repeaterTipoClienti.AutoScroll = true;
                this.m_repeaterTipoClienti.Size += new System.Drawing.Size( 18, 0 );
                this.m_repeaterTipoClienti.Location -= new System.Drawing.Size( 10, 0 );
                this.Size += new System.Drawing.Size( 18, 0 );
            }
            else
            {
                this.m_repeaterTipoClienti.AutoScroll = false;
            }

            this.m_repeaterTipoClienti.Resize += this.RepeaterResizeHandler;
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoClienteForm"/> class.
        /// </summary>
        public TipoClienteForm( IList<ClientiPartial> listaTipologie )
        {
            if( listaTipologie == null )
            {
                throw new ArgumentNullException( "listaTipologie" );
            }

            InitializeComponent();

            this.m_listaTipologie = (List<ClientiPartial>)listaTipologie;
            this.UpdateGfxList( this.m_listaTipologie );
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Trasformo le informazioni sulla GUI in informazioni globali formattate
        /// </summary>
        private void UpdateData()
        {
            foreach( Pannelli.TipoClienteRow tci in this.m_repeaterTipoClienti )
            {
                TipoClienteRow row = tci;

                // Aggiorno direttamente la struttura memorizzata
                var cliente = m_listaTipologie.First(
                    item => item.TipoCliente == row.TipoAssociato && item.ScontoPersonale == row.ScontoAssociato );

                cliente.TipoCliente = tci.TipoAssociato;
                cliente.ScontoPersonale = tci.ScontoAssociato;
                cliente.Quantita = tci.Count;
            }
        }

        /// <summary>
        /// Esegue la somma di tutti i clienti aggiunti
        /// </summary>
        private int CheckSum()
        {
            this.UpdateData();

            int sum = 0;
            foreach( var t in this.m_listaTipologie )
            {
                sum += t.Quantita;
            }

            return sum;
        }

        /// <summary>
        /// Aggiorna le righe per la selezione delle varie quantità, in base alla lista fornita
        /// </summary>
        /// <param name="listaTipologie">La lista delle tipologie.</param>
        private void UpdateGfxList( IList<ClientiPartial> listaTipologie )
        {
            if( listaTipologie == null )
            {
                throw new ArgumentNullException( "listaTipologie" );
            }
            
            this.SuspendLayout();

            this.m_repeaterTipoClienti.Clear();
            this.m_repeaterTipoClienti.Creator = new Tools.Controls.TypeControlCreator( typeof(Pannelli.TipoClienteRow) );
            this.m_repeaterTipoClienti.Resize += RepeaterResizeHandler;

            // Creo la lista delle righe di input a partire dalle informazioni disponibili sui clienti
            foreach( var tipo in m_listaTipologie )
            {
                var tcc = (Pannelli.TipoClienteRow)this.m_repeaterTipoClienti.Add();

                tcc.TipoAssociato = tipo.TipoCliente;
                tcc.ScontoAssociato = tipo.ScontoPersonale;
                tcc.Count = tipo.Quantita;
                tcc.Id = listaTipologie.IndexOf( tipo );

                tcc.AddScontoClick += this.AddScontoHandler;
                tcc.RemoveScontoClick += this.RemoveScontoHandler;
                tcc.CounterChange += delegate { this.OnUpdateDataRequest(); };
            }

            this.ResumeLayout();
            this.Refresh();
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Recupera l'input, solitamente dell'utente
        /// </summary>
        /// <returns>Il valore gestito dalla form</returns>
        public override object GetData()
        {
            object output;

            this.UpdateData();

            switch( this.m_action )
            {
                case TipoClienteState.TipoClienteAction.Sconta:
                    // Struttura dati per le informazioni sull'aggiunta di sconto
                    output = new TipoClienteState.ActionScontaData
                        {
                            Action = TipoClienteState.TipoClienteAction.Sconta,
                            InfoClienti = m_listaTipologie,
                            SelectedCliente = m_clienteSelezionato
                        };
                    break;

                case TipoClienteState.TipoClienteAction.Elimina:
                    // Struttura dati per le informazioni sull'eliminazione di sconto
                    output = new TipoClienteState.ActionScontaData
                        {
                            Action = TipoClienteState.TipoClienteAction.Elimina,
                            InfoClienti = m_listaTipologie,
                            SelectedCliente = m_clienteSelezionato
                        };
                    break;

                case TipoClienteState.TipoClienteAction.Avanti:
                    // Imposto l'operazione di avanti
                    output = new TipoClienteState.StateData
                    {
                        Action = TipoClienteState.TipoClienteAction.Avanti,
                        InfoClienti = m_listaTipologie
                    };
                    break;

                default:
                    // Restituisco semplicemente i dati
                    output = new TipoClienteState.StateData
                        {
                            Action = TipoClienteState.TipoClienteAction.Recupera,
                            InfoClienti = m_listaTipologie
                        };
                    break;
            }

            this.m_action = TipoClienteState.TipoClienteAction.Recupera;
            return output;
        }

        /// <summary>
        /// Aggiorna le righe di selezione ed il prezzo finale in base alla lista fornita.
        /// </summary>
        /// <param name="info">Le informazioni per la form, istanza di <see cref="Model.Inserimento"/></param>
        public override void SetInformations( object info )
        {
            if( info != null )
            {
                var inserimento = (Model.Inserimento)info;
                
                // Creo la lista di informazioni utilizzando l'Inserimento specificato
                var lista = ClientiPartial.SynthesyzeClienti( inserimento );
                lista = ClientiPartial.GetFromParco( inserimento.TipoBiglietto ).Union( lista ).ToList();
                lista = ClientiPartial.CompactData( lista );

                // Aggiorno la grafica exception i dati solo se ci sono delle differenza dalla lista gia in memoria
                if( this.m_listaTipologie.Count != lista.Count )
                {
                    this.m_listaTipologie = lista.ToList();
                    this.UpdateGfxList( this.m_listaTipologie );

                    MessageBox.Show(
                        Properties.Resources.ScontiPlanChangedText,
                        Properties.Resources.ScontiPlanChangedTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Information );
                }

                this.PrezzoParziale = inserimento.CalcolaPrezzo();
            }

            base.SetInformations( info );
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
