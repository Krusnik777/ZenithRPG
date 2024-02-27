using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class FewConditionsTrigger : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private List<ConditionTrigger> m_conditions;

        public UnityEvent OnAllConditionsFulfilled;

        private bool isTriggered = false;
        public bool IsTriggered => isTriggered;

        private void Start()
        {
            foreach (var condition in m_conditions)
            {
                condition.EventOnConditionFulfilled += OnConditionFulfilled;
            }
        }

        private void OnConditionFulfilled(ConditionTrigger condition)
        {
            if (isTriggered) return;

            if (CheckConditions())
            {
                isTriggered = true;

                OnAllConditionsFulfilled?.Invoke();
            }

            condition.EventOnConditionFulfilled -= OnConditionFulfilled;
        }

        private bool CheckConditions()
        {
            foreach (var condition in m_conditions)
            {
                if (!condition.IsTriggered)
                    return false;
            }

            return true;
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
