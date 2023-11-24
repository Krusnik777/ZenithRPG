using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class EquipItem : IItem
    {
        [SerializeField] protected EquipItemInfo m_itemInfo;
        public ItemInfo Info => m_itemInfo;

        private const int defaultAmount = 1;
        private int itemAmount;

        public int Amount { get => itemAmount; set => itemAmount = value; }
        public int MaxAmount => defaultAmount;
        public EquipItemType EquipType => m_itemInfo.EquipType;

        public int DefenseIncrease => m_itemInfo.DefenseIncrease;

        public EquipItem(EquipItemInfo info, int amount = 1)
        {
            m_itemInfo = info;
            itemAmount = amount;
        }

        public IItem Clone()
        {
            var clonedItem = new EquipItem(m_itemInfo, defaultAmount);
            return clonedItem;
        }
    }
}
