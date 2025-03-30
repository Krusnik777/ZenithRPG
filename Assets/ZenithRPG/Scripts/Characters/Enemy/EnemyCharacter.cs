using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class EnemyCharacter : CharacterBase
    {
        [SerializeField] private EnemyStatsInfo m_enemyStatsInfo;

        private EnemyStats enemyStats;
        public override CharacterStats Stats => enemyStats;

        private Enemy enemy;
        public Enemy Enemy => enemy;

        private Coroutine deathRoutine;

        public override void DamageOpponent(CharacterAvatar opponent)
        {
            if (opponent.Character.Stats.CurrentHitPoints <= 0) return;

            if (opponent is not Player) return;

            var player = opponent as Player;

            if (player.IsBlocking && player.CheckForwardGridForEnemy() == enemy)
            {
                player.OnBlock();

                return;
            }

            if (player.State == Player.PlayerState.Rest)
            {
                player.Character.Stats.ChangeCurrentHitPoints(enemy, -enemyStats.Attack * 2, DamageType.Physic); // Damage x2
                player.ChangeRestState();

                return;
            }

            opponent.Character.Stats.ChangeCurrentHitPoints(enemy, -enemyStats.Attack, DamageType.Physic);
        }

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
            var item = enemyStats.DroppedItems[index].CreateItem();
            var itemContainer = Instantiate(item.Info.Prefab, enemy.CurrentTile.transform.position, Quaternion.identity);
            itemContainer.GetComponent<ItemContainer>().SetupCreatedContainer();
            itemContainer.GetComponent<ItemContainer>().AssignItem(item);
        }

        private IEnumerator PlayDeath(object sender)
        {
            enemy.EnemyAI.StopActivity();

            enemy.Die();

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

            OnDead();

            enemy.CurrentTile.SetTileOccupied(null);

            Destroy(gameObject);
        }
    }
}
