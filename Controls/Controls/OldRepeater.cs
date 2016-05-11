using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IndianaPark.Tools.Controls
{
    /// <summary>
    /// Controllo ripetitore. Ripete un pannello comandi in un determinato senso per un
    /// numero impostabile di volte. E' possibile specificare un altro pannello come separatore dei
    /// pannelli ripetuti. La classe espone poi degli appropriati membri per accedere direttamente ai
    /// pannelli.
    /// </summary>
    [Obsolete("L'oggetto è stato sostituito con una versione più robusta exception flessibile")]
    public partial class OldRepeater : UserControl, IEnumerable<UserControl>
    {
        /// <summary>
        /// Direzione delle ripetizioni per il controllo ControlRepeater
        /// </summary>
        public enum RepetitionDirection
        {
            /// <summary>
            /// Ripetizione in senso verticale
            /// </summary>
            Vertical,
            /// <summary>
            /// Ripetizione in senso orizzontale
            /// </summary>
            Horizontal
        }

        #region Campi

        #region Interni

        private Type m_typRepeater;
        /// <summary>
        /// Viene utilizzata per velocizzare le operazioni, come cache
        /// </summary>
        private UserControl m_sampleInstance;
        private Type m_typSeparator;
        private readonly List<UserControl> m_lstControlInstances;
        private RepetitionDirection m_rdDirection = RepetitionDirection.Vertical;

        #endregion

        #region Pubblici

        /// <summary>
        /// Il controllo che si trova tra un elemento e il successivo.
        /// </summary>
        /// <remarks>
        /// Il tipo impostato deve essere o derivare da UserControl
        /// </remarks>
        /// <exception cref="ArgumentNullException">Non si può assegnare valor nullo al campo</exception>
        /// <exception cref="ArgumentException">Il separatore deve ereditare da UserControl</exception>
        [ Category("Repeater"),
          Description("Il Type del pannello separatore") ]
        public Type SeparatorType
        {
            get { return m_typSeparator; }
            set
            {
                if( value == null )
                {
                    return;
                }

                // Accetto solo oggetti che derivano da UserControl
                if( !value.IsSubclassOf( new UserControl().GetType() ) )
                {
                    throw new ArgumentException( "The type must hinerit from System.Windows.Form.UserControl" );
                }
                 
                this.m_typSeparator = value;
            }
        }

        /// <summary>
        /// Il controllo che viene ripetuto.
        /// </summary>
        /// <remarks>
        /// <para>Impostare il tipo di controllo resetta le impostazioni dei singoli controlli.</para>
        /// <para>Il tipo impostato deve essere o derivare da UserControl</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Non si può assegnare valor nullo al campo</exception>
        /// <exception cref="ArgumentException">Il separatore deve ereditare da UserControl</exception>
        [ Category( "Repeater" ),
          Description("Il Type del pannello da ripetere") ]
        public Type RepeatedControlType
        {
            get { return this.m_typRepeater; }
            set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( "value" );
                }

                // Accetto solo oggetti che derivano da UserControl
                if( !value.IsSubclassOf( new UserControl().GetType() ) )
                {
                    throw new ArgumentException( "The type must hinerit from System.Windows.Form.UserControl" );
                }

                int count = this.m_lstControlInstances.Count;

                this.Clear();
                this.m_typRepeater = value;
                this.m_sampleInstance = this.CreateRepeatedControl();
                this.Add( (uint)count );
            }
        }

        /// <summary>
        /// Riferimento in sola lettura ad un elemento ripetuto
        /// </summary>
        /// <param name="index">L'indice del controllo da riferire</param>
        /// <returns>L'index-esimo controllo</returns>
        [ Browsable(false) ]
        public UserControl this[int index]
        {
            get
            {
                return this.m_lstControlInstances[index];
            }
            protected set
            {
                this.m_lstControlInstances[index] = value;
            }
        }

        /// <summary>
        /// Il numero di controlli attualmente in lista.
        /// </summary>
        /// <remarks>
        /// Impostando un nuovo valore la lista dei controlli viene resettata e ricreata da zero, quindi le
        /// impostazioni andranno perdute.
        /// </remarks>
        [ Category("Repeater"),
          Description("Il numero di pannelli visualizzati") ]
        public uint Repetitions
        {
            get { return (uint)this.m_lstControlInstances.Count; }
            set
            {
                this.Clear();
                this.Add( value );
            }
        }

        /// <summary>
        /// La dimensione in cui ripetere il controllo: orizzontalmente o verticalmente
        /// </summary>
        /// <remarks>Assegnando un qualsiasi valore la lista dei controlli viene ridisegnata</remarks>
        [ Category("Repeater"),
          Description("La direzione di ripetizione") ]
        public RepetitionDirection Direction
        {
            get { return m_rdDirection; }
            set
            {
                m_rdDirection = value;

                if( this.m_typRepeater != null )
                {
                    this.DrawControlList();
                }
            }
        }

        #endregion

        #endregion

        #region Costruttori

        /// <summary>
        /// Costruttore senza parametri.
        /// </summary>
        /// <remarks>Da non usare perchè non permette l'uso del controllo. E' presente solo per interagire
        /// correttamente con VisualStudio</remarks>
        public OldRepeater()
        {
            this.m_lstControlInstances = new List<UserControl>();
        }

        /// <summary>
        /// Costruttore con solo il tipo da ripetere
        /// </summary>
        /// <param name="sampleInstance">Un'istanza dell'oggetto da ripetere</param>
        public OldRepeater( UserControl sampleInstance ) : this()
        {
            this.SetRepeatedControl( sampleInstance );
        }

        /// <summary>
        /// Costruttore con il tipo da ripetere e il separatore
        /// </summary>
        /// <param name="sampleInstance">Un'istanza dell'oggetto da ripetere</param>
        /// <param name="separator">Il separatore da inserire tra gli oggetti ripetitu</param>
        public OldRepeater( UserControl sampleInstance, UserControl separator ) : this( sampleInstance )
        {
            this.SetSeparatorControl( separator );
        }

        #endregion

        #region Metodi

        #region Interni

        /// <summary>
        /// Disegna sul controllo la lista dei pannelli da ripetere
        /// </summary>
        /// <remarks>
        /// Le dimensioni del controllo dipendono da quelle del pannello ripetuto, dalla direzione di ripetizione
        /// exception ovviamente dal numero di ripetizioni.
        /// </remarks>
        protected void DrawControlList()
        {
            this.SuspendLayout();
            this.Controls.Clear();
            
            // Disegno tutti i controlli della lista
            int i = 0;
            foreach( UserControl uc in this.m_lstControlInstances )
            {
                // Imposto la posizione del controllo corrente
                Point location = new Point( 0, uc.Size.Height * i );;
                if( this.m_rdDirection == RepetitionDirection.Horizontal )
                {
                    location = new Point( uc.Size.Width * i, 0 );
                }
                uc.Location = location;

                // Aggiungo il nuovo controllo al pannello corrente
                this.Controls.Add( uc );

                i++;
            }

            // Ridimensiono
            this.UpdateSize();

            this.PerformLayout();
        }

        /// <summary>
        /// Crea un'istanza del UserControl da ripetere.
        /// </summary>
        /// <returns>Lo UserControl appena creato</returns>
        protected UserControl CreateRepeatedControl()
        {
            // Controllo che sia stato impostato il tipo
            if( this.m_typRepeater == null )
            {
                throw new InvalidOperationException( "The repeater type is not set" );
            }

            // Istanzio il nuovo tipo di controllo
            return (UserControl)Activator.CreateInstance( this.m_typRepeater );
        }

        /// <summary>
        /// Disegna il controllo impostando le dimensioni in base al contenuto e vincolando
        /// il ridimensionamento a quelle dimensioni.
        /// </summary>
        private void UpdateSize()
        {
            Size size = new Size();
            Size sample = this.m_sampleInstance.Size;
            //UserControl sampleControl = this.CreateRepeatedControl();

            // Dimensione del  controllo
            size = new Size( sample.Width, sample.Height * this.m_lstControlInstances.Count );
            if( this.m_rdDirection == RepetitionDirection.Horizontal )
            {
                size = new Size( sample.Width * this.m_lstControlInstances.Count, sample.Height );
            }

            // Imposto la dimensione ed impedisco il ridimensionamento
            this.Size = size;
            this.MinimumSize = size;
            this.MaximumSize = size;
        }
        
        #endregion

        #region Pubblici

        /// <summary>
        /// Imposta il controllo da ripetere. La chiamata di questo metodo resetta il repeater.
        /// </summary>
        /// <param name="sampleInstance">Un'istanza del controllo da ripetere</param>
        /// <exception cref="ArgumentNullException">E' necessario specificare un'istanza da cui recuperare il tipo</exception>
        public void SetRepeatedControl( UserControl sampleInstance )
        {
            if( sampleInstance == null )
            {
                throw new ArgumentNullException( "sampleInstance" );
            }

            this.RepeatedControlType = sampleInstance.GetType();
        }

        /// <summary>
        /// Imposta il controllo che viene usato come separatore tra i controlli ripetuti.
        /// </summary>
        /// <param name="sampleInstance">Un'istanza del controllo che fa da separatore</param>
        /// <exception cref="ArgumentNullException">E' necessario specificare un'istanza da cui recuperare il tipo</exception>
        public void SetSeparatorControl( UserControl sampleInstance )
        {
            if( sampleInstance == null )
            {
                throw new ArgumentNullException( "sampleInstance" );
            }

            this.SeparatorType = sampleInstance.GetType();
        }

        /// <summary>
        /// Aggiunge un nuovo pannello alla lista
        /// </summary>
        /// <remarks>
        /// L'aggiunta di un controllo consiste nel crare una nuova istanza del pannello da ripetere,
        /// quindi il nuovo controllo avrà di default le impostazioni base di quell'oggetto.
        /// </remarks>
        /// <returns>Il controllo appena aggiunto</returns>
        public UserControl Add()
        {
            // Istanzio il nuovo tipo di controllo
            UserControl newControl = this.CreateRepeatedControl();
            this.m_lstControlInstances.Add( newControl );

            // Configuro il nuovo controllo
            newControl.Name = this.m_typRepeater.Name;
            newControl.Parent = this;

            // Disegno la lista
            this.DrawControlList();

            return newControl;
        }

        /// <summary>
        /// Aggiunge un nuovo pannello in fondo alla lista
        /// </summary>
        /// <param name="count">Quanti pannelli aggiungere</param>
        /// <returns>
        /// Un array con la lista dei controlli aggiunti
        /// </returns>
        public UserControl[] Add( uint count )
        {
            var added = new UserControl[count];

            for( int i = 0; i < count; i++ )
            {
                added[i] = this.Add();
            }

            this.DrawControlList();

            return added;
        }

        /// <summary>
        /// Inserisce un nuovo pannello in corrispondenza dell'indice specificato.
        /// </summary>
        /// <param name="index">L'indice in cui inserire il pannello</param>
        /// <remarks>
        /// Se <paramref name="index"/> è uguale a <see cref="ControlRepeater.Repetitions"/>, questa funzione si
        /// comporta come <see cref="ControlRepeater.Add()"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Prima di usare Insert è necessario avere impostato il tipo di pannello da ripetere
        /// </exception>
        public void Insert( uint index )
        {
            // Controllo che sia stato impostato il tipo
            if( this.m_typRepeater == null )
            {
                throw new InvalidOperationException( "The repeater type is not set" );
            }

            // Istanzio il nuovo tipo di controllo
            UserControl newControl = (UserControl)Activator.CreateInstance( this.m_typRepeater );

            // Configuro il nuovo controllo
            newControl.Name = this.m_typRepeater.Name;
            newControl.Parent = this;
            this.m_lstControlInstances.Insert( (int)index, newControl );

            // Disegno la lista
            this.DrawControlList();
        }

        /// <summary>
        /// Rimuove il pannello in corrispondenza dell'indice specificato.
        /// </summary>
        /// <param name="index">Indice del pannello da rimuovere</param>
        public void Remove( int index )
        {
            // Chiamo dispose del controllo
            this.m_lstControlInstances[index].Dispose();

            // Elimino il controllo exception aggiorno
            this.m_lstControlInstances.RemoveAt( index );
            this.DrawControlList();
        }

        /// <summary>
        /// Determina l'indice di un elemento specifico
        /// </summary>
        /// <param name="item">Istanza del pannello da trovare nella lista</param>
        /// <returns>
        /// Indice di value se si trova nella lista; in caso contrario, -1.
        /// </returns>
        public int IndexOf( UserControl item )
        {
            return this.m_lstControlInstances.IndexOf( item );
        }

        /// <summary>
        /// Consente di rimuovere tutti gli elementi dal controllo.
        /// </summary>
        public void Clear()
        {
            // Chiamo Dispose di ogni controllo prima che questi vengano eliminati
            foreach( UserControl uc in this.m_lstControlInstances )
            {
                uc.Dispose();
            }

            // Cancello la lista exception aggiorno
            this.m_lstControlInstances.Clear();
            if( this.m_typRepeater != null )
            {
                this.DrawControlList();
            }
        }

        /// <summary>
        /// Copia i pannelli in un oggetto Array, a partire da un particolare indice Array.
        /// </summary>
        /// <param name="array">Oggetto unidimensionale Array che rappresenta la destinazione degli elementi copiati. L'indicizzazione di Array deve avere base zero.</param>
        /// <param name="arrayIndex">Indice in base zero di array a partire dal quale viene effettuata la copia.</param>
        public void CopyTo( UserControl[] array, int arrayIndex )
        {
            this.m_lstControlInstances.CopyTo( array, arrayIndex );
        }

        /// <summary>
        /// Restituisce un enumeratore che scorre un insieme.
        /// </summary>
        /// <returns>Oggetto IEnumerator che può essere utilizzato per scorrere l'insieme.</returns>
        public IEnumerator<UserControl> GetEnumerator()
        {
            return this.m_lstControlInstances.GetEnumerator();
        }

        /// <summary>
        /// Restituisce un enumeratore che scorre un insieme.
        /// </summary>
        /// <returns>Oggetto IEnumerator che può essere utilizzato per scorrere l'insieme.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.m_lstControlInstances.GetEnumerator();
        }

        #endregion

        #endregion
    }
}
