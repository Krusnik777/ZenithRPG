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

        private void OnDeath()
        {
            Debug.Log("Defeated");
            Destroy(gameObject);
        }
    }
}
