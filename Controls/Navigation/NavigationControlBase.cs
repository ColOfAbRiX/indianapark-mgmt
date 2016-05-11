using System.Windows.Forms;

namespace IndianaPark.Tools.Navigation
{

    /// <summary>
    /// Un generico controllo di navigazione.
    /// </summary>
    /// <remarks>
    /// Un controllo di navigazione oltre a realizzare i metodi
    /// per il movimento e quelli per l'utilizzo degli eventi dell'interfacci <see cref="INavigator"/> fornisce dei membri
    /// utili per la creazione di controlli che interagiscano con l'utente.
    /// </remarks>
    public abstract class NavigationControlBase : UserControl, INavigator
    {
		#region Fields 

		#region Internal Fields 

        /// <summary>
        /// Mantiene internamente lo stato di navigazione del controllo
        /// </summary>
        private NavigationAction m_nStatus = NavigationAction.Nothing;

		#endregion Internal Fields 

		#region Public Fields 

        /// <summary>
        /// Lo stato dell'oggetto navigatore
        /// </summary>
        public NavigationAction NavigationStatus
        {
            get { return this.m_nStatus;  }
        }

		#endregion Public Fields 

		#endregion Fields 

		#region Events 

        /// <summary>
        /// Generato quando viene richiesta un'azione di navigazione
        /// </summary>
        public event NavigationEvent Navigate;

		#endregion Events 

		#region Methods 

		#region Public Methods 

        /// <summary>
        /// Utilizzato per richiedere al navigatore un'azione di navigazione. 
        /// </summary>
        /// <remarks>Per informazioni su cosa e quali sono le azioni di navigazione vedere <see cref="NavigationAction"/>
        /// Ad ogni chiamata di questo metodo deve corrispondere lo scatenarsi dell'evento <see cref="INavigator.Navigate"/></remarks>
        /// <param name="action">Tipo di azione richiesta</param>
        public void Sail( NavigationAction action )
        {
            if( this.Navigate != null )
            {
                this.Navigate( this, new NavigationEventArgs( action ) );
            }
        }

        /// <summary>
        /// Utilizzato per abilitare o disabilitare nel controllo le operazioni di navigazione
        /// </summary>
        /// <param name="direction">Indica quale direzione di navigazione associata al controllo abilitare o disabilitare</param>
        /// <param name="status">Se <c>true</c> abilita il relativo controllo, se <c>false</c> lo disabilita</param>
        public abstract void EnableControl( NavigationAction direction, bool status );

		#endregion Public Methods 

		#endregion Methods 
    }
}
