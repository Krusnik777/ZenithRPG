using UnityEngine;

namespace DC_ARPG
{
    public class HiddenPit : Pit
    {
        [SerializeField] private GameObject m_trapFloorObject;

        public override void OnInspection(Player player)
        {
            if (m_trapFloorObject)
            {
                ShortMessage.Instance.ShowMessage("Подозрительный пол.");
                EventOnInspection?.Invoke();
            }
            else
            {
                base.OnInspection(player);
            }
        }
    }
}
