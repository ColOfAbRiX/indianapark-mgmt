using System;
using System.Windows.Forms;
using IndianaPark.Licensing;
using IndianaPark.Plugin;
using IndianaPark.Biglietti;
using IndianaPark.Tools.Security.Licensing;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PowerFan
{
    /// <summary>
    /// Plugin per l'emissione e la gestione dei biglietti per il PowerFan
    /// </summary>
    [PluginOwners( "IndianaPark.Biglietti.PluginBiglietti" )]
#if( RELEASE || LICENSE )
    [System.ComponentModel.LicenseProvider( typeof( IpLicenseProvider ) )]
    [EnabledFeatures( SoftwareFeatures.PowerFan )]
#endif
    public sealed class PluginPowerFan : IndianaparkPlugin, IAttrazione
    {
		#region Fields 

		#region Class-wise Fields 

        private static PluginPowerFan ms_GlobalInstance;

		#endregion Class-wise Fields 

#if( RELEASE || LICENSE )
        private System.ComponentModel.License m_pluginLicense;
#endif

        #endregion Fields

        #region Methods

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginPowerFan"/> class.
        /// </summary>
        /// <param name="host">Contenitore del plugin</param>
        /// <exception cref="ArgumentNullException">Il riferimento al contenitore del plugin non può essere <c>null</c></exception>
        public PluginPowerFan( IPluginHost host ) : base( host )
        {
#if( RELEASE || LICENSE )
            this.m_pluginLicense = System.ComponentModel.LicenseManager.Validate( this.GetType(), this );
#endif

            if( PluginPowerFan.ms_GlobalInstance != null )
            {
                throw new ApplicationException( this.GetType() + " instantiated more than once." );
            }
            PluginPowerFan.ms_GlobalInstance = this;

            // Configuro il caricamento della configurazione
            this.RegisterLoadingFunctions( this.LoadConfiguration );
        }

		#endregion Constructors 

		#region Class-wise Methods 

        /// <summary>
        /// Recupera l'istanza globale del plugin.
        /// </summary>
        /// <remarks>
        /// Questo metodo non istanzia il plugin, che deve essere istanziato dal suo <see cref="IPluginManager"/>
        /// </remarks>
        /// <returns>L'istanza globale del Plugin</returns>
        internal static PluginPowerFan GetGlobalInstance()
        {
            if( PluginPowerFan.ms_GlobalInstance == null )
            {
                throw new ApplicationException( typeof( PluginPowerFan ) + " is not istantiated." );
            }

            return PluginPowerFan.ms_GlobalInstance;
        }

        /// <summary>
        /// Permette di accedere all'istanza globale dei parametri di configurazione del plugin da qualunque punto dell'Assembly
        /// </summary>
        /// <remarks>
        /// L'istanza globale corrisponde all'ultimo <see cref="PluginPowerFan"/> istanziato.
        /// </remarks>
        /// <returns>I parametri di configurazione del plugin globale.</returns>
        internal static IConfigValue GetGlobalParameter( string name )
        {
            if( String.IsNullOrEmpty( name ) )
            {
                throw new ArgumentNullException( "name", "The parameter name cannot be null" );
            }

            return PluginPowerFan.ms_GlobalInstance.Parameters[name];
        }

		#endregion Class-wise Methods 

		#region Public Methods 

        /// <summary>
        /// Funzione utilizzata per l'inizializzazione del plugin. Viene chiamata una volta quando il programma si
        /// avvia ed il plugin viene caricato.
        /// </summary>
        /// <returns>
        /// True se il caricamento ha avuto esito positivo, False altrimenti
        /// </returns>
        public override bool Load()
        {
            return true;
        }

        /// <summary>
        /// Funzione utilizzata per la chiusura del plugin.
        /// </summary>
        public override void Dispose()
        {
#if( RELEASE || LICENSE )
            if( this.m_pluginLicense != null )
            {
                this.m_pluginLicense.Dispose();
                this.m_pluginLicense = null;
            }
#endif
            this.OnPluginDisposed();
        }

        /// <summary>
        /// Utilizzata per recuperare un'interfaccia grafica per l'interazione con l'utente. Il controllo utente
        /// recuperato viene, di solito, aggiunto alla finestra principale del programma
        /// </summary>
        /// <returns>
        /// Il pannello da aggiungere all'interfaccia grafica. Null in caso non si debba aggiungere niente
        /// </returns>
        public override Control GetCommandPanel()
        {
            return null;
        }

        /// <summary>
        /// Utilizzata per recuperare un elemento di un menù a tendina da aggiungere al menu principale della GUI.
        /// </summary>
        /// <returns>
        /// Il menu da aggiungere. Null in caso non si debba aggiungere niente
        /// </returns>
        public override ToolStripItem GetDropdownMenu()
        {
            return null;
        }

        /// <summary>
        /// Recupera il nome visualizzato per l'attrazione
        /// </summary>
        /// <returns>Il nome dell'attrazione che verrà visualizzato all'utente</returns>
        public string GetDisplayName()
        {
            return IndianaPark.PowerFan.Properties.Resources.GetDisplayNameValue;
        }

        /// <summary>
        /// Recupera lo stato di partenza del wizard
        /// </summary>
        /// <returns>Lo stato di partenza del wizard per l'emissione dei biglietti</returns>
        public Type GetFirstWizard()
        {
            return typeof( Wizard.TipoAttivitaState );
        }

        /// <summary>
        /// Recupera il costruttore dei dati utilizzato dall'attrazione
        /// </summary>
        /// <returns>Il costruttore dati</returns>
        public IBuilder GetBuilder()
        {
            return new Wizard.PowerFanBuilder();
        }

        /// <summary>
        /// Avvia le routine di gestione del plugin. Il suo scopo è quello di differire l'esecuzione delle routine del
        /// plugin dalle procedure di caricamento; un esempio d'uso è l'impostazione di configurazioni prima dell'
        /// avvio del plugin
        /// </summary>
        /// <remarks>
        /// Il suo scopo è quello di differire l'esecuzione delle routine del plugin dalle procedure di caricamento;
        /// un esempio d'uso è l'impostazione di configurazioni prima dell'avvio del plugin
        /// </remarks>
        public override void Run( IPlugin owner )
        {
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
