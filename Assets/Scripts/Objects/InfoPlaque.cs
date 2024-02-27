using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class InfoPlaque : InspectableObject, IDataPersistence
    {
        [SerializeField] private StoryEventInfo m_storyEventInfo;
        [Space]
        public UnityEvent EventOnInspectionEnd;

        public override void OnInspection(Player player)
        {
            StoryEventManager.Instance.StartEvent(m_storyEventInfo);

            StoryEventManager.Instance.EventOnStoryEventEnded += OnStoryEventEnded;

            base.OnInspection(player);
        }

        private void OnStoryEventEnded()
        {
            EventOnInspectionEnd?.Invoke();

            StoryEventManager.Instance.EventOnStoryEventEnded -= OnStoryEventEnded;
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;

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

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
