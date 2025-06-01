using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class MagicalTorch : InspectableObject, IDataPersistence
    {
        [SerializeField] private GameObject m_fire;

        public UnityEvent OnFired;

        public void FireTorch()
        {
            if (m_fire.activeInHierarchy) return;

            m_fire.SetActive(true);
            OnFired?.Invoke();
        }

        public override void OnInspection(Player player)
        {
            if (m_fire.activeInHierarchy)
            {
                ShortMessage.Instance.ShowMessage("Факел.");
            }
            else
            {
                ShortMessage.Instance.ShowMessage("Потухший факел.");
            }

            base.OnInspection(player);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public bool fired;

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
            s.fired = m_fire.activeInHierarchy;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
            m_fire.SetActive(s.fired);
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
