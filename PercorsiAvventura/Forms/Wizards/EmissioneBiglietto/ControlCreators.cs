using System;
using System.Drawing;
using IndianaPark.Tools.Controls;
using System.Windows.Forms;

namespace IndianaPark.PercorsiAvventura.Forms
{
    /// <summary>
    /// E' il costruttore di un pulsante di selezione gia formattato.
    /// </summary>
    internal class ChoiseButtonCreator : ControlRepeater.ControlCreatorBase
    {
		#region Fields 

        private readonly Size m_size;
        private int m_tabIndex;

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiseButtonCreator"/> class.
        /// </summary>
        /// <param name="buttonSize">La dimensione del pulsante</param>
        public ChoiseButtonCreator( Size buttonSize )
        {
            this.m_size = buttonSize;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Crea un nuovo controllo.
        /// </summary>
        /// <returns>Il nuovo controllo.</returns>
        public override Control CreateNewControl()
        {
            var newButton = new Button
            {
                UseVisualStyleBackColor = true,
                Size = this.m_size,
                Font = new Font( new FontFamily( "Tahoma" ), (float)15.75, FontStyle.Bold ),
                TabIndex = this.m_tabIndex++
            };

            return newButton;
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// E' il costruttore di una riga per la selezione del briefing
    /// </summary>
    internal class BriefingInputCreator : ControlRepeater.ControlCreatorBase
    {
		#region Fields 

        private readonly uint m_maxPosti;
        private int m_tabIndex;

        public uint PostiOccupati { private get; set; }

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="BriefingInputCreator"/> class.
        /// </summary>
        /// <param name="maxPosti">The max posti.</param>
        public BriefingInputCreator( uint maxPosti )
        {
            this.m_maxPosti = maxPosti;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Crea un nuovo controllo.
        /// </summary>
        /// <returns>Il nuovo controllo.</returns>
        public override Control CreateNewControl()
        {
            return new Pannelli.BriefingSelectRow( this.m_maxPosti - this.PostiOccupati ) { TabIndex = this.m_tabIndex++ };
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Crea le etichette per visualizzare il numero di clienti per categoria
    /// </summary>
    internal class BigliettiLabelCreator : ControlRepeater.ControlCreatorBase
    {
        #region Fields

        #region Internal Fields

        private readonly string m_format;

        #endregion Internal Fields

        #region Public Fields

        /// <summary>
        /// Il prezzo del biglietto
        /// </summary>
        public decimal Prezzo { private get; set; }

        /// <summary>
        /// Il numero di clienti
        /// </summary>
        public int Quantita { private get; set; }

        /// <summary>
        /// Il testo identificativo dello sconto.
        /// </summary>
        public Model.ISconto Sconto { private get; set; }

        /// <summary>
        /// Il testo identificativo del tipo cliente
        /// </summary>
        public Model.TipoCliente TipoCliente { private get; set; }

        #endregion Public Fields

        #endregion Fields

        #region Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BigliettiLabelCreator"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        public BigliettiLabelCreator( string format )
        {
            this.m_format = format;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Crea un nuovo controllo.
        /// </summary>
        /// <returns>Il nuovo controllo.</returns>
        public override Control CreateNewControl()
        {
            var output = new Label();
            string sconto = "";

            if( this.TipoCliente == null )
            {
                throw new ArgumentNullException();
            }
            if( this.Sconto != null )
            {
                sconto = String.Format( Properties.Resources.RiassuntoBigliettiSconto, this.Sconto.Nome );
            }

            output.AutoSize = true;
            output.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0 );
            output.Margin = new System.Windows.Forms.Padding( 2 );
            output.Name = "m_riassuntoBiglietti";
            output.Size = new System.Drawing.Size( 192, 16 );
            output.Text = String.Format( this.m_format, this.TipoCliente.Nome, sconto, this.Quantita, this.Prezzo );
            output.Visible = true;

            return output;
        }

        #endregion Public Methods

        #endregion Methods
    }

    /// <summary>
    /// Crea le etichette per visualizzare l'ora di inizio dei briefing
    /// </summary>
    internal class BriefingLabelCreator : ControlRepeater.ControlCreatorBase
    {
        #region Fields

        #region Internal Fields

        private readonly string m_format;

        #endregion Internal Fields

        #region Public Fields

        /// <summary>
        /// Il numero di clienti
        /// </summary>
        public Model.IBriefing Briefing { private get; set; }

        /// <summary>
        /// Il testo identificativo del tipo cliente
        /// </summary>
        public Model.TipoCliente TipoCliente { private get; set; }

        #endregion Public Fields

        #endregion Fields

        #region Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BriefingLabelCreator"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        public BriefingLabelCreator( string format )
        {
            this.m_format = format;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Crea un nuovo controllo.
        /// </summary>
        /// <returns>Il nuovo controllo.</returns>
        public override Control CreateNewControl()
        {
            var output = new Label();

            if( this.TipoCliente == null )
            {
                throw new ArgumentNullException();
            }
            // Non tutte le categorie cliente hanno dei briefings
            if( this.Briefing == null )
            {
                return null;
            }

            output.AutoSize = true;
            output.Font = new System.Drawing.Font( "Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0 );
            output.Margin = new System.Windows.Forms.Padding( 2 );
            output.Name = "m_riassuntoBriefings";
            output.Size = new System.Drawing.Size( 192, 16 );
            output.Text = String.Format( this.m_format, this.TipoCliente.Nome, this.Briefing.Inizio.ToShortTimeString() );
            output.Visible = true;

            return output;
        }

        #endregion Public Methods

        #endregion Methods
    }
}
