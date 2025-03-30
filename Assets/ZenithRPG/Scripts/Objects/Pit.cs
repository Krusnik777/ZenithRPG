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

        public void FillGap()
        {
            var colliders = transform.GetComponentsInChildren<Collider>();

            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].isTrigger) continue;

                colliders[i].enabled = false;
            }
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

        public void Activate(IMovable movable = null)
        {
            if (m_trapFloor == null) return;

            m_trapFloor.DestroyTrapFloor();
        }
    }
}
