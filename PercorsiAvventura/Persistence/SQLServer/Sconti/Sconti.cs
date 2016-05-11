using System;
namespace IndianaPark.PercorsiAvventura.Persistence.SqlServer
{
    partial class TableSconti : System.IEquatable<TableSconti>
    {
        /// <summary>
        /// Copia l'oggetto dell'istanza in un altro oggetto specificato
        /// </summary>
        /// <param name="destination">The destination.</param>
        public void CopyTo( TableSconti destination )
        {
            if( String.Compare( this.Key, destination.Key, false ) != 0 )
            {
                return;
            }

            destination.CtorParameters = this.CtorParameters;
            destination.IsCustom = this.IsCustom;
            destination.IsComitiva = this.IsComitiva;
            destination.IsPersonale = this.IsPersonale;
            destination.Nome = this.Nome;
            destination.TipoSconto = this.TipoSconto;
            destination.Valore = this.Valore;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals( TableSconti other )
        {
            return
                String.Compare( other.CtorParameters, this.CtorParameters, false ) == 0 &&
                other.IsCustom == this.IsCustom &&
                other.IsComitiva == this.IsComitiva &&
                other.IsPersonale == this.IsPersonale &&
                String.Compare( other.Key, this.Key, false ) == 0 &&
                String.Compare( other.Nome, this.Nome, false ) == 0 &&
                String.Compare( other.TipoSconto, this.TipoSconto, false ) == 0 &&
                other.Valore == this.Valore;
        }
    }
}
