namespace DC_ARPG
{
    public class AnyItemSlot : IItemSlot
    {
        public IItem Item { get; protected set; }
        public ItemInfo ItemInfo => Item?.Info;

        public int Amount => IsEmpty? 0 : Item.Amount;
        public int Capacity { get; protected set; }

        public bool IsEmpty => Item == null;
        public bool IsFull => !IsEmpty && Amount >= Capacity;

        private InventoryPocket m_parentPocket;

        public InventoryPocket ParentPocket => m_parentPocket;

        public bool TrySetItemInSlot(IItem item)
        {
            if (!IsEmpty || item == null) return false;

            Item = item;
            Capacity = item.MaxAmount;
            return true;
        }

        public void ClearSlot()
        {
            if (IsEmpty) return;

            Item = null;
        }

        public bool TryClearSlotAndSetItem(IItem item)
        {
            if (item == null) return false;

            ClearSlot();

            return TrySetItemInSlot(item);
        }

        public void SetParent(InventoryPocket parentPocket)
        {
            m_parentPocket = parentPocket;
        }
    }
}
