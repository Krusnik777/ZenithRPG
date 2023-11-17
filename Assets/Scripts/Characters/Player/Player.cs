using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class Player : MonoBehaviour
    {
        [Header("MovementParameters")]
        [SerializeField] private float m_transitionMoveSpeed = 0.5f;
        [SerializeField] private float m_transitionJumpSpeed = 0.3f;
        [SerializeField] private float m_transitionRotateSpeed = 0.25f;

        private Vector3 currentDirection;

        private bool inMovement;
        public bool InMovement => inMovement;

        private bool isJumping;
        public bool IsJumping => isJumping;

        private bool isAttacking;
        public bool IsAttacking => isAttacking;

        private bool isBlocking;
        public bool IsBlocking => isBlocking;

        private int hitCount = 0;

        private Coroutine attackRoutine;

        private bool inIdleState => !(inMovement || isJumping || isAttacking || isBlocking);
        public bool InIdleState => inIdleState;

        private AudioSource m_audioSource;

        private Animator m_animator;

        private bool GetIsGrounded()
        {
            if (GetComponent<Rigidbody>().velocity.y == 0)
                return true;
            else return false;
        }
        public bool IsGrounded => GetIsGrounded();

        public void Move(Vector3 direction)
        {
            if (!inIdleState) return;

            if (currentDirection != direction) currentDirection = direction;

            Ray directionRay = new Ray(transform.position + new Vector3(0, 0.5f, 0), direction);
            RaycastHit hit;

            #if UNITY_EDITOR
            Debug.DrawRay(directionRay.origin, directionRay.direction, Color.red);
            #endif

            if (Physics.Raycast(directionRay, out hit, 1f))
            {
                if (hit.collider && !hit.collider.isTrigger) return;
            }

            StartCoroutine(MoveTo(direction));
        }

        public void Turn(float angle)
        {
            if (!inIdleState) return;

            StartCoroutine(RotateAt(angle));
        }

        public void Jump()
        {
            if (!inIdleState) return;

            if (Mathf.Sign(transform.position.y) < 0)
            {
                Ray upRay = new Ray(transform.position + new Vector3(0, 1.1f, 0), transform.forward);
                RaycastHit upHit;

                if (Physics.Raycast(upRay, out upHit, 1.0f))
                {
                    if (upHit.collider) return;
                }

                StartCoroutine(JumpUp());
                return;
            }

            Ray jumpRay = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
            RaycastHit hit;
            Vector3 distance = Vector3.zero;

            #if UNITY_EDITOR
            Debug.DrawRay(jumpRay.origin, jumpRay.direction, Color.yellow);
            #endif

            if (Physics.Raycast(jumpRay, out hit, 2.0f))
            {
                if (hit.collider)
                {
                    if (hit.distance <= 2.0f && hit.distance > 1.0f)
                    {
                        distance = transform.forward;
                    }
                    else if (hit.distance <= 1.0f)
                    {
                        distance = Vector3.zero;
                    }
                }
            }
            else
            {
                distance = transform.forward * 2;
            }

            StartCoroutine(JumpTo(distance));
        }

        public void Inspect()
        {
            if (!inIdleState) return;

            Ray inspectRay = new Ray(transform.position + new Vector3(0, 0.1f, 0), transform.forward);

            #if UNITY_EDITOR
            Debug.DrawRay(inspectRay.origin, inspectRay.direction, Color.blue);
            #endif

            RaycastHit hit;

            if (Physics.Raycast(inspectRay, out hit, 1f))
            {
                if (hit.collider)
                {
                    if (hit.collider.transform.parent.TryGetComponent(out InspectableObject inspectableObject))
                    {
                        inspectableObject.OnInspection();
                        return;
                    }
                    Debug.Log("Nothing Interesting");
                }
            }
            else Debug.Log("Nothing Interesting at all");
        }

        public void Attack()
        {
            if (!inIdleState && !isAttacking) return;

            if (
                (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f || m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.35f)
                && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + hitCount)
                ) return;

            hitCount++;

            if (hitCount > 2)
            {
                if (isAttacking) return;
                hitCount = 1;
            }

            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
            }
                
            attackRoutine = StartCoroutine(SetAttack(hitCount));
        }

        public void Block(string name)
        {
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
                        
                    };
                    break;
                case "BlockEnd":
                    {
                        StartCoroutine(BlockEnd());
                    };
                    break;
            }

        }

        public void Rest()
        {
            if (inMovement || isJumping) return;

            Debug.Log("Rest");
        }

        public void CheckInventory()
        {
            if (inMovement || isJumping) return;

            Debug.Log("Inventory Opened");
        }

        public void UseActiveItem()
        {
            if (inMovement || isJumping) return;

            Debug.Log("Used Item");
        }

        public void ChangeActiveItem()
        {
            Debug.Log("ChangedItem");
        }

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_animator = GetComponentInChildren<Animator>();
        }

        private void SetMovementAnimations(Vector3 direction, bool value)
        {
            if (direction == transform.forward) m_animator.SetBool("Forward", value);
            if (direction == -transform.forward) m_animator.SetBool("Backward", value);
            if (direction == transform.right) m_animator.SetBool("Right", value);
            if (direction == -transform.right) m_animator.SetBool("Left", value);
        }

        #region Coroutines

        private IEnumerator MoveTo(Vector3 direction)
        {
            inMovement = true;

            var startPosition = transform.position;
            var targetPosition = direction + startPosition;

            var elapsed = 0.0f;

            //m_audioSource.Play();

            SetMovementAnimations(direction, true);

            while (elapsed < m_transitionMoveSpeed)
            {
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / m_transitionMoveSpeed);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            inMovement = false;

            yield return null;
            //yield return new WaitForSeconds(0.1f);

            if (!inMovement || currentDirection != direction) SetMovementAnimations(direction, false);
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

        private IEnumerator JumpTo(Vector3 distance)
        {
            isJumping = true;
            var startPosition = transform.position;
            var targetPosition = distance + startPosition;

            var elapsed = 0.0f;

            //m_audioSource.Play();
            m_animator.Play("Jump");

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            if (distance != Vector3.zero)
            {
                while (elapsed < m_transitionJumpSpeed || transform.position != targetPosition)
                {
                    transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / m_transitionJumpSpeed);
                    elapsed += Time.deltaTime;

                    yield return null;
                }

                transform.position = targetPosition;
            }

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            //yield return new WaitForSeconds(0.1f);

            isJumping = false;
        }

        private IEnumerator JumpUp()
        {
            isJumping = true;
            var startPosition = transform.position;
            var targetPosition = startPosition;
            targetPosition.y = 0.1f;

            var elapsed = 0.0f;

            //m_audioSource.Play();
            m_animator.Play("Jump");

            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / m_transitionJumpSpeed);
                elapsed += Time.deltaTime;

                yield return null;
            }

            startPosition = transform.position;
            targetPosition = transform.position + transform.forward;
            targetPosition.y = 0;
            elapsed = 0.0f;

            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / m_transitionJumpSpeed);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

            //yield return new WaitForSeconds(0.1f);

            isJumping = false;
        }

        private IEnumerator SetAttack(int attackCount)
        {
            isAttacking = true;

            if (attackCount == 1)
            {
                m_animator.SetTrigger("Attack1");

                yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"));
            }

            if (attackCount == 2)
            {
                m_animator.SetTrigger("Attack" + attackCount);

                yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + attackCount));
            }

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + attackCount));

            isAttacking = false;
            hitCount = 0;
            attackRoutine = null;
        }

        private IEnumerator BlockEnd()
        {
            m_animator.SetBool("BlockHold", false);

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("BlockEnd"));

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("BlockEnd"));

            isBlocking = false;
        }

        #endregion
    }
}
