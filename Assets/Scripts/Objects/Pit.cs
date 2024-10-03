using UnityEngine;

namespace DC_ARPG
{
    public class Pit : InspectableObject, IActivableObject
    {
        [SerializeField] private int m_damageAfterFall;
        [SerializeField] private TrapFloor m_trapFloor;

        public int DamageAfterFall => m_damageAfterFall;
        public TrapFloor TrapFloor => m_trapFloor;

        public override bool Disabled => m_trapFloor == null;

        public void UnveilHiddenPit()
        {
            if (m_trapFloor == null) return;

            m_trapFloor.DestroyTrapFloor();
        }

        public override void OnInspection(Player player)
        {
            if (m_trapFloor != null)
            {
                ShortMessage.Instance.ShowMessage("ѕодозрительный пол.");
                base.OnInspection(player);
            }
            else
            {
                ShortMessage.Instance.ShowMessage("яма!");
                base.OnInspection(player);
            }
        }

        public void Activate(CharacterAvatar characterAvatar = null)
        {
            if (m_trapFloor == null) return;

            m_trapFloor.DestroyTrapFloor();
        }
    }
}
