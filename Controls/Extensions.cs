using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using IndianaPark.Tools.Logging;

namespace IndianaPark.Tools
{
    /// <summary>
    /// Classe statica contenente gli Extension Methods personali
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Trova la chiave associata ad un valore in un oggetto <see cref="IDictionary&lt;TKey, TValue&gt;"/>
        /// </summary>
        /// <typeparam name="TKey">Tipo delle chiavi nel dizionario.</typeparam>
        /// <typeparam name="TValue">Tipo dei valori nel dizionario.</typeparam>
        /// <param name="source">Dizionario in cui cercare</param>
        /// <param name="value">Il valore da cercare</param>
        /// <returns>La chiave appartenente al valorce passato, oppure <c>null</c> se non trovato.</returns>
        public static TKey KeyOf<TKey, TValue>( this IDictionary<TKey, TValue> source, TValue value )
        {
            return (from k in source where k.Value.Equals( value ) select k.Key).FirstOrDefault();
        }

        /// <summary>
        /// Controlla se un determinata istanza di un oggetto implementa l'interfaccia specificata
        /// </summary>
        /// <typeparam name="T">Il tipo di dati dell'oggetto da analizzare</typeparam>
        /// <param name="source">L'oggetto da controllare</param>
        /// <param name="interfaccia">L'interfaccia da ricercare</param>
        /// <returns>
        /// 	<c>true</c> se l'oggetto implementa l'interfaccia <paramref name="interfaccia"/>, <c>false</c> altrimenti
        /// </returns>
        /// <remarks>
        /// È possibile passare come <paramref name="source"/> sia un'istanza di un oggetto ma anche un oggetto
        /// di tipo <see cref="Type"/>. In questo caso viene cercato in <see cref="Type.GetInterfaces"/> se è presente
        /// l'interfaccia da cercare.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se non viene specificata nessuna interfaccia.</exception>
        public static bool ImplementsInterface<T>( this T source, Type interfaccia )
        {
            if( interfaccia == null )
            {
                throw new ArgumentNullException( "interfaccia", "You must specify the interface to look for" );
            }

            // Se non ho passato un'interfaccia esco
            if( !interfaccia.IsInterface )
            {
                return false;
            }

            // Se ho passato un oggetto Type, allora lo considero come informazioni sull'istanza
            if( source is Type )
            {
                var tmp = source as Type;
                return tmp.GetInterface( interfaccia.ToString(), false ) != null;
            }

            return source.GetType().GetInterface( interfaccia.ToString(), false ) != null;
        }

        /// <summary>
        /// Controlla se un determinata istanza di un oggetto è un oggetto di un altro tipo
        /// </summary>
        /// <typeparam name="T">Il tipo di dati dell'oggetto da analizzare</typeparam>
        /// <param name="source">L'oggetto da controllare</param>
        /// <param name="oggetto">L'oggetto da ricercare</param>
        /// <returns>
        /// 	<c>true</c> se l'oggetto deriva, in un qualche livello, da <paramref name="oggetto"/>, <c>false</c> altrimenti
        /// </returns>
        /// <remarks>
        /// 	<para>Si intende se nella catena di ereditarietà dell'oggetto <paramref name="source"/> è presente un oggetto di
        /// tipo <paramref name="oggetto"/>.</para>
        /// 	<para>È possibile passare come <paramref name="source"/> sia un'istanza di un oggetto ma anche un oggetto
        /// di tipo <see cref="Type"/>. In questo caso viene cercato in <see cref="Type.GetInterfaces"/> se è presente
        /// l'oggetto da cercare.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se non viene specificata nessuna interfaccia.</exception>
        public static bool IsA<T>( this T source, Type oggetto )
        {
            if( oggetto == null )
            {
                throw new ArgumentNullException( "oggetto", "You must specify the object to look for" );
            }

            // Se non ho passato un'oggetto esco
            if( !oggetto.IsClass )
            {
                return false;
            }

            // Se ho passato un oggetto Type, allora lo considero come informazioni sull'istanza
            if( source is Type )
            {
                var tSource = source as Type;
                return tSource.IsAssignableFrom( oggetto ) || oggetto.IsAssignableFrom( tSource );
            }

            return source.GetType().IsAssignableFrom( oggetto ) || oggetto.IsAssignableFrom( source.GetType() );
        }

        /// <summary>
        /// Serializza un oggetto in un array di bytes
        /// </summary>
        /// <param name="obj">L'oggetto da convertire</param>
        /// <returns>
        /// Un <c>byte[]</c> contente <paramref name="obj"/> serializzato o <c>null</c> se l'oggetto non è
        /// serializzabile.
        /// </returns>
        public static byte[] ToByteArray( this Object obj )
        {
            try
            {
                var bf = new BinaryFormatter();
                using( var ms = new MemoryStream() )
                {
                    bf.Serialize( ms, obj );
                    return ms.ToArray();
                }
            }
            catch( SerializationException )
            {
                return null;
            }
        }

        /// <summary>
        /// Converte un array di bytes in un oggetto
        /// </summary>
        /// <param name="arrBytes">L'array di bytes da convertire</param>
        /// <returns>
        /// Restituisce l'oggetto contenuto in <paramref name="arrBytes"/>, oppure <c>null</c> in caso
        /// di problemi di deserializzazione
        /// </returns>
        public static T ToObject<T>( this byte[] arrBytes )
        {
            try
            {
                using( var memStream = new MemoryStream() )
                {
                    var binForm = new BinaryFormatter();
                    memStream.Write( arrBytes, 0, arrBytes.Length );
                    memStream.Seek( 0, SeekOrigin.Begin );

                    var data = binForm.Deserialize( memStream );
                    if( !(data is T ) )
                    {
                        return default( T );
                    }

                    return (T)data;
                }
            }
            catch( SerializationException )
            {
                return default( T );
            }
        }

        /// <summary>
        /// Calcola il checksum di un file con l'algoritmo hash specificato
        /// </summary>
        /// <param name="stream">L'oggetto FileStream che rappresenta il file</param>
        /// <param name="hash">L'algoritmo HASH da utilizzare</param>
        /// <exception cref="ArgumentNullException">Se viene passato come hash il valore <c>null</c>.</exception>
        /// <returns>Il valore dell'hash calcolato su tutto lo stream</returns>
        public static string Checksum( this FileStream stream, HashAlgorithm hash )
        {
            if( hash == null )
            {
                throw new ArgumentNullException( "You must specify an hash algorithm", "hash" );
            }

            if( stream.CanRead )
            {
                var checksum = hash.ComputeHash( stream );
                return BitConverter.ToString( checksum ).Replace( "-", String.Empty );
            }

            return null;
        }
    }
}

namespace IndianaPark.Tools.Xml
{
    /// <summary>
    /// Classe statica contenente gli Extension Methods personali riguardanti XML
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Serializza un qualsiasi oggetto in XML
        /// </summary>
        /// <typeparam name="T">Il tipo di oggetto da serializzare</typeparam>
        /// <param name="data">L'oggetto da serializzare</param>
        /// <remarks>
        /// <para>Il codice XML viene codificato in formato UTF-16 e formattato secondo comuni regole di leggibilità.</para>
        /// <para>In caso di problemi di serializzazione questo metodo non solleva eccezioni ma restituisce un valore <c>null</c>.</para>
        /// <para>Prestare attenzione durante il debug.</para>
        /// </remarks>
        /// <returns>Una stringa contenente il codice XML dell'oggetto <paramref name="data"/> serializzato</returns>
        public static string ToXml<T>( this T data )
        {
            try
            {
                // Creo il serializzatore
                var xmls = new XmlSerializer( data.GetType() );

                // Scrivo su un MemoryStream per avere più versatilità
                using( var ms = new MemoryStream() )
                {
                    var settings = new XmlWriterSettings
                    {
                        Encoding = Encoding.Unicode,
                        Indent = true,
                        IndentChars = "  ",
                        NewLineChars = Environment.NewLine,
                        ConformanceLevel = ConformanceLevel.Document
                    };

                    using( var writer = XmlWriter.Create( ms, settings ) )
                    {
                        // Bug di XmlSerializer con TimeSpan
                        if( data is TimeSpan )
                        {
                            xmls = new XmlSerializer( typeof( long ) );
                            xmls.Serialize( writer, ((TimeSpan)((object)data)).Ticks );
                        }
                        else
                        {
                            xmls.Serialize( writer, data );
                        }
                    }

                    // Può succedere che la stringa inizi con dei caratteri non desiderati. Li elimino cercando l'inizio dell'XML
                    var cleaner = settings.Encoding.GetString( ms.ToArray() ).Trim();
                    return cleaner.Substring( cleaner.IndexOf( "<?xml" ) );
                }
            }
            catch( InvalidOperationException ioex )
            {
                Logger.Default.Write( ioex, "Exception while serializing an object" );
                return null;
            }
        }

        /// <summary>
        /// Serializza un qualsiasi oggetto in un oggetto XmlDocument
        /// </summary>
        /// <typeparam name="T">Il tipo di oggetto da serializzare</typeparam>
        /// <param name="data">L'oggetto da serializzare</param>
        /// <remarks>
        /// <para>In caso di problemi di serializzazione questo metodo non solleva eccezioni ma restituisce un valore <c>null</c>.</para>
        /// <para>Prestare attenzione durante il debug.</para>
        /// </remarks>
        /// <returns>Un'istanza di XmlDocument contenente il codice XML dell'oggetto <paramref name="data"/> serializzato</returns>
        public static XmlDocument ToXmlDocument<T>( this T data )
        {
            // Converto in stringa XML (quindi in UTF-16)
            var xml = data.ToXml();
            if( string.IsNullOrEmpty( xml ) )
            {
                return null;
            }

            using( var ms = new MemoryStream( Encoding.Unicode.GetBytes( xml ) ) )
            {
                var doc = new XmlDocument();
                doc.Load( ms );
                return doc;
            }
        }

        /// <summary>
        /// Deserializza il codice XML in un oggetto
        /// </summary>
        /// <typeparam name="T">Il tipo di oggetto contenuto nell'XML</typeparam>
        /// <param name="data">Il codice XML dell'oggetto serializzato</param>
        /// <remarks>
        /// <para>In caso di problemi di deserializzazione questo metodo non solleva eccezioni ma restituisce un valore <c>null</c>.</para>
        /// <para>In ultima analisi questo metodo fa uso di <see cref="DeserializeXml(string, Type)"/>. Prestare attenzione</para>
        /// durante il debug.
        /// </remarks>
        /// <returns>
        /// Un'istanza dell'oggetto costruita deserializzando l'XML, oppure <c>null</c> in caso non sia stato possibile
        /// deserializzare l'oggetto.
        /// </returns>
        public static T DeserializeXml<T>( this string data )
        {
            object output = null;

            try
            {
                output = data.DeserializeXml( typeof( T ) );
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Exception while deserializing an XML String" );
            }

            if( output == null )
            {
                return default( T );
            }

            return (T)output;
        }

        /// <summary>
        /// Deserializza il codice XML in un oggetto
        /// </summary>
        /// <param name="data">Il codice XML dell'oggetto serializzato</param>
        /// <param name="type">Il tipo del dato serializzato.</param>
        /// <remarks>
        /// Questa procedura utilizza il metodo <see cref="IsValidXML"/> per controllare la validità del codice XML e di
        /// <see cref="XmlSerializer.CanDeserialize"/>.
        /// </remarks>
        /// <exception cref="ArgumentException">Se <paramref name="data"/> non contiene del codice XML valido.</exception>
        /// <returns>
        /// Un'istanza dell'oggetto costruita deserializzando l'XML
        /// </returns>
        public static object DeserializeXml( this string data, Type type )
        {
            using( var reader = new XmlTextReader( new MemoryStream( Encoding.Unicode.GetBytes( data ) ) ) )
            {
                // Controlla se è un codice XML valido
                if( !data.IsValidXML() )
                {
                    throw new ArgumentException( "The string doesn't contain valid XML", "data" );
                }
                
                var serializer = new XmlSerializer( type );
                
                // Bug di XmlSerializer con TimeSpan
                if( type == typeof( TimeSpan ) )
                {
                    serializer = new XmlSerializer( typeof( long ) );
                    return TimeSpan.FromTicks( (long)serializer.Deserialize( reader ) );
                }

                // Deserializzo
                if( serializer.CanDeserialize( reader ) )
                {
                    return serializer.Deserialize( reader );
                }
                else
                {
                    throw new ArgumentException( "Is not possible to deserialize the string" );
                }
            }
        }

        /// <summary>
        /// Deserializza il codice XML in un oggetto
        /// </summary>
        /// <typeparam name="T">Il tipo dell'oggetto memorizzato nell'XML</typeparam>
        /// <param name="data">Un oggetto XmlDocument contenente il codice XML dell'oggetto serializzato</param>
        /// <returns>
        /// Un'istanza dell'oggetto costruita deserializzando l'XmlDocument oppure <c>null</c> in caso non sia stato
        /// possibile deserializzare l'oggetto.
        /// </returns>
        public static T Deserialize<T>( this XmlDocument data )
        {
            var output = data.Deserialize( typeof( T ) );

            if( output == null )
            {
                return default( T );
            }

            return (T)output;
        }

        /// <summary>
        /// Deserializza il codice XML in un oggetto
        /// </summary>
        /// <param name="data">Un oggetto XmlDocument contenente il codice XML dell'oggetto serializzato</param>
        /// <param name="type">Il tipo del dato serializzato.</param>
        /// <remarks>
        /// <para>In caso di problemi di deserializzazione questo metodo non solleva eccezioni ma restituisce un valore <c>null</c>.</para>
        /// <para>In ultima analisi questo metodo fa uso di <see cref="DeserializeXml(string, Type)"/>.</para>
        /// <para>Prestare attenzione durante il debug.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Se non viene passato <paramref name="type"/></exception>
        /// <returns>Un'istanza dell'oggetto costruita deserializzando l'XmlDocument, oppure <c>null</c> in caso di 
        /// problemi di deserializzazione</returns>
        public static object Deserialize( this XmlDocument data, Type type )
        {
            if( type == null )
            {
                throw new ArgumentNullException( "type", "You must specify a type for the deserialized object" );
            }

            using( var sw = new StringWriter() )
            {
                var xtw = new XmlTextWriter( sw )
                {
                    Formatting = Formatting.Indented,
                    IndentChar = ' ',
                    Indentation = 2
                };

                data.WriteTo( xtw );
                var xml = sw.ToString();

                return xml.DeserializeXml( type );
            }
        }

        /// <summary>
        /// Controlla se il testo contiene del codice XML Well-Formed
        /// </summary>
        /// <param name="value">La stringa da controllare</param>
        /// <returns><c>true</c> se la stringa contiene un codice XML Well Formed, <c>false</c> altrimenti.</returns>
        public static bool IsValidXML( this string value )
        {
            // Controllo se è un XML valido provando a deserializzarlo. Modo migliore per tenere conto della codifica del testo
            try
            {
                using( var reader = new XmlTextReader( new MemoryStream( Encoding.Unicode.GetBytes( value ) ) ) )
                {
                    var serializer = new XmlSerializer( typeof(object) );
                    serializer.CanDeserialize( reader );
                }

                return true;
            }
            catch( Exception )
            {
                // In caso di una qualunque eccezione significa che non è possibile deserializzare, quindi che non è XML
                return false;
            }
        }

        /// <summary>
        /// Salva l'oggetto <see cref="XmlDocument"/> in un file utilizzando il formato standard di formattazione exception
        /// una codifica UTF-16
        /// </summary>
        /// <param name="data">L'oggetto da salvare.</param>
        /// <param name="fileName">Il percorso del file in cui salvare il documento</param>
        /// <returns><c>true</c> se il salvataggio va a buon fine, <c>false</c> altrimenti</returns>
        public static bool SaveFormatted( this XmlDocument data, string fileName )
        {
            try
            {
                // Tento il salvataggio del documento
                using( var xtw = new XmlTextWriter( fileName, Encoding.Unicode ) )
                {
                    xtw.Formatting = Formatting.Indented;
                    xtw.IndentChar = ' ';
                    xtw.Indentation = 2;
                    data.WriteContentTo( xtw );
                    xtw.Flush();
                    xtw.Close();
                }
            }
            catch( Exception ex )
            {
                Logger.Default.Write( ex, "Exception while saving an XML Document" );
                return false;
            }

            return true;
        }
    }
}
