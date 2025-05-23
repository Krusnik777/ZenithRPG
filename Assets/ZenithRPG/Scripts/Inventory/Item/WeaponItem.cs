using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [System.Serializable]
    public class WeaponItem : IItem
    {
        [SerializeField] protected WeaponItemInfo m_itemInfo;

        [SerializeField] private int m_uses = 1;
        public ItemInfo Info => m_itemInfo;

        private const int defaultAmount = 1;
        private int itemAmount;

        public int Amount { get => itemAmount; set => itemAmount = value; }
        public int MaxAmount => defaultAmount;

        public int Uses { get => m_uses; set => m_uses = value; }

        public int AttackIncrease => m_itemInfo.AttackIncrease;
        public bool HasInfiniteUses => m_itemInfo.HasInfiniteUses;

        public int Price => HasInfiniteUses ? m_itemInfo.Price : m_itemInfo.Price * Uses;

        public WeaponItem(WeaponItemInfo info, int uses)
        {
            m_itemInfo = info;
            m_uses = uses;
            itemAmount = defaultAmount;
        }

        public IItem Clone()
        {
            var clonedItem = new WeaponItem(m_itemInfo, m_uses);
            return clonedItem;
        }

        public void Use(object sender, Inventory inventory, IItemSlot slot, UnityAction<object, IItemSlot> onUse)
        {
            inventory.EquipItem(sender, slot);

            UISounds.Instance.PlayItemEquipSound();
        }
    }
}
