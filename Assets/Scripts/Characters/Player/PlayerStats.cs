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
        private float defenseMultiplier;

        #endregion

        #region Events

        public UnityEvent OnLevelUp;
        public UnityEvent OnStrengthUp;
        public UnityEvent OnIntelligenceUp;
        public UnityEvent OnMagicResistUp;

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
        }

        public void SetAttack()
        {
            Attack = (int)((Strength + equippedWeaponDamage) * attackMultiplier);
        }

        public void SetArmorDefense(int armorDefense)
        {
            equippedArmorDefense = armorDefense;
        }

        public void SetDefense()
        {
            Defense = equippedArmorDefense + (int)(MagicResist * defenseMultiplier);
        }

        #endregion

        #region StatsUpMethods

        private void GetLevelUp()
        {
            if (Level >= MaxLevel) return;

            Level++;

            if (Strength < MaxStatLevel)
            {
                Strength++;
                SetAttack();
            }

            if (Intelligence < MaxStatLevel) Intelligence++;

            if (MagicResist < MaxStatLevel)
            {
                MagicResist++;
                SetDefense();
            }
            if (Luck < MaxStatLevel) Luck++;

            HitPoints += (int)(hitPointsGrowthPoints * (hitPointsGrowthMultiplier * Level));
            MagicPoints += (int)(magicPointsGrowthPoints * (magicPointsGrowthMultiplier * Level));
            SetBaseParametersPoints();

            OnLevelUp?.Invoke();
        }

        private void GetStrengthUp()
        {
            if (Strength >= MaxStatLevel) return;

            Strength++;
            SetAttack();
            OnStrengthUp?.Invoke();
        }

        private void GetIntelligenceUp()
        {
            if (Intelligence >= MaxStatLevel) return;

            Intelligence++;
            OnIntelligenceUp?.Invoke();
        }

        private void GetMagicResistUp()
        {
            if (MagicResist >= MaxStatLevel) return;

            MagicResist++;
            SetDefense();
            MagicPoints += (int)(MagicResist * magicPointsGrowthMultiplier);
            OnMagicResistUp?.Invoke();
        }

        #endregion

        #region MethodsForStatsChecks

        public void AddExperience(int experience)
        {
            if (Level >= MaxLevel) return;

            CurrentExperiencePoints += experience;

            if (CurrentExperiencePoints >= GetExperienceForLevelUp())
            {
                CurrentExperiencePoints -= GetExperienceForLevelUp();
                GetLevelUp();
            }
        }

        public void AddStrengthExperience(int strengthExperience)
        {
            CurrentStrengthExperiencePoints += strengthExperience;

            if (CurrentStrengthExperiencePoints >= GetExperienceForStrengthUp())
            {
                CurrentStrengthExperiencePoints -= GetExperienceForStrengthUp();
                GetStrengthUp();
            }
        }

        public void AddIntelligenceExperience(int intelligenceExperience)
        {
            CurrentIntelligenceExperiencePoints += intelligenceExperience;

            if (CurrentIntelligenceExperiencePoints >= GetExperienceForIntelligenceUp())
            {
                CurrentIntelligenceExperiencePoints -= GetExperienceForIntelligenceUp();
                GetIntelligenceUp();
            }
        }

        public void AddMagicResistExperience(int magicResistExperience)
        {
            CurrentMagicResistExperiencePoints += magicResistExperience;

            if (CurrentMagicResistExperiencePoints >= GetExperienceForMagicResistUp())
            {
                CurrentMagicResistExperiencePoints -= GetExperienceForMagicResistUp();
                GetMagicResistUp();
            }
        }

        #endregion
    }
}
