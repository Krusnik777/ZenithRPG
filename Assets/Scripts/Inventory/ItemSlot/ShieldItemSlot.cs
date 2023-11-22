namespace DC_ARPG
{
    public class ShieldItemSlot : IItemSlot<EquipItem>
    {
        public EquipItem Item { get; protected set; }
        public ItemInfo ItemInfo => Item?.Info;

        public int Amount => IsEmpty ? 0 : Item.Amount;
        public int Capacity { get; protected set; }

        public bool IsEmpty => Item == null;
        public bool IsFull => !IsEmpty && Amount >= Capacity;

        public void SetItemInSlot(EquipItem item)
        {
            if (!IsEmpty || item.EquipType != EquipItemType.Shield) return;

            Item = item;
            Capacity = item.MaxAmount;
        }

        public void ClearSlot()
        {
            if (IsEmpty) return;

            Item = null;
        }
    }
}
