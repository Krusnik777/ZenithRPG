using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class PlayerCharacter : MonoBehaviour, IDependency<Player>
    {
        [SerializeField] private PlayerStatsInfo m_playerStatsInfo;
        [SerializeField] private Magic m_availableMagic;
        [SerializeField] private WeaponItem m_brokenWeapon;
        [Header("Effects")]
        [SerializeField] private GameObject m_hitEffectPrefab;
        public Magic AvailableMagic => m_availableMagic;
        public WeaponItem BrokenWeapon => m_brokenWeapon;

        private PlayerStats playerStats;
        public PlayerStats PlayerStats => playerStats;

        private Inventory inventory;
        public Inventory Inventory => inventory;

        private Player m_player;
        public void Construct(Player player) => m_player = player;

        private int m_money;
        public int Money => m_money;

        public float DropItemBooster => (float) playerStats.Luck / playerStats.MaxStatLevel;

        public event UnityAction EventOnMoneyAdded;
        public event UnityAction EventOnMoneySpend;

        // TEST
        [SerializeField] private NotUsableItem[] notUsableItems;
        [SerializeField] private UsableItem[] usableItems;
        [SerializeField] private EquipItem[] armorItems;
        [SerializeField] private EquipItem[] shieldItems;
        [SerializeField] private WeaponItem[] weaponItems;
        [SerializeField] private MagicItem[] magicItems;

        public void AddMoney(int amount)
        {
            m_money += amount;

            EventOnMoneyAdded?.Invoke();
        }

        public void SpendMoney(int amount)
        {
            m_money -= amount;

            EventOnMoneySpend?.Invoke();
        }

        private void Awake()
        {
            playerStats = new PlayerStats();
            playerStats.InitStats(m_playerStatsInfo);

            inventory = new Inventory(3, 9, 3);

            playerStats.EventOnHitPointsChange += OnHitPointsChange;
            playerStats.EventOnDeath += OnDeath;

            inventory.EventOnTransitCompleted += OnEquipItemChange;
            inventory.EventOnItemRemoved += OnEquipItemRemoved;
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
            inventory.TryToAddItem(this, usableItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[7], inventory.UsableItemSlots[1]);
            inventory.TryToAddItem(this, usableItems[1]);

            inventory.TryToAddItem(this, notUsableItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[8], inventory.ExtraPockets[0].ItemSlots[5]);

            inventory.TryToAddItem(this, usableItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[8], inventory.ExtraPockets[1].ItemSlots[2]);

            inventory.TryToAddItem(this, magicItems[0]);
            inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[8], inventory.ExtraPockets[2].ItemSlots[7]);

            AddMoney(999);
        }

        private void OnDestroy()
        {
            playerStats.EventOnHitPointsChange -= OnHitPointsChange;
            playerStats.EventOnDeath -= OnDeath;

            inventory.EventOnTransitCompleted -= OnEquipItemChange;
            inventory.EventOnItemRemoved -= OnEquipItemRemoved;
        }

        private void OnHitPointsChange(int change)
        {
            if (change < 0 && playerStats.CurrentHitPoints != 0)
            {
                m_player.CharacterSFX.PlayGetHitSound();

                var hitEffect = Instantiate(m_hitEffectPrefab, m_player.transform.position, Quaternion.identity);

                Destroy(hitEffect, 1f);
            }
        }

        private void OnDeath(object sender)
        {
            var slot = inventory.MainPocket.FindSlot(PassiveEffect.PassiveType.Revival);

            if (slot != null)
            {
                (slot.ItemInfo as NotUsableItemInfo).PassiveEffect.GetEffect(m_player, slot);

                // Effect?
                // Broken Item Sound?

                return;
            }

            m_player.CharacterSFX.PlayDeathSound();

            m_player.Animator.Play("Death");

            Debug.Log("Game Over");
            // And turn off everything
        }

        private void OnEquipItemRemoved(object sender, IItemSlot slot)
        {
            if (slot == inventory.WeaponItemSlot) UpdateAttack(slot);
            if (slot == inventory.ArmorItemSlot) UpdateArmorDefense(slot);
            if (slot == inventory.ShieldItemSlot) UpdateShieldDefense(slot);
        }

        private void OnEquipItemChange(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            if (fromSlot == inventory.WeaponItemSlot)
                UpdateAttack(fromSlot);
            if (toSlot == inventory.WeaponItemSlot)
                UpdateAttack(toSlot);

            if (fromSlot == inventory.ArmorItemSlot)
                UpdateArmorDefense(fromSlot);
            if (toSlot == inventory.ArmorItemSlot)
                UpdateArmorDefense(toSlot);

            if (fromSlot == inventory.ShieldItemSlot)
                UpdateShieldDefense(fromSlot);
            if (toSlot == inventory.ShieldItemSlot)
                UpdateShieldDefense(toSlot);
        }

        private void UpdateAttack(IItemSlot slot)
        {
            if (!slot.IsEmpty) playerStats.SetWeaponDamage((slot.Item as WeaponItem).AttackIncrease);
            else playerStats.SetWeaponDamage(0);
        }

        private void UpdateArmorDefense(IItemSlot slot)
        {
            if (!slot.IsEmpty) playerStats.SetArmorDefense((slot.Item as EquipItem).DefenseIncrease);
            else playerStats.SetArmorDefense(0);
        }

        private void UpdateShieldDefense(IItemSlot slot)
        {
            if (!slot.IsEmpty) playerStats.SetShieldDefense((slot.Item as EquipItem).DefenseIncrease);
            else playerStats.SetShieldDefense(0);
        }

    }
}
