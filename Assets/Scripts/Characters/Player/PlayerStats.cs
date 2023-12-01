using UnityEngine.Events;

namespace DC_ARPG
{
    public class PlayerStats : CharacterStats<PlayerStatsInfo>
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

        private int equippedWeaponDamage = 0;
        private float attackMultiplier;

        private int equippedArmorDefense = 0;
        private int equippedShieldDefense = 0;
        private float defenseMultiplier;

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

        #endregion

        public override void InitStats(PlayerStatsInfo playerInfo)
        {
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

            attackMultiplier = playerInfo.AttackMultiplier;
            defenseMultiplier = playerInfo.DefenseMultiplier;

            SetAttack();
            SetDefense();

            CurrentExperiencePoints = 0;
            CurrentStrengthExperiencePoints = 0;
            CurrentIntelligenceExperiencePoints = 0;
            CurrentMagicResistExperiencePoints = 0;
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

            CurrentExperiencePoints += experience;

            while (CurrentExperiencePoints >= GetExperienceForLevelUp())
            {
                CurrentExperiencePoints -= GetExperienceForLevelUp();
                GetLevelUp();
            }

            EventOnExperienceChange?.Invoke();
        }

        public void AddStrengthExperience(int strengthExperience)
        {
            if (Strength >= MaxStatLevel) return;

            CurrentStrengthExperiencePoints += strengthExperience;

            while (CurrentStrengthExperiencePoints >= GetExperienceForStrengthUp())
            {
                CurrentStrengthExperiencePoints -= GetExperienceForStrengthUp();
                GetStrengthUp();
            }

            EventOnStrengthExperienceChange?.Invoke();
        }

        public void AddIntelligenceExperience(int intelligenceExperience)
        {
            if (Intelligence >= MaxStatLevel) return;

            CurrentIntelligenceExperiencePoints += intelligenceExperience;

            while (CurrentIntelligenceExperiencePoints >= GetExperienceForIntelligenceUp())
            {
                CurrentIntelligenceExperiencePoints -= GetExperienceForIntelligenceUp();
                GetIntelligenceUp();
            }

            EventOnIntelligenceExperienceChange?.Invoke();
        }

        public void AddMagicResistExperience(int magicResistExperience)
        {
            if (MagicResist >= MaxStatLevel) return;

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
