using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public abstract class CharacterAvatar : MonoBehaviour
    {
        [Header("MovementParameters")]
        [SerializeField] protected float m_transitionMoveSpeed = 0.5f;
        [SerializeField] protected float m_transitionJumpSpeed = 0.3f;
        [SerializeField] protected float m_transitionRotateSpeed = 0.25f;
        [SerializeField] protected float m_afterJumpDelay = 0.1f;
        [SerializeField] protected float m_transitionFallSpeed = 0.25f;
        [SerializeField] protected AnimationCurve m_jumpUpCurve;
        [Header("Attack & Defense")]
        [SerializeField] private int m_attackHits = 2;
        [SerializeField] private BlockStamina m_blockStamina;

        public abstract CharacterBase Character { get; }

        #region Tile

        protected Tile currentTile;
        public virtual Tile CurrentTile => currentTile;

        public Tile GetCurrentTile()
        {
            Tile tile = null;

            Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), -Vector3.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1f, 1, QueryTriggerInteraction.Ignore))
            {
                tile = hit.collider.GetComponentInParent<Tile>();
            }

            return tile;
        }

        public void SetCurrentTile(Tile tile) => currentTile = tile;

        #endregion

        #region Parameters

        protected bool inMovement;
        public bool InMovement => inMovement;

        public event UnityAction EventOnFallStart;

        protected bool isFalling;
        protected bool isFallen;
        public bool IsFallingOrFallen => isFalling || isFallen;

        protected bool isJumping;
        public bool IsJumping => isJumping;

        protected bool isAttacking;
        public bool IsAttacking => isAttacking;

        protected int hitCount = 0;

        protected Coroutine attackRoutine;

        protected bool isBlocking;
        public bool IsBlocking => isBlocking;
        public event UnityAction EventOnBlock;

        protected virtual bool inIdleState => !(inMovement || isJumping || isFalling || isAttacking || isBlocking);
        public bool InIdleState => inIdleState;

        protected Animator m_animator;
        public Animator Animator => m_animator;

        #endregion

        #region SupportMethods

        public void LandAfterJump()
        {
            // Can do something
        }

        public virtual void LandAfterFall()
        {
            isFalling = false;
            isFallen = true;

            m_animator.ResetTrigger("Fall"); // Just to be safe
        }

        #endregion

        #region AvatarActions

        public void Move(Vector2 inputDirection)
        {
            if (isFallen) return;

            if (!inIdleState) return;

            Vector3 direction = GetDirection(inputDirection);

            if (direction == Vector3.zero)
            {
                m_animator.SetFloat("MovementX", 0);
                m_animator.SetFloat("MovementZ", 0);

                return;
            }

            currentTile = GetCurrentTile();

            Tile targetTile = currentTile.FindNeighbourByDirection(direction);

            if (targetTile == null) return;

            if (targetTile.OccupiedBy != null || targetTile.Type == TileType.Obstacle) return;

            if (targetTile.Type == TileType.Closable && targetTile.CheckClosed()) return;

            m_animator.SetFloat("MovementX", inputDirection.x);
            m_animator.SetFloat("MovementZ", inputDirection.y);

            StartCoroutine(MoveTo(targetTile));
        }

        public void Turn(float angle)
        {
            if (!inIdleState) return;

            StartCoroutine(RotateAt(angle));
        }

        public void Jump()
        {
            if (!inIdleState) return;

            if (isFallen)
            {
                Tile targetTile = currentTile.FindNeighbourByDirection(transform.forward);

                if (targetTile == null) return;

                if (targetTile.OccupiedBy != null || targetTile.Type == TileType.Obstacle) return;

                if (targetTile.Type == TileType.Closable && targetTile.CheckClosed()) return;

                StartCoroutine(JumpUpForward(targetTile));

                isFallen = false;

                return;
            }

            currentTile = GetCurrentTile();

            var forwardTiles = currentTile.FindTwoForwardTiles(transform.forward);

            if (forwardTiles[0] == null)
            {
                StartCoroutine(JumpInPlace());
            }
            else
            {
                if (forwardTiles[0].OccupiedBy != null || forwardTiles[0].Type == TileType.Closable && forwardTiles[0].CheckClosed())
                {
                    StartCoroutine(JumpInPlace());
                }
                else
                {
                    if (forwardTiles[1] != null)
                    {
                        if (forwardTiles[1].OccupiedBy != null || forwardTiles[1].Type == TileType.Obstacle || forwardTiles[1].Type == TileType.Closable && forwardTiles[1].CheckClosed())
                        {
                            if (forwardTiles[0].Type != TileType.Obstacle)
                            {
                                StartCoroutine(JumpToForwardTile(forwardTiles[0]));
                            }
                            else
                            {
                                StartCoroutine(JumpInPlace());
                            }
                        }
                        else
                        {
                            StartCoroutine(JumpTwoTilesForward(forwardTiles));
                        }
                    }
                    else
                    {
                        if (forwardTiles[0].Type != TileType.Obstacle)
                        {
                            StartCoroutine(JumpToForwardTile(forwardTiles[0]));
                        }
                        else
                        {
                            StartCoroutine(JumpInPlace());
                        }
                    }
                }
            }
        }

        public void Fall()
        {
            if (IsFallingOrFallen) return;

            StartCoroutine(FallRoutine());
        }

        public virtual void Attack()
        {
            if (!inIdleState && !isAttacking) return;

            if (
                (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f || m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.4f)
                && m_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackState.Attack" + hitCount)
                ) return;

            hitCount++;

            if (hitCount > m_attackHits)
            {
                if (isAttacking) return; // Wait combo end
                hitCount = 1;
            }

            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
            }

            attackRoutine = StartCoroutine(SetAttack(hitCount));
        }

        public virtual void Block(string name)
        {
            if (m_blockStamina != null)
                if (m_blockStamina.DefenseBreaked) return;

            if (isAttacking)
            {
                if (attackRoutine != null)
                {
                    StopCoroutine(attackRoutine);
                }

                ResetAttack();

                m_animator.SetTrigger("AttackToBlock");
                isBlocking = true;
            }

            if (!inIdleState && !isBlocking) return;

            switch (name)
            {
                case "BlockStart":
                    {
                        m_animator.SetBool("BlockHold", true);
                        isBlocking = true;
                    };
                    break;
                case "BlockHold":
                    {
                        if (!isBlocking)
                        {
                            m_animator.SetBool("BlockHold", true);
                            isBlocking = true;
                        }
                    };
                    break;
                case "BlockEnd":
                    {
                        StartCoroutine(BlockEnd());
                    };
                    break;
            }
        }

        public virtual void OnBlock()
        {
            EventOnBlock?.Invoke();

            if (m_blockStamina == null) return;

            m_blockStamina.SpendStamina();

            if (!m_blockStamina.DefenseBreaked)
            {
                m_animator.SetTrigger("BlockHit");
            }
            else
            {
                // TEMP

                StartCoroutine(BlockEnd());
            }
        }

        public CharacterAvatar CheckForwardGridForOpponent()
        {
            RaycastHit hit;

            var lookRay = new Ray(transform.position + new Vector3(0, 0.4f, 0), transform.forward);

            if (Physics.Raycast(lookRay, out hit, 1f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.transform.parent.TryGetComponent(out CharacterAvatar opponent))
                    {
                        return opponent;
                    }
                }
            }
            return null;
        }

        #endregion

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();

            currentTile = GetCurrentTile();
            if (currentTile != null) currentTile.SetTileOccupied(this);

            if (m_blockStamina != null) m_blockStamina.InitStamina(this);
        }

        private Vector3 GetDirection(Vector2 inputDirection)
        {
            Vector3 direction = Vector3.zero;

            if (inputDirection.x != 0 && inputDirection.y == 0)
            {
                direction = transform.right * Mathf.Sign(inputDirection.x);
            }

            if (inputDirection.y != 0 && inputDirection.x == 0)
            {
                direction = transform.forward * Mathf.Sign(inputDirection.y);
            }

            return direction;
        }

        private void UpdateNewPosition()
        {
            if (currentTile != null)
            {
                currentTile.SetTileOccupied(null);

                if (currentTile.Type == TileType.Mechanism) currentTile.ReturnMechanismToDefault();

                currentTile = GetCurrentTile();

                if (currentTile != null)
                {
                    if (currentTile.Type == TileType.Mechanism)
                    {
                        currentTile.GetTileReaction(this);
                    }

                    if (currentTile.Type == TileType.Pit)
                    {
                        currentTile.GetTileReaction();
                        Fall();
                    }
                }
            }
        }

        private void ResetAttack()
        {
            isAttacking = false;
            hitCount = 0;
            attackRoutine = null;
        }

        #region Coroutines

        private IEnumerator MoveTo(Tile targetTile)
        {
            inMovement = true;
            targetTile.SetTileOccupied(this);

            var startPosition = transform.position;
            var targetPosition = targetTile.transform.position;

            var elapsed = 0.0f;

            while (elapsed < m_transitionMoveSpeed)
            {
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / m_transitionMoveSpeed);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            UpdateNewPosition();

            //yield return null;

            inMovement = false;

            yield return null;
            //yield return new WaitForSeconds(0.1f);

            if (!inMovement)
            {
                m_animator.SetFloat("MovementX", 0);
                m_animator.SetFloat("MovementZ", 0);
                if (!isFalling) m_animator.SetTrigger("IdleTrigger");
            }
        }

        private IEnumerator RotateAt(float angle)
        {
            inMovement = true;
            var startRotation = transform.rotation;
            var targetRotation = transform.rotation * Quaternion.Euler(Vector3.up * angle);

            var elapsed = 0.0f;

            if (angle == -90)
            {
                m_animator.Play("TurnLeft");

                yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("TurnLeft"));
            }

            if (angle == 90)
            {
                m_animator.Play("TurnRight");

                yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("TurnRight"));
            }

            if (angle == 180)
            {
                m_animator.Play("Turn180");

                yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Turn180"));
            }

            while (elapsed < m_transitionRotateSpeed)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / m_transitionRotateSpeed);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;

            if (angle == -90) yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("TurnLeft"));
            if (angle == 90) yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("TurnRight"));
            if (angle == 180) yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Turn180"));

            inMovement = false;
        }

        private IEnumerator JumpToForwardTile(Tile targetTile)
        {
            isJumping = true;

            targetTile.SetTileOccupied(this);

            var startPosition = transform.position;
            Vector3 targetPosition = targetTile.transform.position;

            var elapsed = 0.0f;
            var time = m_transitionJumpSpeed;
            time *= 1.5f; // maybe to do different times to variables

            m_animator.Play("Jump");

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            while (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
            {
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / time);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            UpdateNewPosition();

            if (isFalling)
            {
                yield return null;
            }
            else
            {
                yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

                yield return new WaitForSeconds(m_afterJumpDelay);
            }

            isJumping = false;
        }

        private IEnumerator JumpTwoTilesForward(Tile[] tiles)
        {
            isJumping = true;

            tiles[0].SetTileOccupied(this);
            tiles[1].SetTileOccupied(this);

            var startPosition = transform.position;
            Vector3 targetPosition = tiles[1].transform.position;

            var elapsed = 0.0f;
            var time = m_transitionJumpSpeed;

            m_animator.Play("Jump");

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            while (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
            {
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / time);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            UpdateNewPosition();

            tiles[0].SetTileOccupied(null);

            if (isFalling)
            {
                yield return null;
            }
            else
            {
                yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

                yield return new WaitForSeconds(m_afterJumpDelay);
            }

            isJumping = false;
        }

        private IEnumerator JumpInPlace()
        {
            isJumping = true;

            m_animator.Play("Jump");

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            yield return new WaitForSeconds(m_afterJumpDelay);

            isJumping = false;
        }

        private IEnumerator JumpUpForward(Tile targetTile)
        {
            isJumping = true;

            targetTile.SetTileOccupied(this);

            var startPosition = transform.position;
            Vector3 targetPosition = targetTile.transform.position;

            var elapsed = 0.0f;
            var time = m_transitionJumpSpeed;
            time *= 1.5f;

            m_animator.Play("Jump");

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            while (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
            {
                elapsed += Time.deltaTime;

                float elapsedToTime = elapsed / time;
                float heightEvaluated = m_jumpUpCurve.Evaluate(elapsedToTime);

                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsedToTime);
                transform.position = new Vector3(transform.position.x, heightEvaluated, transform.position.z);

                /*
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / time);
                elapsed += Time.deltaTime;*/

                yield return null;
            }

            transform.position = targetPosition;

            UpdateNewPosition();

            if (isFalling)
            {
                yield return null;
            }
            else
            {
                yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

                yield return new WaitForSeconds(m_afterJumpDelay);
            }

            isJumping = false;
        }

        private IEnumerator FallRoutine()
        {
            EventOnFallStart?.Invoke();

            isFalling = true;

            yield return null; // delay for fall event

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = new Vector3(transform.position.x, -1, transform.position.z);

            var elapsed = 0.0f;

            m_animator.Play("FallState.Fall");

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("FallState.Fall"));

            while (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
            {
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / m_transitionFallSpeed);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            m_animator.SetTrigger("Fall");

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("FallState.LandAfterFall"));

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("FallState.LandAfterFall"));

            yield return new WaitForSeconds(m_afterJumpDelay);
        }

        private IEnumerator SetAttack(int attackCount)
        {
            isAttacking = true;

            if (attackCount == 1)
            {
                m_animator.SetTrigger("Attack1");

                yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackState.Attack1"));
            }

            if (attackCount > 1 && attackCount <= m_attackHits)
            {
                m_animator.SetTrigger("Attack" + attackCount);

                yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackState.Attack" + attackCount));
            }

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackState.Attack" + attackCount));

            ResetAttack();
        }

        private IEnumerator BlockEnd()
        {
            m_animator.SetBool("BlockHold", false);

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("BlockState.BlockEnd"));

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("BlockState.BlockEnd"));

            isBlocking = false;

            m_animator.ResetTrigger("AttackToBlock"); // just to be safe
        }

        #endregion
    }
}
