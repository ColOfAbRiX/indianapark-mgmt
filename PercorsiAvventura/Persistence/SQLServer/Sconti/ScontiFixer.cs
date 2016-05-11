using System;
using System.Collections.Generic;
using IndianaPark.Tools;
using IndianaPark.PercorsiAvventura.Model;
using System.Linq;

namespace IndianaPark.PercorsiAvventura.Persistence.SqlServer
{
    /// <summary>
    /// Oggetto <see cref="ObjectFixerClient"/> per persistenza degli sconti di tipo SqlServer
    /// </summary>
    public class SqlScontoClient : ObjectFixerClient
    {
		#region Methods 

		#region Internal Methods 

        /// <summary>
        /// Ottiene il corretto tipo di oggetto da utilizzare per estrarre le informazioni dell'oggetto da salvare/caricare
        /// </summary>
        /// <param name="tipo">Tipo dell'oggetto da salvare/caricare</param>
        /// <returns>
        /// Un'istanza di <see cref="IObjectFixable"/> relativo a <paramref name="tipo"/> indicato
        /// </returns>
        protected override IObjectFixable GetWrapper( Type tipo )
        {
            var fixer = new ScontiFixer( tipo );

            // Oggetto Model.ScontoConteggio
            if( tipo == typeof( Model.ScontoConteggio ) )
            {
                fixer = new ScontoConteggioFixer();
            }

            // Oggetto Model.ScontoFisso
            if( tipo == typeof( Model.ScontoFisso ) )
            {
                fixer = new ScontoFissoFixer();
            }

            // Oggetto Model.ScontoOmaggio
            if( tipo == typeof( Model.ScontoOmaggio ) )
            {
                fixer = new ScontoOmaggioFixer();
            }

            // Oggetto Model.ScontoOmaggio
            if( tipo == typeof( Model.ScontoCambiaValore ) )
            {
                fixer = new ScontoValoreFissoFixer();
            }

            return fixer;
        }

		#endregion Internal Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Oggetto di tipo <see cref="IObjectFixable"/> che si occupa della persistenza di oggetti di tipo
    /// <see cref="Model.ScontoBase"/> generici.
    /// </summary>
    /// <remarks>
    /// Gli oggetti <see cref="Model.ScontoBase"/> generici hanno due parametri nel costruttore, uno di tipo
    /// <see cref="string"/> corrispondente al nome dello sconto, exception l'altro di tipo <see cref="double"/> che è
    /// il valore dello sconto.
    /// </remarks>
    internal class ScontiFixer : IObjectFixable
    {
		#region Fields 

		#region Internal Fields 

        private readonly Type m_tipo;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ScontiFixer"/> class.
        /// </summary>
        /// <param name="sconto">The sconto.</param>
        public ScontiFixer( Model.ISconto sconto ) : this( sconto.GetType() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScontiFixer"/> class.
        /// </summary>
        /// <param name="tipo">The tipo.</param>
        public ScontiFixer( Type tipo )
        {
            if( !tipo.ImplementsInterface( typeof( Model.ISconto ) ) )
            {
                throw new ArgumentException( "tipo" );
            }

            this.m_tipo = tipo;
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Restituisce i valori con cui è possibile reistanziare l'oggetto
        /// </summary>
        /// <returns>Un oggetto <see cref="IList&lt;T&gt;"/> contenente i valori dei parametri con cui
        /// è stato istanziato l'oggetto a cui l'istanza fa riferimen</returns>
        public virtual IList<object> GetDefaultCtorParams( ISconto instance )
        {
            return new List<object> { instance.Nome, instance.Sconto };
        }

        /// <summary>
        /// Crea un'istanza dell'oggetto utilizzando la lista di parametri specificata
        /// </summary>
        /// <param name="parameters">La lista di parametri</param>
        /// <returns>Un'istanza dello sconto costruita con i parametri indicati.</returns>
        public virtual ISconto GetInstanceFromParams( IList<object> parameters )
        {
            // Mi assicuro che i parametri siano del tipo corretto
            parameters[0] = Convert.ToString( parameters[0] );
            parameters[1] = Convert.ToDouble( parameters[1] );

            // Istanzio il tipo di sconto 
            return (Model.ISconto)Activator.CreateInstance( this.m_tipo, parameters.ToArray() );
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Oggetto <see cref="ScontiFixer"/> per gli sconti di tipo <see cref="Model.ScontoConteggio"/>
    /// </summary>
    internal class ScontoConteggioFixer : ScontiFixer
    {
		#region Methods 

		#region Constructors 

        public ScontoConteggioFixer() : base( typeof( Model.ScontoConteggio ) )
        {
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Restituisce i valori con cui è possibile reistanziare l'oggetto
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>
        /// Un oggetto <see cref="IList&lt;T&gt;"/> contenente i valori dei parametri con cui
        /// è stato istanziato l'oggetto a cui l'istanza fa riferimen
        /// </returns>
        public override IList<object> GetDefaultCtorParams( ISconto instance )
        {
            return this.GetDefaultCtorParams( (Model.ScontoConteggio)instance );
        }

        /// <summary>
        /// Restituisce i valori con cui è possibile reistanziare l'oggetto
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>
        /// Un oggetto <see cref="IList&lt;T&gt;"/> contenente i valori dei parametri con cui
        /// è stato istanziato l'oggetto a cui l'istanza fa riferimen
        /// </returns>
        public IList<object> GetDefaultCtorParams( Model.ScontoConteggio instance )
        {
            var standard = new List<object>( base.GetDefaultCtorParams( instance ) );
            standard.Add( instance.Conteggio );
            return standard;
        }

        /// <summary>
        /// Crea un'istanza dell'oggetto utilizzando la lista di parametri specificata
        /// </summary>
        /// <param name="parameters">La lista di parametri</param>
        /// <returns>
        /// Un'istanza dello sconto costruita con i parametri indicati.
        /// </returns>
        public override Model.ISconto GetInstanceFromParams( IList<object> parameters )
        {
            if( parameters == null || parameters.Count != 3 )
            {
                throw new ArgumentException( "parameters" );
            }

            return new Model.ScontoConteggio( (string)parameters[0], (double)parameters[1], (uint)parameters[2] );
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Oggetto <see cref="ScontiFixer"/> per gli sconti di tipo <see cref="Model.ScontoFisso"/>
    /// </summary>
    internal class ScontoFissoFixer : ScontiFixer
    {
		#region Methods 

		#region Constructors 

        public ScontoFissoFixer() : base( typeof( Model.ScontoFisso ) )
        {
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Crea un'istanza dell'oggetto utilizzando la lista di parametri specificata
        /// </summary>
        /// <param name="parameters">La lista di parametri</param>
        /// <returns>
        /// Un'istanza dello sconto costruita con i parametri indicati.
        /// </returns>
        public override Model.ISconto GetInstanceFromParams( IList<object> parameters )
        {
            if( parameters == null || parameters.Count != 2 )
            {
                throw new ArgumentException( "parameters" );
            }

            return new Model.ScontoFisso( (string)parameters[0], Convert.ToDecimal( parameters[1] ) );
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Oggetto <see cref="ScontiFixer"/> per gli sconti di tipo <see cref="Model.ScontoOmaggio"/>
    /// </summary>
    internal class ScontoOmaggioFixer : ScontiFixer
    {
		#region Methods 

		#region Constructors 

        public ScontoOmaggioFixer() : base( typeof( Model.ScontoOmaggio ) )
        {
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Restituisce i valori con cui è possibile reistanziare l'oggetto
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>
        /// Un oggetto <see cref="IList&lt;T&gt;"/> contenente i valori dei parametri con cui
        /// è stato istanziato l'oggetto a cui l'istanza fa riferimen
        /// </returns>
        public override IList<object> GetDefaultCtorParams( ISconto instance )
        {
            return this.GetDefaultCtorParams( (Model.ScontoOmaggio)instance );
        }

        /// <summary>
        /// Restituisce i valori con cui è possibile reistanziare l'oggetto
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>
        /// Un oggetto <see cref="IList&lt;T&gt;"/> contenente i valori dei parametri con cui
        /// è stato istanziato l'oggetto a cui l'istanza fa riferimen
        /// </returns>
        public IList<object> GetDefaultCtorParams( Model.ScontoOmaggio instance )
        {
            var standard = new List<object>( base.GetDefaultCtorParams( instance ) );
            standard.RemoveAt( 1 );
            return standard;
        }

        /// <summary>
        /// Crea un'istanza dell'oggetto utilizzando la lista di parametri specificata
        /// </summary>
        /// <param name="parameters">La lista di parametri</param>
        /// <returns>
        /// Un'istanza dello sconto costruita con i parametri indicati.
        /// </returns>
        public override Model.ISconto GetInstanceFromParams( IList<object> parameters )
        {
            if( parameters == null || parameters.Count != 1 )
            {
                throw new ArgumentException( "parameters" );
            }

            return new Model.ScontoOmaggio( (string)parameters[0] );
        }

		#endregion Public Methods 

		#endregion Methods 
    }

    /// <summary>
    /// Oggetto <see cref="ScontiFixer"/> per gli sconti di tipo <see cref="Model.ScontoScontoValoreFissoValoreFisso"/>
    /// </summary>
    internal class ScontoValoreFissoFixer : ScontiFixer
    {
        #region Methods

        #region Constructors

        public ScontoValoreFissoFixer() : base( typeof( Model.ScontoCambiaValore ) )
        {
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Crea un'istanza dell'oggetto utilizzando la lista di parametri specificata
        /// </summary>
        /// <param name="parameters">La lista di parametri</param>
        /// <returns>
        /// Un'istanza dello sconto costruita con i parametri indicati.
        /// </returns>
        public override Model.ISconto GetInstanceFromParams( IList<object> parameters )
        {
            if( parameters == null || parameters.Count != 2 )
            {
                throw new ArgumentException( "parameters" );
            }

            return new Model.ScontoFisso( (string)parameters[0], Convert.ToDecimal( parameters[1] ) );
        }

        #endregion Public Methods

        #endregion Methods
    }
}