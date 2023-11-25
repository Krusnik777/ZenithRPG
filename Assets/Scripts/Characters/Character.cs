using UnityEngine;

namespace DC_ARPG
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private PlayerStatsInfo m_playerStatsInfo;
        private PlayerStats playerStats;
        public PlayerStats PlayerStats => playerStats;

        private Inventory inventory;
        public Inventory Inventory => inventory;

        // TEST
        [SerializeField] private NotUsableItem[] notUsableItems;
        [SerializeField] private UsableItem[] usableItems;
        [SerializeField] private EquipItem[] armorItems;
        [SerializeField] private EquipItem[] shieldItems;
        [SerializeField] private WeaponItem[] weaponItems;
        [SerializeField] private MagicItem[] magicItems;

        private void Awake()
        {
            playerStats = new PlayerStats();
            playerStats.InitStats(m_playerStatsInfo);

            inventory = new Inventory(3, 9, 3);
        }

        private void Start()
        {
            // TEST

            inventory.TryToAddItem(this, armorItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.ArmorItemSlot);
            inventory.TryToAddItem(this, armorItems[1]); //1

            inventory.TryToAddItem(this, shieldItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[1], inventory.ShieldItemSlot);
            inventory.TryToAddItem(this, shieldItems[1]); //2

            inventory.TryToAddItem(this, weaponItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[2], inventory.WeaponItemSlot);
            inventory.TryToAddItem(this, weaponItems[1]);  //3

            inventory.TryToAddItem(this, magicItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[3], inventory.MagicItemSlot);
            inventory.TryToAddItem(this, magicItems[1]); //4

            inventory.TryToAddItem(this, notUsableItems[0]); //5
            inventory.TryToAddItem(this, notUsableItems[1]); //6

            inventory.TryToAddItem(this, usableItems[0]); //7
            inventory.TryToAddItem(this, usableItems[1]); //8
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[7], inventory.UsableItemSlots[0]);
            inventory.TryToAddItem(this, usableItems[1]);

            inventory.TryToAddItem(this, notUsableItems[0]);




        }
    }
}
