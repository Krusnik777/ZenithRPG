using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(DoorAnimator))]
    public class Door : InspectableObject, IDataPersistence
    {
        //[SerializeField] private Animator m_animator;
        [SerializeField] private DoorAnimator m_animator;
        [SerializeField] private Collider m_collider;
        [SerializeField] private bool m_openableDirectly;
        [SerializeField] private bool m_locked;
        [SerializeField] private bool m_requireSpecialKey;
        [SerializeField] private UsableItemInfo m_specificKeyItemInfo;
        [Header("PositionTriggers")]
        [SerializeField] private PositionTrigger m_forwardPositionTrigger;
        [SerializeField] private PositionTrigger m_backwardPositionTrigger;
        [Header("MinimapIndicator")]
        [SerializeField] private GameObject m_minimapIndicatorPrefab;

        public bool Locked => m_locked;
        public bool RequireSpecialKey => m_requireSpecialKey;
        public UsableItemInfo SpecificKeyItemInfo => m_specificKeyItemInfo;

        public event UnityAction EventOnDoorOpened;
        public event UnityAction EventOnDoorClosed;

        public bool StandingInFrontOfDoor => m_forwardPositionTrigger.InRightPosition || m_backwardPositionTrigger.InRightPosition;

        private DoorSFX m_doorSFX;

        //private bool inClosedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosedState");
        private bool inClosedState => m_animator.InInitState;
        public bool Closed => inClosedState;
        //private bool inOpenedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenedState");
        private bool inOpenedState => m_animator.InActiveState;
        public bool Opened => inOpenedState;

        public override bool Disabled => m_collider.isTrigger;

        public override string InfoText
        {
            get
            {
                if (inClosedState) m_infoText = "Открыть";
                if (inOpenedState) m_infoText = "Закрыть";

                return m_infoText;
            }
        }

        public void ShowMinimapIndication()
        {
            if (m_minimapIndicatorPrefab != null)
            {
                var indicator = Instantiate(m_minimapIndicatorPrefab, transform.position, Quaternion.identity);

                Destroy(indicator, 1.1f);
            }
        }

        public void ChangeOpenableDirectly(bool state)
        {
            m_openableDirectly = state;
            if (state) m_doorSFX.PlayUnlockedSound();
            else
            {
                if (inOpenedState) Close();
            }
        }

        public void ChangeUnlocked(bool state)
        {
            if (state) Unlock();
            else Lock();
        }

        public void Lock()
        {
            m_locked = true;
            if (inOpenedState) Close();
        }

        public void Unlock()
        {
            m_locked = false;

            if (m_openableDirectly)
            {
                m_doorSFX.PlayUnlockedSound();
                if (StandingInFrontOfDoor) ShortMessage.Instance.ShowMessage("Открыто.");
            }

            if (inClosedState) Open();
        }

        public override void OnInspection(Player player)
        {
            if (m_openableDirectly)
            {
                if (!StandingInFrontOfDoor)
                {
                    // Not Possible but just to be sure

                    ShortMessage.Instance.ShowMessage("Дверь. С этой стороны не открыть.");
                    return;
                }

                if (m_locked)
                {
                    if (!m_requireSpecialKey) ShortMessage.Instance.ShowMessage("Закрыто на замок.");
                    else ShortMessage.Instance.ShowMessage("Закрыто на необычный замок.");

                    m_doorSFX.PlayLockedSound();
                }
                else
                {
                    if (inClosedState) Open();
                    if (inOpenedState) Close();

                    EventOnInspection?.Invoke();
                }
            }
            else
            {
                if (StandingInFrontOfDoor)
                {
                    ShortMessage.Instance.ShowMessage("Не поддается.");

                    m_doorSFX.PlayLockedSound();
                }
            }
        }

        private void Awake()
        {
            m_doorSFX = GetComponentInChildren<DoorSFX>();
        }

        private void Open()
        {
            if (inOpenedState) return;

            //m_animator.SetTrigger("Open");
            m_animator.Play();
            m_doorSFX.PlayUseSound();
            m_collider.isTrigger = true;

            EventOnDoorOpened?.Invoke();
        }

        private void Close()
        {
            if (inClosedState) return;

            //m_animator.SetTrigger("Close");
            m_animator.ResetToInit();
            m_doorSFX.PlayUseSound();
            m_collider.isTrigger = false;

            EventOnDoorClosed?.Invoke();
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public bool openableDirectly;
            public bool locked;
            public bool colliderState;
            //public int animatorState;
            public bool animatorActiveState;

            public DataState() { }
        }

        [Header("Serialize")]
        [SerializeField] private string m_prefabId;
        [SerializeField] private string m_id;
        [SerializeField] private bool m_isSerializable = true;
        public string PrefabId => m_prefabId;
        public string EntityId => m_id;
        public bool IsCreated => false;

        public bool IsSerializable() => m_isSerializable;

        public string SerializeState()
        {
            DataState s = new DataState();

            s.enabled = gameObject.activeInHierarchy;
            s.openableDirectly = m_openableDirectly;
            s.locked = m_locked;
            s.colliderState = m_collider.isTrigger;
            if (gameObject.activeInHierarchy)
                //s.animatorState = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
                s.animatorActiveState = m_animator.InActiveState;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
            m_openableDirectly = s.openableDirectly;
            m_locked = s.locked;
            m_collider.isTrigger = s.colliderState;
            if (s.enabled)
                //m_animator.Play(s.animatorState);
                m_animator.ChangeInitialState(s.animatorActiveState);
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
