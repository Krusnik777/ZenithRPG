using System.Collections.Generic;

namespace DC_ARPG
{
    [System.Serializable]
    public class InventoryPocketData
    {
        public ItemData[] ItemsData;

        public InventoryPocketData(InventoryPocket inventoryPocket) 
        {
            ItemsData = new ItemData[inventoryPocket.ItemSlots.Length];

            for (int i = 0; i < ItemsData.Length; i++)
            {
                ItemsData[i] = new ItemData(inventoryPocket.ItemSlots[i].Item);
            }
        }
    }

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
                ItemSlots[i].SetParent(this);
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

        public IItemSlot FindSlot(IItemSlot slot)
        {
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                if (ItemSlots[i].IsEmpty)
                    return ItemSlots[i];
            }
            return null;
        }

        public bool TryFindSlot(PassiveEffect.PassiveType passiveEffect, out IItemSlot slot)
        {
            slot = null;

            foreach (var s in ItemSlots)
            {
                if (s.Item is NotUsableItem)
                {
                    if ((s.ItemInfo as NotUsableItemInfo).PassiveEffect.EffectType == passiveEffect)
                    {
                        slot = s;
                        return true;
                    }
                }
            }

            return false;
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

        public void ClearAllNotEmptySlots()
        {
            foreach (var slot in ItemSlots)
            {
                if (!slot.IsEmpty)
                {
                    slot.ClearSlot();
                }
            }
        }

        public void TrySetItemsInSlots(IItem[] items)
        {
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                ItemSlots[i].TrySetItemInSlot(items[i].Clone());
            }
        }

        public void TrySetItemsInSlots(ItemData[] itemsData)
        {
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                ItemSlots[i].TrySetItemInSlot(itemsData[i].CreateItem());
            }
        }
    }
}
