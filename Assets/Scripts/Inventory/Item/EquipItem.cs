using UnityEngine;

namespace DC_ARPG
{
    public class EquipItem : Item
    {
        [SerializeField] protected EquipItemInfo m_itemInfo;

        [SerializeField] private bool m_isEquipped;
        public override ItemInfo Info => m_itemInfo;
        public bool IsEquipped => m_isEquipped;
        public EquipItemType Type => m_itemInfo.EquipType;

        public EquipItem(EquipItemInfo info, bool isEquipped)
        {
            m_itemInfo = info;
            m_isEquipped = isEquipped;
        }

        public EquipItem Clone()
        {
            var clonedItem = new EquipItem(m_itemInfo, m_isEquipped);
            return clonedItem;
        }
    }
}
