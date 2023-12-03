using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(Animator))]
    public class Door : InspectableObject
    {
        [SerializeField] private Collider m_collider;
        [SerializeField] private bool m_openableDirectly;
        [SerializeField] private bool m_locked;
        [SerializeField] private bool m_requireSpecialKey;
        [SerializeField] private UsableItemInfo m_specificKeyItemInfo;
        [Header("PositionTriggers")]
        [SerializeField] private PositionTrigger m_forwardPositionTrigger;
        [SerializeField] private PositionTrigger m_backwardPositionTrigger;
        [Header("Ceilings")]
        [SerializeField] private GameObject m_forwardRoomCeiling;
        [SerializeField] private GameObject m_backwardRoomCeiling;
        public bool Locked => m_locked;
        public bool RequireSpecialKey => m_requireSpecialKey;
        public UsableItemInfo SpecificKeyItemInfo => m_specificKeyItemInfo;

        public bool StandingInFrontOfDoor => m_forwardPositionTrigger.InRightPosition || m_backwardPositionTrigger.InRightPosition;

        private Animator m_animator;

        private bool inClosedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosedState") : true;
        private bool inOpenedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenedState") : false;

        public void Lock()
        {
            m_locked = true;
            if (inOpenedState) Close();
        }

        public void Unlock()
        {
            m_locked = false;
            if (StandingInFrontOfDoor) ShortMessage.Instance.ShowMessage("Открыто.");
            if (inClosedState) Open();
        }

        public override void OnInspection(Player player)
        {
            if (!StandingInFrontOfDoor)
            {
                ShortMessage.Instance.ShowMessage("Дверь. С этой стороны не открыть.");
                return;
            }

            if (m_locked)
            {
                if (!m_requireSpecialKey) ShortMessage.Instance.ShowMessage("Закрыто.");
                else ShortMessage.Instance.ShowMessage("Закрыто на необычный замок.");
                return;
            }

            if (!m_openableDirectly && StandingInFrontOfDoor) return;

            if (inClosedState) Open();
            if (inOpenedState) Close();

            EventOnInspection?.Invoke();
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        private void Open()
        {
            m_animator.SetTrigger("Open");
            m_collider.isTrigger = true;

            if (StandingInFrontOfDoor)
            {
                if (m_forwardPositionTrigger.InRightPosition) m_forwardRoomCeiling.SetActive(false);
                if (m_backwardPositionTrigger.InRightPosition) m_backwardRoomCeiling.SetActive(false);
            }
        }

        private void Close()
        {
            m_animator.SetTrigger("Close");
            m_collider.isTrigger = false;

            if (StandingInFrontOfDoor)
            {
                if (m_forwardPositionTrigger.InRightPosition) m_forwardRoomCeiling.SetActive(true);
                if (m_backwardPositionTrigger.InRightPosition) m_backwardRoomCeiling.SetActive(true);
            }
        }
    }
}
