using UnityEngine.Events;

namespace DC_ARPG
{
    public class WeaponItemSlot : IItemSlot
    {
        public IItem Item { get; protected set; }
        public ItemInfo ItemInfo => Item?.Info;

        public int Amount => IsEmpty ? 0 : Item.Amount;
        public int Capacity { get; protected set; }

        public bool IsEmpty => Item == null;
        public bool IsFull => !IsEmpty && Amount >= Capacity;

        public event UnityAction<object> EventOnAttack;
        public event UnityAction<object> EventOnBrokenWeapon;

        public bool TrySetItemInSlot(IItem item)
        {
            if (item == null) return false;

            if (!(item is WeaponItem))
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

            if (!(item is WeaponItem))
            {
                UnityEngine.Debug.Log("WrongTypeOfItem");
                return false;
            }

            ClearSlot();

            return TrySetItemInSlot(item);
        }

        public void UseWeapon(object sender, Player player, UnityAction OnBroken = null)
        {
            var weaponItem = Item as WeaponItem;

            weaponItem.Uses--;

            if (weaponItem.Uses <= 0)
            {
                var playerCharacter = player.Character as PlayerCharacter;

                playerCharacter.Inventory.RemoveItemFromInventory(sender, playerCharacter.Inventory.WeaponItemSlot);
                playerCharacter.Inventory.SetBrokenWeapon(sender, playerCharacter.BrokenWeapon);

                UISounds.Instance.PlaySwordBreakSound();

                OnBroken?.Invoke();
                EventOnBrokenWeapon?.Invoke(sender);
            }

            EventOnAttack?.Invoke(sender);
        }
    }
}
