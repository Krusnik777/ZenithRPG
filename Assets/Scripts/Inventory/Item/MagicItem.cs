using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class MagicItem : IItem
    {
        [SerializeField] protected MagicItemInfo m_itemInfo;

        [SerializeField] private int m_uses = 1;
        public ItemInfo Info => m_itemInfo;

        private const int defaultAmount = 1;
        private int itemAmount;

        public int Amount { get => itemAmount; set => itemAmount = value; }
        public int MaxAmount => defaultAmount;

        public int Uses { get => m_uses; set => m_uses = value; }

        public int MagicPointsForUse => m_itemInfo.MagicPointsForUse;
        public bool HasInfiniteUses => m_itemInfo.HasInfiniteUses;

        public int Price => m_itemInfo.Price;

        public MagicItem(MagicItemInfo info, int uses)
        {
            m_itemInfo = info;
            m_uses = uses;
            itemAmount = defaultAmount;
        }

        public IItem Clone()
        {
            var clonedItem = new MagicItem(m_itemInfo, m_uses);
            return clonedItem;
        }
    }
}
