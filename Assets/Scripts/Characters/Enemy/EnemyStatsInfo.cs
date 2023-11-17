using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "EnemyStatsInfo", menuName = "ScriptableObjects/CharacterStatsInfo/EnemyStatsInfo")]
    public class EnemyStatsInfo : CharacterStatsInfo
    {
        [Header("PassiveStats")]
        public int Attack;
        public int Defense;
        [Header("Drops")]
        public int ExperiencePoints;
        public int DroppedGold;
        // DroppedItem
        // Chance For ItemDrop
    }
}
