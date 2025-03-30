using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "PlayerStatsInfo", menuName = "ScriptableObjects/CharacterStatsInfo/PlayerStatsInfo")]
    public class PlayerStatsInfo : CharacterStatsInfo
    {
        [Header("Limits")]
        public int MaxLevel = 99;
        public int MaxStatLevel = 150;

        [Header("VariablesForUpgradableStats")]
        public int LevelGrowthPoints;
        public int StrengthGrowthPoints;
        public int IntelligenceGrowthPoints;
        public int MagicResistGrowthPoints;
        public int HitPointsGrowthPoints = 10;
        public int MagicPointsGrowthPoints = 5;

        [Header("StatsGrowthMultipliers")]
        public float ExperienceGrowthMultiplier = 0.5f;
        public float StrengthGrowthMultiplier = 0.1f;
        public float IntelligenceGrowthMultiplier = 0.2f;
        public float MagicResistGrowthMultiplier = 0.1f;
        public float HitPointsGrowthMultiplier = 0.5f;
        public float MagicPointsGrowthMultiplier = 0.5f;

        [Header("BaseExperiencePointsForUpgradableStats")]
        public int BaseStrengthExperience = 5;
        public int BaseIntelligenceExperience = 3;
        public int BaseMagicResistExperience = 3;

        [Header("PassiveStatsMultipliers")]
        public float AttackMultiplier = 1.1f;
        public float DefenseMultiplier = 0.8f;

        [Header("RecoveryRates")]
        public float HitPointsRecoveryRate = 1.0f;
        public float MagicPointsRecoveryRate = 5.0f;
    }
}

