using System;
using System.Windows.Forms;

namespace IndianaPark.PercorsiAvventura.Pannelli
{
    /// <summary>
    /// Pannello per l'inserimento delle quantità per le tipologie dei clienti
    /// </summary>
    public partial class TipoClienteRow : UserControl
    {
		#region Fields 

		#region Internal Fields 

        private Model.ISconto m_scontoAssociato;
        private Model.TipoCliente m_tipoAssociato;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Quantità associata alla tipologia cliente
        /// </summary>
        public int Count
        {
            get { return this.udcContatore.Counter; }
            set
            {
                this.udcContatore.Counter = value;
            }
        }

        /// <summary>
        /// Il <see cref="Model.ISconto"/> associato con la riga di input.
        /// </summary>
        /// <remarks>
        /// Se alla riga di input è associato uno sconto viene visualizzato un pulsnte per eliminare lo sconto
        /// </remarks>
        public Model.ISconto ScontoAssociato
        {
            get { return this.m_scontoAssociato; }
            set
            {
                this.m_scontoAssociato = value;

                if( value != null )
                {
                    this.m_buttonDelete.Visible = true;
                    this.m_buttonSconto.Visible = false;

                    this.lblTipo.Text = string.Format( "{0} ({1})", this.m_tipoAssociato.Nome, this.m_scontoAssociato.Nome );
                }
                else
                {
                    this.m_buttonSconto.Visible = true;
                    this.m_buttonDelete.Visible = false;

                    this.lblTipo.Text = string.Format( "{0}", this.m_tipoAssociato.Nome );
                }
            }
        }

        /// <summary>
        /// Il <see cref="Model.TipoCliente"/> associato con la riga di input
        /// </summary>
        public Model.TipoCliente TipoAssociato
        {
            get { return this.m_tipoAssociato; }
            set
            {
                this.m_tipoAssociato = value;

                if( this.ScontoAssociato != null )
                {
                    this.lblTipo.Text = string.Format( "{0} ({1})", this.m_tipoAssociato.Nome, this.m_scontoAssociato.Nome );
                }
                else
                {
                    this.lblTipo.Text = string.Format( "{0}", this.m_tipoAssociato.Nome );
                }
            }
        }

        /// <summary>
        /// Identificativo dell'elemento associato con questa riga
        /// </summary>
        public int Id { get; set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Evento associato alla richiesta di aggiunta di un cliente scontato
        /// </summary>
        public event EventHandler AddScontoClick;

        /// <summary>
        /// Evento associato alla richiesta di eliminazione di un cliente scontato
        /// </summary>
        public event EventHandler RemoveScontoClick;

        /// <summary>
        /// Indica che è stato modificato il valore del contatore
        /// </summary>
        public event EventHandler CounterChange;

		#region Raisers 

        /// <summary>
        /// Avvia l'evento <see cref="TipoClienteRow.AddScontoClick"/>
        /// </summary>
        protected void OnAddScontoClick()
        {
            if( this.AddScontoClick != null )
            {
                this.AddScontoClick( this, new EventArgs() );
            }
        }

        /// <summary>
        /// Avvia l'evento <see cref="TipoClienteRow.RemoveScontoClick"/>
        /// </summary>
        protected void OnRemoveScontoClick()
        {
            if( this.RemoveScontoClick != null )
            {
                this.RemoveScontoClick( this, new EventArgs() );
            }
        }

        /// <summary>
        /// Avvia l'evento <see cref="TipoClienteRow.CounterChange"/>
        /// </summary>
        protected void OnCounterChange()
        {
            if( this.CounterChange != null )
            {
                this.CounterChange( this, new EventArgs() );
            }
        }

        #endregion Raisers 

		#region Event Handlers 

        private void AddScontoClickHandler( object sender, EventArgs e )
        {
            this.OnAddScontoClick();
        }

        private void RemoveScontoClickHandler( object sender, EventArgs e )
        {
            this.OnRemoveScontoClick();
        }

        private void CounterChangeHandler( object sender, EventArgs e )
        {
            this.OnCounterChange();

            // In caso di zero avviso che forse si vuole l'eliminazione del record
            if( this.udcContatore.Counter == 0 && m_scontoAssociato != null )
            {
                this.OnRemoveScontoClick();
            }
        }

        #endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TipoClienteRow"/> class.
        /// </summary>
        public TipoClienteRow()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#endregion Methods 
    }
}
