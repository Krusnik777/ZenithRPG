using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class ChaseState : EnemyState
    {
        [SerializeField] private PatrolRestState m_patrolRestState;
        [SerializeField] private MoveAction m_moveAction;
        [SerializeField] private WaitAction m_waitAction;
        [SerializeField] private ChaseStateControlTransition m_chaseStateControlTransition;
        [SerializeField] private AttackStateTransition m_attackStateTransition;
        private Stack<Tile> path = new Stack<Tile>();
        private bool pathAssigned;

        private Tile targetedTile;
        private void ClearTargetedTile()
        {
            if (targetedTile != null)
            {
                targetedTile.TargetedBy = null;
                targetedTile = null;
            }
        }

        public override void OnStart(EnemyAIController controller)
        {
            pathAssigned = false;

            m_chaseStateControlTransition.Init();
            m_attackStateTransition.Init();

            for (int i = 0; i < m_chaseStateControlTransition.Decisions.Length; i++) m_chaseStateControlTransition.Decisions[i].OnStart(controller);
            for (int i = 0; i < m_attackStateTransition.Decisions.Length; i++) m_attackStateTransition.Decisions[i].OnStart(controller);

            m_waitAction.OnStart(controller);

            controller.OnChaseStartInvoke();
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

            m_waitAction.Act(controller);
        }

        public override void CheckTransitions(EnemyAIController controller)
        {
            if (m_moveAction.MeetObstacle || m_moveAction.ReachedTarget)
            {
                for (int i = 0; i < m_attackStateTransition.Decisions.Length; i++)
                {
                    if (m_attackStateTransition.Decisions[i].Decide(controller))
                    {
                        controller.OnChaseEndInvoke();
                        controller.StartState(m_attackStateTransition.TargetState, m_attackStateTransition.Decisions[i]);
                        return;
                    }
                }
            }

            if (m_moveAction.MeetObstacle || m_moveAction.ReachedTarget) pathAssigned = false;

            for (int i = 0; i < m_chaseStateControlTransition.Decisions.Length; i++)
            {
                if (m_chaseStateControlTransition.Decisions[i].Decide(controller))
                {
                    m_waitAction.OnStart(controller);
                    return;
                }
            }

            if (m_waitAction.IsOver)
            {
                controller.OnChaseEndInvoke();
                controller.StartState(m_patrolRestState);
            }
        }

        private void TryAssignPath(EnemyAIController controller)
        {
            FindPathToPlayer(controller);

            if (!pathAssigned) return;

            m_moveAction.SetPath(path);

            m_moveAction.OnStart(controller);
        }

        private void FindPathToPlayer(EnemyAIController controller)
        {
            if (LevelState.Instance.Player == null || LevelState.Instance.Player.IsFallingOrFallen || EnemyAIController.BusyFindingPath)
            {
                //StopChasingAndStartPatrol();

                return;
            }

            // Get available tiles Near Player from LevelState
            var potentialTargets = LevelState.Instance.GetAvailableNeighborTilesToPlayer();

            if (potentialTargets.Count == 0)
            {
                //StopChasingAndStartPatrol();

                return;
            }

            // Choose shortest path or available
            EnemyAIController.BusyFindingPath = true;

            Stack<Tile> shortestPath = controller.PathFinder.GetShortestPath(potentialTargets, out Tile target);

            EnemyAIController.BusyFindingPath = false;

            if (shortestPath == null || shortestPath.Count == 0)
            {
                // Do nothing and stand at place if no paths
                //if (m_state == EnemyState.Chase) StopChasingAndStartPatrol();
            }
            else
            {
                path.Clear();
                path = shortestPath;

                ClearTargetedTile(); // just to be safe
                target.TargetedBy = controller.Enemy;
                targetedTile = target;

                pathAssigned = true;
            }
        }
    }
}
