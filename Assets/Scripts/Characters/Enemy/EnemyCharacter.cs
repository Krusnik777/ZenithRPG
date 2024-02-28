using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class EnemyCharacter : MonoBehaviour
    {
        [SerializeField] private EnemyStatsInfo m_enemyStatsInfo;
        [Space]
        public UnityEvent OnEnemyDeath;

        private EnemyStats enemyStats;
        public EnemyStats EnemyStats => enemyStats;

        private Enemy enemy;
        public Enemy Enemy => enemy;

        private Coroutine deathRoutine;

        private void Awake()
        {
            enemy = GetComponent<Enemy>();

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
                if (enemy != null)
                {
                    enemy.CharacterSFX.PlayGetHitSFX(enemy.transform.position);
                }
            }
        }

        private void OnDeath(object sender)
        {
            if (deathRoutine != null) return;

            if (sender is Player)
            {
                (sender as Player).Character.PlayerStats.AddExperience(enemyStats.ExperiencePoints);
                (sender as Player).Character.AddMoney(enemyStats.DroppedGold);
            }

            deathRoutine = StartCoroutine(PlayDeath(sender));
        }

        private void DropItem(int index)
        {
            var item = enemyStats.DroppedItems[index].DroppedItemData.CreateItem();
            var itemContainer = Instantiate(enemyStats.DroppedItems[index].DroppedItemData.ItemInfo.Prefab, transform.position, Quaternion.identity);
            itemContainer.GetComponent<ItemContainer>().SetupCreatedContainer();
            itemContainer.GetComponent<ItemContainer>().AssignItem(item);
        }

        private IEnumerator PlayDeath(object sender)
        {
            enemy.EnemyAI.StopActivity();

            if (enemy != null)
            {
                enemy.CharacterSFX.PlayDeathSFX(enemy.transform.position);
            }

            enemy.Animator.Play("Death");

            yield return new WaitUntil(() => enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

            yield return new WaitForSeconds(1.5f);

            if (sender is Player)
            {
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

            OnEnemyDeath?.Invoke();

            Destroy(gameObject);
        }
    }
}
