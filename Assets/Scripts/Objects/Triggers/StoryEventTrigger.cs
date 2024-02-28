using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(BoxCollider))]
    public class StoryEventTrigger : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private StoryEventInfo m_storyEventInfo;
        [Space]
        public UnityEvent EventOnStoryEventEnd;

        private void OnDestroy()
        {
            StoryEventManager.Instance.EventOnStoryEventEnded -= OnStoryEventEnded;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Player>())
            {
                StoryEventManager.Instance.StartEvent(m_storyEventInfo);

                StoryEventManager.Instance.EventOnStoryEventEnded += OnStoryEventEnded;
            }
        }

        private void OnStoryEventEnded()
        {
            EventOnStoryEventEnd?.Invoke();

            Destroy(gameObject);
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
