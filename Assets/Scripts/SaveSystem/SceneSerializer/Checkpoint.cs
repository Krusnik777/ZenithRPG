using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(BoxCollider))]
    public class Checkpoint : MonoBehaviour, IDataPersistence
    {
        private bool m_used = false;

        private event UnityAction eventOnSaved;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent(out Player player))
            {
                if (!m_used)
                {
                    m_used = true;

                    eventOnSaved += OnSaveCompleted;

                    player.SavePosition(transform.position);

                    SceneSerializer.Instance.SaveSceneData();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnSaveCompleted()
        {
            eventOnSaved -= OnSaveCompleted;

            Destroy(gameObject);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public bool used;

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

            s.used = m_used;
            s.enabled = gameObject.activeInHierarchy;

            eventOnSaved?.Invoke();

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            m_used = s.used;
            gameObject.SetActive(s.enabled);
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
