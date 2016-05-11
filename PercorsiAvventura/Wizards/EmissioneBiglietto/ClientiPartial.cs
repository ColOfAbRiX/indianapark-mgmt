using System;
using System.Collections.Generic;
using System.Linq;
using IndianaPark.PercorsiAvventura.Model;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Associa le informazioni necessare per la creazione dei singoli clienti dalle informazioni sulle tipologie
    /// </summary>
    public class ClientiPartial : ICloneable, IEquatable<ClientiPartial>
    {
        #region Fields 

        #region Public Fields 

        /// <summary>
        /// Briefing in cui vanno inseriti i clienti di questa tipologia
        /// </summary>
        public Model.IBriefing Briefing { get; set; }

        /// <summary>
        /// La quantità di clienti da creare
        /// </summary>
        public int Quantita { get; set; }

        /// <summary>
        /// Lo sconto personale applicato alla tipologia cliente
        /// </summary>
        public Model.ISconto ScontoPersonale { get; set; }

        /// <summary>
        /// Il tipo di cliente a cui sono associate le informazioni
        /// </summary>
        public Model.TipoCliente TipoCliente { get; set; }

        #endregion Public Fields 

        #endregion Fields 

        #region Methods 

        #region Class-wise Methods 

        /// <summary>
        /// Crea una lista di <see cref="ClientiPartial"/> dalle informazioni di <see cref="Model.Parco"/>
        /// </summary>
        /// <returns>La lista delle informazioni sulle tipologie cliente</returns>
        public static IList<ClientiPartial> GetFromParco( Model.TipoBiglietto tipoBiglietto )
        {
            var infoClienti = new List<ClientiPartial>();

            // Trasformo la lista delle tipologie cliente del parco in informazioni complete per la creazione dei clienti
            foreach( var row in tipoBiglietto.TipiCliente.Values )// Model.TipoCliente.GetAllTipologie() )
            {
                infoClienti.Add( new ClientiPartial
                                 {
                                     TipoCliente = row,
                                     ScontoPersonale = null,
                                     Quantita = 0
                                 } );
            }

            return infoClienti;
        }

        /// <summary>
        /// Sintetizza le informazioni di una lista di <see cref="Model.Cliente"/> in una lista di <see cref="ClientiPartial"/>
        /// </summary>
        /// <param name="clienti">La lista dei clienti</param>
        /// <returns>Una lista di <see cref="ClientiPartial"/> costruita come riassunto della lista clienti in ingresso</returns>
        public static IList<ClientiPartial> SynthesyzeClienti( IEnumerable<Cliente> clienti )
        {
            if( clienti == null )
            {
                throw new ArgumentNullException( "clienti" );
            }

            // Riassumo le informazioni dei clienti
            var partial = from c in clienti
                          group c by new { c.TipoCliente, c.Sconto, c.ScontoComitiva, c.Briefing } into gr
                          select new
                                 {
                                     gr.Key.TipoCliente,
                                     gr.Key.Sconto,
                                     gr.Key.ScontoComitiva,
                                     gr.Key.Briefing,
                                     Quantita = gr.Count()
                                 };

            var output = new List<ClientiPartial>();
            foreach( var p in partial )
            {
                output.Add( new ClientiPartial { Quantita = p.Quantita, ScontoPersonale = p.Sconto, TipoCliente = p.TipoCliente, Briefing = p.Briefing } );
            }

            return output;
        }

        /// <summary>
        /// Accorpa le tuple (TipoBiglietto, ScontoPersonale) duplicate della lista clienti
        /// </summary>
        /// <remarks>
        /// La funzione viene utilizzata principalmente perchè il meccanismo di aggiunta sconti prevede che lo sconto venga sempre aggiunto
        /// all'ultimo elemento della lista, che così risulta sempre duplicato.
        /// </remarks>
        public static IList<ClientiPartial> CompactData( IEnumerable<ClientiPartial> clienti )
        {
            // Copio la lista per averne una di riferimento immutabile
            var listaLavoro = new List<ClientiPartial>();

            foreach( var info in clienti )
            {
                ClientiPartial cliente = info;
                var copia = listaLavoro.Find(
                    current => current.TipoCliente == cliente.TipoCliente && current.ScontoPersonale == cliente.ScontoPersonale
                );

                // Ci sono elementi da unire
                if( copia != null )
                {
                    // Trovo la quantità complessiva delle righe
                    copia.Quantita += info.Quantita;
                }
                else
                {
                    listaLavoro.Add( info );
                }
            }

            return listaLavoro;
        }
        
        #endregion Class-wise Methods 

        #region Public Methods 

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        object ICloneable.Clone()
        {
            var nuovo = new ClientiPartial
                        {
                            Quantita = this.Quantita,
                            TipoCliente = this.TipoCliente,
                            ScontoPersonale = this.ScontoPersonale,
                            Briefing = this.Briefing
                        };

            return nuovo;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        public ClientiPartial Clone()
        {
            return (ClientiPartial)((ICloneable)this).Clone();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( ClientiPartial other )
        {
            if( other == null )
            {
                return false;
            }

            return (this.Briefing == null ? other.Briefing == null : this.Briefing.Equals( other.Briefing )) &&
                   this.Quantita.Equals( other.Quantita ) &&
                   (this.ScontoPersonale == null ? other.ScontoPersonale == null : this.ScontoPersonale.Equals( other.ScontoPersonale )) &&
                   this.TipoCliente.Equals( other.TipoCliente );
        }

        #endregion Public Methods 

        #endregion Methods 
    }
}