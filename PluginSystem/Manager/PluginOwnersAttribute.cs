using System;
using System.Collections.Generic;

namespace IndianaPark.Plugin
{
    /// <summary>
    /// Attributo che descrive a quali altri plugin appartiene un plugin.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Solo gli oggetti proprietari del plugin possono eseguire un plugin.
    /// </para>
    /// </remarks>
    public sealed class PluginOwnersAttribute : Attribute
    {
        private readonly IList<string> m_owners;

        /// <summary>
        /// Costruttore della classe <see cref="PluginOwnersAttribute"/>.
        /// </summary>
        /// <remarks>
        /// <para>La stringa identificativa di un proprietario deve essere comprensiva di namespace</para>
        /// <para>Una stringa identifica una classe proprietaria e non un'istanza.</para>
        /// </remarks>
        /// <param name="owners">Il percorso completo delle classi che sono proprietarie del plugin</param>
        public PluginOwnersAttribute( params string[] owners )
        {
            this.m_owners = owners;
        }

        /// <summary>
        /// La lista dei proprietari del plugin
        /// </summary>
        public IList<string> Owners
        {
            get
            {
                return this.m_owners;
            }
        }

        /// <summary>
        /// Determina se contiene il proprietario specificato
        /// </summary>
        /// <param name="name">Nome del plugin</param>
        /// <returns>
        /// 	<c>true</c> se contiene il proprietario specificato, <c>false</c> altrimenti.
        /// </returns>
        public bool Contains( string name )
        {
            return this.m_owners.Contains( name );
        }

        /// <summary>
        /// Restituisce un <see cref="System.String"/> che contiene la lista completa dei proprietari
        /// </summary>
        /// <returns>
        /// Un oggetto <see cref="System.String"/> che contiene la lista completa dei proprietari
        /// </returns>
        public override string ToString()
        {
            string output = "";

            foreach( string owner in this.m_owners )
            {
                output += owner + ", ";
            }
            output = output.Substring( 0, output.Length - 2 );

            return output;
        }
    }
}