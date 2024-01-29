using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class Player : CharacterAvatar, IDependency<PlayerCharacter>
    {
        public enum PlayerState
        {
            Active,
            Rest
        }

        [Header("Magic")]
        [SerializeField] private float m_castDelay = 1.0f;
        [Header("Weapon")]
        [SerializeField] private Weapon m_weapon;

        private PlayerCharacter m_character;
        public void Construct(PlayerCharacter character) => m_character = character;
        public PlayerCharacter Character => m_character;

        private Ray m_lookRay;

        private PlayerState m_playerState;
        public PlayerState State => m_playerState;

        private bool isCasting;
        public bool IsCasting => isCasting;

        protected override bool inIdleState => !(inMovement || isJumping || isAttacking || isBlocking || isCasting);

        #region PlayerActions

        public override void Attack()
        {
            if (Character.Inventory.WeaponItemSlot.IsEmpty) return;
            base.Attack();
        }

        public void Inspect()
        {
            if (!inIdleState) return;

            var inspectableObject = CheckForwardGridForInsectableObject();

            if (inspectableObject != null)
            {
                inspectableObject.OnInspection(this);
            }
            else
            {
                ShortMessage.Instance.ShowMessage("Ничего интересного.");
            }
        }

        public void UseMagic()
        {
            if (!inIdleState) return;

            if (Character.Inventory.MagicItemSlot.IsEmpty) return;

            StartCoroutine(CastMagic());
        }

        public void Rest()
        {
            if (!inIdleState) return;

            if (m_playerState == PlayerState.Active)
            {
                m_playerState = PlayerState.Rest;

                RestState.Instance.StartRest();

                return;
            }

            if (m_playerState == PlayerState.Rest)
            {
                RestState.Instance.EndRest();

                m_playerState = PlayerState.Active;
            }
        }

        public void UseActiveItem()
        {
            if (!inIdleState) return;

            Character.Inventory.UseItem(this, Character.Inventory.ActiveItemSlot);
        }

        public void ChooseLeftActiveItem()
        {
            Character.Inventory.SetActiveItemSlot(this, 0);
        }

        public void ChooseMiddleActiveItem()
        {
            Character.Inventory.SetActiveItemSlot(this, 1);
        }

        public void ChooseRightActiveItem()
        {
            Character.Inventory.SetActiveItemSlot(this, 2);
        }

        #endregion

        public void ShowSwordBreakEffect()
        {
            CharacterSFX.PlayBrokenSwordEffect(m_weapon.transform.position);
        }

        public InspectableObject CheckForwardGridForInsectableObject()
        {
            RaycastHit hit;

            if (Physics.Raycast(m_lookRay, out hit, 1f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.transform.parent.TryGetComponent(out InspectableObject inspectableObject))
                    {
                        return inspectableObject;
                    }
                }
            }
            return null;
        }

        public Enemy CheckForwardGridForEnemy()
        {
            RaycastHit hit;

            if (Physics.Raycast(m_lookRay, out hit, 1f, 1, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.transform.parent.TryGetComponent(out Enemy enemy))
                    {
                        return enemy;
                    }
                }
            }
            return null;
        }

        public bool CheckForwardGridIsEmpty()
        {
            RaycastHit hit;

            if (Physics.Raycast(m_lookRay, out hit, 1f))
            {
                if (hit.collider && !hit.collider.isTrigger)
                {
                    return false;
                }
            }

            return true;
        }

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            m_lookRay = new Ray(transform.position + new Vector3(0, 0.1f, 0), transform.forward);
        }

        #region Coroutines

        private IEnumerator CastMagic()
        {
            isCasting = true;

            m_animator.SetTrigger("CastMagic");

            var elapsed = 0.0f;

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("CastMagic"));

            while(elapsed < m_castDelay)
            {
                elapsed += Time.deltaTime;

                yield return null;
            }

            Character.Inventory.MagicItemSlot.UseMagic(this, this);

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("CastMagic"));

            isCasting = false;
        }

        #endregion
    }
}
