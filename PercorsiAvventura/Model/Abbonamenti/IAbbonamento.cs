using System;

namespace IndianaPark.PercorsiAvventura.Model 
{
	/// <summary>
	/// Rappresenta un generico abbonamento, con i metodi base che lo definiscono.
	/// </summary>
	public interface IAbbonamento : ICloneable, IEquatable<Abbonamento>
	{
		#region Fields

		/// <summary>
		/// Il costo dell'abbonamento nuovo.
		/// </summary>
		decimal CostoAbbonamento
		{
			get;
		}

		/// <summary>
		/// La data di emissione dell'abbonamento
		/// </summary>
		DateTime Emissione
		{
			get;
		}

		/// <summary>
		/// Il nominativo a cui � registrato l'abbonamento
		/// </summary>
		string Nominativo
		{
			get;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Controlla che l'abbonamento sia ancora valido. Ogni implementazione utilizza i
		/// propri criteri per determinare la validit�
		/// </summary>
		bool ControllaValidita();

		/// <summary>
		/// Restituisce un'identificativo sul tipo di abbonamento.
		/// </summary>
		Type GetTipoAbbonamento();

		/// <summary>
		/// Utilizza un'ingresso dell'abbonamento e restituisce un valore che indica se
		/// l'abbonamento � ancora valido e quindi � stato usato (non � valido se per
		/// esempio � scaduto).
		/// </summary>
		bool UsaAbbonamento();

		#endregion
	}
}