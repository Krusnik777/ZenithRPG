using UnityEngine;

namespace DC_ARPG
{
    public class Character : MonoBehaviour, IDependency<Player>
    {
        [SerializeField] private PlayerStatsInfo m_playerStatsInfo;
        private PlayerStats playerStats;
        public PlayerStats PlayerStats => playerStats;

        private Inventory inventory;
        public Inventory Inventory => inventory;

        private Player m_player;
        public void Construct(Player player) => m_player = player;

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

            playerStats.EventOnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            playerStats.EventOnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            var slot = inventory.MainPocket.FindSlot(PassiveEffect.PassiveType.Revival);

            if (slot != null)
            {
                (slot.ItemInfo as NotUsableItemInfo).PassiveEffect.GetEffect(m_player, slot);
                return;
            }

            Debug.Log("Game Over");
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
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[8], inventory.ExtraPockets[0].ItemSlots[5]);

            inventory.TryToAddItem(this, usableItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[8], inventory.ExtraPockets[1].ItemSlots[2]);

            inventory.TryToAddItem(this, magicItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[8], inventory.ExtraPockets[2].ItemSlots[7]);
        }

    }
}
