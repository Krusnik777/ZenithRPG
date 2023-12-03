using UnityEngine;

namespace DC_ARPG
{
    public class TrapGun : InspectableObject
    {
        [SerializeField] private GameObject m_holeObject;
        [SerializeField] private Magic m_magic;
        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Подозрительная щель.");

            base.OnInspection(player);
        }

        public void Shoot()
        {
            m_magic.CreateFireball(gameObject, m_holeObject.transform.position, m_holeObject.transform.rotation);
        }
    }
}
