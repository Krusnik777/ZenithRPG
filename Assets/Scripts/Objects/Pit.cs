using UnityEngine;

namespace DC_ARPG
{
    public class Pit : InspectableObject
    {
        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("яма!");
            base.OnInspection(player);
        }
    }
}
