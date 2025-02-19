using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class DroppedItem
    {
        [SerializeField] private ItemInfo m_itemInfo;
        [SerializeField] private int m_amount;
        [Range(0, 1)] public float ItemDropChance;

        public IItem CreateItem()
        {
            ItemData itemData = new ItemData(m_itemInfo, m_amount);

            IItem item = itemData.CreateItem();

            return item;
        }
    }

    [CreateAssetMenu(fileName = "EnemyStatsInfo", menuName = "ScriptableObjects/CharacterStatsInfo/EnemyStatsInfo")]
    public class EnemyStatsInfo : CharacterStatsInfo
    {
        [Header("PassiveStats")]
        public int AttackIncrease;
        public int Defense;
        [Header("Drops")]
        public int ExperiencePoints;
        public int DroppedGold;
        public DroppedItem[] DroppedItems;
    }
}
