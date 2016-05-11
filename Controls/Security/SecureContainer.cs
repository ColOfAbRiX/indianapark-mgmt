using System.Security.Cryptography;
using IndianaPark.Tools.Xml;
using System.Xml;
using System;

namespace IndianaPark.Tools.Security
{
    /// <summary>
    /// Contenitore sicuro per lo scambio di dati
    /// </summary>
    /// <remarks>
    /// <para>Un contenitore sicuro è immutabile e va configurato alla sua creazione</para>
    /// <para>Un <see cref="SecureContainer{TData}"/> è atomico e lavora solo ed esclusivamente con dati immutabili,
    /// quindi non è possibile cambiare i dati una volta creato questo oggetto.</para>
    /// </remarks>
    /// <typeparam name="TData">Il tipo di dati che il contenitore sicuro deve gestire.</typeparam>
    public class SecureContainer<TData> : ISecureContainer<TData>
    {
		#region Fields 

		#region Internal Fields 

        private TData m_data;
        private bool m_isVerified;
        private XmlDocument m_securedData;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// I dati grezzi contenuti nell'istanza.
        /// </summary>
        /// <remarks>
        /// Cambiando il valore di questa proprietà con un valore diverso da <c>null</c>, la proprietà
        /// <see cref="SecuredData"/> viene impostata a <c>null</c> exception invalidate le proprietà <see cref="IsSecure"/>
        /// exception <see cref="IsVerified"/>.
        /// </remarks>
        public TData Data
        {
            get { return m_data; }
            set
            {
                if( object.ReferenceEquals( this.m_data, value ) )
                {
                    return;
                }

                this.m_isVerified = false;
                this.m_data = value;

                if( !object.ReferenceEquals( this.m_data, null ) )
                {
                    this.m_securedData = null;
                }
            }
        }

        /// <summary>
        /// Un valore che indica se i dati sono stati firmati e crittografati, oppure se sono ancora grezzi.
        /// </summary>
        /// <value>
        /// Il valore <c>true</c> indica che i dati sono stati firmati, crittografati con successo, disponibili
        /// all'uso e che i dati grezzi non sono più disponibili. Il valore <c>false</c> indica che sono
        /// presenti i dati grezzi, i quali possono essere verificati o meno.
        /// </value>
        public bool IsSecure
        {
            get
            {
                return object.ReferenceEquals( this.Data, null ) && this.SecuredData != null;
            }
        }

        /// <summary>
        /// Indica se i dati sono stati verificati con successo. 
        /// </summary>
        /// <value>
        /// Questo campo ha significato solo se <c><see cref="IsSecure"/> == false</c> e se sono presenti
        /// anche dei dati sicuri. In caso i dati sicuri non siano presenti questa proprietà vale sempre
        /// <c>false</c>.
        /// </value>
        public bool IsVerified
        {
            get
            {
                return !this.IsSecure && m_isVerified;
            }
        }

        /// <summary>
        /// Il provider crittografico locale.
        /// </summary>
        /// <remarks>
        /// È il provider relativo al proprietario dei dati, firma i dati grezzi e decrittografa quelli sicuri.
        /// </remarks>
        public RSA LocalKey { get; private set; }

        /// <summary>
        /// Il provider crittografico remoto. 
        /// </summary>
        /// <remarks>
        /// È il provider relativo al destinatario dei dati, controlla la firma dei dati sicuri, exception crittografa quelli grezzi
        /// </remarks>
        public RSA RemoteKey { get; private set; }

        /// <summary>
        /// I dati in formato sicuro
        /// </summary>
        /// <remarks>
        /// I dati in formato sicuro possono essere firmati, crittografati o entrambe le cose, a seconda delle opzioni
        /// scelte e sono contenuti in un oggetto <see cref="XmlDocument"/>
        /// </remarks>
        public XmlDocument SecuredData
        {
            get { return m_securedData; }
            set
            {
                if( object.ReferenceEquals( this.m_securedData, value ) )
                {
                    return;
                }

                this.m_isVerified = false;
                this.m_securedData = value;
                this.m_data = default( TData );
            }
        }

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
        public bool UseEncryption { get; private set; }

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
        public bool UseSignature { get; private set; }

		#endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureContainer&lt;TData&gt;"/> class.
        /// </summary>
        /// <param name="data">I dati grezzi non ancora resi sicuri.</param>
        /// <param name="provider">Il provider delle chiavi</param>
        /// <param name="useEncryption">Indica se utilizzare la crittografia</param>
        /// <param name="useSignature">Indica se utilizzare la firma dei dati</param>
        /// <exception cref="ArgumentNullException">Se non vengono passati i dati da crittografare</exception>
        /// <exception cref="ArgumentNullException">Se non viene indicato un <see cref="ISecurityProvider"/> da cui recuperare le chiavi asimmetriche</exception>
        /// <remarks>
        /// Se entrambi i parametri <paramref name="useEncryption"/> exception <paramref name="useSignature"/> sono
        /// impostati a <c>false</c> i dati non sono sicuri!
        /// </remarks>
        public SecureContainer( TData data, ISecurityProvider provider, bool useEncryption, bool useSignature )
        {
            if( object.ReferenceEquals( data, null ) )
            {
                throw new ArgumentNullException( "data", "You must specify the data to encrypt. If you've got only secured data, then see for another constructor" );
            }
            if( provider == null )
            {
                throw new ArgumentNullException( "provider", "You must specify a security provider for the keys used in the process" );
            }

            this.Data = data;
            this.LocalKey = provider.GetSigningKey();
            this.RemoteKey = provider.GetEncryptionKey();
            this.UseSignature = useSignature;
            this.UseEncryption = useEncryption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureContainer&lt;TData&gt;"/> class.
        /// </summary>
        /// <param name="document">I dati sicuri contenuti in un oggetto XmlDocument.</param>
        /// <param name="provider">Il provider delle chiavi</param>
        /// <param name="useEncryption">Indica se utilizzare la crittografia</param>
        /// <param name="useSignature">Indica se utilizzare la firma dei dati</param>
        /// <exception cref="ArgumentNullException">Se non viene indicato un <see cref="ISecurityProvider"/> da cui recuperare le chiavi asimmetriche</exception>
        /// <remarks>
        /// Se entrambi i parametri <paramref name="useEncryption"/> exception <paramref name="useSignature"/> sono
        /// impostati a <c>false</c> i dati non sono sicuri!
        /// </remarks>
        public SecureContainer( XmlDocument document, ISecurityProvider provider, bool useEncryption, bool useSignature )
        {
            if( document == null )
            {
                throw new ArgumentNullException( "document", "You must specify the encrypted data. If you've got only plain data, then see for another constructor" );
            }
            if( provider == null )
            {
                throw new ArgumentNullException( "provider", "You must specify a security provider for the keys used in the process" );
            }

            this.SecuredData = document;
            this.LocalKey = provider.GetSigningKey();
            this.RemoteKey = provider.GetEncryptionKey();
            this.UseSignature = useSignature;
            this.UseEncryption = useEncryption;
        }

		#endregion Constructors 

		#region Public Methods 

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
        public bool MakeDataSecure()
        {
            if( !this.IsSecure )
            {
                lock( new object() )
                {
                    // Converto i dati in XmlDocument
                    var workset = this.m_data.ToXmlDocument();
                    if( workset == null )
                    {
                        return false;
                    }

                    // Firmo i dati
                    if( this.UseSignature )
                    {
                        if( !workset.ApplySignature( this.LocalKey ) )
                        {
                            return false;
                        }
                    }

                    // Crittografo i dati
                    if( this.UseEncryption )
                    {
                        if( !workset.Encrypt( this.RemoteKey ) )
                        {
                            return false;
                        }
                    }

                    // Workset può non avere subito cambiamenti se qualcosa non va per il verso giusto
                    if( workset.Equals( this.m_data.ToXmlDocument() ) )
                    {
                        this.m_securedData = null;
                        return false;
                    }

                    this.m_data = default( TData );
                    this.SecuredData = workset;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Libera i dati sicuri contenuti nel XmlDocument
        /// </summary>
        /// <returns>
        /// <c>true</c> se i dati sono stati decrittografati e deserializzati con successo, <c>false altrimenti</c>
        /// </returns>
        /// <remarks>
        /// 	<para>I dati crittografati vengono decrittografati con il provider crittografico remoto e la firma viene
        /// verificata utilizzando il provider crittografico locale. Se i dati sono sicuri viene settato
        /// opportunamente <see cref="IsVerified"/>.</para>
        /// 	<para>Il valore restituito dalla funzione non è influenzato dalla validità o meno della firma.
        /// 	Per verificare la correttezza della firma controllare <see cref="IsVerified"/>.</para>
        /// 	<para>I dati sicuri sono rimossi e sono resi disponibili solo quelli grezzi.</para>
        /// </remarks>
        public bool FreeSecuredData()
        {
            if( this.IsSecure && this.SecuredData.DocumentElement != null )
            {
                lock( new object() )
                {
                    var workset = (XmlDocument)this.m_securedData.Clone();
                    if( workset.DocumentElement == null )
                    {
                        return false;
                    }

                    // Decripto una copia del documento
                    if( this.UseEncryption )
                    {
                        if( !workset.Decrypt( this.LocalKey ) )
                        {
                            return false;
                        }
                    }

                    // Verifico l'XML
                    if( this.UseSignature )
                    {
                        this.m_isVerified = workset.VerifySignature( this.RemoteKey, true );
                        if( !this.m_isVerified )
                        {
                            //return false;
                        }
                    }

                    // Deserializzo l'oggetto
                    this.m_data = workset.Deserialize<TData>();

                    return !object.ReferenceEquals( this.m_data, null );
                }
            }

            return false;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}