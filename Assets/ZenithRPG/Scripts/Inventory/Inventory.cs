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
        public UsableItemSlot ActiveItemSlot { get; private set; }

        public InventoryPocket MainPocket { get; private set;  }
        public InventoryPocket[] ExtraPockets { get; private set; }

        #endregion

        #region Events

        public event UnityAction<object, IItemSlot> EventOnItemAdded;
        public event UnityAction<object, IItemSlot> EventOnItemUsed;
        public event UnityAction<object, IItemSlot> EventOnItemRemoved;
        public event UnityAction<object, IItemSlot, IItemSlot> EventOnTransitCompleted;

        public event UnityAction<object, int> EventOnActiveItemChanged;

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

        public void SetActiveItemSlot(object sender, int slotNumber)
        {
            if (ActiveItemSlot == UsableItemSlots[slotNumber]) return;

            ActiveItemSlot = UsableItemSlots[slotNumber];

            EventOnActiveItemChanged?.Invoke(sender, slotNumber);
        }

        public int GetActiveItemSlotNumber()
        {
            for (int i = 0; i < UsableItemSlots.Length; i++)
            {
                if (UsableItemSlots[i] == ActiveItemSlot)
                    return i;
            }

            return 0;
        }

        public void SetBrokenWeapon(object sender, IItem brokenWeapon)
        {
            WeaponItemSlot.TrySetItemInSlot(brokenWeapon.Clone());
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

            SetActiveItemSlot(this, 0);

            MainPocket = new InventoryPocket(itemSlotsAmountInPocket, InventoryPocket.PocketType.Main);

            ExtraPockets = new InventoryPocket[extraPocketsAmount];
            for (int i = 0; i < ExtraPockets.Length; i++)
            {
                ExtraPockets[i] = new InventoryPocket(itemSlotsAmountInPocket, InventoryPocket.PocketType.Extra);
            }
        }

        public void FillInventoryByInventoryData(PlayerInventoryData inventoryData)
        {
            ClearAllNotEmptySlots();

            UnlockedPockets = inventoryData.UnlockedPockets;

            WeaponItemSlot.TrySetItemInSlot(inventoryData.EquippedWeapon.CreateItem());
            ArmorItemSlot.TrySetItemInSlot(inventoryData.EquippedArmor.CreateItem());
            ShieldItemSlot.TrySetItemInSlot(inventoryData.EquippedShield.CreateItem());
            MagicItemSlot.TrySetItemInSlot(inventoryData.EquippedMagicItem.CreateItem());

            for (int i = 0; i < UsableItemSlots.Length; i++)
            {
                UsableItemSlots[i].TrySetItemInSlot(inventoryData.EquippedUsableItems[i].CreateItem());
            }

            SetActiveItemSlot(this, inventoryData.ActiveItemSlotIdNumber);

            MainPocket.TrySetItemsInSlots(inventoryData.MainPocketData.ItemsData);

            for (int i = 0; i < ExtraPockets.Length; i++)
            {
                ExtraPockets[i].TrySetItemsInSlots(inventoryData.ExtraPocketsData[i].ItemsData);
            }
        }

        #region Public GetMethods

        public WeaponItem GetEquippedWeapon() => WeaponItemSlot.Item as WeaponItem;
        public EquipItem GetEquippedArmor() => ArmorItemSlot.Item as EquipItem;
        public EquipItem GetEquippedShield() => ShieldItemSlot.Item as EquipItem;
        public MagicItem GetEquippedMagicItem() => MagicItemSlot.Item as MagicItem;

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
            if (MainPocket.IsFull) return false;

            var availableSlot = MainPocket.GetEmptySlot();
            availableSlot.TrySetItemInSlot(item.Clone());

            EventOnItemAdded?.Invoke(sender, availableSlot);

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
            if (slot.IsEmpty)
            {
                UISounds.Instance.PlayInventoryActionFailureSound();
                return;
            }

            slot.Item.Use(sender, this, slot, EventOnItemUsed);

            /*
            if (slot.Item is NotUsableItem)
            {
                Debug.Log("NotUsable");
                UISounds.Instance.PlayInventoryActionFailureSound();
                return;
            }

            if (slot.Item is UsableItem)
            {
                (slot.ItemInfo as UsableItemInfo).UseEffect.Use(m_player, slot.Item);

                if (slot.Item.Amount <= 0)
                {
                    RemoveItemFromInventory(sender, slot);
                    return;
                }

                EventOnItemUsed?.Invoke(sender, slot);
            }
            else EquipItem(sender, slot);
            */
        }

        public void EquipItem(object sender, IItemSlot slot)
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

        public void ClearAllNotEmptySlots()
        {
            if (!WeaponItemSlot.IsEmpty) WeaponItemSlot.ClearSlot();
            if (!ArmorItemSlot.IsEmpty) ArmorItemSlot.ClearSlot();
            if (!ShieldItemSlot.IsEmpty) ShieldItemSlot.ClearSlot();
            if (!MagicItemSlot.IsEmpty) MagicItemSlot.ClearSlot();

            foreach (var usableItemSlot in UsableItemSlots)
            {
                if (!usableItemSlot.IsEmpty) usableItemSlot.ClearSlot();
            }

            MainPocket.ClearAllNotEmptySlots();

            foreach (var extraPocket in ExtraPockets)
            {
                extraPocket.ClearAllNotEmptySlots();
            }
        }

        #endregion

        #region Private Methods

        private void HandleTransitFromSlotToSlot(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            if (fromSlot == toSlot) return;

            if (!toSlot.IsEmpty && toSlot.ItemInfo == fromSlot.ItemInfo)
            {
                if (toSlot.ItemInfo is UsableItemInfo || toSlot.ItemInfo is NotUsableItemInfo)
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
                else SwapItemsInSlots(sender, fromSlot, toSlot);
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
                if (toSlot.GetType() != fromSlot.GetType())
                {
                    if (toSlot.Item.GetType() != fromSlot.Item.GetType())
                    {
                        Debug.Log("Swap Error -> Not Same SlotType And Not Same ItemType");
                        return;
                    }

                    if (toSlot.Item is EquipItem && fromSlot.Item is EquipItem)
                    {
                        if ((toSlot.Item as EquipItem).EquipType != (fromSlot.Item as EquipItem).EquipType)
                        {
                            Debug.Log("Swap Error -> Not Same SlotType, Same ItemType: is EquipItem, but not same EquipType");
                            return;
                        }
                    }
                }

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
        #endregion
    }
}
