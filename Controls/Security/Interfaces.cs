using System.Security.Cryptography;
using System.Xml;

namespace IndianaPark.Tools.Security
{
    /// <summary>
    /// Interfaccia che rappresenta un contenitore sicuro per lo scambio di dati
    /// </summary>
    /// <typeparam name="TData">Il tipo di dati che il contenitore sicuro deve gestire.</typeparam>
    public interface ISecureContainer<TData>
    {
        #region Fields 

        /// <summary>
        /// I dati grezzi contenuti nell'istanza.
        /// </summary>
        /// <remarks>
        /// I dati grezzi sono i dati che devono ancora essere elaborati, che devono essere firmati e crittografati.
        /// </remarks>
        TData Data { get; set; }

        /// <summary>
        /// Un valore che indica se i dati sono stati firmati e crittografati, oppure se sono ancora grezzi.
        /// </summary>
        /// <value>
        /// Il valore <c>true</c> indica che i dati sono stati firmati, crittografati con successo, disponibili
        /// all'uso e che i dati grezzi non sono più disponibili. Il valore <c>false</c> indica che sono
        /// presenti i dati grezzi, i quali possono essere verificati o meno.
        /// </value>
        bool IsSecure { get; }

        /// <summary>
        /// Indica se i dati sono stati verificati con successo. 
        /// </summary>
        /// <value>
        /// Questo campo ha significato solo se <c><see cref="IsSecure"/> == false</c> e se sono presenti
        /// anche dei dati sicuri. In caso i dati sicuri non siano presenti questa proprietà vale sempre
        /// <c>false</c>.
        /// </value>
        bool IsVerified { get; }

        /// <summary>
        /// Il provider crittografico locale.
        /// </summary>
        /// <remarks>
        /// È il provider relativo al proprietario dei dati, firma i dati grezzi e decrittografa quelli sicuri.
        /// </remarks>
        RSA LocalKey { get; }

        /// <summary>
        /// Il provider crittografico remoto. 
        /// </summary>
        /// <remarks>
        /// È il provider relativo al destinatario dei dati, controlla la firma dei dati sicuri, exception crittografa quelli grezzi
        /// </remarks>
        RSA RemoteKey { get; }

        /// <summary>
        /// I dati in formato sicuro
        /// </summary>
        /// <remarks>
        /// I dati in formato sicuro possono essere firmati, crittografati o entrambe le cose, a seconda delle opzioni
        /// scelte e sono contenuti in un oggetto <see cref="XmlDocument"/>
        /// </remarks>
        XmlDocument SecuredData { get; set; }

        /// <summary>
        /// Indica se i dati utilizzano la crittogravia
        /// </summary>
        /// <remarks>
        /// <para>Ovvero è l'impostazione che indica se i dati sono da crittografare se grezzi o se sono stati crittografati
        /// nel caso in cui <c><see cref="IsSecure"/> == true</c>.</para>
        /// <para>Nel caso <c><see cref="IsSecure"/> == true</c> viene utilizzata la chiave <see cref="RemoteKey"/> per le
        /// operazioni di crittografia.</para>
        /// </remarks>
        /// <value><c>true</c> se usano la crittografia, <c>false</c> altrimenti.</value>
        bool UseEncryption { get; }

        /// <summary>
        /// Indica se i dati utilizzano la firma
        /// </summary>
        /// <remarks>
        /// <para>Ovvero è l'impostazione che indica se i dati sono da firmare se grezzi o se sono stati firmati nel caso in
        /// cui <c><see cref="IsSecure"/> == true</c>.</para>
        /// <para>Nel caso <c><see cref="IsSecure"/> == true</c> viene utilizzata la chiave <see cref="LocalKey"/> per le
        /// operazioni di firma.</para>
        /// </remarks>
        /// <value><c>true</c> se usano la firma, <c>false</c> altrimenti.</value>
        bool UseSignature { get; }

        #endregion Fields 

        #region Methods 

        /// <summary>
        /// Rende i dati grezzi sicuri.
        /// </summary>
        /// <remarks>
        /// <para>I dati grezzi vengono serializzati in XML, a questi dati viene apposta una firma utilizzando il
        /// provider crittografico locale <see cref="LocalKey"/> e quindi il documento XML viene crittografato con il
        /// provider crittografico remoto <see cref="RemoteKey"/>.</para>
        /// <para>I dati grezzi vengono rimossi e sono disponibili solo quelli sicuri.</para>
        /// </remarks>
        /// <returns>
        /// <c>true</c> se i dati sono stati resi sicuri con successo, <c>false</c> altrimenti
        /// </returns>
        bool MakeDataSecure();

        /// <summary>
        /// Libera i dati sicuri contenuti nel XmlDocument
        /// </summary>
        /// <remarks>
        /// <para>I dati crittografati vengono decrittografati con il provider crittografico remoto e la firma viene
        /// verificata utilizzando il provider crittografico locale. Se i dati sono sicuri viene settato
        /// opportunamente <see cref="SecureContainer{TData}.IsVerified"/></para>
        /// <para>I dati sicuri sono rimossi e sono resi disponibili solo quelli grezzi</para>
        /// </remarks>
        /// <c>true</c> se i dati sono stati liberati con successo, <c>false</c> altrimenti
        bool FreeSecuredData();

        #endregion Methods 
    }

    /// <summary>
    /// Interfaccia che rappresenta l'oggetto che fornisce le chiavi di sicurezza ai <see cref="ISecureContainer&lt;TData&gt;"/>
    /// </summary>
    public interface ISecurityProvider
    {
        /// <summary>
        /// Recupera la chiave con cui si firmano i dati.
        /// </summary>
        /// <remarks>Deve essere presente anche la chiave privata</remarks>
        /// <returns>La chiave utilizzata per firmare i dati</returns>
        RSA GetSigningKey();

        /// <summary>
        /// Recupera la chiave con cui si verificano le firme dei dati
        /// </summary>
        /// <returns>La chiave utilizzata per verificare le firme dei dati</returns>
        RSA GetVerifyingKey();

        /// <summary>
        /// Recupera la chiave utilizzata per crittografare
        /// </summary>
        /// <returns>Un oggetto di tipo <see cref="RSA"/> utilizzato per crittografare i dati</returns>
        RSA GetEncryptionKey();

        /// <summary>
        /// Recupera la chiave utilizzata per decrittografare
        /// </summary>
        /// <remarks>Deve essere presente anche la chiave privata</remarks>
        /// <returns>Un oggetto di tipo <see cref="RSA"/> utilizzato per decrittografare i dati</returns>
        RSA GetDecryptionKey();
    }
}