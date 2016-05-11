using System;
using System.Collections.Generic;
using System.Linq;

namespace IndianaPark.PercorsiAvventura.Model
{
    /// <summary>
    /// Classe astratta che implementa le funzionalità degli sconti comitiva
    /// </summary>
    public abstract class ScontoComitiva : ScontoBase, IScontoComitiva
    {
        #region Methods

        #region  Constructor 

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="nome">Il nome dello sconto</param>
        /// <param name="sconto">Valore dello sconto fisso, non negativo</param>
        protected ScontoComitiva( string nome, double sconto )
        {
            if( String.IsNullOrEmpty( nome ) )
            {
                throw new ArgumentNullException( "nome" );
            }
            if( sconto < 0 )
            {
                throw new ArgumentOutOfRangeException( "sconto", "The parameter must be non-negative" );
            }

            this.m_sconto = sconto;
            this.m_nome = nome;
        }

        #endregion

        /// <summary>
        /// Crea una lista di applicazione di sconti relativa alla lista clienti indicata.
        /// La funzione tratta indistintamente tutti i clienti, ovvero applica gli sconti considerando tutta
        /// la lista in ingresso.
        /// </summary>
        /// <param name="clienti">La lista clienti a cui applicare gli sconti. Non deve essere modificata</param>
        /// <returns>
        /// Una lista di sconti comitiva, delle stesse dimensioni della lista clienti in ingresso, in cui ogni
        /// elemento è associato all'elemento con lo stesso indice nella lista clienti. Questa lista contiene
        /// gli sconti comitiva effettivamente applicati (quindi un riferimento allo sconto oppure null).
        /// </returns>
        protected abstract IList<IScontoComitiva> ApplicaSconto( IList<Cliente> clienti );

        /// <summary>
        /// La funzione determina il piano di sconti da applicare ad una lista clienti,
        /// cioè deve: determinare se applicare lo sconto comitiva, quali sconti
        /// eventualmente gia presenti non conteggiare exception, se richiesto, applicare gli
        /// sconti ai clienti che ne necessitano.
        /// Restituisce un array con indici associati alla lista clienti in ingresso; in
        /// ogni indice è presente o meno un riferimento allo sconto da applicare.
        /// </summary>
        /// <param name="listaClienti">
        /// La lista dei clienti utilizzata per determinare il piano di sconti. Si suppone che non sia stato applicato
        /// nessuno sconto comitiva
        /// </param>
        public List<Cliente> PianoSconti( List<Cliente> listaClienti )
        {
            List<Cliente>[] listaOutput = { new List<Cliente>() };

            // Creo due deep copy della lista in ingresso per non sporcarla
            listaClienti.ForEach( item => listaClienti[listaClienti.IndexOf( item )] = item.Clone() );
            listaClienti.ForEach( item => listaOutput[0].Add( item.Clone() ) );

            // Divido i clienti in due insiemi disgiunti: clienti con lo sconto exception clienti senza sconto
            var listaScontati = listaClienti.FindAll( obj => (obj.Sconto != null && obj.Sconto.CanOmit) ).ToList();
            var listaInteri = listaClienti.Except( listaScontati.AsEnumerable() ).ToList();

            // Ordino la lista dei clienti scontati in base ad un criterio specifico
            listaScontati.Sort( this.ClienteCompare );

            var prezzoMinimo = decimal.MaxValue;
            var migliorPiano = new List<IScontoComitiva>();

            while( true )
            {
                // Trovo la lista di sconti comitiva utilizzando i clienti con biglietto intero
                var listaSconti = this.ApplicaSconto( listaInteri ).ToList();

                // Calcolo il prezzo
                var prezzo = this.CalcolaPrezzoTotale( listaInteri, listaScontati, listaSconti );

                // Trovo la combinazione di clienti scontati/clienti interi che mi da il prezzo minimo
                if( prezzo < prezzoMinimo )
                {
                    prezzoMinimo = prezzo;
                    migliorPiano = listaSconti;
                }
                
                // Un elemento della lista scontati passa nella lista interi (exception quindi viene rimosso lo sconto)
                if( listaScontati.Count > 0 )
                {
                    listaScontati[0].Sconto = null;
                    listaInteri.Add( listaScontati[0] );
                    listaScontati.RemoveAt( 0 );
                }
                else if( listaScontati.Count == 0 )
                {
                    break;
                }
            }

            listaScontati = listaOutput[0].FindAll( obj => obj.Sconto != null );
            listaScontati.Sort( this.ClienteCompare );
            listaInteri = listaOutput[0].Except( listaScontati.AsEnumerable() ).ToList();

            // Unisco le due liste: in cima ci sono i prezzi interi, poi quelli scontati
            listaOutput[0] = listaInteri.Union( listaScontati ).ToList();

            // Applico il piano di sconti migliore alla lista clienti
            for( int i = 0; i < migliorPiano.Count; i++ )
            {
                listaOutput[0][i].ScontoComitiva = null;

                // Assegno il rispettivo ScontoComitiva
                if( migliorPiano[i] != null )
                {
                    listaOutput[0][i].Sconto = null;
                    listaOutput[0][i].ScontoComitiva = this;
                }

                // Annullo lo ScontoPersonale ai clienti che ancora sono nel piano migliore ma che sono segnati come Scontati
                if( listaInteri.Count <= i )    // ==> listaOutput[0].Count - listaScontati.Count => listaOutput[0].Count - i
                {
                    listaOutput[0][i].Sconto = null;
                }
            }

            // Riunisco le liste scontati exception interi
            return listaOutput[0];
        }

        /// <summary>
        /// Confronta due oggetti Cliente in base al prezzo personale. Viene utilizzata per determinare a chi
        /// togliere lo sconto personale in base a questo confronto.
        /// </summary>
        /// <param name="x">Il primo oggetto Cliente da confrontare</param>
        /// <param name="y">Il secondo oggetto Cliente da confrontare</param>
        /// <returns>
        /// Un numero minore di zero se il prezzo personale di X è minore di quello di Y, un numero
        /// strettamente positivo se il prezzo di X è maggiore di quello di Y, zero se sono uguali
        /// </returns>
        protected int ClienteCompare( Cliente x, Cliente y )
        {
            // In cima alla lista c'è il cliente che paga di più
            return Math.Sign( - (double)y.GetPrezzoPersonale() + (double)x.GetPrezzoPersonale() );
        }

        /// <summary>
        /// Funzione che calcola il prezzo effettivo che paga l'intera lista clienti applicandovi
        /// gli sconti comitiva indicati nella lista
        /// </summary>
        /// <param name="clientiInteri">La lista clienti con biglietto intero</param>
        /// <param name="clientiScontati">La lista clienti con sconto personale</param>
        /// <param name="scontiComitiva">La lista di sconti comitiva da applicare a clientiInteri</param>
        /// <returns>Il prezzo che paga tutta la lista clienti</returns>
        private decimal CalcolaPrezzoTotale( IList<Cliente> clientiInteri, IList<Cliente> clientiScontati, IList<IScontoComitiva> scontiComitiva )
        {
            decimal totale = 0;

            // Costo dei clienti con biglietto gia scontato
            foreach( Cliente cliente in clientiScontati )
            {
                totale += cliente.GetPrezzoPersonale();
            }

            // Costo dei clienti con biglietto intero applicandovi gli sconti comitiva
            for( int i = 0; i < clientiInteri.Count; i++ )
            {
                decimal prezzo = clientiInteri[i].GetPrezzoPersonale();

                if( scontiComitiva[i] != null )
                {
                    prezzo = scontiComitiva[i].ScontaPrezzo( prezzo );
                }

                totale += prezzo;
            }

            return totale;
        }

        #endregion
    }
}