using UnityEngine;

namespace DC_ARPG
{
    public abstract class EnemyState
    {
        [SerializeField] protected EnemyState m_nextState;

        public abstract void Act(EnemyAIController controller);

        public abstract void StartNextState(EnemyAIController controller);
    }
}
