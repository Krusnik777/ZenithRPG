namespace DC_ARPG
{
    public class UsableItemSlot : IItemSlot<UsableItem>
    {
        public UsableItem Item { get; protected set; }

        public int Amount { get; protected set; }
        public int Capacity { get; protected set; }

        public bool IsEmpty => Item == null;
        public bool IsFull => !IsEmpty && Amount >= Capacity;

        public void SetItemInSlot(UsableItem item)
        {
            if (!IsEmpty) return;

            Item = item;
            Amount = item.Amount;
            Capacity = item.MaxAmount;
        }

        public void ClearSlot()
        {
            if (IsEmpty) return;

            Item = null;
        }
    }
}
