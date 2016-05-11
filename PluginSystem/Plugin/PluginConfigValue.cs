using System;
using IndianaPark.Plugin.Persistence;

namespace IndianaPark.Plugin
{
    /// <summary>
    /// Dati resi disponibili quando il valore del parametro viene cambiato
    /// </summary>
    public class PluginConfigWriteAccessArgument : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfigWriteAccessArgument"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public PluginConfigWriteAccessArgument( object oldValue, object newValue )
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Valore originale del parametro
        /// </summary>
        public object OldValue { get; private set; }

        /// <summary>
        /// Nuovo valore da memorizzare al posto del vecchio
        /// </summary>
        public object NewValue { get; set; }
    }

    /// <summary>
    /// Rappresenta un valore di configurazione di un plugin
    /// </summary>
    public class PluginConfigValue : IConfigValue, IConfigPersistent
    {
        #region Fields

        #region Internals

        private IConfigPersistence m_persistence;
        private readonly string m_name;
        private readonly string m_description;
        private readonly bool m_public;
        private readonly bool m_readonly;
        private readonly Type m_type;
        private object m_value;

        #endregion

        #region Public

        /// <summary>
        /// Nome del valore di configurazione
        /// </summary>
        public string Name
        {
            get { return this.m_name; }
        }

        /// <summary>
        /// Valore di configurazione
        /// </summary>
        public object Value
        {
            get
            {
                this.OnReading();
                return this.m_value;
            }
            set
            {
                if( value != null )
                {
                    // Controllo per mantenere la coerenza con il tipo precedente
                    if( value.GetType() != this.m_type )
                    {
                        throw new ArgumentException( "Type mismatch when assign config value" );
                    }
                    // Se la proprietà è di sola lettura non scrivo
                    if( this.m_readonly )
                    {
                        return;
                    }
                }
                
                this.m_value = this.OnChanging( this.m_value, value );
                this.OnChanged();
            }
        }

        /// <summary>
        /// Il tipo di dato che l'istanza gestisce
        /// </summary>
        public Type ValueType
        {
            get
            {
                return this.m_type;
            }
        }

        /// <summary>
        /// Descrizione del dato di configurazione
        /// </summary>
        public string Description
        {
            get { return this.m_description; }
        }

        /// <summary>
        /// Indica se la configurazione è pubblica, cioè se può essere modificata dagli utente
        /// </summary>
        public bool IsPublic
        {
            get { return this.m_public; }
        }

        /// <summary>
        /// Indica se la configurazione è di sola lettura
        /// </summary>
        public bool IsReadonly
        {
            get { return this.m_readonly; }
        }

        #endregion

        #endregion

        #region Operators

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==( PluginConfigValue leftOperand, PluginConfigValue rightOperand )
        {
            return ReferenceEquals( leftOperand, rightOperand );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=( PluginConfigValue leftOperand, PluginConfigValue rightOperand )
        {
            return !(leftOperand == rightOperand);
        }

        #endregion

        #region Methods

        #region Constructors

        /// <summary>
        /// Costruttore. Il dato di configurazione creato viene impostato a null, pubblico e rw
        /// </summary>
        /// <exception cref="ArgumentNullException">Il nome del plugin è obbligatorio</exception>
        /// <param name="name">Nome del dato di configurazione</param>
        public PluginConfigValue( string name ) : this( name, null, null, true, false )
        {
        }

        /// <summary>
        /// Costruttore. Il dato di configurazione creato viene impostato come pubblico e rw
        /// </summary>
        /// <param name="name">Nome del dato di configurazione</param>
        /// <param name="value">Valore del dato di configurazione. Può anche essere null</param>
        public PluginConfigValue( string name, object value ) : this( name, value, null, true, false )
        {
        }

        /// <summary>
        /// Costruttore. Il dato di configurazione creato viene impostato come pubblico e rw
        /// </summary>
        /// <param name="name">Nome del dato di configurazione</param>
        /// <param name="value">Valore del dato di configurazione. Può anche essere null</param>
        /// <param name="description">Descrizione del valore di configurazione</param>
        public PluginConfigValue( string name, object value, string description ) : this( name, value, description, true, false )
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <remarks>
        /// La classe è costruita in modo tale da non permettere variazioni sul tipo di dato memorizzato. Il tipo di dato memorizzato
        /// viene impostato in fase di creazione e rimane lo stesso per tutta la vita dell'istanza.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> non può essere null</exception>
        /// <param name="name">Nome del dato di configurazione</param>
        /// <param name="value">Valore del dato di configurazione. Non può essere null. Se serve null utilizzare il costruttore apposito</param>
        /// <param name="isPublic">Indica se la configurazione è pubblica</param>
        /// <param name="isReadonly">Indica se la configurazione è di sola lettura</param>
        /// <param name="description">Descrizione del valore di configurazione</param>
        public PluginConfigValue( string name, object value, string description, bool isPublic, bool isReadonly )
        {
            if( string.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name" );
            }

            this.m_name = name;
            this.m_type = value.GetType();
            this.Value = value;
            this.m_public = isPublic;
            this.m_readonly = isReadonly;
            this.m_description = description;
        }

        /// <summary>
        /// Costruttore. Il dato di configurazione creato viene impostato come pubblico e rw
        /// </summary>
        /// <param name="name">Nome del dato di configurazione</param>
        /// <param name="type">Tipo di dato del valore di configurazione</param>
        /// <param name="description">Descrizione del valore di configurazion</param>
        public PluginConfigValue( string name, Type type, string description )
            : this( name, type, description, true, false )
        {
        }

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <remarks>
        /// <para>La classe è costruita in modo tale da non permettere variazioni sul tipo di dato memorizzato. Il tipo di dato memorizzato
        /// viene impostato in fase di creazione e rimane lo stesso per tutta la vita dell'istanza.</para>
        /// <para>Il valore dell'oggetto è impostato a <c>null</c></para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> non può essere null</exception>
        /// <param name="name">Nome del dato di configurazione</param>
        /// <param name="valueType">Tipo di dato del valore di configurazione</param>
        /// <param name="isPublic">Indica se la configurazione è pubblica</param>
        /// <param name="isReadonly">Indica se la configurazione è di sola lettura</param>
        /// <param name="description">Descrizione del valore di configurazione</param>
        public PluginConfigValue( string name, Type valueType, string description, bool isPublic, bool isReadonly )
        {
            if( string.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name" );
            }

            this.m_name = name;
            this.m_type = valueType;
            this.Value = null;
            this.m_public = isPublic;
            this.m_readonly = isReadonly;
            this.m_description = description;
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Richiamato quando c'è un accesso in lettura al valore di configurazione
        /// </summary>
        /// <remarks>
        /// L'evento è generato prima che il valore venga restituito
        /// </remarks>
        public event EventHandler Reading;

        /// <summary>
        /// Richiamato quando c'è un accesso in scrittura al valore di configurazione
        /// </summary>
        /// <remarks>
        /// L'evento è generato prima che il vecchio valore venga sovrascritto
        /// </remarks>
        public event EventHandler<PluginConfigWriteAccessArgument> Changing;

        /// <summary>
        /// Richiamato quando il valore del parametro è stato modificato
        /// </summary>
        public event EventHandler Changed;

        #region Raisers

        /// <summary>
        /// Utilizzata per generare l'evento <see cref="PluginConfigValue.Reading"/>
        /// </summary>
        protected void OnReading()
        {
            if( this.Reading != null )
            {
                var ea = new EventArgs();
                this.Reading( this, ea );
            }
        }

        /// <summary>
        /// Utilizzata per generare l'evento <see cref="PluginConfigValue.Changed"/>
        /// </summary>
        protected void OnChanged()
        {
            if( this.Changed != null )
            {
                var ea = new EventArgs();
                this.Changed( this, ea );
            }
        }

        /// <summary>
        /// Utilizzata per generare l'evento <see cref="PluginConfigValue.Changing"/>
        /// </summary>
        /// <param name="oldValue">Il vecchio valore</param>
        /// <param name="newValue">Il nuovo valore</param>
        protected object OnChanging( object oldValue, object newValue )
        {
            if( this.Changing != null )
            {
                var ea = new PluginConfigWriteAccessArgument( oldValue, newValue );
                this.Changing( this, ea );
                return ea.NewValue;
            }

            return newValue;
        }

        #endregion

        #region Event Handlers

        private void ParameterAutoSaveHandler( object sender, EventArgs e )
        {
            this.SaveParameter();
        }

        #endregion

        #endregion

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <c>true</c> se i due briefing hanno lo stesso inizio e appartengono alla stessa tipologia, <c>false</c> altrimenti
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( IConfigValue other )
        {
            return this.m_name == other.Name &&
                   this.m_type == other.ValueType &&
                   this.m_value.Equals( other.Value );
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals( object obj )
        {
            if( obj == null )
            {
                return base.Equals( obj );
            }

            if( obj is IConfigValue )
            {
                return this.Equals( (IConfigValue)obj );
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// L'oggetto che serve per mantenere la persistenza
        /// </summary>
        public Persistence.IConfigPersistence Persistence
        {
            get { return m_persistence; }
            set
            {
                // Elimino il gestore dal valore vecchio
                if( m_persistence != null )
                {
                    this.Changed -= ParameterAutoSaveHandler;
                }

                m_persistence = value;

                // Aggiungo il gestore dal valore nuovo
                if( m_persistence != null )
                {
                    this.Changed += ParameterAutoSaveHandler;
                }
            }
        }

        /// <summary>
        /// Permette di impostare e recuperare il nome del proprietario del valore
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Tenta di salvare il parametro dal database
        /// </summary>
        public void SaveParameter()
        {
            if( this.Persistence != null && !String.IsNullOrEmpty( this.OwnerName ) )
            {
                this.Persistence.SaveParameter( this, this.OwnerName );
            }
        }
    }
}