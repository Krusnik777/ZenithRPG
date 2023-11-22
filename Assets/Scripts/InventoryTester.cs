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
            inventory = new Inventory(1, 3, 2);
            Debug.Log("Inventory created: " + inventory);
        }

        private void Start()
        {
            inventory.TryToAddItem(this, notUsableItem);
            inventory.TryToAddItem(this, usableItem);
            inventory.TryToAddItem(this, armorItem);

            inventory.TryToAddItem(this, shieldItem);

            Debug.Log("Transit: " + inventory.MainPocket.ItemSlots[0].Item + " -> " + inventory.ExtraPockets[0].ItemSlots[0].Item);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.ExtraPockets[0].ItemSlots[0]);
            Debug.Log("Amount: " + inventory.ExtraPockets[0].ItemSlots[0].Item.Amount + " Capacity: " + inventory.ExtraPockets[0].ItemSlots[0].Capacity);

            inventory.TryToAddItem(this, notUsableItem2);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[0].Item + "Amount: " + inventory.MainPocket.ItemSlots[0].Item.Amount);

            Debug.Log("Transit: " + inventory.MainPocket.ItemSlots[0].Item + " -> " + inventory.ExtraPockets[0].ItemSlots[0].Item);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.ExtraPockets[0].ItemSlots[0]);
            Debug.Log("Amount: " + inventory.ExtraPockets[0].ItemSlots[0].Item.Amount + " Capacity: " + inventory.ExtraPockets[0].ItemSlots[0].Capacity);

            inventory.TryToAddItem(this, magicItem);
            Debug.Log("Added: " + inventory.MainPocket.ItemSlots[0].Item + "Amount: " + inventory.MainPocket.ItemSlots[0].Item.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[0].Item.MaxAmount);

            Debug.Log("Transit: " + inventory.MainPocket.ItemSlots[0].Item + " -> " + inventory.ExtraPockets[0].ItemSlots[0].Item);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.ExtraPockets[0].ItemSlots[0]);
            Debug.Log("Item: " + inventory.ExtraPockets[0].ItemSlots[0].Item);
            Debug.Log("Amount: " + inventory.ExtraPockets[0].ItemSlots[0].Item.Amount + " Capacity: " + inventory.ExtraPockets[0].ItemSlots[0].Capacity);

            Debug.Log(inventory.MainPocket.ItemSlots[0].Item);
            Debug.Log("Amount: " + inventory.MainPocket.ItemSlots[0].Item.Amount + " Capacity: " + inventory.MainPocket.ItemSlots[0].Capacity);
            Debug.Log(inventory.MainPocket.ItemSlots[1].Item);
            Debug.Log(inventory.MainPocket.ItemSlots[2].Item);

            //inventory.TryToAddItem(this, notUsableItem);

            /*
            inventory.TryToAddItem(this, notUsableItem);
            
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.ExtraPockets[0].ItemSlots[0]);
            Debug.Log(inventory.ExtraPockets[0].ItemSlots[0].Item);
            Debug.Log(inventory.ExtraPockets[0].ItemSlots[0].Amount);
            Debug.Log(inventory.ExtraPockets[0].ItemSlots[0].Capacity);

            inventory.TryToAddItem(this, notUsableItem);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.ExtraPockets[0].ItemSlots[0]);
            Debug.Log(inventory.ExtraPockets[0].ItemSlots[0].Item);
            Debug.Log(inventory.ExtraPockets[0].ItemSlots[0].Amount);
            Debug.Log(inventory.ExtraPockets[0].ItemSlots[0].Capacity);
            Debug.Log(inventory.ExtraPockets[0].ItemSlots[1].Item);*/

        }
    }
}
