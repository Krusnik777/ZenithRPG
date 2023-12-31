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

            enemyStats.EventOnHitPointsChange += OnHitPointsChange;
            enemyStats.EventOnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            enemyStats.EventOnHitPointsChange -= OnHitPointsChange;
            enemyStats.EventOnDeath -= OnDeath;
        }

        private void OnHitPointsChange(int change)
        {
            if (change < 0 && enemyStats.CurrentHitPoints != 0)
            {
                Enemy enemy = GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.CharacterSFX.PlayGetHitSound();

                    // Show Damage Effect
                }
            }
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

            Enemy enemy = GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.CharacterSFX.PlayDeathSound();

                // Show Death Animation
            }

            // WAIT FOR SOUND and ANIMATION END => Coroutine?

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
