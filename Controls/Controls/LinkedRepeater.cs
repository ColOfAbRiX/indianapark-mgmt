using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Oggetto <see cref="ControlRepeater"/> che permette di associare un oggetto ad ogni elemento creato
    /// </summary>
    /// <remarks>
    /// Ad ogni controllo 
    /// </remarks>
    public partial class LinkedRepeater<TLink> : ControlRepeater
    {
		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Gets the <see cref="System.Windows.Forms.Control"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Control this[TLink index]
        {
            get
            {
                var controls = this.Controls.Cast<Control>();
                return controls.FirstOrDefault( item => item.Tag.Equals( index ) );
            }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedRepeater{TLink}"/> class.
        /// </summary>
        public LinkedRepeater()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedRepeater{TLink}"></see> class.
        /// </summary>
        /// <param name="creator">L'oggetto utilizzato per creare i <see cref="Control"></see> da aggiungere.</param>
        public LinkedRepeater( ControlCreatorBase creator ) : this()
        {
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Aggiunge un controllo alla collezione e lo collega ad un oggetto
        /// </summary>
        /// <param name="obj">L'oggetto a cui collegare il controllo</param>
        /// <returns>Un nuovo controllo o il controllo gia associato con l'oggetto <paramref name="obj"/></returns>
        public Control Add( TLink obj )
        {
            if( !this.Contains( obj ) )
            {
                var newControl = base.Add();
                newControl.Tag = obj;
                return newControl;
            }

            return this[obj];
        }

        /// <summary>
        /// Rimuove il controllo associato con un oggetto
        /// </summary>
        /// <param name="obj">L'oggetto a cui è associato il controllo da rimuovere</param>
        /// <returns>Il controllo che è appena stato rimosso</returns>
        public Control Remove( TLink obj )
        {
            var remove = this[obj];
            
            if( remove != null )
            {
                base.Remove( remove );
            }

            return remove;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="obj">L'oggetto associato al controllo di cui si vuole sapere la posizione</param>
        /// <returns>
        /// The index of <paramref name="obj"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf( TLink obj )
        {
            return base.IndexOf( this[obj] );
        }

        /// <summary>
        /// Determines whether [contains] [the specified obj].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified obj]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains( TLink obj )
        {
            var controls = this.Controls.Cast<Control>();
            return controls.Any( item => item.Tag.Equals( obj ) );
        }

        /// <summary>
        /// Restituisce un enumeratore che scorre l'insieme dei link
        /// </summary>
        /// <remarks>
        /// L'enumeratore scorre solamente l'insieme dei link, exception non gli item che non hanno link ad oggetti
        /// </remarks>
        /// <returns>Un enumeratore che scorre l'insieme dei link.</returns>
        public Dictionary<TLink, Control>.Enumerator GetLinkEnumerator()
        {
            var output = new Dictionary<TLink, Control>();

            foreach( Control control in this.Controls )
            {
                if( control != null )
                {
                    output.Add( (TLink)control.Tag, control );
                }
            }

            return output.GetEnumerator();
        }
        
        #endregion Public Methods 

		#endregion Methods 
    }
}
