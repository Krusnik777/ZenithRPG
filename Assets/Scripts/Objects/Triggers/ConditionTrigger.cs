using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class ConditionTrigger : MonoBehaviour, IDataPersistence
    {
        public UnityEvent OnTrigger;

        public event UnityAction<ConditionTrigger> EventOnConditionFulfilled;

        protected bool isTriggered = false;
        public bool IsTriggered => isTriggered;

        public void OnConditionFulfilled()
        {
            if (isTriggered) return;

            isTriggered = true;

            OnTrigger?.Invoke();
            EventOnConditionFulfilled?.Invoke(this);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public bool isTriggered;

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
            s.isTriggered = isTriggered;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
            isTriggered = s.isTriggered;
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
