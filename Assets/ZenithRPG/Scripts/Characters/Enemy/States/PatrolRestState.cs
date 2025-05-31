using UnityEngine;

namespace DC_ARPG
{
    public class PatrolRestState : EnemyState
    {
        [SerializeField] private PatrolState m_patrolState;
        [SerializeField] private WaitAction m_waitAction;
        [SerializeField] private ChaseStateTransition m_chaseStateTransition;

        public override void OnStart(EnemyAIController controller)
        {
            m_waitAction.OnStart(controller);

            m_chaseStateTransition.Init();

            for (int i = 0; i < m_chaseStateTransition.Decisions.Length; i++) m_chaseStateTransition.Decisions[i].OnStart(controller);
        }

        public override void DoActions(EnemyAIController controller)
        {
            m_waitAction.Act(controller);
        }

        public override void CheckTransitions(EnemyAIController controller)
        {
            for (int i = 0; i < m_chaseStateTransition.Decisions.Length; i++)
            {
                if (m_chaseStateTransition.Decisions[i].Decide(controller))
                {
                    controller.StartState(m_chaseStateTransition.TargetState, m_chaseStateTransition.Decisions[i]);
                    return;
                }
            }

            if (m_waitAction.IsOver) controller.StartState(m_patrolState);
        }
    }
}
