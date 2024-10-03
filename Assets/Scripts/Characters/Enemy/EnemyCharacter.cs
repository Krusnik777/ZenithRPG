using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class EnemyCharacter : CharacterBase
    {
        [SerializeField] private EnemyStatsInfo m_enemyStatsInfo;
        [Space]
        public UnityEvent OnEnemyDeath;

        private EnemyStats enemyStats;
        public override CharacterStats Stats => enemyStats;

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
                    OnHit();
                }
            }
        }

        private void OnDeath(object sender)
        {
            if (deathRoutine != null) return;

            if (sender is Player)
            {
                var player = sender as Player;

                (player.Character.Stats as PlayerStats).AddExperience(enemyStats.ExperiencePoints);
                (player.Character as PlayerCharacter).AddMoney(enemyStats.DroppedGold);
            }

            deathRoutine = StartCoroutine(PlayDeath(sender));
        }

        private void DropItem(int index)
        {
            var item = enemyStats.DroppedItems[index].DroppedItemData.CreateItem();
            var itemContainer = Instantiate(enemyStats.DroppedItems[index].DroppedItemData.ItemInfo.Prefab, enemy.CurrentTile.transform.position, Quaternion.identity);
            itemContainer.GetComponent<ItemContainer>().SetupCreatedContainer();
            itemContainer.GetComponent<ItemContainer>().AssignItem(item);
        }

        private IEnumerator PlayDeath(object sender)
        {
            enemy.EnemyAI.StopActivity();

            enemy.Animator.Play("Death");

            yield return new WaitUntil(() => enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

            yield return new WaitForSeconds(1.5f);

            if (sender is Player)
            {
                for (int i = 0; i < enemyStats.DroppedItems.Length; i++)
                {
                    if (Random.value + ((sender as Player).Character as PlayerCharacter).DropItemBooster > 1 - enemyStats.DroppedItems[i].ItemDropChance)
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
