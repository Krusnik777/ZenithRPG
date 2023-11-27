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
        public event UnityAction<object, IItemSlot> EventOnItemUsed;
        public event UnityAction<object, IItemSlot> EventOnItemRemoved;
        public event UnityAction<object, IItemSlot, IItemSlot> EventOnTransitCompleted;

        #endregion

        #region Support Parameters and Methods

        public int UnlockedPockets = 3;

        private IItemSlot m_fromSlot;

        public void SetFromSlot(IItemSlot slot)
        {
            m_fromSlot = slot;
        }

        public void TransitToSlot(object sender, IItemSlot toSlot)
        {
            TransitFromSlotToSlot(sender, m_fromSlot, toSlot);
            m_fromSlot = null;
        }

        #endregion

        public Inventory(int extraPocketsAmount = 3, int itemSlotsAmountInPocket = 9, int usableItemSlotsAmount = 3)
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

        public IItem GetEquippedWeapon() => WeaponItemSlot.Item;
        public IItem GetEquippedArmor() => ArmorItemSlot.Item;
        public IItem GetEquippedShield() => ShieldItemSlot.Item;
        public IItem GetEquippedMagicItem() => MagicItemSlot.Item;

        public IItem[] GetAllItemsInPocket(InventoryPocket pocket) => pocket.GetAllItems();

        public IItem[] GetAllSameItemsInPocket(InventoryPocket pocket, IItem item) => pocket.GetAllSameItems(item);

        public IItem[] GeActiveUsableItems()
        {
            var activeItems = new List<IItem>();
            foreach (var slot in UsableItemSlots)
            {
                if (!slot.IsEmpty && slot.Item is UsableItem)
                    activeItems.Add(slot.Item as UsableItem);
            }
            return activeItems.ToArray();
        }

        #endregion

        #region Public Actions with Inventory

        public bool TryToAddItem(object sender, IItem item)
        {
            if (MainPocket.IsFull)
            {
                ShortMessage.Instance.ShowMessage("Нет места в инвентаре.");
                return false;
            }
            var availableSlot = MainPocket.GetEmptySlot();
            availableSlot.TrySetItemInSlot(item.Clone());

            EventOnItemAdded?.Invoke();

            ShortMessage.Instance.ShowMessage("Добавлено в инвентарь: " + item.Info.Title);

            return true;
        }

        public void TransitFromSlotToSlot(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            if (fromSlot.IsEmpty) return;

            HandleTransitFromSlotToSlot(sender, fromSlot, toSlot);

            #region OBSOLETE
            /*
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
            */
            #endregion
        }

        public void RemoveItemFromInventory(object sender, IItemSlot slot)
        {
            if (slot.IsEmpty) return;

            slot.ClearSlot();

            EventOnItemRemoved?.Invoke(sender, slot);
        }

        public void UseItem(object sender, IItemSlot slot)
        {
            if (slot.IsEmpty) return;

            if (slot.Item is NotUsableItem)
            {
                Debug.Log("NotUsable");
                return;
            }

            if (slot.Item is UsableItem)
            {
                //slot.Item.Use();
                slot.Item.Amount--;
                Debug.Log("ItemUsed");

                if (slot.Item.Amount <= 0)
                {
                    RemoveItemFromInventory(sender, slot);
                    return;
                }

                EventOnItemUsed?.Invoke(sender, slot);
            }
            else EquipItem(sender, slot);
        }

        public IItemSlot TryToGetInventorySlot(IItemSlot slot)
        {
            if (slot == WeaponItemSlot) return WeaponItemSlot;
            if (slot == ArmorItemSlot) return ArmorItemSlot;
            if (slot == ShieldItemSlot) return ShieldItemSlot;
            if (slot == MagicItemSlot) return MagicItemSlot;

            for (int i = 0; i < UsableItemSlots.Length; i++)
            {
                if (UsableItemSlots[i] == slot) return UsableItemSlots[i];
            }

            var findedSlot = MainPocket.FindSlot(slot);

            if (findedSlot != null) return findedSlot;

            for (int i = 0; i < ExtraPockets.Length; i++)
            {
                findedSlot = ExtraPockets[i].FindSlot(slot);
                if (findedSlot != null) return findedSlot;
            }

            return null;
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

        private void HandleTransitFromSlotToSlot(object sender, IItemSlot fromSlot, IItemSlot toSlot)
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

                    if (fits) fromSlot.ClearSlot();
                    else fromSlot.Item.Amount = amountLeft;

                    EventOnTransitCompleted?.Invoke(sender, fromSlot, toSlot);
                }
                return;
            }

            if (!toSlot.IsEmpty && toSlot.ItemInfo != fromSlot.ItemInfo)
                SwapItemsInSlots(sender, fromSlot, toSlot);

            if (toSlot.IsEmpty)
                PlaceItemInEmptySlot(sender, fromSlot, toSlot);
        }

        private void SwapItemsInSlots(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            if (!toSlot.IsEmpty)
            {
                var swappedItem = toSlot.Item;

                if (toSlot.TryClearSlotAndSetItem(fromSlot.Item) == false) return;

                if (fromSlot.TryClearSlotAndSetItem(swappedItem) == false)
                {
                    Debug.Log("Swap Error -> fromSlot.TryClearSlotAndSetItem(swappedItem) = false");
                }

                EventOnTransitCompleted?.Invoke(sender, fromSlot, toSlot);
            }
            else PlaceItemInEmptySlot(sender, fromSlot, toSlot);
        }

        private void PlaceItemInEmptySlot(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            if (toSlot.TrySetItemInSlot(fromSlot.Item) == false) return;
            fromSlot.ClearSlot();

            EventOnTransitCompleted?.Invoke(sender, fromSlot, toSlot);
        }

        private void EquipItem(object sender, IItemSlot slot)
        {
            if (slot.Item is WeaponItem)
            {
                TransitFromSlotToSlot(sender, slot, WeaponItemSlot);
            }

            if (slot.Item is MagicItem)
            {
                TransitFromSlotToSlot(sender, slot, MagicItemSlot);
            }

            if (slot.Item is EquipItem)
            {
                var item = slot.Item as EquipItem;

                if (item.EquipType == EquipItemType.Armor)
                    TransitFromSlotToSlot(sender, slot, ArmorItemSlot);

                if (item.EquipType == EquipItemType.Shield)
                    TransitFromSlotToSlot(sender, slot, ShieldItemSlot);
            }
        }

        #endregion
    }
}
