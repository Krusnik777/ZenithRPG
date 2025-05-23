using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public enum EnemyStateEnum
    {
        Patrol,
        Chase,
        Battle
    }

    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private Enemy m_enemy;
        [SerializeField] private FieldOfView m_enemyFOV;
        [Header("AIParameters")]
        [SerializeField] private Tile[] m_patrolField;
        [SerializeField] private float m_chaseRange = 6.0f;
        [SerializeField] private float m_closeRange = 1.5f;
        [Header("ForTimers")]
        [SerializeField] private float m_patrolRestTime = 5.0f;
        [SerializeField] private float m_chaseDurationTime = 20.0f;
        [SerializeField] private float m_attackMinDelayTime = 1.0f;
        [SerializeField] private float m_attackMaxDelayTime = 3.0f;
        [Header("MovementParamsForAI")]
        [SerializeField] private float m_moveSpeed = 1.25f;
        [SerializeField] private float m_turnSpeed = 5f;

        private static bool _busyFindingPath;

        public event UnityAction<EnemyAIController> EventOnChaseStarted;
        public event UnityAction<EnemyAIController> EventOnChaseEnded;

        public FieldOfView EnemyFieldOfView => m_enemyFOV;
        public bool SeePlayer => m_enemyFOV.CanSeePlayer;

        private PathFinder pathFinder;

        private Stack<Tile> path = new Stack<Tile>();

        private EnemyStateEnum m_state;
        public EnemyStateEnum State => m_state;

        public bool InChaseState => m_state == EnemyStateEnum.Chase;

        private Vector3 currentDirection;
        private Vector3 headingDirection;
        private Vector3 velocity;
        private Quaternion targetRotation;

        private bool isMoving = false;
        private bool isTurning = false;
        private bool isChasing = false;
        private bool isStopped = false;

        private Timer m_patrolRestTimer;
        private Timer m_chaseTimer;
        private Timer m_attackTimer;

        private float changingChaseRange;

        private Tile targetedTile;
        private void ClearTargetedTile()
        {
            if (targetedTile != null)
            {
                targetedTile.TargetedBy = null;
                targetedTile = null;
            }
        }

        private void CalculateHeadingDirection(Vector3 targetPosition) => headingDirection = (targetPosition - transform.position).normalized;
        private void SetHorizontalVelocity() => velocity = headingDirection * m_moveSpeed;

        private bool CheckPlayerInChaseRange() => Vector3.Distance(transform.position, m_enemyFOV.PlayerGameObject.transform.position) < changingChaseRange ? true : false;
        private bool CheckCloseRange() => Vector3.Distance(transform.position, m_enemyFOV.PlayerGameObject.transform.position) < m_closeRange ? true : false;

        public bool CheckForPlayerInAttackRange()
        {
            Ray attackRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);

            if (Physics.Raycast(attackRay, 1f, m_enemyFOV.TargetMask))
            {
                return true;
            }

            return false;
        }

        public void StartChaseState()
        {
            StartState(EnemyStateEnum.Chase);

            EventOnChaseStarted?.Invoke(this);

            m_chaseTimer.Start(m_chaseDurationTime);

            changingChaseRange = m_chaseRange;

            Debug.Log("Started Chase");
        }

        public void StartPatrolState()
        {
            StartState(EnemyStateEnum.Patrol);
        }

        public void StartBattleState()
        { 
            StartState(EnemyStateEnum.Battle); 
        }

        public void StopActivity()
        {
            isStopped = true;

            StopMoving();
            if (m_state == EnemyStateEnum.Chase) StopChasing();
        }

        public void ResumeActivity()
        {
            isStopped = false;
        }

        private void StartState(EnemyStateEnum state)
        {
            m_state = state;
        }

        private void Start()
        {
            pathFinder = new PathFinder(m_enemy);

            InitTimers();

            currentDirection = transform.forward;
        }

        private void Update()
        {
            if (isStopped) return;

            if (m_enemy.IsPushedBack) return;

            UpdateTimers();

            ChooseAction();
        }

        private void ChooseAction()
        {
            if (m_state == EnemyStateEnum.Patrol) Patrol();

            if (m_state == EnemyStateEnum.Chase) Chase();

            if (m_state == EnemyStateEnum.Battle) Fight();
        }

        private void Patrol()
        {
            if (m_patrolRestTimer.IsFinished)
            {
                if (!isMoving)
                {
                    int index = Random.Range(0, m_patrolField.Length);
                    FindPath(m_patrolField[index]);
                }
                else
                {
                    if (!isTurning) Move();
                    else Turn();
                }
            }
            else
            {
                SeeIfPlayerNearBy();
            }
        }

        private void Chase()
        {
            if (!m_chaseTimer.IsFinished)
            {
                if (!isChasing)
                {
                    FindPathToPlayer();
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
                    changingChaseRange--;

                    m_chaseTimer.Start(m_chaseDurationTime);
                }
                else
                {
                    StopChasingAndStartPatrol();
                } 
            }
        }

        private void Fight()
        {
            if (m_attackTimer.IsFinished)
            {
                m_enemy.Attack();

                float timeForNextAttack = Random.Range(m_attackMinDelayTime, m_attackMaxDelayTime);

                m_attackTimer.Start(timeForNextAttack);
            }

            if (!m_enemy.IsAttacking)
            {
                if (CheckForPlayerInAttackRange() == false)
                    StartChaseState();
            }
        }

        private void Move()
        {
            if (path.Count > 0)
            {
                Tile t = path.Peek();

                if (t.OccupiedBy == null)
                {
                    t.SetTileOccupied(m_enemy);
                }

                if (t.OccupiedBy != (IMovable) m_enemy || t.Type == TileType.Closable && t.CheckClosed())
                {
                    StopMoving();
                    if (m_state == EnemyStateEnum.Chase) StopChasing();
                    return;
                }

                Vector3 targetPosition = t.transform.position;

                m_enemy.StartMovement();

                // Calculate unit position on top of target tile

                if (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
                {
                    CalculateHeadingDirection(targetPosition);
                    SetHorizontalVelocity();

                    if (currentDirection != headingDirection)
                    {
                        isTurning = true;
                        m_enemy.StopMovement();
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

                    if (t != m_enemy.CurrentTile) m_enemy.UpdateNewPosition(t);

                    path.Pop();

                    if (m_state == EnemyStateEnum.Patrol)
                    {
                        SeeIfPlayerNearBy();
                    }
                }
            }
            else
            {
                StopMoving();

                if (isChasing)
                {
                    if (LevelState.Instance.Player != null)
                    {
                        if (!LevelState.Instance.Player.IsFallingOrFallen && CheckCloseRange() == true)
                        {
                            CalculateHeadingDirection(m_enemyFOV.PlayerGameObject.transform.position);

                            if (currentDirection != headingDirection)
                            {
                                isTurning = true;
                                return;
                            }
                        }
                    }

                    StopChasing();
                }

                if (m_state == EnemyStateEnum.Patrol) m_patrolRestTimer.Start(m_patrolRestTime);
            }
        }

        private void Turn()
        {
            targetRotation = Quaternion.LookRotation(headingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_turnSpeed * Time.deltaTime);

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
            m_enemy.StopMovement();

            isMoving = false;

            if (path.Count > 0)
            {
                Tile t = path.Peek();

                if (t.OccupiedBy == (IMovable) m_enemy && t != m_enemy.CurrentTile)
                {
                    t.SetTileOccupied(null);
                }
            }
        }

        private void StopChasing()
        {
            if (CheckForPlayerInAttackRange() == true)
            {
                StartBattleState();

                EventOnChaseEnded?.Invoke(this);
            }

            isChasing = false;

            ClearTargetedTile();
        }

        private void StopChasingAndStartPatrol()
        {
            StopMoving();
            StopChasing();

            Debug.Log("Started Patrol");
            EventOnChaseEnded?.Invoke(this);

            StartPatrolState();
        }

        private void SeeIfPlayerNearBy()
        {
            if (isTurning) return;

            if (SeePlayer || CheckCloseRange() == true)
            {
                if (isMoving) StopMoving();
                StartChaseState();
            }
        }

        private void FindPath(Tile target, bool targetNeighbourTile = false)
        {
            if (target == null || _busyFindingPath) return;

            _busyFindingPath = true;

            Stack<Tile> newPath = pathFinder.CalculatePath(target, targetNeighbourTile);

            _busyFindingPath = false;

            if (newPath == null || newPath.Count == 0)
            {
                if (m_state == EnemyStateEnum.Chase) StopChasingAndStartPatrol();
            }
            else
            {
                path.Clear();
                path = newPath;

                isMoving = true;
                if (m_state == EnemyStateEnum.Chase) isChasing = true;
            }
        }

        private void FindPathToPlayer()
        {
            if (LevelState.Instance.Player == null || LevelState.Instance.Player.IsFallingOrFallen || _busyFindingPath)
            {
                StopChasingAndStartPatrol();

                return;
            }

            // Get available tiles Near Player from LevelState
            var potentialTargets = LevelState.Instance.GetAvailableNeighborTilesToPlayer();

            if (potentialTargets.Count == 0)
            {
                StopChasingAndStartPatrol();

                return;
            }

            // Choose shortest path or available
            _busyFindingPath = true;

            Stack<Tile> shortestPath = pathFinder.GetShortestPath(potentialTargets, out Tile target);

            _busyFindingPath = false;

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
                target.TargetedBy = m_enemy;
                targetedTile = target;

                isMoving = true;
                isChasing = true;
            }

            /*
            if (LevelState.Instance.ChasingEnemies.Count > 1)
            {
                // Get available tiles Near Player from LevelState
                var potentialTargets = LevelState.Instance.GetAvailableNeighborTilesToPlayer();

                if (potentialTargets.Count == 0)
                {
                    StopChasingAndStartPatrol();

                    return;
                }

                // Choose shortest path or available
                _busyFindingPath = true;

                Stack<Tile> shortestPath = pathFinder.GetShortestPath(potentialTargets, out Tile target);

                _busyFindingPath = false;

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
                    target.TargetedBy = m_enemy;
                    targetedTile = target;

                    isMoving = true;
                    isChasing = true;
                }
            }
            else
            {
                FindPath(LevelState.Instance.Player.CurrentTile, true);
            }*/
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
    }
}
