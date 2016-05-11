using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IndianaPark.Tools;
using IndianaPark.Plugin;

namespace IndianaPark.Forms
{
    /// <summary>
    /// Visualizza informazioni sui plugin caricati e permette di configurarli
    /// </summary>
    public partial class Plugins : Form
    {
        private PluginManager m_manager;
        private string m_selectedPlugin;

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="manager">
        /// Istanza del PluginManager che gestisce i plugin della finestra principale
        /// </param>
        public Plugins(PluginManager manager)
        {
            InitializeComponent();

            this.m_manager = manager;

            // Riempio la lista dei plugin
            foreach( KeyValuePair<string, IPlugin> plugin in manager )
            {
                listPlugins.Items.Add(
                    new ListViewItem( new[] {
                        plugin.Value.Name,
                        System.IO.Path.GetFileName(plugin.Value.Location),
                        (!plugin.ImplementsInterface( typeof( IPluginRunnable ) ) ).ToString(),
                        OwnerList(plugin.Value)
                    } )
                );
            }
        }

        #region Internals

        private static string OwnerList( IPlugin plugin )
        {
            var attributes = plugin.GetType().GetCustomAttributes( typeof( PluginOwnersAttribute ), true ) as PluginOwnersAttribute[];

            if( attributes != null )
            {
                if( attributes.GetLength(0) > 0 )
                {
                    return attributes[0].ToString();
                }
            }

            return "";
        }

        // FORM CONFIGURAZIONE PLUGIN
        private void buttonParametri_Click( object sender, EventArgs e )
        {
            PluginConfig pg = new PluginConfig( this.m_manager[this.m_selectedPlugin] );
            pg.ShowDialog( this );
        }

        // SELEZIONE DI PLUGIN
        private void listPlugins_ItemSelectionChanged( object sender, ListViewItemSelectionChangedEventArgs e )
        {
            if( e.IsSelected )
            {
                this.m_selectedPlugin = e.Item.SubItems[0].Text;

                bool containsValues = false;
                foreach( IConfigValue config in this.m_manager[this.m_selectedPlugin].Parameters.Values )
                {
                    containsValues |= config.IsPublic;
                }

                // Abilito o disabilito il pulsante in base a quello che ho trovato
                this.buttonParametri.Enabled = containsValues;
            }
        }

        #endregion

        private void listPlugins_DoubleClick( object sender, EventArgs e )
        {
            if( this.buttonParametri.Enabled )
            {
                this.buttonParametri_Click( sender, e );
            }
        }
    }
}
