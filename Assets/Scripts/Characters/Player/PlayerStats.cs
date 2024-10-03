using UnityEngine.Events;

namespace DC_ARPG
{
    public class PlayerStats : CharacterStats
    {
        #region VariablesForUpgradableStats

        private int levelGrowthPoints;
        private int strengthGrowthPoints;
        private int intelligenceGrowthPoints;
        private int magicResistGrowthPoints;
        private int hitPointsGrowthPoints;
        private int magicPointsGrowthPoints;

        private float experienceGrowthMultiplier;
        private float strengthGrowthMultiplier;
        private float intelligenceGrowthMultiplier;
        private float magicResistGrowthMultiplier;
        private float hitPointsGrowthMultiplier;
        private float magicPointsGrowthMultiplier;

        public int GetExperienceForLevelUp() => (int)(levelGrowthPoints * Level * experienceGrowthMultiplier);
        public int GetExperienceForStrengthUp() => (int)(strengthGrowthPoints * Strength * strengthGrowthMultiplier);
        public int GetExperienceForIntelligenceUp() => (int)(intelligenceGrowthPoints * Intelligence * intelligenceGrowthMultiplier);
        public int GetExperienceForMagicResistUp() => (int)(magicResistGrowthPoints * MagicResist * magicResistGrowthMultiplier);

        #endregion

        #region CurrentStats/Parameters

        public int MaxLevel { get; protected set; }
        public int MaxStatLevel { get; protected set; }

        public int CurrentExperiencePoints { get; private set; }
        public int CurrentStrengthExperiencePoints { get; private set; }
        public int CurrentIntelligenceExperiencePoints { get; private set; }
        public int CurrentMagicResistExperiencePoints { get; private set; }

        public int BaseStrengthExperience { get; private set; }
        public int BaseIntelligenceExperience { get; private set; }
        public int BaseMagicResistExperience { get; private set; }

        private int equippedWeaponDamage = 0;
        private float attackMultiplier;

        private int equippedArmorDefense = 0;
        private int equippedShieldDefense = 0;
        private float defenseMultiplier;

        public float HitPointsRecoveryRate { get; protected set; }
        public float MagicPointsRecoveryRate { get; protected set; }

        #endregion

        #region Events

        public event UnityAction EventOnLevelUp;
        public event UnityAction EventOnStrengthUp;
        public event UnityAction EventOnIntelligenceUp;
        public event UnityAction EventOnMagicResistUp;
        public event UnityAction EventOnLuckUp;

        public event UnityAction EventOnAttackChange;
        public event UnityAction EventOnDefenseChange;

        public event UnityAction EventOnExperienceChange;
        public event UnityAction EventOnStrengthExperienceChange;
        public event UnityAction EventOnIntelligenceExperienceChange;
        public event UnityAction EventOnMagicResistExperienceChange;

        public event UnityAction EventOnStatsUpdated;

        #endregion

        public override void InitStats(CharacterStatsInfo characterInfo)
        {
            if (characterInfo is not PlayerStatsInfo) return;

            var playerInfo = characterInfo as PlayerStatsInfo;

            base.InitStats(playerInfo);

            MaxLevel = playerInfo.MaxLevel;
            MaxStatLevel = playerInfo.MaxStatLevel;

            levelGrowthPoints = playerInfo.LevelGrowthPoints;
            strengthGrowthPoints = playerInfo.StrengthGrowthPoints;
            intelligenceGrowthPoints = playerInfo.IntelligenceGrowthPoints;
            magicResistGrowthPoints = playerInfo.MagicResistGrowthPoints;
            hitPointsGrowthPoints = playerInfo.HitPointsGrowthPoints;
            magicPointsGrowthPoints = playerInfo.MagicPointsGrowthPoints;

            experienceGrowthMultiplier = playerInfo.ExperienceGrowthMultiplier;
            strengthGrowthMultiplier = playerInfo.StrengthGrowthMultiplier;
            intelligenceGrowthMultiplier = playerInfo.IntelligenceGrowthMultiplier;
            magicResistGrowthMultiplier = playerInfo.MagicResistGrowthMultiplier;
            hitPointsGrowthMultiplier = playerInfo.HitPointsGrowthMultiplier;
            magicPointsGrowthMultiplier = playerInfo.MagicPointsGrowthMultiplier;

            BaseStrengthExperience = playerInfo.BaseStrengthExperience;
            BaseIntelligenceExperience = playerInfo.BaseIntelligenceExperience;
            BaseMagicResistExperience = playerInfo.BaseMagicResistExperience;

            attackMultiplier = playerInfo.AttackMultiplier;
            defenseMultiplier = playerInfo.DefenseMultiplier;

            HitPointsRecoveryRate = playerInfo.HitPointsRecoveryRate;
            MagicPointsRecoveryRate = playerInfo.MagicPointsRecoveryRate;

            SetAttack();
            SetDefense();

            CurrentExperiencePoints = 0;
            CurrentStrengthExperiencePoints = 0;
            CurrentIntelligenceExperiencePoints = 0;
            CurrentMagicResistExperiencePoints = 0;
        }

        public void UpdateStats(PlayerStatsData playerStatsData)
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

            SetAttack();
            SetDefense();

            EventOnStatsUpdated?.Invoke();
        }

        #region PassiveStatsSetMethods

        public void SetWeaponDamage(int weaponDamage)
        {
            equippedWeaponDamage = weaponDamage;
            SetAttack();
        }

        public void SetAttack()
        {
            Attack = (int)((Strength + equippedWeaponDamage) * attackMultiplier);

            EventOnAttackChange?.Invoke();
        }

        public void SetArmorDefense(int armorDefense)
        {
            equippedArmorDefense = armorDefense;
            SetDefense();
        }

        public void SetShieldDefense(int shieldDefense)
        {
            equippedShieldDefense = shieldDefense;
            SetDefense();
        }

        public void SetDefense()
        {
            Defense = equippedArmorDefense + equippedShieldDefense + (int)(MagicResist * defenseMultiplier);

            EventOnDefenseChange?.Invoke();
        }

        #endregion

        #region StatsUpMethods

        private void GetLevelUp()
        {
            if (Level >= MaxLevel) return;

            Level++;

            GetStrengthUp();

            GetIntelligenceUp();

            GetMagicResistUp();

            GetLuckUp(1);

            HitPoints += (int)(hitPointsGrowthPoints * (hitPointsGrowthMultiplier * Level));
            MagicPoints += (int)(magicPointsGrowthPoints * (magicPointsGrowthMultiplier * Level));
            SetBaseParametersPoints();

            EventOnLevelUp?.Invoke();
        }

        private void GetStrengthUp()
        {
            if (Strength >= MaxStatLevel) return;

            Strength++;
            SetAttack();

            EventOnStrengthUp?.Invoke();
        }

        private void GetIntelligenceUp()
        {
            if (Intelligence >= MaxStatLevel) return;

            Intelligence++;
            MagicPoints += (int)(Intelligence * magicPointsGrowthMultiplier);
            CurrentMagicPoints = MagicPoints;

            EventOnIntelligenceUp?.Invoke();
        }

        private void GetMagicResistUp()
        {
            if (MagicResist >= MaxStatLevel) return;

            MagicResist++;
            SetDefense();
            
            EventOnMagicResistUp?.Invoke();
        }

        public void GetLuckUp(int amount)
        {
            if (Luck >= MaxStatLevel) return;

            Luck += amount;

            EventOnLuckUp?.Invoke();
        }

        #endregion

        #region MethodsForStatsExperience

        public void AddExperience(int experience)
        {
            if (Level >= MaxLevel) return;

            if (experience <= 0) return;

            CurrentExperiencePoints += experience;

            while (CurrentExperiencePoints >= GetExperienceForLevelUp())
            {
                CurrentExperiencePoints -= GetExperienceForLevelUp();
                GetLevelUp();
            }

            EventOnExperienceChange?.Invoke();
        }

        public void AddStrengthExperience(int senderLevel)
        {
            if (Strength >= MaxStatLevel) return;

            int strengthExperience = BaseStrengthExperience * senderLevel / Level;

            if (strengthExperience < 1) strengthExperience = 1;

            CurrentStrengthExperiencePoints += strengthExperience;

            while (CurrentStrengthExperiencePoints >= GetExperienceForStrengthUp())
            {
                CurrentStrengthExperiencePoints -= GetExperienceForStrengthUp();
                GetStrengthUp();
            }

            EventOnStrengthExperienceChange?.Invoke();
        }

        public void AddIntelligenceExperience(int senderLevel)
        {
            if (Intelligence >= MaxStatLevel) return;

            int intelligenceExperience = BaseIntelligenceExperience * senderLevel / Level;

            if (intelligenceExperience < 1) intelligenceExperience = 1;

            CurrentIntelligenceExperiencePoints += intelligenceExperience;

            while (CurrentIntelligenceExperiencePoints >= GetExperienceForIntelligenceUp())
            {
                CurrentIntelligenceExperiencePoints -= GetExperienceForIntelligenceUp();
                GetIntelligenceUp();
            }

            EventOnIntelligenceExperienceChange?.Invoke();
        }

        public void AddMagicResistExperience(int senderLevel)
        {
            if (MagicResist >= MaxStatLevel) return;

            int magicResistExperience = BaseMagicResistExperience * senderLevel / Level;

            if (magicResistExperience < 1) magicResistExperience = 1;

            CurrentMagicResistExperiencePoints += magicResistExperience;

            while (CurrentMagicResistExperiencePoints >= GetExperienceForMagicResistUp())
            {
                CurrentMagicResistExperiencePoints -= GetExperienceForMagicResistUp();
                GetMagicResistUp();
            }

            EventOnMagicResistExperienceChange?.Invoke();
        }

        #endregion
    }
}
