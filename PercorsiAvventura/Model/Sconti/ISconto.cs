using System;
using System.Collections.Generic;

namespace IndianaPark.PercorsiAvventura.Model 
{
	/// <summary>
	/// Interfaccia che rappresenta uno sconto.
	/// </summary>
	public interface ISconto : ICloneable, IEquatable<ISconto>
    {
        #region Fields

        /// <summary>
        /// Il valore dello sconto applicato. Il significato di questo campo pu� cambiare da
        /// realizzazione a realizzazione, leggere la documentazione.
        /// </summary>
        double Sconto { get; }

        /// <summary>
        /// Il nome dello sconto
        /// </summary>
        string Nome { get; }

        /// <summary>
        /// Indica se lo sconto pu� essere tolto a favore di un piano sconti pi� favorevole
        /// </summary>
        bool CanOmit { get; }

        #endregion

        #region Methods

        /// <summary>
		/// Il metodo applica lo sconto al prezzo e ne ritorna il risultato. La funzione
		/// non fa distinzione se � necessario o no applicare lo sconto: il risultato �
		/// sempre il prezzo scontato.
		/// </summary>
		/// <param name="prezzoBase">Il prezzo non scontato a cui applicare lo
		/// sconto</param>
		decimal ScontaPrezzo(decimal prezzoBase);

        #endregion
    }
}