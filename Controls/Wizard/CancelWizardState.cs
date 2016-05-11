using System.Windows.Forms;
using IndianaPark.Tools.Navigation;

namespace IndianaPark.Tools.Wizard
{
    /// <summary>
    /// Stato che rappresenta la richiesta di annullamento dell'operazione
    /// </summary>
    /// <remarks>
    /// Lo stato visualizza un messaggio nel quale si richiede se l'utente vuole veramente terminare il wizard
    /// </remarks>
    public class CancelWizardState : WizardStateBase
    {
		#region Fields 

		#region Internal Fields 

        private DialogResult m_result;

		#endregion Internal Fields 

		#endregion Fields 

		#region Methods 

		#region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelWizardState"/> class.
        /// </summary>
        /// <param name="wizard">Oggetto <see cref="Wizard"/> a cui appartiene lo stato</param>
        /// <param name="previous">Lo stato da cui viene creato questo stato. Vedi <see cref="IState"/>.</param>
        public CancelWizardState( Wizard wizard, IState previous ) : base( wizard, previous )
        {
        }

		#endregion Constructors 

		#region Public Methods 

        /// <summary>
        /// Avvia la gestione dello stato
        /// </summary>
        public override void EnterState( IBuilder builder )
        {
            using( var bw = new System.ComponentModel.BackgroundWorker() )
            {
                bw.DoWork += delegate
                {
                    this.m_result = MessageBox.Show( Properties.Resources.CancelWizardText, Properties.Resources.CancelWizardTitle, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button2 );
                };

                bw.RunWorkerCompleted += ( sender, e ) =>
                {
                    if( this.m_result == DialogResult.Yes )
                    {
                        this.OnStatusChangeRequested( NavigationAction.Cancel );
                        return;
                    }
                    this.OnStatusChangeRequested( NavigationAction.Back );
                };
                bw.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Salva i dati acquisiti dallo stato
        /// </summary>
        /// <param name="builder">L'oggetto <see cref="IBuilder"/> dove salvare i dati</param>
        public override void ExitState( IBuilder builder )
        {
            return;
        }

		#endregion Public Methods 

		#endregion Methods 
    }
}