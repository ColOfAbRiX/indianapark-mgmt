using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using IndianaPark.Tools.Xml;
using IndianaPark.Tools.Logging;

namespace IndianaPark.Plugin.Persistence
{
    /// <summary>
    /// Oggetto che si occupa di salvare i parametri di configurazione dei plugin su un file XML
    /// </summary>
    public sealed class PluginConfigLocalPersistence : IConfigPersistence
    {
		#region Fields 

		#region Internal Fields 

        private readonly string m_configFile;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfigLocalPersistence"/> class.
        /// </summary>
        /// <param name="configFile">The config file.</param>
        public PluginConfigLocalPersistence( string configFile )
        {
            if( String.IsNullOrEmpty( configFile ) )
            {
                throw new ArgumentNullException( "configFile" );
            }

            this.m_configFile = configFile;
        }

		#endregion Constructors 

		#region Class-wise Methods 

        // ReSharper disable PossibleNullReferenceException
        private static IConfigValue ConvertFromEntity( XmlNode input )
        {
            if( input == null )
            {
                throw new ArgumentNullException( "input", "The input to convert cannot be null" );
            }

            object paramValue = null;

            // Provo a creare un'istanza dell'oggetto a partire dal valore della sua stringa
            var paramType = Type.GetType( input["type"].FirstChild.Value, true );

            // Deserializzo il valore
            paramValue = input["value"].FirstChild.Value.DeserializeXml( paramType );

            var description = input["description"].FirstChild == null ? "" : input["description"].FirstChild.Value;
            var isPublic = bool.Parse( input["attributes"].Attributes["public"].Value );
            var isReadonly = bool.Parse( input["attributes"].Attributes["public"].Value );

            // Questa distinzione è richiesta da PluginConfigValue
            if( paramValue == null )
            {
                return new Plugin.PluginConfigValue( input.Attributes["name"].Value, paramType, description, isPublic, isReadonly );
            }

            return new Plugin.PluginConfigValue( input.Attributes["name"].Value, paramValue, description, isPublic, isReadonly );
        }

        // ReSharper restore PossibleNullReferenceException
        private static XmlNode ConvertFromConfigValue( XmlDocument root, IConfigValue input, string owner )
        {
            try
            {
                // Serializzo il valore
                var value = input.Value.ToXml();

                // Nodo del parametro
                var output = root.CreateElement( "parameter" );

                // Attributo di parameter
                var tempAttrib = root.CreateAttribute( "name" );
                tempAttrib.Value = input.Name;
                output.Attributes.Append( tempAttrib );

                // Attributo di parameter
                tempAttrib = root.CreateAttribute( "owner" );
                tempAttrib.Value = owner;
                output.Attributes.Append( tempAttrib );

                // Nodo del tipo di dato
                var tempNode = root.CreateElement( "type" );
                tempNode.AppendChild( root.CreateCDataSection( input.ValueType.AssemblyQualifiedName ) );
                output.AppendChild( tempNode );

                // Nodo del valore
                tempNode = root.CreateElement( "value" );
                tempNode.AppendChild( root.CreateCDataSection( value ) );
                output.AppendChild( tempNode );

                // Nodo della descrizione
                tempNode = root.CreateElement( "description" );
                tempNode.AppendChild( root.CreateTextNode( input.Description ) );
                output.AppendChild( tempNode );

                // Nodo degli attributi
                tempNode = root.CreateElement( "attributes" );

                // Attributo di attributes
                tempAttrib = root.CreateAttribute( "readonly" );
                tempAttrib.Value = input.IsReadonly.ToString();
                tempNode.Attributes.Append( tempAttrib );

                // Attributo di attributes
                tempAttrib = root.CreateAttribute( "public" );
                tempAttrib.Value = input.IsPublic.ToString();
                tempNode.Attributes.Append( tempAttrib );

                output.AppendChild( tempNode );

                return output;
            }
            catch
            {
                return null;
            }
        }

		#endregion Class-wise Methods 

		#region Internal Methods 

        private XmlDocument OpenXmlDocument()
        {
            var xd = new XmlDocument();
            if( File.Exists( this.m_configFile ) )
            {
                xd.Load( this.m_configFile );
            }

            if( xd.DocumentElement == null )
            {
                xd.AppendChild( xd.CreateElement( "parameters" ) );
            }
            return xd;
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Carica tutti i parametri dei plugin
        /// </summary>
        /// <param name="owner">Il nome del plugin di cui caricare i parametri</param>
        /// <returns>La lista con i valori di configurazione per il plugin</returns>
        public IDictionary<string, IConfigValue> LoadAllParameters( string owner )
        {
            var output = new Dictionary<string, IConfigValue>();

            // Carico il documento
            var document = this.OpenXmlDocument();
            if( document == null )
            {
                return output;
            }

            // Carico l'elemento radice
            var root = document.DocumentElement;
            if( root == null )
            {
                return output;
            }

            var nodi = root.SelectNodes( String.Format( "/parameters/parameter[@owner='{0}']", owner ) );
            if( nodi == null )
            {
                return output;
            }

            foreach( XmlNode n in nodi )
            {
                try
                {
                    var tmp = PluginConfigLocalPersistence.ConvertFromEntity( n );
                    if( tmp != null )
                    {
                        output.Add( tmp.Name, tmp );
                    }
                }
                catch( Exception ex )
                {
                    // Utilizzato in fase di caricamento dei singoli parametri dall'XML
                    Logger.Default.Write( ex, "Error while loading parameter for a plugin into the XML file" );
                }
            }

            return output;
        }

        /// <summary>
        /// Carica un parametro
        /// </summary>
        /// <param name="name">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui fa parte</param>
        /// <returns>Il valore di configurazione del plugin</returns>
        public IConfigValue LoadParameter( string name, string owner )
        {
            // Carico il documento
            var document = this.OpenXmlDocument();
            if( document == null )
            {
                return null;
            }

            // Carico l'elemento radice
            var root = document.DocumentElement;
            if( root == null )
            {
                return null;
            }

            try
            {
                // Recupero il nodo che fa match
                var nodo = root.SelectSingleNode( String.Format( "/parameters/parameter[@name='{0}' and @owner='{1}']", name, owner ) );
                if( nodo == null )
                {
                    return null;
                }

                // Converto exception restituisco il parametro
                return PluginConfigLocalPersistence.ConvertFromEntity( nodo );
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Error while loading a parameter for a plugin into the XML file" );
                return null;
            }
        }

        /// <summary>
        /// Salva un parametro
        /// </summary>
        /// <param name="config">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui fa parte</param>
        /// <returns>Il valore di configurazione del plugin</returns>
        public bool SaveParameter( IConfigValue config, string owner )
        {
            // Carico il documento
            var document = this.OpenXmlDocument();
            if( document == null )
            {
                return false;
            }

            // Carico l'elemento radice
            var root = document.DocumentElement;
            if( root == null )
            {
                return false;
            }

            try
            {
                // Se il parametro è gia presente lo elimino
                var nodo = root.SelectSingleNode( String.Format( "/parameters/parameter[@name='{0}' and @owner='{1}']", config.Name, owner ) );
                if( nodo != null )
                {
                    root.RemoveChild( nodo );
                }

                // Creo ed aggiungo il nuovo nodo
                var nuovoNodo = PluginConfigLocalPersistence.ConvertFromConfigValue( document, config, owner );
                root.AppendChild( nuovoNodo );

                document.Save( this.m_configFile );
                return true;
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Error while saving a parameter for a plugin into the XML file" );
                return false;
            }
        }

        /// <summary>
        /// Salva tutti i parametri dei plugin
        /// </summary>
        /// <param name="configs">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui caricare i parametri</param>
        /// <returns><c>true</c> se tutti i parametri sono stati salvati correttamente, <c>false</c> se anche uno solo non è stato salvato</returns>
        public bool SaveAllParameters( IDictionary<string, IConfigValue> configs, string owner )
        {
            // Carico il documento
            var document = this.OpenXmlDocument();
            if( document == null )
            {
                return false;
            }

            // Carico l'elemento radice
            var root = document.DocumentElement;
            if( root == null )
            {
                return false;
            }

            // Salvo tutti i parametri uno alla volta
            var output = true;
            foreach( var c in configs.Values )
            {
                try
                {
                    var nodo = root.SelectSingleNode( String.Format( "/parameters/parameter[@name='{0}' and @owner='{1}']", c.Name, owner ) );
                    if( nodo != null )
                    {
                        root.RemoveChild( nodo );
                    }

                    var nuovoNodo = PluginConfigLocalPersistence.ConvertFromConfigValue( document, c, owner );
                    root.AppendChild( nuovoNodo );
                }
                catch( Exception ex )
                {
                    // Utilizzato in fase di caricamento dei singoli parametri dall'XML
                    Logger.Default.Write( ex, "Error while saving a parameter for a plugin into the XML file" );
                    output = false;
                }
            }

            document.Save( this.m_configFile );
            return output;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}
