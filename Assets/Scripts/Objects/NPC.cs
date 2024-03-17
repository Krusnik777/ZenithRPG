using UnityEngine;

namespace DC_ARPG
{
    public class NPC : InspectableObject, IDataPersistence
    {
        [SerializeField] private string m_animationState;

        private StoryEvent[] m_storyEvents;

        private int activeStoryEvent = 0;

        private Animator m_animator;

        private PositionTrigger m_positionTrigger;
        public bool StandingInFrontOfNPC => m_positionTrigger != null ? m_positionTrigger.InRightPosition : false;

        public override string InfoText
        {
            get
            {
                if (!StandingInFrontOfNPC) return string.Empty;
                
                m_infoText = "Поговорить";

                return m_infoText;
            }
        }

        public void ChangeActiveStoryToNext() => activeStoryEvent++;

        public override void OnInspection(Player player)
        {
            if (!StandingInFrontOfNPC) return;

            if (activeStoryEvent < m_storyEvents.Length)
            {
                m_storyEvents[activeStoryEvent].StartStoryEvent();
            }

            base.OnInspection(player);
        }

        private void Awake()
        {
            m_animator = GetComponentInChildren<Animator>();
            m_positionTrigger = GetComponentInChildren<PositionTrigger>();
            m_storyEvents = GetComponentsInChildren<StoryEvent>();
        }

        private void Start()
        {
            m_animator.Play(m_animationState);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public int activeStory;

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

            s.activeStory = activeStoryEvent;
            s.enabled = gameObject.activeInHierarchy;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            activeStoryEvent = s.activeStory;
            gameObject.SetActive(s.enabled);
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
