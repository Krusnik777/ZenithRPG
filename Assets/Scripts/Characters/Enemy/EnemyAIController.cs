using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private Tile[] m_patrolField;
        //[SerializeField] private Tile[] m_allField;
        [SerializeField] private float m_chaseRange = 6.0f;
        [SerializeField] private float m_closeRange = 1.5f;

        [Header("ForTimers")]
        [SerializeField] private float m_patrolRestTime = 5.0f;
        [SerializeField] private float m_chaseDurationTime = 20.0f;
        [SerializeField] private float m_attackMinDelayTime = 1.0f;
        [SerializeField] private float m_attackMaxDelayTime = 3.0f;

        public event UnityAction<EnemyAIController> EventOnChaseStarted;
        public event UnityAction<EnemyAIController> EventOnChaseStopped;

        private Enemy m_enemy;
        private bool SeePlayer => m_enemy.CheckForPlayerInSightRange();

        private Stack<Tile> path = new Stack<Tile>();
        private Tile currentTile;
        private Tile targetTile;
        public Tile TargetTile => targetTile;
        private Tile playerTile => GetTargetTile(m_enemy.DetectedPlayerGameObject);

        private Vector3 currentDirection;
        private Vector3 headingDirection;
        private Vector3 velocity;
        private Quaternion targetRotation;

        private bool isMoving = false;
        private bool isTurning = false;
        private bool isChasing = false;
        private bool isStopped = false;

        private float moveSpeed = 1.25f;
        private float turnSpeed = 5f;

        private Timer m_patrolRestTimer;
        private Timer m_chaseTimer;
        private Timer m_attackTimer;

        private void GetCurrentTile() => currentTile = GetTargetTile(gameObject);
        private void CalculateHeadingDirection(Vector3 targetPosition) => headingDirection = (targetPosition - transform.position).normalized;
        private void SetHorizontalVelocity() => velocity = headingDirection * moveSpeed;

        private bool CheckPlayerInChaseRange() => Vector3.Distance(transform.position, m_enemy.DetectedPlayerGameObject.transform.position) < m_chaseRange ? true : false;
        private bool CheckCloseRange() => Vector3.Distance(transform.position, m_enemy.DetectedPlayerGameObject.transform.position) < m_closeRange ? true : false;

        public void UpdateActivity()
        {
            StopMoving();
            if (m_enemy.State == EnemyState.Chase) StopChasing();
        }

        public void StopActivity()
        {
            isStopped = true;

            StopMoving();
            isChasing = false;
        }

        public void ResumeActivity()
        {
            isStopped = false;
        }

        private void Start()
        {
            m_enemy = GetComponent<Enemy>();

            InitTimers();

            currentDirection = transform.forward;
        }

        private void Update()
        {
            if (isStopped) return;

            UpdateTimers();

            ChooseAction();
        }

        private void ChooseAction()
        {
            if (m_enemy.State == EnemyState.Patrol) Patrol();

            if (m_enemy.State == EnemyState.Chase) Chase();

            if (m_enemy.State == EnemyState.Battle) Fight();
        }

        private void Patrol()
        {
            if (m_patrolRestTimer.IsFinished)
            {
                if (!isMoving)
                {
                    int index = Random.Range(0, m_patrolField.Length);
                    CalculatePath(m_patrolField[index]);
                }
                else
                {
                    if (!isTurning) Move();
                    else Turn();
                }
            }

            if (!isTurning)
            {
                if (SeePlayer || CheckCloseRange() == true)
                {
                    m_enemy.StartChase();
                    StopMoving();

                    Debug.Log("Started Chase");
                    EventOnChaseStarted?.Invoke(this);

                    m_chaseTimer.Start(m_chaseDurationTime);
                }
            }
        }

        private void Chase()
        {
            if (!m_chaseTimer.IsFinished)
            {
                if (!isChasing)
                {
                    CalculatePath(playerTile);

                    isChasing = true;
                }
                else
                {
                    if (!isTurning) Move();
                    else Turn();
                }
            }
            else
            {
                if (CheckPlayerInChaseRange() == true)
                {
                    m_chaseTimer.Start(m_chaseDurationTime);
                }
                else
                {
                    StopMoving();
                    isChasing = false;

                    Debug.Log("Started Patrol");
                    EventOnChaseStopped?.Invoke(this);

                    m_enemy.StartPatrol();
                } 
            }
        }

        private void Fight()
        {
            if (m_attackTimer.IsFinished)
            {
                m_enemy.Attack();

                m_attackTimer.Start(Random.Range(m_attackMinDelayTime, m_attackMaxDelayTime));
            }

            if (!m_enemy.IsAttacking)
            {
                if (m_enemy.CheckForPlayerInAttackRange() == false)
                {
                    m_enemy.StartChase();

                    EventOnChaseStarted?.Invoke(this);

                    m_chaseTimer.Start(m_chaseDurationTime);
                }
            }
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

        private void MoveToTile(Tile tile)
        {
            path.Clear();

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

                m_enemy.Animator.SetFloat("MovementZ", 1);

                // Calculate unit position on top of target tile

                if (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
                {
                    CalculateHeadingDirection(targetPosition);
                    SetHorizontalVelocity();

                    if (currentDirection != headingDirection)
                    {
                        isTurning = true;
                        m_enemy.Animator.SetTrigger("IdleTrigger");
                        m_enemy.Animator.SetFloat("MovementZ", 0);
                        return;
                    }

                    // Moving

                    //transform.forward = headingDirection;
                    transform.position += velocity * Time.deltaTime;
                }
                else
                {
                    // tile center reached

                    transform.position = targetPosition;

                    if (isChasing)
                    {
                        if (m_enemy.CheckForwardGridForObstacle())
                        {
                            StopMoving();
                            StopChasing();
                            return;
                        }

                        if (m_enemy.CheckForwardGridForAlly())
                        {
                            Debug.Log("here");

                            StopMoving();
                            StopChasing();
                            return;
                        }
                    }

                    path.Pop();
                }

                if (m_enemy.State == EnemyState.Chase)
                {
                    if (LevelState.Instance.Player.InMovement)
                    {
                        StopMoving();
                        StopChasing();
                        return;
                    }
                }
            }
            else
            {
                StopMoving();

                if (isChasing)
                {
                    if (playerTile != null)
                    {
                        CalculateHeadingDirection(m_enemy.DetectedPlayerGameObject.transform.position);

                        if (currentDirection != headingDirection)
                        {
                            isTurning = true;
                            return;
                        }
                    }

                    StopChasing();
                }

                if (m_enemy.State == EnemyState.Patrol) m_patrolRestTimer.Start(m_patrolRestTime);
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

        private void StopMoving()
        {
            m_enemy.Animator.SetTrigger("IdleTrigger");
            m_enemy.Animator.SetFloat("MovementZ", 0);

            ClearCurrentTile();
            isMoving = false;
        }

        private void StopChasing()
        {
            if (m_enemy.CheckForPlayerInAttackRange() == true)
            {
                m_enemy.StartAttack();

                EventOnChaseStopped?.Invoke(this);
            }

            isChasing = false;
        }

        private void ClearCurrentTile()
        {
            if (currentTile != null)
            {
                currentTile.Current = false;
                currentTile = null;
            }
        }

        private void CalculatePath(Tile target)
        {
            if (target == null) return;

            LevelState.Instance.ComputeAdjacencyList(target);

            GetCurrentTile();

            List<Tile> openList = new List<Tile>();
            List<Tile> closedList = new List<Tile>();

            openList.Add(currentTile);
            currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
            currentTile.f = currentTile.h;

            while(openList.Count > 0)
            {
                Tile t = FindLowestF(openList);

                closedList.Add(t);

                if (t == target)
                {
                    if (m_enemy.State == EnemyState.Chase)
                    {
                        targetTile = FindEndTile(t);
                        MoveToTile(targetTile);
                    }
                    else MoveToTile(t);

                    return;
                }

                foreach (var tile in t.NeighborTiles)
                {
                    if (closedList.Contains(tile))
                    {
                        // Do nothing, already processed
                    }
                    else if (openList.Contains(tile))
                    {
                        float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                        if (tempG < tile.g)
                        {
                            tile.ParentTile = t;

                            tile.g = tempG;
                            tile.f = tile.g + tile.h;
                        }
                    }
                    else
                    {
                        tile.ParentTile = t;
                        tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                        tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                        tile.f = tile.g + tile.h;

                        openList.Add(tile);
                    }
                }
            }

            // what to do if no path to target
            Debug.Log("Path not found");
        }

        private Tile FindLowestF(List<Tile> list)
        {
            Tile lowest = list[0];

            foreach (var t in list)
            {
                if (t.f < lowest.f)
                {
                    lowest = t;
                }
            }

            list.Remove(lowest);

            return lowest;
        }

        private Tile FindEndTile(Tile t)
        {
            Stack<Tile> tempPath = new Stack<Tile>();

            Tile next = t.ParentTile;

            while (next != null)
            {
                tempPath.Push(next);
                next = next.ParentTile;
            }

            if (t.ParentTile != null) t.ParentTile.Target = true;

            return t.ParentTile;

            /*
            if (tempPath.Count <= chaseRange)
            {
                return t.ParentTile;
            }

            Tile endTile = null;
            for (int i = 0; i <= chaseRange; i++)
            {
                endTile = tempPath.Pop();
            }

            return endTile;*/
        }

        #region Timers

        private void InitTimers()
        {
            m_patrolRestTimer = new Timer(m_patrolRestTime);
            m_chaseTimer = new Timer(m_chaseDurationTime);
            m_attackTimer = new Timer(m_attackMaxDelayTime);
        }

        private void UpdateTimers()
        {
            m_patrolRestTimer.RemoveTime(Time.deltaTime);
            m_chaseTimer.RemoveTime(Time.deltaTime);
            m_attackTimer.RemoveTime(Time.deltaTime);
        }

        #endregion

        #region OBSOLETE

        private List<Tile> selectableTiles = new List<Tile>();

        private void ComputeAdjacencyList(Tile[] tileField, Tile target)
        {
            foreach (var tile in tileField)
            {
                tile.FindNeighbors(target);
            }
        }

        private void FindSelectableTiles(Tile[] tileField)
        {
            ComputeAdjacencyList(tileField, null);
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

        private void SetTarget(Tile target)
        {
            if (target.Selectable)
            {
                MoveToTile(target);
            }
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

        #endregion

    }
}
