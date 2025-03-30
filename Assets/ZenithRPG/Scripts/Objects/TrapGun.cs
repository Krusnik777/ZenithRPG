using UnityEngine;

namespace DC_ARPG
{
    public class TrapGun : InspectableObject, IDataPersistence
    {
        [SerializeField] private GameObject m_holeObject;
        [SerializeField] private FireBallFlight m_fireBallPrefab;

        private bool disabled = false;

        public void SetTrapActive(bool state)
        {
            disabled = !state;
        }

        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Подозрительное отверстие.");

            base.OnInspection(player);
        }

        public void Shoot()
        {
            if (disabled) return;

            var fireBall = Instantiate(m_fireBallPrefab, m_holeObject.transform.position, m_holeObject.transform.rotation);
            fireBall.SetParent(gameObject);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public bool disabled;

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
            s.disabled = disabled;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
            disabled = s.disabled;
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
