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

        private PlayerCharacter m_character;
        public void Construct(PlayerCharacter character) => m_character = character;
        public PlayerCharacter Character => m_character;

        private Ray m_lookRay;

        private PlayerState m_playerState;
        public PlayerState State => m_playerState;

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

            Character.Inventory.MagicItemSlot.UseMagic(this, this);
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



        #endregion
    }
}
