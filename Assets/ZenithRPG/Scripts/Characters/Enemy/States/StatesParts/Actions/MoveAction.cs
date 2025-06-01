using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class MoveAction : EnemyAction
    {
        [SerializeField] private bool m_chasingMovement;
        [SerializeField] private float m_speed = 1.25f;
        [SerializeField] private float m_turnSpeed = 5f;

        public bool MeetObstacle { get; private set; }
        public bool InCenterOfTile { get; private set; }
        public bool ReachedTarget { get; private set; }

        private Stack<Tile> path;

        private bool isTurning = false;

        private Vector3 currentDirection;
        private Vector3 headingDirection;
        private Vector3 velocity;
        private Quaternion targetRotation;

        public void SetPath(Stack<Tile> path)
        {
            this.path = path;
        }

        public override void OnStart(EnemyAIController controller)
        {
            currentDirection = controller.transform.forward;

            isTurning = false;

            ResetFlags();
        }

        public override void Act(EnemyAIController controller)
        {
            if (!isTurning) Move(controller);
            else Turn(controller);
        }

        private void CalculateHeadingDirection(Vector3 targetPosition, EnemyAIController controller)
                    => headingDirection = (targetPosition - controller.transform.position).normalized;
        private void SetHorizontalVelocity() => velocity = headingDirection * m_speed;

        private void Move(EnemyAIController controller)
        {
            ResetFlags();

            if (path.Count > 0)
            {
                Tile t = path.Peek();

                if (t.OccupiedBy == null)
                {
                    t.SetTileOccupied(controller.Enemy);
                }

                if (t.OccupiedBy != (IMovable)controller.Enemy || t.Type == TileType.Closable && t.CheckClosed())
                {
                    StopMoving(controller);
                    MeetObstacle = true;

                    return;
                }

                Vector3 targetPosition = t.transform.position;

                controller.Enemy.StartMovement();

                // Calculate unit position on top of target tile

                if (Vector3.Distance(controller.transform.position, targetPosition) >= 0.05f)
                {
                    CalculateHeadingDirection(targetPosition, controller);
                    SetHorizontalVelocity();

                    if (currentDirection != headingDirection)
                    {
                        isTurning = true;
                        controller.Enemy.StopMovement();
                        return;
                    }

                    // Moving

                    //transform.forward = headingDirection;
                    controller.transform.position += velocity * Time.deltaTime;
                }
                else
                {
                    // tile center reached

                    controller.transform.position = targetPosition;

                    if (t != controller.Enemy.CurrentTile) controller.Enemy.UpdateNewPosition(t);

                    path.Pop();

                    InCenterOfTile = true;
                }
            }
            else
            {
                StopMoving(controller);

                if (m_chasingMovement)
                {
                    if (LevelState.Instance.Player != null)
                    {
                        if (!LevelState.Instance.Player.IsFallingOrFallen)
                        {
                            CalculateHeadingDirection(LevelState.Instance.Player.transform.position, controller);

                            if (currentDirection != headingDirection)
                            {
                                isTurning = true;
                                return;
                            }
                        }
                    }
                }

                ReachedTarget = true;
            }
        }

        private void Turn(EnemyAIController controller)
        {
            targetRotation = Quaternion.LookRotation(headingDirection);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, m_turnSpeed * Time.deltaTime);

            var angle = Quaternion.Angle(controller.transform.rotation, targetRotation);

            if (angle <= 5)
            {
                controller.transform.forward = headingDirection;
                currentDirection = controller.transform.forward;
                isTurning = false;
            }
        }

        private void StopMoving(EnemyAIController controller)
        {
            controller.Enemy.StopMovement();

            if (path.Count > 0)
            {
                Tile t = path.Peek();

                if (t.OccupiedBy == (IMovable)controller.Enemy && t != controller.Enemy.CurrentTile)
                {
                    t.SetTileOccupied(null);
                }
            }
        }

        private void ResetFlags()
        {
            MeetObstacle = false;
            InCenterOfTile = false;
            ReachedTarget = false;
        }
    }
}
