using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Controllo ripetitore. Ripete un pannello comandi in un determinato senso per un numero impostabile di volte.
    /// </summary>
    public partial class ControlRepeater : UserControl, ICollection
    {
		#region Nested Classes 

        /// <summary>
        /// Utilizzato per creare un nuovo <see cref="Control"/> da aggiungere al repeater. Il modo in cui la creazione
        /// avviene è lasciata all'implementazione concreta della classe. Impego del pattern Builder.
        /// </summary>
        public abstract class ControlCreatorBase
        {
		    #region Methods 

		    #region Public Methods 

            /// <summary>
            /// Crea un nuovo controllo da aggiungere al <see cref="ControlRepeater"/>
            /// </summary>
            /// <returns>Il nuovo controllo da aggiungere.</returns>
            public abstract Control CreateNewControl();

		    #endregion Public Methods 

		    #endregion Methods 
        }

		#endregion Nested Classes 

		#region Fields 

		#region Public Fields 

        /// <summary>
        /// Gets or sets a value indicating whether the container enables the user to scroll to any controls placed outside of its visible boundaries.
        /// </summary>
        /// <value></value>
        /// <returns>true if the container enables auto-scrolling; otherwise, false. The default value is false.
        /// </returns>
        [Description( "Indica se le barre di scorrimento vengono automaticamente visualizzate se il contenuto del controllo non rientra nell'area visibile." )]
        public new bool AutoScroll
        {
            get { return this.m_FlowPanel.AutoScroll; }
            set { this.m_FlowPanel.AutoScroll = base.AutoScroll = value; }
        }

        /// <summary>
        /// Utilizzare AutoSize per forzare il ridimensionamento in base al contenuto.
        /// </summary>
        [Description( "Specifica se per un controllo verrà eseguilo il ridimensionamento automatico in base al contenuto." )]
        public new bool AutoSize
        {
            get { return this.m_FlowPanel.AutoSize; }
            set
            {
                this.m_FlowPanel.AutoSize = base.AutoSize = value;
                //this.m_FlowPanel.AutoSizeMode = this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            }
        }

        /// <summary>
        /// Gets or sets how the control will resize itself.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A value from the <see cref="T:System.Windows.Forms.AutoSizeMode"/> enumeration. The default is <see cref="F:System.Windows.Forms.AutoSizeMode.GrowOnly"/>.
        /// </returns>
        [Browsable( true )]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { this.m_FlowPanel.AutoSizeMode = base.AutoSizeMode = value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A <see cref="T:System.Windows.Forms.Control.ControlCollection"/> representing the collection of controls contained within the control.
        /// </returns>
        public new ControlCollection Controls
        {
            get { return this.m_FlowPanel.Controls; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        [Browsable( false )]
        public int Count
        {
            get { return this.m_FlowPanel.Controls.Count; /*return this.m_controls.Count;*/ }
        }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        /// <value>The creator.</value>
        [Browsable( false )]
        public ControlCreatorBase Creator { get; set; }

        /// <summary>
        /// Ottiene o imposta un valore che indica la direzione del flusso del controllo.
        /// </summary>
        /// <value>The flow direction.</value>
        [Description( "Specifica la direzione di ripetizione dei controlli" )]
        public FlowDirection FlowDirection
        {
            get { return this.m_FlowPanel.FlowDirection; }
            set { this.m_FlowPanel.FlowDirection = value; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
        /// </returns>
        [Browsable( false )]
        public bool IsSynchronized
        {
            get { return false; }
        }

                /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        [Browsable( false )]
        public object SyncRoot
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public Control this[ int index ]
        {
            get { return this.m_FlowPanel.Controls[index]; }
        }

        /// <summary>
        /// Ottiene o imposta un valore che indica se il contenuto del controllo deve essere mandato a capo o troncato.
        /// </summary>
        /// <value><c>true</c> se il contenuto deve essere mandato a capo; in caso contrario, <c>false</c>c>. Il valore predefinito è <c>true</c>c>.</value>
        [Description( "Indica se il contenuto viene mandato a capo o troncato in corrispondenza del limite del controllo" )]
        public bool WrapContent
        {
            get { return this.m_FlowPanel.WrapContents; }
            set { this.m_FlowPanel.WrapContents = value; }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlRepeater"/> class.
        /// </summary>
        public ControlRepeater()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlRepeater"/> class.
        /// </summary>
        /// <param name="creator">L'oggetto utilizzato per creare i <see cref="Control"/> da aggiungere.</param>
        public ControlRepeater( ControlCreatorBase creator )
        {
            InitializeComponent();
            this.Creator = creator;
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>Restituisce un riferimento al nuovo oggetto <see cref="Control"/> appena creato ed aggiunto.</returns>
        /// <exception cref="T:System.ArgumentNullException">Se <see cref="Creator"/> non è impostato su un oggetto valido.</exception>
        public Control Add()
        {
            if( this.Creator == null )
            {
                throw new ArgumentNullException( "this.Creator", "The field Creator is null" );
            }

            var newControl = this.Creator.CreateNewControl();
            if( newControl != null )
            {
                this.m_FlowPanel.Controls.Add( newControl );
            }
            return newControl;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Clear()
        {
            this.m_FlowPanel.Controls.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Control> GetEnumerator()
        {
            return this.m_FlowPanel.Controls.Cast<Control>().GetEnumerator();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public new bool Contains( Control item )
        {
            return this.m_FlowPanel.Controls.Contains( item );
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove( Control item )
        {
            try
            {
                this.m_FlowPanel.Controls.Remove( item );
            }
            catch( Exception )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf( Control item )
        {
            return this.m_FlowPanel.Controls.IndexOf( item );
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt( int index )
        {
            this.m_FlowPanel.Controls.RemoveAt( index );
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo( Array array, int index )
        {
            this.m_FlowPanel.Controls.CopyTo( array, index );
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Crea un nuovo controllo per un <see cref="ControlRepeater"/>
    /// </summary>
    /// <remarks>
    /// La creazione del controllo si basa sull'istanziare un nuovo oggetto del tipo specificato
    /// </remarks>
    public class TypeControlCreator : ControlRepeater.ControlCreatorBase
    {
		#region Fields 

		#region Internal Fields 

        private readonly Type m_controlType;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeControlCreator"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Deve essere specificato il tipo di controllo.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="controlType"/> deve derivare da <see cref="Control"/></exception>
        /// <param name="controlType">Tipo del controllo da creare</param>
        public TypeControlCreator( Type controlType )
        {
            if( controlType == null )
            {
                throw new ArgumentNullException( "controlType", "You must specify the type of the control to be created" );
            }
            if( !controlType.IsSubclassOf( typeof( Control ) ) )
            {
                throw new ArgumentOutOfRangeException( "controlType", "The control you want to creat must inherith from System.Forms.Control" );
            }

            this.m_controlType = controlType;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Crea un nuovo controllo.
        /// </summary>
        /// <returns>Il nuovo controllo.</returns>
        public override Control CreateNewControl()
        {
            return (Control)Activator.CreateInstance( this.m_controlType );
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Crea un nuovo controllo per un <see cref="ControlRepeater"/>
    /// </summary>
    /// <remarks>
    /// La creazione del controllo si basa sulla clonazione di un oggetto preesistente, quindi ne vengono copiate le
    /// proprietà.
    /// </remarks>
    public class PrototypeControlCreator : ControlRepeater.ControlCreatorBase
    {
		#region Fields 

		#region Internal Fields 

        private readonly ICloneable m_prototype;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="PrototypeControlCreator"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Il prototipo non può essere <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="prototype"/> deve derivare da <see cref="Control"/></exception>
        /// <param name="prototype">L'istanza dell'oggetto di riferimento come prototipo</param>
        public PrototypeControlCreator( ICloneable prototype )
        {
            if( prototype == null )
            {
                throw new ArgumentNullException( "prototype", "The parameter cannot be null" );
            }
            if( !prototype.GetType().IsSubclassOf( typeof( Control ) ) )
            {
                throw new ArgumentOutOfRangeException( "prototype", "The parameter must inherit from Control" );
            }
            if( !prototype.ImplementsInterface( typeof(ICloneable) ) )
            {
                throw new ArgumentOutOfRangeException( "prototype", "The parameter must implement ICloneable interface" );
            }

            // Mi assicuro che l'istanza che memorizzo non possa venire intaccata da nessuno (o quasi: deep copy)
            this.m_prototype = (ICloneable)prototype.Clone();
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Crea un nuovo controllo clonando l'oggetto di riferimento
        /// </summary>
        /// <returns>Il nuovo controllo.</returns>
        public override Control CreateNewControl()
        {
            return (Control)this.m_prototype.Clone();
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
