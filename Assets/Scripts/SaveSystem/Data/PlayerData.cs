namespace DC_ARPG
{
    [System.Serializable]
    public class PlayerStatsData
    {
        public int Level;

        public int HitPoints;
        public int CurrentHitPoints;

        public int MagicPoints;
        public int CurrentMagicPoints;

        public int Strength;
        public int Intelligence;
        public int MagicResist;
        public int Luck;

        public int CurrentExperiencePoints;
        public int CurrentStrengthExperiencePoints;
        public int CurrentIntelligenceExperiencePoints;
        public int CurrentMagicResistExperiencePoints;

        public PlayerStatsData(PlayerStats playerStats)
        {
            Level = playerStats.Level;

            HitPoints = playerStats.HitPoints;
            CurrentHitPoints = playerStats.CurrentHitPoints;

            MagicPoints = playerStats.MagicPoints;
            CurrentMagicPoints = playerStats.CurrentMagicPoints;

            Strength = playerStats.Strength;
            Intelligence = playerStats.Intelligence;
            MagicResist = playerStats.MagicResist;
            Luck = playerStats.Luck;

            CurrentExperiencePoints = playerStats.CurrentExperiencePoints;
            CurrentStrengthExperiencePoints = playerStats.CurrentStrengthExperiencePoints;
            CurrentIntelligenceExperiencePoints = playerStats.CurrentIntelligenceExperiencePoints;
            CurrentMagicResistExperiencePoints = playerStats.CurrentMagicResistExperiencePoints;
        }

        public PlayerStatsData(PlayerStatsData playerStatsData)
        {
            Level = playerStatsData.Level;

            HitPoints = playerStatsData.HitPoints;
            CurrentHitPoints = playerStatsData.CurrentHitPoints;

            MagicPoints = playerStatsData.MagicPoints;
            CurrentMagicPoints = playerStatsData.CurrentMagicPoints;

            Strength = playerStatsData.Strength;
            Intelligence = playerStatsData.Intelligence;
            MagicResist = playerStatsData.MagicResist;
            Luck = playerStatsData.Luck;

            CurrentExperiencePoints = playerStatsData.CurrentExperiencePoints;
            CurrentStrengthExperiencePoints = playerStatsData.CurrentStrengthExperiencePoints;
            CurrentIntelligenceExperiencePoints = playerStatsData.CurrentIntelligenceExperiencePoints;
            CurrentMagicResistExperiencePoints = playerStatsData.CurrentMagicResistExperiencePoints;
        }
    }

    [System.Serializable]
    public class PlayerInventoryData
    {
        public int UnlockedPockets;

        public ItemData EquippedWeapon;
        public ItemData EquippedArmor;
        public ItemData EquippedShield;
        public ItemData EquippedMagicItem;

        public ItemData[] EquippedUsableItems;
        public int ActiveItemSlotIdNumber;

        public InventoryPocketData MainPocketData;
        public InventoryPocketData[] ExtraPocketsData;

        public PlayerInventoryData(Inventory inventory)
        {
            UnlockedPockets = inventory.UnlockedPockets;

            EquippedWeapon = new ItemData(inventory.GetEquippedWeapon());
            EquippedArmor = new ItemData(inventory.GetEquippedArmor());
            EquippedShield = new ItemData(inventory.GetEquippedShield());
            EquippedMagicItem = new ItemData(inventory.GetEquippedMagicItem());

            EquippedUsableItems = new ItemData[inventory.UsableItemSlots.Length];

            for (int i = 0; i < EquippedUsableItems.Length; i++)
            {
                EquippedUsableItems[i] = new ItemData(inventory.UsableItemSlots[i].Item);
            }

            ActiveItemSlotIdNumber = inventory.GetActiveItemSlotNumber();

            MainPocketData = new InventoryPocketData(inventory.MainPocket);

            ExtraPocketsData = new InventoryPocketData[inventory.ExtraPockets.Length];

            for (int i = 0; i < ExtraPocketsData.Length; i++)
            {
                ExtraPocketsData[i] = new InventoryPocketData(inventory.ExtraPockets[i]);
            }
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public PlayerStatsData PlayerStatsData;
        public PlayerInventoryData PlayerInventoryData;
        public int Money;

        public PlayerData() { }

        public PlayerData(PlayerStats playerStats,Inventory inventory, int money)
        {
            PlayerStatsData = new PlayerStatsData(playerStats);
            PlayerInventoryData = new PlayerInventoryData(inventory);
            Money = money;
        }
    }
}
