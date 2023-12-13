using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class DroppedItem
    {
        public ItemData DroppedItemData;
        [Range(0, 1)] public float ItemDropChance;
    }

    [CreateAssetMenu(fileName = "EnemyStatsInfo", menuName = "ScriptableObjects/CharacterStatsInfo/EnemyStatsInfo")]
    public class EnemyStatsInfo : CharacterStatsInfo
    {
        [Header("PassiveStats")]
        public int Attack;
        public int Defense;
        [Header("Drops")]
        public int ExperiencePoints;
        public int DroppedGold;
        public DroppedItem[] DroppedItems;
    }
}
