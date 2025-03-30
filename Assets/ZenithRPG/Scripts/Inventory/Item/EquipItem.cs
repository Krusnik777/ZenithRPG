using UnityEngine;
using UnityEngine.Events;

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

        public int Price => m_itemInfo.Price;

        public EquipItem(EquipItemInfo info)
        {
            m_itemInfo = info;
            itemAmount = defaultAmount;
        }

        public IItem Clone()
        {
            var clonedItem = new EquipItem(m_itemInfo);
            return clonedItem;
        }

        public void Use(object sender, Inventory inventory, IItemSlot slot, UnityAction<object, IItemSlot> onUse)
        {
            inventory.EquipItem(sender, slot);

            UISounds.Instance.PlayItemEquipSound();
        }
    }
}
