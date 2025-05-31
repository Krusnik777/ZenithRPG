using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class AttackStateTransition : IEnemyStateTransition
    {
        [SerializeField] private EnemyState m_attackState;
        [Header("Decisions")]
        [SerializeField] private PlayerInAttackRangeDecision m_playerInAttackRangeDecision;

        private EnemyDecision[] enemyDecisions;
        public EnemyDecision[] Decisions => enemyDecisions;

        public EnemyState TargetState => m_attackState;

        public void Init()
        {
            enemyDecisions = new EnemyDecision[] { m_playerInAttackRangeDecision };
        }
    }
}
