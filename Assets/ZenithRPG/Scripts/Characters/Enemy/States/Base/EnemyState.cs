using UnityEngine;

namespace DC_ARPG
{
    public abstract class EnemyState : MonoBehaviour
    {
        public abstract void OnStart(EnemyAIController controller);
        public abstract void DoActions(EnemyAIController controller);
        public abstract void CheckTransitions(EnemyAIController controller);
    }
}
