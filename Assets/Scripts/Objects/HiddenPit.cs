using UnityEngine;

namespace DC_ARPG
{
    public class HiddenPit : Pit
    {
        [SerializeField] private TrapFloor m_trapFloor;

        public TrapFloor TrapFloor => m_trapFloor;

        public void UnveilHiddenPit()
        {
            if (m_trapFloor == null) return;

            m_trapFloor.DestroyTrapFloor();
        }

        public override void OnInspection(Player player)
        {
            if (m_trapFloor != null)
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
