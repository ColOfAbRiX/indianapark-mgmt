using System.Collections.Generic;

namespace IndianaPark.Plugin.Persistence
{
    /// <summary>
    /// Interfaccia che rappresenta la possibilità di salvare e caricare i dati da database
    /// </summary>
    public interface IConfigPersistence
    {
        /// <summary>
        /// Carica tutti i parametri dei plugin
        /// </summary>
        /// <param name="owner">Il nome del plugin di cui caricare i parametri</param>
        /// <returns>La lista con i valori di configurazione per il plugin</returns>
        IDictionary<string, IConfigValue> LoadAllParameters( string owner );

        /// <summary>
        /// Carica un parametro
        /// </summary>
        /// <param name="name">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui fa parte</param>
        /// <returns>Il valore di configurazione del plugin</returns>
        IConfigValue LoadParameter( string name, string owner );

        /// <summary>
        /// Salva un parametro
        /// </summary>
        /// <param name="config">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui fa parte</param>
        /// <returns>Il valore di configurazione del plugin</returns>
        bool SaveParameter( IConfigValue config, string owner );

        /// <summary>
        /// Salva tutti i parametri dei plugin
        /// </summary>
        /// <param name="configs">Il nome del parametro</param>
        /// <param name="owner">Il nome del plugin di cui caricare i parametri</param>
        /// <returns>La lista con i valori di configurazione per il plugin</returns>
        bool SaveAllParameters( IDictionary<string, IConfigValue> configs, string owner );
    }
}