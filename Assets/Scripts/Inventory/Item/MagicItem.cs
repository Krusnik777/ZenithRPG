using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class MagicItem : IItem
    {
        [SerializeField] protected MagicItemInfo m_itemInfo;

        [SerializeField] private int m_uses;
        public ItemInfo Info => m_itemInfo;

        private int defaultAmount = 1;
        public int Amount { get => defaultAmount; set => defaultAmount = value; }
        public int MaxAmount => defaultAmount;
        public int Uses => m_uses;

        public MagicItem(MagicItemInfo info, int uses, int amount = 1)
        {
            m_itemInfo = info;
            m_uses = uses;
            Amount = amount;
        }

        public IItem Clone()
        {
            var clonedItem = new MagicItem(m_itemInfo, m_uses, 1);
            return clonedItem;
        }
    }
}
