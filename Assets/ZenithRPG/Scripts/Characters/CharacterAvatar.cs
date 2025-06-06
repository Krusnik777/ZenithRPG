using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public abstract class CharacterAvatar : MonoBehaviour, IMovable
    {
        [Header("MovementParameters")]
        [SerializeField] protected float m_transitionMoveSpeed = 0.5f;
        [SerializeField] protected float m_transitionJumpSpeed = 0.3f;
        [SerializeField] protected float m_transitionRotateSpeed = 0.25f;
        [SerializeField] protected float m_afterJumpDelay = 0.1f;
        [SerializeField] protected float m_transitionFallSpeed = 0.25f;
        [SerializeField] protected AnimationCurve m_jumpUpCurve;
        [Header("Attack & Defense")]
        [SerializeField] protected int m_attackHits = 2;
        [SerializeField] protected BlockStamina m_blockStamina;

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

        public virtual void UpdateNewPosition(Tile newTile = null)
        {
            if (currentTile == null) return;

            currentTile.SetTileOccupied(null);

            if (currentTile.Type == TileType.Mechanism) currentTile.ReturnMechanismToDefault();

            if (newTile != null) currentTile = newTile;
            else currentTile = GetCurrentTile();

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
        public virtual event UnityAction<int> EventOnAttack;

        protected int hitCount = 0;

        protected Coroutine attackRoutine;

        protected bool isBlocking;
        public bool IsBlocking => isBlocking;
        public event UnityAction EventOnParry;
        public event UnityAction EventOnBlock;
        public event UnityAction EventOnBlockBreak;

        protected bool isKicking;
        public bool IsKicking => isKicking;

        protected virtual bool inIdleState => !(inMovement || isJumping || isFalling || isAttacking || isBlocking || isKicking);
        public bool InIdleState => inIdleState;

        protected Animator m_animator;

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

        public virtual void RecoverAfterKick()
        {
            isKicking = false;

            m_animator.ResetTrigger("Kick"); // Just to be safe
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
                if (forwardTiles[1] != null)
                {
                    StartCoroutine(JumpTwoTilesForward(forwardTiles));
                }
                else
                {
                    StartCoroutine(JumpToForwardTile(forwardTiles[0]));
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
                m_animator.SetBool("BlockHold", true);
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

        public virtual void Kick()
        {
            if (!inIdleState) return;

            isKicking = true;

            m_animator.SetTrigger("Kick");
        }

        public virtual void OnBlock()
        {
            if (m_blockStamina == null) return;

            bool animatorChecker = m_animator.GetCurrentAnimatorStateInfo(0).IsName("BlockState.BlockStart");

            if (animatorChecker)
            {
                /*
                if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f && m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
                {
                    EventOnParry?.Invoke();

                    return;
                }*/

                EventOnParry?.Invoke();

                return;
            }

            m_blockStamina.SpendStamina();

            if (!m_blockStamina.DefenseBreaked)
            {
                if (!animatorChecker)
                    m_animator.SetTrigger("BlockHit");

                EventOnBlock?.Invoke();
            }
            else
            {
                EventOnBlockBreak?.Invoke();

                StartCoroutine(BlockEnd());
            }
        }

        public virtual void Die()
        {
            m_animator.Play("Death");
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

        public Kickable CheckForwardGridForKickableObject()
        {
            RaycastHit hit;

            var lookRay = new Ray(transform.position + new Vector3(0, 0.4f, 0), transform.forward);

            if (Physics.Raycast(lookRay, out hit, 1f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.transform.parent.TryGetComponent(out Kickable kickable))
                    {
                        return kickable.enabled ? kickable : null;
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

        protected virtual void ResetAttack()
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

            UpdateNewPosition(targetTile);

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

            UpdateNewPosition(targetTile);

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

                EventOnAttack?.Invoke(attackCount);
            }

            if (attackCount > 1 && attackCount <= m_attackHits)
            {
                m_animator.SetTrigger("Attack" + attackCount);

                yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackState.Attack" + attackCount));

                EventOnAttack?.Invoke(attackCount);
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
