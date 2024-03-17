using UnityEngine;

namespace DC_ARPG
{
    public class DeletableObstacle : MonoBehaviour, IDataPersistence
    {
        [Header("MinimapIndicator")]
        [SerializeField] private GameObject m_minimapIndicatorPrefab;

        public void DeleteObstacleWithIndication()
        {
            if (m_minimapIndicatorPrefab != null)
            {
                var indicator = Instantiate(m_minimapIndicatorPrefab, transform.position, Quaternion.identity);

                Destroy(indicator, 1.1f);
            }

            Destroy(gameObject);
        }

        public void DeleteObstacle()
        {
            // sfx + animation?
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
