using UnityEngine;

namespace DC_ARPG
{
    public class WeaponItem : Item<WeaponItemInfo>
    {
        [SerializeField] private bool m_isEquipped;
        [SerializeField] private int m_uses;
    }
}
