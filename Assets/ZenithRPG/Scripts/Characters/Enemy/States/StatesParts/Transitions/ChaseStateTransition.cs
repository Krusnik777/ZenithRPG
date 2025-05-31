
using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class ChaseStateTransition : IEnemyStateTransition
    {
        [SerializeField] private EnemyState m_chaseState;
        [Header("Decisions")]
        [SerializeField] private FeelPlayerDecision m_feelPlayerDecision;
        [SerializeField] private SawPlayerDecision m_sawPlayerDecision;
        [SerializeField] private ReceivedDamageByPlayerDecision m_receivedDamageByPlayerDecision;

        private EnemyDecision[] enemyDecisions;
        public EnemyDecision[] Decisions => enemyDecisions;

        public EnemyState TargetState => m_chaseState;

        public void Init()
        {
            enemyDecisions = new EnemyDecision[] {m_feelPlayerDecision, m_sawPlayerDecision, m_receivedDamageByPlayerDecision};
        }
    }
}
