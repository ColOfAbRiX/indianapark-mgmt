using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Controllo utente per il conteggio su/giu.
    /// </summary>
    /// <remarks>
    /// Visualizza un campo testo, un pulsante "+" ed un pulsante "-" con cui l'utente incrementa o decrementa il
    /// conteggio. Il controllo permette anche l'inserimento diretto del valore nel campo testo.
    /// </remarks>
    public partial class UpDownCounter : UserControl
    {
		#region Fields 

		#region Internal Fields 

        private int m_iCounter;
        private int m_iStep = 1;
        private int m_iMax = int.MaxValue;
        private int m_iMin = int.MinValue;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Valore corrente del contatore
        /// </summary>
        [ Category( "Counter" ),
          Description( "Valore del contatore" ) ]
        public int Counter
        {
            get { return m_iCounter; }
            set
            {
                // Assegnazione con controllo dei limiti
                this.m_iCounter = Math.Max( this.m_iMin, value );
                this.m_iCounter = Math.Min( this.m_iMax, this.m_iCounter );

                this.OnChange();

                // Aggiornamento della GUI
                this.m_countInput.Text = this.m_iCounter.ToString();
            }
        }

        /// <summary>
        /// Valore massimo che può assumere il contatore
        /// </summary>
        /// <remarks>
        /// Il contatore si fermerà a questo valore se si usano i pulsanti, o viene impostato a questo valore se il
        /// numero è inserito testualmente.
        /// </remarks>
        [ Category( "Counter" ),
          Description( "Valore massimo che può assumere il contatore" ) ]
        public int Max
        {
            get { return this.m_iMax; }
            set { this.m_iMax = value; }
        }

        /// <summary>
        /// Valore minimo che può assumere il contatore, anche negativo
        /// </summary>
        /// <remarks>
        /// Il contatore si fermerà a questo valore se si usano i pulsanti, o viene impostato a questo valore se il
        /// numero è inserito testualmente.
        /// </remarks>
        [Category( "Counter" ),
          Description( "Valore minimo che può assumere il contatore, anche negativo" ) ]
        public int Min
        {
            get { return this.m_iMin; }
            set { this.m_iMin = value; }
        }

        /// <summary>
        /// Valore assoluto dell'incremento/decremento
        /// </summary>
        /// <remarks>
        /// Uno step maggiore di 1 non può in ogni caso eccedere i limiti impostati con <see cref="Max"/> exception 
        /// <see cref="Min"/>
        /// </remarks>
        [ Category( "Counter" ),
          Description( "Valore assoluto dell'incremento/decremento" )]
        public uint Step
        {
            get { return (uint)this.m_iStep; }
            set { this.m_iStep = (int)Math.Abs(value); }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Accade quando il conteggio cambia
        /// </summary>
        public event EventHandler Change;

		#region Raisers 

        /// <summary>
        /// Utilizzata per sollevare l'evento Change
        /// </summary>
        protected void OnChange()
        {
            if( this.Change != null )
            {
                this.Change( this, new EventArgs() );
            }
        }

		#endregion Raisers 

		#region Event Handlers 

        /// <summary>
        /// Decremento del valore del contatore
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SubtractClickHandler(object sender, EventArgs e)
        {
            this.StepDown();
        }

        /// <summary>
        /// Incremento del valore del contatore
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AddClickHandler(object sender, EventArgs e)
        {
            this.StepUp();
        }

        /// <summary>
        /// Modifica manuale del valore che richiede il controllo del formato di input
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CountClickHandler(object sender, EventArgs e)
        {
            int newCounter;

            // In caso di problemi di formato rimetto il valore precedente
            try
            {
                newCounter = Convert.ToInt16(this.m_countInput.Text);
            }
#pragma warning disable
            catch( FormatException fe )
            {
                newCounter = this.m_iCounter;
            }
#pragma warning restore

            this.Counter = newCounter;
        }

        /// <summary>
        /// Tempo di attesa prima di attivare la ripetizione dell'azione. Paragonabile al delay iniziale prima
        /// ripetere un carattere tenendo premuto un tasto
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DelayTickHandler(object sender, EventArgs e)
        {
            // Faccio partire la ripetizione
            this.tmrRepeater.Enabled = true;
            // Fermo il delay iniziale
            this.tmrDelay.Enabled = false;
        }

        /// <summary>
        /// Ad ogni tick ripete l'azione su cui l'utente ha il mouse premuto
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RepeaterTickHandler(object sender, EventArgs e)
        {
            if (this.m_addButton.Focused)
            {
                // Chiamo ripetutamente l'azione di aggiungi
                this.AddClickHandler(this, new EventArgs());
            }
            else if (this.m_subtractButton.Focused)
            {
                // Chiamo ripetutamente l'azione di sottrai
                this.SubtractClickHandler(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gestisce gli eventi in cui si comincia a tenere premuto un pulsante
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            this.tmrDelay.Enabled = true;
        }

        /// <summary>
        /// Gestisce gli eventi in cui si finisce di tenere premuto un pulsante
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            this.tmrDelay.Enabled = false;
            this.tmrRepeater.Enabled = false;
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Costruttore. Inizializza il conteggio a zero
        /// </summary>
        public UpDownCounter() : this( 0 )
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="start">Valore di partenza del contatore.</param>
        public UpDownCounter( int start )
        {
            InitializeComponent();
            this.Counter = start;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Incrementa il valore del contatore
        /// </summary>
        /// <returns>Il nuovo valore, che può non essere necessariamente quello aspettato, per rispetto degli estremi</returns>
        public int StepUp()
        {
            this.Counter += this.m_iStep;
            return this.Counter;
        }

        /// <summary>
        /// Decrementa il valore del contatore
        /// </summary>
        /// <returns>Il nuovo valore, che può non essere necessariamente quello aspettato, per rispetto degli estremi</returns>
        public int StepDown()
        {
            this.Counter -= this.m_iStep;
            return this.Counter;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
