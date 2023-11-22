namespace DC_ARPG
{
    public interface IItemSlot<T> where T:IItem
    {
        T Item { get; }
        ItemInfo ItemInfo { get; }

        int Amount { get; }
        int Capacity { get; }

        bool IsEmpty { get; }
        bool IsFull { get; }

        void SetItemInSlot(T item);
        void ClearSlot();
    }
}
