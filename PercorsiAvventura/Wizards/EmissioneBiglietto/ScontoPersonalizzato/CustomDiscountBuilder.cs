using System;
using IndianaPark.Tools.Wizard;
using IndianaPark.PercorsiAvventura.Model;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Abstract Factory per la creazione di uno sconto personalizzato
    /// </summary>
    public abstract class ScontoCreator
    {
        /// <summary>
        /// Valore testuale utilizzato per la creazione dello sconto
        /// </summary>
        public string Valore { get; set; }

        /// <summary>
        /// Crea lo sconto implementato dalla classe derivata
        /// </summary>
        /// <returns>Un oggetto <see cref="Model.ISconto"/></returns>
        public abstract ISconto CreateISconto();
    }

    /// <summary>
    /// Classe per la creazione di uno oggetto <see cref="Model.ScontoFisso"/>
    /// </summary>
    public class ScontoFissoCreator : ScontoCreator
    {
        /// <summary>
        /// Crea lo sconto implementato dalla classe derivata
        /// </summary>
        /// <returns>Un oggetto <see cref="Model.ISconto"/></returns>
        public override ISconto CreateISconto()
        {
            decimal sconto = decimal.Parse( this.Valore );
            return new Model.ScontoFisso( String.Format( "Sconto -{0:C}", sconto ), sconto );
        }
    }

    /// <summary>
    /// Classe per la creazione di uno oggetto <see cref="Model.ScontoPercentuale"/>
    /// </summary>
    public class ScontoPercentualeCreator : ScontoCreator
    {
        /// <summary>
        /// Crea lo sconto implementato dalla classe derivata
        /// </summary>
        /// <returns>Un oggetto <see cref="Model.ISconto"/></returns>
        public override ISconto CreateISconto()
        {
            double sconto = double.Parse( this.Valore ) / 100;
            return new Model.ScontoPercentuale( String.Format( "Sconto -{0,2}%", sconto * 100 ), sconto );
        }
    }

    /// <summary>
    /// Classe per la creazione di uno oggetto <see cref="Model.Sconti.ScontoValoreFisso"/>
    /// </summary>
    public class ScontoCambiaValoreCreator : ScontoCreator
    {
        /// <summary>
        /// Crea lo sconto implementato dalla classe derivata
        /// </summary>
        /// <returns>Un oggetto <see cref="Model.ISconto"/></returns>
        public override ISconto CreateISconto()
        {
            decimal prezzo = decimal.Parse( this.Valore );
            return new Model.ScontoCambiaValore( String.Format( "Prezzo Fisso {0:C}", prezzo ), prezzo );
        }
    }

    /// <summary>
    /// Builder per la creazione di uno sconto personalizzato
    /// </summary>
    public class CustomDiscountBuilder : GenericWizardBuilderBase<ScontoCreator, Model.ISconto>
    {
		#region Fields 

		#region Internal Fields 

        private ISconto m_sconto;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Il contenitore di dati per il Builder dei biglietti
        /// </summary>
        public override ScontoCreator Storage { get; protected set; }

        #endregion Public Fields 

		#endregion Fields 

		#region Methods 

		#region Public Methods 

        /// <summary>
        /// Imposta il valore di <see cref="CustomDiscountBuilder.Storage"/>
        /// </summary>
        /// <param name="value">Il valore da impostare</param>
        public void SetScontoCreator( ScontoCreator value )
        {
            this.Storage = value;
        }

        /// <summary>
        /// Costruisce e restituisce il risultato della costruzione
        /// </summary>
        /// <returns><c>true</c> se la funziona ha costruito i dati con successo, <c>false</c> altrimenti.</returns>
        public override bool BuildResult()
        {
            if( this.Storage != null && !String.IsNullOrEmpty( this.Storage.Valore ) )
            {
                this.m_sconto = this.Storage.CreateISconto();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Restituisce il risultato costruito con <see cref="IBuilder.BuildResult"/>
        /// </summary>
        /// <returns>
        /// L'oggetto costruito, oppure <c>null</c> se <see cref="IBuilder.BuildResult"/> non è mai stata
        /// chiamata o la sua chiamata non è riuscita
        /// </returns>
        public override ISconto GetResult()
        {
            return this.m_sconto;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}