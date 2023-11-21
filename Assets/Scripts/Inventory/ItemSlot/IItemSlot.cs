namespace DC_ARPG
{
    public interface IItemSlot<T> where T:Item
    {
        T Item { get; }
        int Amount { get; }
        int Capacity { get; }

        bool IsEmpty { get; }
        bool IsFull { get; }

        void SetItemInSlot(T item);
        void ClearSlot();
    }
}
