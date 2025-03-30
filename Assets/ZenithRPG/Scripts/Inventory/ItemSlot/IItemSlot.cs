namespace DC_ARPG
{
    public interface IItemSlot
    {
        IItem Item { get; }
        ItemInfo ItemInfo { get; }

        int Amount { get; }
        int Capacity { get; }

        bool IsEmpty { get; }
        bool IsFull { get; }

        bool TrySetItemInSlot(IItem item);
        void ClearSlot();
        bool TryClearSlotAndSetItem(IItem item);
    }
}
