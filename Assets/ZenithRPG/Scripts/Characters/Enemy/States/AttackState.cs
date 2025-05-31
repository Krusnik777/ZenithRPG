using UnityEngine;

namespace DC_ARPG
{
    public class AttackState : EnemyState
    {
        [SerializeField] private ChaseState m_chaseState;
        [SerializeField] private AttackAction m_attackAction;
        [SerializeField] private WaitAction m_waitAction;
        [SerializeField] private AttackStateTransition m_attackStateTransition;

        public override void OnStart(EnemyAIController controller)
        {
            m_attackStateTransition.Init();
            for (int i = 0; i < m_attackStateTransition.Decisions.Length; i++) m_attackStateTransition.Decisions[i].OnStart(controller);

            m_attackAction.OnStart(controller);

            m_waitAction.SetTime(m_attackAction.AttackDelay);
            m_waitAction.OnStart(controller);
        }

        public override void DoActions(EnemyAIController controller)
        {
            if (m_waitAction.IsOver)
            {
                m_attackAction.Act(controller);

                m_waitAction.SetTime(m_attackAction.AttackDelay);
                m_waitAction.OnStart(controller);
            }
            else
            {
                m_waitAction.Act(controller);
            }
        }

        public override void CheckTransitions(EnemyAIController controller)
        {
            if (controller.Enemy.IsAttacking) return;

            for (int i = 0; i < m_attackStateTransition.Decisions.Length; i++)
            {
                if (!m_attackStateTransition.Decisions[i].Decide(controller))
                {
                    controller.StartState(m_chaseState, m_attackStateTransition.Decisions[i]);
                    return;
                }
            }
        }
    }
}
