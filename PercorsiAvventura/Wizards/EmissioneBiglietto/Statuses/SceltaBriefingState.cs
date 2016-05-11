using System;
using System.Collections.Generic;
using System.Linq;
using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Navigation;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PercorsiAvventura.Wizard
{
    /// <summary>
    /// Rappresenta lo stato dove l'utente seleziona in quali briefing inserire i biglietti creati
    /// </summary>
    public sealed class SceltaBriefingState : EmissioneBaseState<BigliettoPercorsiBuilder>
    {
        #region Enumerations 

        private enum Direction
        {
            Avanti,
            Indietro
        }

        #endregion Enumerations 

        #region Fields 

        private IDictionary<string, Model.TipoCliente> m_tipiCliente;
        private int m_currentTipoCliente;
        private Model.IBriefing m_briefingScelto;
        private ClientiPartial m_workingCliente;
        private Direction m_direction = Direction.Avanti;

		#endregion Fields 

		#region Events 

		#region Event Handlers 

        /// <summary>
        /// Chiamata quando la finestra grafica ha disponibili i dati. Recupera i dati dalla WizardForm e notifica il cambio di stato.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The <see cref="IndianaPark.Tools.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        protected override void DataReadyHandler( object source, NavigationEventArgs e )
        {
            this.m_briefingScelto = WizardForm.ConvertUserData<Model.IBriefing>( this.UserData );

            switch( e.Status )
            {
                case NavigationAction.Next:
                    this.m_direction = Direction.Avanti;
                    this.MoveNext();
                    break;

                case NavigationAction.Back:
                    this.m_direction = Direction.Indietro;
                    this.MoveBack();
                    break;

                default:
                    this.OnStatusChangeRequested( e.Status );
                    break;
            }
        }

		#endregion Event Handlers 

		#endregion Events 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="SceltaBriefingState"/> class.
        /// </summary>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        /// <remarks>
        /// Ogni stato ricorda il suo precedente, che è lo stato che lo ha creato e non quello da cui si arriva
        /// </remarks>
        public SceltaBriefingState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
        }

		#endregion Constructors 

		#region Internal Methods 

        /// <summary>
        /// Imposta i dati interni per passare allo stato <see cref="SceltaBriefingState"/> successivo
        /// </summary>
        private void MoveNext()
        {
            // Indico di utilizzare la prossima tipologia cliente
            this.m_currentTipoCliente++;
            // Lo stato successivo è sempre lo stesso ma con parametri differenti
            this.NextState = this;
            // Se sono alla fine delle tipologie cliente proseguo col wizard
            if( m_tipiCliente.Count == m_currentTipoCliente )
            {
                this.NextState = this.StatePool.GetUniqueType( new RiassuntoState( this.Wizard, this ) );
                this.m_direction = Direction.Indietro;
            }

            this.OnStatusChangeRequested( NavigationAction.Next );
        }

        /// <summary>
        /// Imposta i dati interni per passare allo stato <see cref="SceltaBriefingState"/> precedente
        /// </summary>
        private void MoveBack()
        {
            // Indico di utilizzare la prossima tipologia cliente
            this.m_currentTipoCliente--;
            // Lo stato successivo è sempre lo stesso ma con parametri differenti
            this.NextState = this;
            // Solo se sono tornato indietro fino al primo tipo cliente vado effettivamente indietro negli stati
            if( this.m_currentTipoCliente < 0 )
            {
                this.OnStatusChangeRequested( NavigationAction.Back );
                this.m_direction = Direction.Avanti;
                return;
            }

            this.OnStatusChangeRequested( NavigationAction.Next );
        }

		#endregion Internal Methods 

		#region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato
        /// </summary>
        /// <param name="builder"></param>
        /// <remarks>
        /// La procedura standard prevede che questo metodo sia chiamato mentre è ancora attivo il gestore degli eventi
        /// dello stato precedente.
        /// </remarks>
        public override void EnterState( BigliettoPercorsiBuilder builder )
        {
            this.m_tipiCliente = builder.Storage.TipoBiglietto.TipiCliente;

            // Recupero la tipologia cliente corrente
            var listaClienti = m_tipiCliente.ToList();
            if( m_currentTipoCliente == listaClienti.Count )
            {
                this.MoveBack();
                return;
            }
            var tipoCliente = listaClienti[m_currentTipoCliente].Value;

            // La scelta del briefing si applica a tipologie di clienti, ma le informazioni nel builder sono suddivise
            // per tipologie cliente exception tipologie sconto. Devo ovviare a questo fatto
            this.m_workingCliente = builder.Storage.Clienti.Aggregate( new ClientiPartial(),
                ( result, current ) =>
                {
                    if( current.TipoCliente.Equals(tipoCliente) )
                    {
                        result.Quantita += current.Quantita;
                        result.Briefing = current.Briefing;
                    }
                    return result;
                }
            );

            this.m_workingCliente.TipoCliente = tipoCliente;
            this.m_workingCliente.ScontoPersonale = null;
            this.m_briefingScelto = m_workingCliente.Briefing;

            // Se l'utente non può scegliere il briefing, viene preso il valore automaticamente
            if( tipoCliente.TipiBriefing != null && !tipoCliente.TipiBriefing.AskUser )
            {
                m_briefingScelto = tipoCliente.TipiBriefing.TrovaBriefing( DateTime.Now + tipoCliente.TipiBriefing.WalkTime );
            }

            // Se non sono stati impostati biglietti per la tipologia corrente proseguo
            // Non tutte le tipologie cliente hanno briefing. Se non ce l'hanno passo alla tipologia successiva
            if( this.m_workingCliente.Quantita == 0 || tipoCliente.TipiBriefing == null || !tipoCliente.TipiBriefing.AskUser )
            {
                if( m_direction == Direction.Avanti )
                {
                    this.MoveNext();
                }
                else
                {
                    this.MoveBack();
                }
                return;
            }

            // Creo la form
            this.StateForm = new Forms.SceltaBriefingForm( m_workingCliente );
            base.EnterState( builder );
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( BigliettoPercorsiBuilder builder )
        {
            // Imposto il tipo di briefing scelto
            for( int i = 0; i < builder.Storage.Clienti.Count; i++ )
            {
                if( builder.Storage.Clienti[i].TipoCliente == m_workingCliente.TipoCliente )
                {
                    builder.Storage.Clienti[i].Briefing = m_briefingScelto;
                }
            }
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}