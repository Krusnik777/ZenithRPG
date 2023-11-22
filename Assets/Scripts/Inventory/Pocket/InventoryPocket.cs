using System.Collections.Generic;

namespace DC_ARPG
{
    public class InventoryPocket
    {
        public enum PocketType
        {
            Main,
            Extra
        }

        public AnyItemSlot[] ItemSlots { get; private set; }
        public bool IsFull => CheckFullness();
        public bool IsEmpty => CheckEmptiness();
        public PocketType Type { get; private set; }

        public InventoryPocket(int itemSlotsAmount, PocketType type)
        {
            ItemSlots = new AnyItemSlot[itemSlotsAmount];
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                ItemSlots[i] = new AnyItemSlot();
            }
            Type = type;
        }

        private bool CheckFullness()
        {
            foreach (var slot in ItemSlots)
            {
                if (slot.IsEmpty)
                    return false;
            }
            return true;
        }

        private bool CheckEmptiness()
        {
            foreach (var slot in ItemSlots)
            {
                if (slot.IsFull)
                    return false;
            }
            return true;
        }

        public AnyItemSlot GetEmptySlot()
        {
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                if (ItemSlots[i].IsEmpty)
                    return ItemSlots[i];
            }
            return null;
        }

        public IItem FindItem(IItem item)
        {
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                if (!ItemSlots[i].IsEmpty && ItemSlots[i].ItemInfo == item.Info)
                        return item;
            }
            return null;
        }

        public IItem[] GetAllItems()
        {
            var allItems = new List<IItem>();

            foreach (var slot in ItemSlots)
            {
                if (!slot.IsEmpty)
                    allItems.Add(slot.Item);
            }
            return allItems.ToArray();
        }

        public IItem[] GetAllSameItems(IItem item)
        {
            var allSameItems = new List<IItem>();

            foreach (var slot in ItemSlots)
            {
                if (!slot.IsEmpty && slot.ItemInfo == item.Info)
                        allSameItems.Add(slot.Item);
            }
            return allSameItems.ToArray();
        }

    }
}
