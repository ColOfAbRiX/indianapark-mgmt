using System;
using System.Windows.Forms;

namespace IndianaPark.PercorsiAvventura.Pannelli
{
    /// <summary>
    /// Utilizzato per permettere all'utente di seleziona un briefing e di occuparne i posti
    /// </summary>
    public partial class BriefingSelectRow : UserControl
    {
		#region Fields 

		#region Internal Fields 

        private readonly uint m_maxPosti;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BriefingSelectRow"/> is choosen.
        /// </summary>
        public bool Choosen
        {
            get
            {
                return this.m_selected.Checked;
            }

            set
            {
                this.m_selected.Checked = value;
            }
        }

        /// <summary>
        /// Imposta l'ora del briefing a cui questo controllo fa riferimento
        /// </summary>
        public DateTime OrarioBriefing
        {
            get
            {
                return DateTime.Parse( this.m_orario.Text );
            }

            set
            {
                this.m_orario.Text = value.ToLongTimeString();
            }
        }

        /// <summary>
        /// Posti disponibili
        /// </summary>
        /// <value>The posti.</value>
        public int Posti
        {
            get
            {
                return int.Parse( this.m_posti.Text );
            }
            private set
            {
                this.m_posti.Text = value.ToString();
            }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Avviene quando viene selezionato questo briefing
        /// </summary>
        public event EventHandler ChoosenChanged;

		#region Event Handlers 

        private void CheckedChangedHandler( object sender, EventArgs e )
        {
            // Applico un'immagine se il controllo è selezionato
            if( this.m_selected.Checked )
            {
                this.m_selected.Image = Properties.Resources.check;
            }
            else
            {
                this.m_selected.Image = null;
            }

            // Avvio l'evento
            if( this.ChoosenChanged != null )
            {
                this.ChoosenChanged( this, new EventArgs() );
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="BriefingSelectRow"/> class.
        /// </summary>
        /// <param name="maxPosti">The max posti.</param>
        public BriefingSelectRow( uint maxPosti )
        {
            InitializeComponent();
            this.m_maxPosti = maxPosti;
            this.Posti = (int)maxPosti;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Occupa i posti specificati
        /// </summary>
        /// <param name="posti">Il numero di posti da occupare.</param>
        /// <returns>Il numero di posti dispinibili rimasti</returns>
        /// <remarks>
        /// La funzione occupa i posti anche se questi divengono negativi. E' compito del chiamante verificare se questo è permesso
        /// </remarks>
        public int Occupa( uint posti )
        {
            this.Posti -= (int)posti;
            return this.Posti;
        }

        /// <summary>
        /// Libera i posti specificati
        /// </summary>
        /// <param name="posti">Il numero di posti da liberare</param>
        /// <returns>Il numero di posti disponibili rimasti</returns>
        /// <remarks>Il numero massimo di posti liberi è determinato alla creazione del controllo</remarks>
        public int Libera( uint posti )
        {
            this.Posti += (int)posti;
            this.Posti = (int)Math.Min( this.Posti, this.m_maxPosti );
            return this.Posti;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
