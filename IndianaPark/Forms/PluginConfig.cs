using System;
using System.ComponentModel;
using System.Windows.Forms;
using IndianaPark.Plugin;

namespace IndianaPark.Forms
{
    /// <summary>
    /// Finestra per la visualizzazione e modifica dei parametri di configurazione dei plugin
    /// </summary>
    public partial class PluginConfig : Form
    {
        private readonly IPlugin m_plugin;

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="plugin">Plugin di cui visualizzare la configurazione</param>
        public PluginConfig(IPlugin plugin)
        {
            InitializeComponent();

            this.m_plugin = plugin;

            var bag = new PropertyBag();

            if( plugin.Parameters.Count > 0 )
            {
                // Aggiungo le configurazioni alla lista
                foreach( var config in plugin.Parameters.Values )
                {
                    // Solo le configurazioni pubbliche sono ammesse
                    if( config.IsPublic )
                    {
                        //var value = config.Value == null ? "" : config.Value;
                        var property = new PropertySpec( config.Name, config.ValueType, plugin.Name, config.Description, config.Value );

                        // La configurazione può essere solo visualizzata
                        if( config.IsReadonly )
                        {
                            if( property.Attributes == null )
                            {
                                property.Attributes = new Attribute[1];
                            }

                            property.Attributes[0] = new ReadOnlyAttribute( true );
                        }

                        bag.Properties.Add( property );
                    }
                }

                // Imposto i getter exception setter
                bag.SetValue += this.SetHandler;
                bag.GetValue += this.GetHandler;

                this.propertyGrid.SelectedObject = bag;
            }
        }

        /// <summary>
        /// Recupera il valore di una configurazione del plugin
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.Windows.Forms.PropertySpecEventArgs"/> instance containing the event data.</param>
        private void GetHandler(object sender, PropertySpecEventArgs e)
        {
            e.Value = this.m_plugin.Parameters[e.Property.Name].Value;
        }

        /// <summary>
        /// Salva il valore di una configurazione del plugin
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="exception">The <see cref="System.Windows.Forms.PropertySpecEventArgs"/> instance containing the event data.</param>
        private void SetHandler( object sender, PropertySpecEventArgs e )
        {
            this.m_plugin.Parameters[e.Property.Name].Value = e.Value;
        }
    }
}
