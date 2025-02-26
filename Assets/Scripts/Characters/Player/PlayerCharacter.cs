using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class PlayerCharacter : CharacterBase
    {
        [System.Serializable]
        public class EquippedItems
        {
            public EquipItem Armor;
            public EquipItem Shield;
            public WeaponItem Weapon;
            public MagicItem Magic;
        }

        [SerializeField] private Player m_player;
        [SerializeField] private PlayerStatsInfo m_playerStatsInfo;
        [SerializeField] private int m_startedMoney = 999;
        [SerializeField] private WeaponItem m_brokenWeapon;
        [Header("Inventory Initial State")]
        [SerializeField] private int m_unlockedPockets;
        [SerializeField] private EquippedItems m_equippedItems;
        [SerializeField] private ItemData[] m_items;

        public Player Player => m_player;
        public WeaponItem BrokenWeapon => m_brokenWeapon;

        private PlayerStats playerStats;
        public override CharacterStats Stats => playerStats;

        private Inventory inventory;
        public Inventory Inventory => inventory;

        private int m_money;
        public int Money => m_money;

        public float DropItemBooster => (float) playerStats.Luck / playerStats.MaxStatLevel;

        public event UnityAction EventOnMoneyAdded;
        public event UnityAction EventOnMoneySpend;

        public override void DamageOpponent(CharacterAvatar opponent)
        {
            if (opponent.Character.Stats.CurrentHitPoints <= 0) return;

            opponent.Character.Stats.ChangeCurrentHitPoints(m_player, -playerStats.Attack, DamageType.Physic);
            playerStats.AddStrengthExperience(opponent.Character.Stats.Level);

            var weaponItem = m_equippedItems.Weapon;
            if (!weaponItem.HasInfiniteUses) inventory.WeaponItemSlot.UseWeapon(m_player, m_player);
        }

        public void UpdatePlayerCharacter(PlayerData playerData)
        {
            SetupInitialPlayerStats();

            playerStats.UpdateStats(playerData.PlayerStatsData);

            SetupInitialInventory();

            inventory.FillInventoryByInventoryData(playerData.PlayerInventoryData);
            UpdateAttack(inventory.WeaponItemSlot);
            UpdateArmorDefense(inventory.ArmorItemSlot);
            UpdateShieldDefense(inventory.ShieldItemSlot);

            m_money = 0;
            AddMoney(playerData.Money);
        }

        public void UnlockExtraPocket()
        {
            if (inventory.UnlockedPockets == 3) return;

            inventory.UnlockedPockets++;
        }

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
            SetupInitialPlayerStats();

            SetupInitialInventory();
        }

        private void OnDestroy()
        {
            playerStats.EventOnHitPointsChange -= OnHitPointsChange;
            playerStats.EventOnDeath -= OnDeath;

            inventory.EventOnTransitCompleted -= OnEquipItemChange;
            inventory.EventOnItemRemoved -= OnEquipItemRemoved;
        }

        private void SetupInitialPlayerStats()
        {
            if (playerStats != null) return;

            playerStats = new PlayerStats();
            playerStats.InitStats(m_playerStatsInfo);

            playerStats.EventOnHitPointsChange += OnHitPointsChange;
            playerStats.EventOnDeath += OnDeath;

            m_money = 0;
            AddMoney(m_startedMoney);
        }

        private void SetupInitialInventory()
        {
            if (inventory != null) return;

            inventory = new Inventory(3, 9, 3);
            inventory.UnlockedPockets = m_unlockedPockets;

            SetInitialItems();

            inventory.EventOnTransitCompleted += OnEquipItemChange;
            inventory.EventOnItemRemoved += OnEquipItemRemoved;
        }

        private void SetInitialItems()
        {
            // Set Initial Equipped Items
            {
                if (m_equippedItems.Armor.Info != null)
                {
                    inventory.TryToAddItem(this, m_equippedItems.Armor);
                    inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.ArmorItemSlot);
                    UpdateArmorDefense(inventory.ArmorItemSlot);
                }

                if (m_equippedItems.Shield.Info != null)
                {
                    inventory.TryToAddItem(this, m_equippedItems.Shield);
                    inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.ShieldItemSlot);
                    UpdateShieldDefense(inventory.ShieldItemSlot);
                }

                if (m_equippedItems.Weapon.Info != null)
                {
                    inventory.TryToAddItem(this, m_equippedItems.Weapon);
                    inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.WeaponItemSlot);
                    UpdateAttack(inventory.WeaponItemSlot);
                }

                if (m_equippedItems.Magic.Info != null)
                {
                    inventory.TryToAddItem(this, m_equippedItems.Magic);
                    inventory.TransitFromSlotToSlot(this, inventory.MainPocket.ItemSlots[0], inventory.MagicItemSlot);
                }
            }

            // Set Initial Inventory Items
            {
                foreach (var itemData in m_items)
                {
                    inventory.TryToAddItem(this, itemData.CreateItem());
                }
            }
        }

        private void OnHitPointsChange(int change)
        {
            if (change < 0 && playerStats.CurrentHitPoints != 0)
            {
                OnHit();
            }
        }

        private void OnDeath(object sender)
        {
            if (inventory.MainPocket.TryFindSlot(PassiveEffect.PassiveType.Revival, out IItemSlot slot))
            {
                var revivalItem = slot.ItemInfo as NotUsableItemInfo;

                revivalItem.PassiveEffect.GetEffect(m_player, slot);

                return;
            }

            MusicCommander.Instance.PlayGameOverMusic();

            m_player.Die();

            OnDead();
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
