using IndianaPark.Biglietti.Wizard;
using IndianaPark.Tools.Wizard;

namespace IndianaPark.PowerFan.Wizard
{
    ///<summary>
    /// Classe utilizzata per raggruppare tutte le proprietà comuni degli stati del PowerFan
    ///</summary>
    public abstract class PowerfanBaseState : EmissioneBaseState<PowerFanBuilder>
    {
        protected PowerfanBaseState( Tools.Wizard.Wizard wizard, IState previous ) : base( wizard, previous )
        {
        }
    }
}
