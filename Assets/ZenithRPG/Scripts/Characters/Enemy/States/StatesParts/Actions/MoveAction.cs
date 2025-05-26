using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class MoveAction : EnemyAction
    {
        [SerializeField] private float m_speed = 1.25f;
        [SerializeField] private float m_turnSpeed = 5f;

        private Stack<Tile> path;

        private bool isMoving = false;
        private bool isTurning = false;

        private Vector3 currentDirection;
        private Vector3 headingDirection;
        private Vector3 velocity;
        private Quaternion targetRotation;

        public MoveAction(Stack<Tile> path)
        {
            this.path = path;
        }

        public override void OnStart(EnemyAIController controller)
        {
            currentDirection = controller.transform.forward;

            isMoving = true;
        }

        public override void Act(EnemyAIController controller)
        {
            if (!isMoving) return;

            if (!isTurning) Move(controller);
            else Turn(controller);
        }

        private void CalculateHeadingDirection(Vector3 targetPosition, EnemyAIController controller)
                    => headingDirection = (targetPosition - controller.transform.position).normalized;
        private void SetHorizontalVelocity() => velocity = headingDirection * m_speed;

        private void Move(EnemyAIController controller)
        {
            if (path.Count > 0)
            {
                Tile t = path.Peek();

                if (t.OccupiedBy == null)
                {
                    t.SetTileOccupied(controller.Enemy);
                }

                // TO TRANSITION ?

                if (t.OccupiedBy != (IMovable)controller.Enemy || t.Type == TileType.Closable && t.CheckClosed())
                {
                    StopMoving(controller);
                    //if (m_state == EnemyStateEnum.Chase) StopChasing();
                    return;
                }

                // TO TRANSITION ?

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

                    // TO TRANSITION ?

                    /*if (m_state == EnemyStateEnum.Patrol)
                    {
                        SeeIfPlayerNearBy();
                    }*/

                    // TO TRANSITION ?
                }
            }
            else
            {
                StopMoving(controller);
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

            isMoving = false;

            if (path.Count > 0)
            {
                Tile t = path.Peek();

                if (t.OccupiedBy == (IMovable)controller.Enemy && t != controller.Enemy.CurrentTile)
                {
                    t.SetTileOccupied(null);
                }
            }
        }
    }
}
