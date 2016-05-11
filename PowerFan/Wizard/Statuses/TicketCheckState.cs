using System.Collections.Generic;
using IndianaPark.Biglietti.Wizard;

namespace IndianaPark.PowerFan.Wizard
{
    public class TicketCheckState : TipoAttrazioneState
    {
        public TicketCheckState( Tools.Wizard.Wizard wizard, IList<TipoAttrazioneState.AttrazioneData> listaAttrazioni ) : base( wizard, listaAttrazioni )
        {
        }
    }
}
