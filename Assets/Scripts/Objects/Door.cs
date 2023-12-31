using UnityEngine;
using UnityEngine.Events;

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

        public event UnityAction EventOnDoorOpened;
        public event UnityAction EventOnDoorClosed;

        public bool StandingInFrontOfDoor => m_forwardPositionTrigger.InRightPosition || m_backwardPositionTrigger.InRightPosition;

        private Animator m_animator;
        private DoorSFX m_doorSFX;

        private bool inClosedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosedState") : true;
        public bool Closed => inClosedState;
        private bool inOpenedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenedState") : false;
        public bool Opened => inOpenedState;

        public void Lock()
        {
            m_locked = true;
            if (inOpenedState) Close();
        }

        public void Unlock()
        {
            m_locked = false;

            m_doorSFX.PlayUnlockedSound();

            if (StandingInFrontOfDoor) ShortMessage.Instance.ShowMessage("�������.");
            if (inClosedState) Open();
        }

        public override void OnInspection(Player player)
        {
            if (!StandingInFrontOfDoor)
            {
                // Not Possible but just to be sure

                ShortMessage.Instance.ShowMessage("�����. � ���� ������� �� �������.");
                return;
            }

            if (m_locked)
            {
                if (!m_requireSpecialKey) ShortMessage.Instance.ShowMessage("�������.");
                else ShortMessage.Instance.ShowMessage("������� �� ��������� �����.");

                m_doorSFX.PlayLockedSound();

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
            m_doorSFX = GetComponentInChildren<DoorSFX>();
        }

        private void Open()
        {
            m_animator.SetTrigger("Open");
            m_doorSFX.PlayUseSound();
            m_collider.isTrigger = true;

            if (StandingInFrontOfDoor)
            {
                if (m_forwardPositionTrigger.InRightPosition) m_forwardRoomCeiling.SetActive(false);
                if (m_backwardPositionTrigger.InRightPosition) m_backwardRoomCeiling.SetActive(false);
            }

            EventOnDoorOpened?.Invoke();
        }

        private void Close()
        {
            m_animator.SetTrigger("Close");
            m_doorSFX.PlayUseSound();
            m_collider.isTrigger = false;

            if (StandingInFrontOfDoor)
            {
                if (m_forwardPositionTrigger.InRightPosition) m_forwardRoomCeiling.SetActive(true);
                if (m_backwardPositionTrigger.InRightPosition) m_backwardRoomCeiling.SetActive(true);
            }

            EventOnDoorClosed?.Invoke();
        }
    }
}
