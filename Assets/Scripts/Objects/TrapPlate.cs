using UnityEngine;

namespace DC_ARPG
{
    public class TrapPlate : InspectableObject
    {
        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Ловушка!");

            base.OnInspection(player);
        }
    }
}
