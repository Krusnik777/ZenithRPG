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

        private bool inMovement;
        public bool InMovement => inMovement;

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
            if (InMovement) return;

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
            if (InMovement) return;

            StartCoroutine(RotateAt(angle));
        }

        public void Jump()
        {
            if (InMovement) return;

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
            if (InMovement) return;

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
            Debug.Log("Attack");
        }

        public void Block()
        {
            Debug.Log("Block");
        }

        public void Rest()
        {
            Debug.Log("Rest");
        }

        public void CheckInventory()
        {
            Debug.Log("Inventory Opened");
        }

        public void UseActiveItem()
        {
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

            SetMovementAnimations(direction, false);
        }

        private IEnumerator RotateAt(float angle)
        {
            inMovement = true;
            var startRotation = transform.rotation;
            var targetRotation = transform.rotation * Quaternion.Euler(Vector3.up * angle);

            var elapsed = 0.0f;

            while (elapsed < m_transitionRotateSpeed)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / m_transitionRotateSpeed);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
            inMovement = false;
        }

        private IEnumerator JumpTo(Vector3 distance)
        {
            inMovement = true;
            var startPosition = transform.position;
            var targetPosition = distance + startPosition;

            var elapsed = 0.0f;

            //m_audioSource.Play();
            m_animator.Play("Jump");

            while (elapsed < m_transitionJumpSpeed || transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(startPosition, targetPosition, elapsed / m_transitionJumpSpeed);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            inMovement = false;
        }

        private IEnumerator JumpUp()
        {
            inMovement = true;
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

            inMovement = false;
        }

        #endregion
    }
}
