using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private Tile[] m_movementField;
        [SerializeField] private float m_restTime = 5.0f;

        private List<Tile> selectableTiles = new List<Tile>();

        private Stack<Tile> path = new Stack<Tile>();
        private Tile currentTile;

        private Vector3 headingDirection;
        private Vector3 velocity;

        private bool isMoving = false;

        private float moveSpeed = 1f;

        private Animator m_animator;

        private Timer m_restTimer;

        private void Start()
        {
            m_animator = GetComponentInChildren<Animator>();

            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            if (!m_restTimer.IsFinished) return;

            Patrol();
        }

        private void Patrol()
        {
            if (!isMoving)
            {
                FindSelectableTiles();

                int index = Random.Range(0, m_movementField.Length);
                SetTarget(m_movementField[index]);
            }
            else
            {
                Move();
            }
        }

        private void FindSelectableTiles()
        {
            ComputeAdjacencyList();
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
        }

        private void ComputeAdjacencyList()
        {
            foreach (var tile in m_movementField)
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
            RaycastHit hit;
            Tile tile = null;

            if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
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

                m_animator.SetBool("Forward", true);

                // Calculate unit position on top of target tile

                if (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
                {
                    CalculateHeadingDirection(targetPosition);
                    SetHorizontalVelocity();

                    // Moving

                    transform.forward = headingDirection;
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
                m_animator.SetBool("Forward", false);

                RemoveSelectableTiles();
                isMoving = false;

                m_restTimer.Start(m_restTime);
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
