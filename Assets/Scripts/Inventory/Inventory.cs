using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class Inventory
    {
        #region Elements

        public WeaponItemSlot WeaponItemSlot { get; private set; }
        public ArmorItemSlot ArmorItemSlot { get; private set; }
        public ShieldItemSlot ShieldItemSlot { get; private set; }
        public MagicItemSlot MagicItemSlot { get; private set; }

        public UsableItemSlot[] UsableItemSlots { get; private set; }

        public InventoryPocket MainPocket { get; private set;  }
        public InventoryPocket[] ExtraPockets { get; private set; }

        #endregion

        #region Events

        public event UnityAction EventOnItemAdded;
        public event UnityAction<object, IItem> EventOnItemRemoved;
        public event UnityAction EventOnInventoryChange;

        #endregion

        public Inventory(int extraPocketsAmount, int itemSlotsAmountInPocket, int usableItemSlotsAmount)
        {
            WeaponItemSlot = new WeaponItemSlot();
            ArmorItemSlot = new ArmorItemSlot();
            ShieldItemSlot = new ShieldItemSlot();
            MagicItemSlot = new MagicItemSlot();

            UsableItemSlots = new UsableItemSlot[usableItemSlotsAmount];
            for (int i = 0; i < UsableItemSlots.Length; i++)
            {
                UsableItemSlots[i] = new UsableItemSlot();
            }

            MainPocket = new InventoryPocket(itemSlotsAmountInPocket, InventoryPocket.PocketType.Main);

            ExtraPockets = new InventoryPocket[extraPocketsAmount];
            for (int i = 0; i < ExtraPockets.Length; i++)
            {
                ExtraPockets[i] = new InventoryPocket(itemSlotsAmountInPocket, InventoryPocket.PocketType.Extra);
            }
        }

        #region Public GetMethods

        public WeaponItem GetEquippedWeapon() => WeaponItemSlot.Item;
        public EquipItem GetEquippedArmor() => ArmorItemSlot.Item;
        public EquipItem GetEquippedShield() => ShieldItemSlot.Item;
        public MagicItem GetEquippedMagicItem() => MagicItemSlot.Item;

        public IItem[] GetAllItemsInPocket(InventoryPocket pocket) => pocket.GetAllItems();

        public IItem[] GetAllSameItemsInPocket(InventoryPocket pocket, IItem item) => pocket.GetAllSameItems(item);

        public UsableItem[] GeActiveUsableItems()
        {
            var activeItems = new List<UsableItem>();
            foreach (var slot in UsableItemSlots)
            {
                if (!slot.IsEmpty)
                    activeItems.Add(slot.Item);
            }
            return activeItems.ToArray();
        }

        #endregion

        #region Public Actions with Inventory

        public void TryToAddItem(object sender, IItem item)
        {
            if (MainPocket.IsFull)
            {
                Debug.Log("No place for this item");
                return;
            }
            var availableSlot = MainPocket.GetEmptySlot();
            availableSlot.SetItemInSlot(item);

            EventOnItemAdded?.Invoke();
            EventOnInventoryChange?.Invoke();
        }

        public void TransitFromSlotToSlot(object sender, IItemSlot<IItem> fromSlot, IItemSlot<IItem> toSlot)
        {
            if (fromSlot.IsEmpty) return;

            if (!(fromSlot is AnyItemSlot) && toSlot is AnyItemSlot)
                HandleTransitFromSlotToSlot(fromSlot, toSlot);

            if (fromSlot is AnyItemSlot && !(toSlot is AnyItemSlot))
            {
                if (toSlot is WeaponItemSlot && fromSlot.Item is WeaponItem)
                    SwapItemsInSlots(fromSlot, toSlot);

                if (fromSlot.Item is EquipItem)
                {
                    var item = fromSlot.Item as EquipItem;

                    if (toSlot is ArmorItemSlot && item.EquipType == EquipItemType.Armor)
                        SwapItemsInSlots(fromSlot, toSlot);

                    if (toSlot is ShieldItemSlot && item.EquipType == EquipItemType.Shield)
                        SwapItemsInSlots(fromSlot, toSlot);
                }

                if (toSlot is MagicItemSlot && fromSlot.Item is MagicItem)
                    SwapItemsInSlots(fromSlot, toSlot);

                if (toSlot is UsableItemSlot && fromSlot.Item is UsableItem)
                    HandleTransitFromSlotToSlot(fromSlot, toSlot);
            }

            if (fromSlot is AnyItemSlot && toSlot is AnyItemSlot)
                HandleTransitFromSlotToSlot(fromSlot, toSlot);
        }

        public void RemoveItemFromInventory(object sender, AnyItemSlot slot)
        {
            var item = slot.Item;
            slot.ClearSlot();

            EventOnItemRemoved?.Invoke(sender, item);
            EventOnInventoryChange?.Invoke();
        }

        public IItem TryToGetItem(IItem item, InventoryPocket pocket)
        {
            if (pocket.IsEmpty)
            {
                Debug.Log("Pocket is Empty");
                return null;
            }
            return pocket.FindItem(item);
        }

        #endregion

        #region Private Methods

        private void HandleTransitFromSlotToSlot(IItemSlot<IItem> fromSlot, IItemSlot<IItem> toSlot)
        {
            if (!toSlot.IsEmpty && toSlot.ItemInfo == fromSlot.ItemInfo)
            {
                var slotCapacity = toSlot.Capacity;
                bool fits = fromSlot.Amount + toSlot.Amount <= slotCapacity;
                var amountToAdd = fits ? fromSlot.Amount : slotCapacity - toSlot.Amount;
                var amountLeft = fromSlot.Amount - amountToAdd;

                if (amountToAdd != 0)
                {
                    toSlot.Item.Amount += amountToAdd;
                    Debug.Log(toSlot.Item.Amount);

                    if (fits) fromSlot.ClearSlot();
                    else fromSlot.Item.Amount = amountLeft;

                    EventOnInventoryChange?.Invoke();
                }
                return;
            }

            if (!toSlot.IsEmpty && toSlot.ItemInfo != fromSlot.ItemInfo)
                SwapItemsInSlots(fromSlot, toSlot);

            if (toSlot.IsEmpty)
                PlaceItemInEmptySlot(fromSlot, toSlot);
        }

        private void SwapItemsInSlots(IItemSlot<IItem> fromSlot, IItemSlot<IItem> toSlot)
        {
            if (!toSlot.IsEmpty)
            {
                var swappedItem = toSlot.Item;

                toSlot.ClearSlot();
                toSlot.SetItemInSlot(fromSlot.Item);

                fromSlot.ClearSlot();
                fromSlot.SetItemInSlot(swappedItem);

                EventOnInventoryChange?.Invoke();
            }
            else PlaceItemInEmptySlot(fromSlot, toSlot);
        }

        private void PlaceItemInEmptySlot(IItemSlot<IItem> fromSlot, IItemSlot<IItem> toSlot)
        {
            toSlot.SetItemInSlot(fromSlot.Item);
            fromSlot.ClearSlot();

            EventOnInventoryChange?.Invoke();
        }

        #endregion
    }
}
