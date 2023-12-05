using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private Tile[] m_patrolField;
        [SerializeField] private Tile[] m_allField;
        [SerializeField] private float m_restTime = 5.0f;

        private Enemy m_enemy;

        private List<Tile> selectableTiles = new List<Tile>();

        private Stack<Tile> path = new Stack<Tile>();
        private Tile currentTile;

        private Vector3 currentDirection;
        private Vector3 headingDirection;
        private Vector3 velocity;
        private Quaternion targetRotation;

        private bool isMoving = false;
        private bool isTurning = false;
        private bool isChasing = false;

        private float moveSpeed = 1f;
        private float turnSpeed = 5f;

        private Timer m_restTimer;

        private void Start()
        {
            m_enemy = GetComponent<Enemy>();

            InitTimers();

            currentDirection = transform.forward;
        }

        private void Update()
        {
            UpdateTimers();

            ChooseAction();
        }

        private void ChooseAction()
        {
            if (m_enemy.State == EnemyState.Patrol) if (m_restTimer.IsFinished) Patrol();

            if (m_enemy.State == EnemyState.Chase) Chase();

            if (m_enemy.State == EnemyState.Battle) Fight();
        }

        private void Patrol()
        {
            if (!isMoving)
            {
                FindSelectableTiles(m_allField);
                /*
                 int index = Random.Range(0, m_patrolField.Length);
                 SetTarget(m_patrolField[index]);*/

                int index = Random.Range(0, selectableTiles.Count);
                SetTarget(selectableTiles[index]);

            }
            else
            {
                if (!isTurning) Move();
                else Turn();
            }

            if (!isTurning) m_enemy.CheckForPlayerInSightRange();
        }

        private void Chase()
        {
            if (!isChasing)
            {
                StopMoving();
                FindSelectableTiles(m_allField);

                var playerTile = GetTargetTile(m_enemy.PlayerGameObject);
                playerTile.FindNeighbors();

                if (playerTile.NeighborTiles == null)
                {
                    RemoveSelectableTiles();
                    m_enemy.CheckForPlayerInSightRange();
                    return;
                }

                var shortestDistance = playerTile.NeighborTiles[0].Distance;
                int closestTileIndex = 0;

                for(int i = 1; i < playerTile.NeighborTiles.Count; i++)
                {
                    if (playerTile.NeighborTiles[i].Distance < shortestDistance)
                    {
                        shortestDistance = playerTile.NeighborTiles[i].Distance;
                        closestTileIndex = i;
                    }
                }

                SetTarget(playerTile.NeighborTiles[closestTileIndex]);
                isChasing = true;

            }
            else
            {
                if (!isTurning) Move();
                else Turn();
            }  
        }

        private void Fight()
        {
            m_enemy.Attack();
            m_enemy.CheckForPlayerInAttackRange();
        }

        private void FindSelectableTiles(Tile[] tileField)
        {
            ComputeAdjacencyList(tileField);
            GetCurrentTile();

            Queue<Tile> process = new Queue<Tile>();

            process.Enqueue(currentTile);
            currentTile.Visited = true;

            while (process.Count > 0)
            {
                Tile t = process.Dequeue();

                selectableTiles.Add(t);
                t.Selectable = true;

                foreach (var tile in t.NeighborTiles)
                {
                    if (!tile.Visited)
                    {
                        tile.ParentTile = t;
                        tile.Visited = true;
                        tile.Distance = 1 + t.Distance;
                        process.Enqueue(tile);
                    }
                }
            }

            Debug.Log(selectableTiles.Count);
        }

        private void ComputeAdjacencyList(Tile[] tileField)
        {
            foreach (var tile in tileField)
            {
                tile.FindNeighbors();
            }
        }

        private void GetCurrentTile()
        {
            currentTile = GetTargetTile(gameObject);
            currentTile.Current = true;
        }

        private Tile GetTargetTile(GameObject target)
        {
            Tile tile = null;

            Ray ray = new Ray(target.transform.position + new Vector3(0, 0.1f, 0), -Vector3.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1f, 1, QueryTriggerInteraction.Ignore))
            {
                tile = hit.collider.GetComponentInParent<Tile>();
            }

            return tile;
        }

        private void SetTarget(Tile target)
        {
            if (target.Selectable)
            {
                MoveToTile(target);
            }
        }

        private void MoveToTile(Tile tile)
        {
            path.Clear();

            tile.Target = true;
            isMoving = true;

            Tile next = tile;

            while (next != null)
            {
                path.Push(next);

                next = next.ParentTile;
            }
        }

        private void Move()
        {
            if (path.Count > 0)
            {
                Tile t = path.Peek();
                Vector3 targetPosition = t.transform.position;

                m_enemy.Animator.SetBool("Forward", true);

                // Calculate unit position on top of target tile

                if (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
                {
                    CalculateHeadingDirection(targetPosition);
                    SetHorizontalVelocity();

                    // Moving

                    if (currentDirection != headingDirection)
                    {
                        isTurning = true;
                        m_enemy.Animator.SetBool("Forward", false);
                        return;
                    }

                    //transform.forward = headingDirection;
                    transform.position += velocity * Time.deltaTime;
                }
                else
                {
                    // tile center reached
                    transform.position = targetPosition;
                    path.Pop();
                }
            }
            else
            {
                StopMoving();

                if (isChasing)
                {
                    CalculateHeadingDirection(m_enemy.PlayerGameObject.transform.position);

                    if (currentDirection != headingDirection)
                    {
                        isTurning = true;
                        return;
                    }

                    m_enemy.CheckForPlayerInAttackRange();

                    isChasing = false;
                }

                m_restTimer.Start(m_restTime);
            }
        }

        private void Turn()
        {
            targetRotation = Quaternion.LookRotation(headingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            var angle = Quaternion.Angle(transform.rotation, targetRotation);

            if (angle <= 5)
            {
                transform.forward = headingDirection;
                currentDirection = transform.forward;
                isTurning = false;
            }
        }

        private void CalculateHeadingDirection(Vector3 targetPosition)
        {
            headingDirection = targetPosition - transform.position;
            headingDirection.Normalize();
        }

        private void SetHorizontalVelocity()
        {
            velocity = headingDirection * moveSpeed;
        }

        private void StopMoving()
        {
            m_enemy.Animator.SetBool("Forward", false);

            RemoveSelectableTiles();
            isMoving = false;
        }

        private void RemoveSelectableTiles()
        {
            if (currentTile != null)
            {
                currentTile.Current = false;
                currentTile = null;
            }

            foreach (var tile in selectableTiles)
            {
                tile.Reset();
            }

            selectableTiles.Clear();
        }

        #region Timers

        private void InitTimers()
        {
            m_restTimer = new Timer(m_restTime);
        }

        private void UpdateTimers()
        {
            m_restTimer.RemoveTime(Time.deltaTime);
        }

        #endregion

    }
}
