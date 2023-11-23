using UnityEngine;

namespace DC_ARPG
{
    public class InventoryTester : MonoBehaviour
    {
        [SerializeField] private NotUsableItem notUsableItem;
        [SerializeField] private NotUsableItem notUsableItem2;
        [SerializeField] private UsableItem usableItem;
        [SerializeField] private EquipItem armorItem;
        [SerializeField] private EquipItem shieldItem;
        [SerializeField] private WeaponItem weaponItem;
        [SerializeField] private MagicItem magicItem;

        private Inventory inventory;

        private void Awake()
        {
            inventory = new Inventory(1, 7, 2);
            Debug.Log("Inventory created: " + inventory);
        }

        private void Start()
        {
            inventory.TryToAddItem(this, notUsableItem);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[0].Item + " Amount: " + inventory.MainPocket.ItemSlots[0].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[0].Item?.MaxAmount);
            inventory.TryToAddItem(this, notUsableItem2);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[1].Item + " Amount: " + inventory.MainPocket.ItemSlots[1].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[1].Item?.MaxAmount);
            inventory.TryToAddItem(this, usableItem);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[2].Item + " Amount: " + inventory.MainPocket.ItemSlots[2].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[2].Item?.MaxAmount);
            inventory.TryToAddItem(this, armorItem);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[3].Item + " Amount: " + inventory.MainPocket.ItemSlots[3].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[3].Item?.MaxAmount);
            inventory.TryToAddItem(this, shieldItem);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[4].Item + " Amount: " + inventory.MainPocket.ItemSlots[4].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[4].Item?.MaxAmount);
            inventory.TryToAddItem(this, weaponItem);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[5].Item + " Amount: " + inventory.MainPocket.ItemSlots[5].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[5].Item?.MaxAmount);
            inventory.TryToAddItem(this, magicItem);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[6].Item + " Amount: " + inventory.MainPocket.ItemSlots[6].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[6].Item?.MaxAmount);

            Debug.Log("1: " + inventory.MainPocket.ItemSlots[0].Item + " Amount: " + inventory.MainPocket.ItemSlots[0].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[0].Item?.MaxAmount);
            Debug.Log("2: " + inventory.MainPocket.ItemSlots[1].Item + " Amount: " + inventory.MainPocket.ItemSlots[1].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[1].Item?.MaxAmount);
            Debug.Log("3: " + inventory.MainPocket.ItemSlots[2].Item + " Amount: " + inventory.MainPocket.ItemSlots[2].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[2].Item?.MaxAmount);
            Debug.Log("4: " + inventory.MainPocket.ItemSlots[3].Item + " Amount: " + inventory.MainPocket.ItemSlots[3].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[3].Item?.MaxAmount);
            Debug.Log("5: " + inventory.MainPocket.ItemSlots[4].Item + " Amount: " + inventory.MainPocket.ItemSlots[4].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[4].Item?.MaxAmount);
            Debug.Log("6: " + inventory.MainPocket.ItemSlots[5].Item + " Amount: " + inventory.MainPocket.ItemSlots[5].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[5].Item?.MaxAmount);
            Debug.Log("7: " + inventory.MainPocket.ItemSlots[6].Item + " Amount: " + inventory.MainPocket.ItemSlots[6].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[6].Item?.MaxAmount);

            Debug.Log("Weapon: " + inventory.WeaponItemSlot.Item + " Amount: " + inventory.WeaponItemSlot.Item?.Amount + " Capacity: " + inventory.WeaponItemSlot.Item?.MaxAmount);

            Debug.Log("Transit: " + inventory.MagicItemSlot.Item + " -> " + inventory.WeaponItemSlot.Item);
            inventory.TransitFromSlotToSlot(this, inventory.MagicItemSlot, inventory.WeaponItemSlot);
            Debug.Log("Weapon: " + inventory.WeaponItemSlot.Item + " Amount: " + inventory.WeaponItemSlot.Item?.Amount + " Capacity: " + inventory.WeaponItemSlot.Item?.MaxAmount);
            Debug.Log("5: " + inventory.MainPocket.ItemSlots[5].Item + " Amount: " + inventory.MainPocket.ItemSlots[5].Item?.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[5].Item?.MaxAmount);

        }
    }
}
