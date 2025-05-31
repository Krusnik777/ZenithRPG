using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class ChaseStateControlTransition : IEnemyStateTransition
    {
        [SerializeField] private EnemyState m_chaseState;
        [Header("Decisions")]
        [SerializeField] private PlayerInChaseRangeDecision m_playerInChaseRangeDecision;
        [SerializeField] private SawPlayerDecision m_sawPlayerDecision;

        private EnemyDecision[] enemyDecisions;
        public EnemyDecision[] Decisions => enemyDecisions;

        public EnemyState TargetState => m_chaseState;

        public void Init()
        {
            enemyDecisions = new EnemyDecision[] {m_playerInChaseRangeDecision, m_sawPlayerDecision};
        }

    }
}
