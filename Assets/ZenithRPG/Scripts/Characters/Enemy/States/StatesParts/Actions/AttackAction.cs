using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class AttackAction : EnemyAction
    {
        [SerializeField] private float m_attackMinDelayTime = 1.0f;
        [SerializeField] private float m_attackMaxDelayTime = 3.0f;

        public float AttackDelay { get; private set; }

        public override void OnStart(EnemyAIController controller)
        {
            // Attack At Start
            controller.Enemy.Attack();

            AttackDelay = Random.Range(m_attackMinDelayTime, m_attackMaxDelayTime);
        }

        public override void Act(EnemyAIController controller)
        {
            controller.Enemy.Attack();

            AttackDelay = Random.Range(m_attackMinDelayTime, m_attackMaxDelayTime);
        }
    }
}
