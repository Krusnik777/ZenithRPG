using UnityEngine;

namespace DC_ARPG
{
    public class EnemyCharacter : MonoBehaviour
    {
        [SerializeField] private EnemyStatsInfo m_enemyStatsInfo;
        private EnemyStats enemyStats;
        public EnemyStats EnemyStats => enemyStats;

        private void Awake()
        {
            enemyStats = new EnemyStats();
            enemyStats.InitStats(m_enemyStatsInfo);

            enemyStats.EventOnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            enemyStats.EventOnDeath -= OnDeath;
        }

        private void OnDeath(object sender)
        {
            if (sender is Player)
            {
                (sender as Player).Character.PlayerStats.AddExperience(enemyStats.ExperiencePoints);
                (sender as Player).Character.AddMoney(enemyStats.DroppedGold);

                for (int i = 0; i < enemyStats.DroppedItems.Length; i++)
                {
                    if (Random.value + (sender as Player).Character.DropItemBooster > 1 - enemyStats.DroppedItems[i].ItemDropChance)
                    {
                        DropItem(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < enemyStats.DroppedItems.Length; i++)
                {
                    if (Random.value > 1 - enemyStats.DroppedItems[i].ItemDropChance)
                    {
                        DropItem(i);
                    }
                }
            }

            Destroy(gameObject);
        }

        private void DropItem(int index)
        {
            var item = enemyStats.DroppedItems[index].DroppedItemData.CreateItem();
            var itemContainer = Instantiate(enemyStats.DroppedItems[index].DroppedItemData.ItemInfo.Prefab, transform.position, Quaternion.identity);
            itemContainer.GetComponent<ItemContainer>().AssignItem(item);
        }
    }
}
