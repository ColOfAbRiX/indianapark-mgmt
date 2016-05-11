using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace IndianaPark.Tools.Security
{
    /// <summary>
    /// Extension Methods for Security
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Controlla se l'algoritmo RSA possiede una chiave privata
        /// </summary>
        /// <param name="alg">The alg.</param>
        /// <returns>
        /// 	<c>true</c> se l'algoritmo dispone anche di una chiave privata, <c>false</c> se dispone
        ///     solo di una chiave pubblica.
        /// </returns>
        public static bool PossesPrivateKey( this RSA alg )
        {
            // Provo ad esportare anche la chiave privata. Se non ci riesco vuol dire che non c'è
            try
            {
                var param = alg.ExportParameters( true );
                if( param.D == null )
                {
                    return false;
                }
                return true;
            }
            catch( CryptographicException )
            {
                return false;
            }
        }

        /// <summary>
        /// Verify the signature of an XmlDocument against an asymmetric algorithm and return the result.
        /// </summary>
        /// <param name="doc">Il documento da verificare.</param>
        /// <param name="key">L'algoritmo di crittografia</param>
        /// <param name="remove">Indica se rimuovere la signature dal documento</param>
        /// <returns>
        /// 	<c>true</c> if the XML is correctly signed, <c>false</c> otherwise
        /// </returns>
        /// <remarks>
        /// 	<para>Reference: http://msdn.microsoft.com/en-us/library/ms229950(v=VS.90).aspx</para>
        /// 	<para>Il documento <paramref name="doc"/> deve essere conforme allo standard W3C http://www.w3.org/TR/xmldsig-core/</para>
        /// </remarks>
        public static bool VerifySignature( this XmlDocument doc, AsymmetricAlgorithm key, bool remove )
        {
            // Check arguments.
            if( doc == null )
            {
                throw new ArgumentNullException( "doc", "You must specify a document where to verify signature" );
            }
            if( key == null )
            {
                throw new ArgumentNullException( "key", "You must specify the unsigning key to verify signature" );
            }

            // Create a new SignedXml object and pass it
            // the XML document class.
            var signedXml = new SignedXml( doc );

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            var nodeList = doc.GetElementsByTagName( "Signature" );

            // Return false if no signature was found.
            if( nodeList.Count <= 0 )
            {
                return false;
            }

            // This example only supports one signature for
            // the entire XML document.  Return false
            // if more than one signature was found.
            if( nodeList.Count >= 2 )
            {
                return false;
            }

            // Load the first <signature> node.  
            signedXml.LoadXml( (XmlElement)nodeList[0] );

            // Check the signature and return the result.
            var output = signedXml.CheckSignature( key );

            if( remove && doc.DocumentElement != null )
            {
                doc.DocumentElement.RemoveChild( doc.GetElementsByTagName( "Signature" )[0] );
            }

            return output;
        }

        /// <summary>
        /// Sign an XmlDocument with an asymmetric algorithm.
        /// </summary>
        /// <param name="doc">Il documento da firmare</param>
        /// <param name="key">L'algoritmo di crittografia</param>
        /// <returns>
        /// 	<c>true</c> se la firma è stata applicata con successo, <c>false</c> altrimenti
        /// </returns>
        /// <remarks>
        /// 	<para>Reference: http://msdn.microsoft.com/en-us/library/ms229745(v=VS.90).aspx</para>
        /// 	<para>L'algoritmo specificato in <paramref name="key"/> deve possedere una chiave privata.</para>
        /// 	<para>Il documento <paramref name="doc"/> deve essere conforme allo standard W3C http://www.w3.org/TR/xmldsig-core/</para>
        /// </remarks>
        public static bool ApplySignature( this XmlDocument doc, AsymmetricAlgorithm key )
        {
            // Check arguments.
            if( doc == null )
            {
                throw new ArgumentNullException( "doc", "You must specify a document where to apply signature" );
            }
            if( key == null )
            {
                throw new ArgumentNullException( "key", "You must specify the signing key to apply signature" );
            }

            try
            {
                // Create a SignedXml object.
                var signedXml = new SignedXml( doc ) { SigningKey = key };

                // Create a reference to be signed.
                var reference = new Reference { Uri = "" };

                // Add an enveloped transformation to the reference.
                var env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform( env );

                // Add the reference to the SignedXml object.
                signedXml.AddReference( reference );

                // Compute the signature.
                signedXml.ComputeSignature();

                // Get the XML representation of the signature and save
                // it to an XmlElement object.
                var xmlDigitalSignature = signedXml.GetXml();

                // Append the element to the XML document.
                if( doc.DocumentElement != null )
                {
                    doc.DocumentElement.AppendChild( doc.ImportNode( xmlDigitalSignature, true ) );
                }
            }
            catch( Exception )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Encrypts an XmlDocument with the specified RSA algorithm
        /// </summary>
        /// <param name="doc">Il documento XmlDocument da crittografare</param>
        /// <param name="alg">L'algoritmo RSA da utilizzare per crittografare il documento.</param>
        /// <returns>
        /// 	<c>true</c> se la crittografia del documento è andata a buon fine, <c>false</c> altrimenti.
        /// </returns>
        /// <remarks>
        /// 	<para>Reference: http://msdn.microsoft.com/en-us/library/ms229746(v=VS.90).aspx</para>
        /// 	<para>Il parametro <paramref name="doc"/> viene modificato seguendo lo standard W3C http://www.w3.org/TR/xmlenc-core/</para>
        /// </remarks>
        public static bool Encrypt( this XmlDocument doc, RSA alg )
        {
            // Check the arguments.
            if( doc == null )
            {
                throw new ArgumentNullException( "doc", "You must specify the document to encrypt" );
            }
            if( alg == null )
            {
                throw new ArgumentNullException( "alg", "You must specify the crypting key to encript the document" );
            }

            // Find the specified element in the XmlDocument object and create a new XmlElemnt object.
            var elementToEncrypt = doc.DocumentElement;
            if( elementToEncrypt == null )
            {
                return false;
            }

            SymmetricAlgorithm sessionKey = null;

            try
            {
                // Create a 256 bit Rijndael key.
                sessionKey = new AesManaged { KeySize = 256 };


                // Encrypt the element and add it to an EncryptedData element.
                var encryptedElement = new EncryptedXml().EncryptData( elementToEncrypt, sessionKey, false );

                // Construct an EncryptedData object and populate it with the desired encryption information.
                var edElement = new EncryptedData
                {
                    Type = EncryptedXml.XmlEncElementUrl,
                    Id = elementToEncrypt.GetHashCode().ToString(),
                    /* Create an EncryptionMethod element so that the receiver knows which algorithm to use for decryption.*/
                    EncryptionMethod = new EncryptionMethod( EncryptedXml.XmlEncAES256Url )
                };

                // Add the encrypted element data to the EncryptedData object.
                edElement.CipherData.CipherValue = encryptedElement;


                // Encrypt the session key and add it to an EncryptedKey element.
                var encryptedKey = EncryptedXml.EncryptKey( sessionKey.Key, alg, false );

                // Construct an EncryptedKey object and populate it with the desired encryption information.
                var ek = new EncryptedKey { EncryptionMethod = new EncryptionMethod( EncryptedXml.XmlEncRSA15Url ) };
                // Add the encrypted key to the EncryptedKey object.
                ek.CipherData.CipherValue = encryptedKey;

                // Create a new DataReference element for the KeyInfo element. This optional
                // element specifies which EncryptedData uses this key. An XML document can have
                // multiple EncryptedData elements that use different keys.
                var dRef = new DataReference
                {
                    /* Specify the EncryptedData URI.*/
                    Uri = "#" + elementToEncrypt.GetHashCode()
                };
                // Add the DataReference to the EncryptedKey.
                ek.AddReference( dRef );

                // Set the KeyInfo element to specify the name of the RSA key.
                var kin = new KeyInfoName
                {
                    /* Specify a name for the key.*/
                    Value = "Key"
                };

                // Add the KeyInfoName element to the EncryptedKey object.
                ek.KeyInfo.AddClause( kin );

                // Add the encrypted key to the EncryptedData object.
                edElement.KeyInfo.AddClause( new KeyInfoEncryptedKey( ek ) );


                // Replace the element from the original XmlDocument
                // object with the EncryptedData element.
                EncryptedXml.ReplaceElement( elementToEncrypt, edElement, false );
            }
            catch( Exception )
            {
                return false;
            }
            finally
            {
                if( sessionKey != null )
                {
                    sessionKey.Clear();
                }
            }

            return true;
        }

        /// <summary>
        /// Decrypts the specified XmlDocument
        /// </summary>
        /// <param name="doc">Il documento XmlDocument da decrittografare.</param>
        /// <param name="alg">L'algoritmo RSA da utilizzare per decrittografare il documento.</param>
        /// <returns>
        /// 	<c>true</c> se la decrittografia del documento è andata a buon fine, <c>false</c> altrimenti.
        /// </returns>
        /// <remarks>
        /// 	<para>Reference: http://msdn.microsoft.com/en-us/library/ms229919(v=VS.90).aspx</para>
        /// 	<para>L'algoritmo specificato in <paramref name="alg"/> deve possedere una chiave privata.</para>
        /// 	<para>Il documento <paramref name="doc"/> deve essere conforme allo standard W3C http://www.w3.org/TR/xmlenc-core/</para>
        /// </remarks>
        public static bool Decrypt( this XmlDocument doc, RSA alg )
        {
            // Check the arguments.
            if( doc == null )
            {
                throw new ArgumentNullException( "doc", "You must specify the document to deencrypt" );
            }
            if( alg == null )
            {
                throw new ArgumentNullException( "alg", "You must specify the decrypting key to deencript the document" );
            }

            // Copio i dati per non toccare gli originali
            var workset = (XmlDocument)doc.Clone();
            if( workset.DocumentElement == null )
            {
                throw new ArgumentNullException( "doc", "The document you specified doesn't contain a valid Document Element" );
            }

            // Create a new EncryptedXml object.
            var eXml = new EncryptedXml( workset );

            // Add a key-name mapping.
            // This method can only decrypt documents that present the specified key name.
            eXml.AddKeyNameMapping( "Key", alg );

            try
            {
                // Decrypt the element.
                eXml.DecryptDocument();
            }
            catch( CryptographicException )
            {
                return false;
            }

            // Sostituisco l'XmlElement crittografato con quello in chiaro (a causa di un bug di EncryptedXml
            doc.ReplaceChild( doc.ImportNode( workset.DocumentElement, true ), doc.DocumentElement );

            return true;
        }
    }
}
