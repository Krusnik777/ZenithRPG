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

            StoryEventManager.Instance.EventOnStoryEventEnded -= OnStoryEventEnded;

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
        [SerializeField] private string m_id;
        public string EntityId => m_id;

        public bool IsSerializable()
        {
            return false;
        }

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

        #endregion
    }
}
