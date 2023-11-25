using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class UsableItem : IItem
    {
        [SerializeField] protected UsableItemInfo m_itemInfo;
        
        [SerializeField] private int m_amount = 1;
        public ItemInfo Info => m_itemInfo;
        public int Amount { get => m_amount; set => m_amount = value; }
        public int MaxAmount => m_itemInfo.MaxAmount;

        public UsableItem(UsableItemInfo info, int amount)
        {
            m_itemInfo = info;
            m_amount = amount > MaxAmount ? MaxAmount : amount;
        }

        public IItem Clone()
        {
            var clonedItem = new UsableItem(m_itemInfo, m_amount);
            return clonedItem;
        }
    }
}
