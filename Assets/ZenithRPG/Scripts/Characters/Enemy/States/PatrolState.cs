using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class PatrolState : EnemyState
    {
        [SerializeField] private PatrolRestState m_restState;
        [SerializeField] private Tile[] m_patrolField;
        [SerializeField] private MoveAction m_moveAction;
        [SerializeField] private ChaseStateTransition m_chaseStateTransition;

        private Stack<Tile> path = new Stack<Tile>();
        private bool pathAssigned;

        public override void OnStart(EnemyAIController controller)
        {
            pathAssigned = false;

            m_chaseStateTransition.Init();

            for (int i = 0; i < m_chaseStateTransition.Decisions.Length; i++) m_chaseStateTransition.Decisions[i].OnStart(controller);

            //TryAssignPath(controller);
        }

        public override void DoActions(EnemyAIController controller)
        {
            if (!pathAssigned)
            {
                TryAssignPath(controller);
            }
            else
            {
                m_moveAction.Act(controller);
            }
        }

        public override void CheckTransitions(EnemyAIController controller)
        {
            if (m_moveAction.InCenterOfTile)
            {
                for (int i = 0; i < m_chaseStateTransition.Decisions.Length; i++)
                {
                    if (m_chaseStateTransition.Decisions[i].Decide(controller))
                    {
                        controller.StartState(m_chaseStateTransition.TargetState, m_chaseStateTransition.Decisions[i]);
                        return;
                    }
                }
            }

            if (m_moveAction.ReachedTarget)
            {
                controller.StartState(m_restState);

                return;
            }

            if (m_moveAction.MeetObstacle) OnStart(controller);
        }

        private void TryAssignPath(EnemyAIController controller)
        {
            int index = Random.Range(0, m_patrolField.Length);
            FindPath(controller.PathFinder, m_patrolField[index]);

            if (!pathAssigned) return;

            m_moveAction.SetPath(path);

            m_moveAction.OnStart(controller);
        }

        private void FindPath(PathFinder pathFinder, Tile target, bool targetNeighbourTile = false)
        {
            if (target == null || EnemyAIController.BusyFindingPath) return;

            EnemyAIController.BusyFindingPath = true;

            Stack<Tile> newPath = pathFinder.CalculatePath(target, true, targetNeighbourTile);

            EnemyAIController.BusyFindingPath = false;

            if (newPath == null || newPath.Count == 0)
            {
                //if (m_state == EnemyStateEnum.Chase) StopChasingAndStartPatrol();
            }
            else
            {
                path.Clear();
                path = newPath;

                pathAssigned = true;
            }
        }
    }
}
