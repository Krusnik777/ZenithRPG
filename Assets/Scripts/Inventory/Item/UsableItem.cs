using UnityEngine;

namespace DC_ARPG
{
    public class UsableItem : Item
    {
        [SerializeField] protected UsableItemInfo m_itemInfo;
        
        [SerializeField] private int m_amount;
        public override ItemInfo Info => m_itemInfo;
        public override int Amount => m_amount;
        public override int MaxAmount => m_itemInfo.MaxAmount;

        public UsableItem(UsableItemInfo info, int amount)
        {
            m_itemInfo = info;
            m_amount = amount;
        }

        public UsableItem Clone()
        {
            var clonedItem = new UsableItem(m_itemInfo, m_amount);
            return clonedItem;
        }
    }
}
