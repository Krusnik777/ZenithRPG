using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class WeaponItem : IItem
    {
        [SerializeField] protected WeaponItemInfo m_itemInfo;

        [SerializeField] private int m_uses;
        public ItemInfo Info => m_itemInfo;

        private const int defaultAmount = 1;
        private int itemAmount;

        public int Amount { get => itemAmount; set => itemAmount = value; }
        public int MaxAmount => defaultAmount;
        public int Uses => m_uses;

        public WeaponItem(WeaponItemInfo info, int uses, int amount = 1)
        {
            m_itemInfo = info;
            m_uses = uses;
            itemAmount = amount;
        }

        public IItem Clone()
        {
            var clonedItem = new WeaponItem(m_itemInfo, m_uses, defaultAmount);
            return clonedItem;
        }
    }
}
