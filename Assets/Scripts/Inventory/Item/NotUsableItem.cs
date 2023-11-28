using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class NotUsableItem : IItem
    {
        [SerializeField] protected NotUsableItemInfo m_itemInfo;

        [SerializeField] private int m_amount = 1;
        public ItemInfo Info => m_itemInfo;
        public int Amount { get => m_amount; set => m_amount = value; }
        public int MaxAmount => m_itemInfo.MaxAmount;

        public int Price => m_itemInfo.Price * m_amount;

        public NotUsableItem(NotUsableItemInfo info, int amount)
        {
            m_itemInfo = info;
            m_amount = MaxAmount != 1 && amount > MaxAmount ? MaxAmount : amount;
        }

        public IItem Clone()
        {
            var clonedItem = new NotUsableItem(m_itemInfo, m_amount);
            return clonedItem;
        }
    }
}
