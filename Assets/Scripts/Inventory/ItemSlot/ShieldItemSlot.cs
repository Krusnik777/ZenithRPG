namespace DC_ARPG
{
    public class ShieldItemSlot : IItemSlot
    {
        public IItem Item { get; protected set; }
        public ItemInfo ItemInfo => Item?.Info;

        public int Amount => IsEmpty ? 0 : Item.Amount;
        public int Capacity { get; protected set; }

        public bool IsEmpty => Item == null;
        public bool IsFull => !IsEmpty && Amount >= Capacity;

        public bool TrySetItemInSlot(IItem item)
        {
            if (item == null) return false;

            if (!(item is EquipItem))
            {
                UnityEngine.Debug.Log("WrongTypeOfItem");
                return false;
            }

            if (!IsEmpty || (item as EquipItem).EquipType != EquipItemType.Shield) return false;

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

            if (!(item is EquipItem))
            {
                UnityEngine.Debug.Log("WrongTypeOfItem");
                return false;
            }

            if ((item as EquipItem).EquipType != EquipItemType.Shield)
            {
                UnityEngine.Debug.Log("WrongEquipTypeOfItem");
                return false;
            }

            ClearSlot();

            return TrySetItemInSlot(item);
        }
    }
}
