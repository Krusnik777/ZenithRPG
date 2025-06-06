using UnityEngine.Events;
using static UnityEditor.Progress;

namespace DC_ARPG
{
    public class MagicItemSlot : IItemSlot
    {
        public IItem Item { get; protected set; }
        public ItemInfo ItemInfo => Item?.Info;

        public int Amount => IsEmpty ? 0 : Item.Amount;
        public int Capacity { get; protected set; }

        public bool IsEmpty => Item == null;
        public bool IsFull => !IsEmpty && Amount >= Capacity;

        public event UnityAction<object, MagicItem> EventOnMagicUsed;

        public bool TrySetItemInSlot(IItem item)
        {
            if (item == null) return false;

            if (!(item is MagicItem))
            {
                UnityEngine.Debug.Log("WrongTypeOfItem");
                return false;
            }

            if (!IsEmpty) return false;

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

            if (!(item is MagicItem))
            {
                UnityEngine.Debug.Log("WrongTypeOfItem");
                return false;
            }

            ClearSlot();

            return TrySetItemInSlot(item);
        }

        public void UseMagic(object sender, Player player)
        {
            var item = Item as MagicItem;

            (ItemInfo as MagicItemInfo).Magic.Use(player, item);

            if (item.Uses <= 0)
            {
                var character = player.Character as PlayerCharacter;

                character.Inventory.RemoveItemFromInventory(this, character.Inventory.MagicItemSlot);

                UISounds.Instance.PlayMagicItemDisappearSound();
            }

            EventOnMagicUsed?.Invoke(sender, Item as MagicItem);
        }
    }
}
