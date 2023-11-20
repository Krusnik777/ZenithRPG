using UnityEngine;

namespace DC_ARPG
{
    public class ItemSlot : MonoBehaviour
    {
        public enum SlotType
        {
            ItemSlot,
            ActiveSlot,
            MagicSlot,
            ArmorSlot,
            ShieldSlot,
            WeaponSlot
        }

        [SerializeField] private SlotType m_type;

        public Item<ItemInfo> Item { get; private set; }

        public int Amount { get; private set; }
        public int Capacity { get; private set; }


        public bool IsEmpty => Item == null;
        public bool IsFull => !IsEmpty && Amount >= Capacity;

        public void SetItemInSlot(Item<ItemInfo> item)
        {
            if (!IsEmpty) return;
        }

        public void ClearSlot()
        {
            if (IsEmpty) return;

            Item = null;
        }

    }
}
