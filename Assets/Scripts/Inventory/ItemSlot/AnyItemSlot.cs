namespace DC_ARPG
{
    public class AnyItemSlot: IItemSlot<IItem>
    {
        public IItem Item { get; protected set; }
        public ItemInfo ItemInfo => Item?.Info;

        public int Amount => IsEmpty? 0 : Item.Amount;
        public int Capacity { get; protected set; }

        public bool IsEmpty => Item == null;
        public bool IsFull => !IsEmpty && Amount >= Capacity;

        public void SetItemInSlot(IItem item)
        {
            if (!IsEmpty) return;

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
