using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class EquipItem : IItem
    {
        [SerializeField] protected EquipItemInfo m_itemInfo;
        public ItemInfo Info => m_itemInfo;

        private int defaultAmount = 1;
        public int Amount { get => defaultAmount; set => defaultAmount = value; }
        public int MaxAmount => defaultAmount;
        public EquipItemType EquipType => m_itemInfo.EquipType;

        public EquipItem(EquipItemInfo info, int amount = 1)
        {
            m_itemInfo = info;
            Amount = amount;
        }

        public IItem Clone()
        {
            var clonedItem = new EquipItem(m_itemInfo, 1);
            return clonedItem;
        }
    }
}
