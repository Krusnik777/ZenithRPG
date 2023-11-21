using UnityEngine;

namespace DC_ARPG
{
    public class WeaponItem : Item
    {
        [SerializeField] protected WeaponItemInfo m_itemInfo;

        [SerializeField] private bool m_isEquipped;
        [SerializeField] private int m_uses;
        public override ItemInfo Info => m_itemInfo;
        public int Uses => m_uses;
        public bool IsEquipped => m_isEquipped;

        public WeaponItem(WeaponItemInfo info, int uses, bool isEquipped)
        {
            m_itemInfo = info;
            m_uses = uses;
            m_isEquipped = isEquipped;
        }

        public WeaponItem Clone()
        {
            var clonedItem = new WeaponItem(m_itemInfo, m_uses, m_isEquipped);
            return clonedItem;
        }
    }
}
