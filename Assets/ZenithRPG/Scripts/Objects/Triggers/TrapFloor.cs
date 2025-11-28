using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class TrapFloor : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private GameObject m_floorBreakEffectPrefab;
        [SerializeField] private float m_effectDestroyTime = 1.0f;

        public void DestroyTrapFloor()
        {
            if (m_floorBreakEffectPrefab != null)
            {
                var effect = Instantiate(m_floorBreakEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, m_effectDestroyTime);
            }

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

        #if UNITY_EDITOR

        [ContextMenu(nameof(GenerateIdForAllTrapFloors))]
        private void GenerateIdForAllTrapFloors()
        {
            if (Application.isPlaying) return;

            List<TrapFloor> trapFloorsInScene = new List<TrapFloor>();
            trapFloorsInScene.AddRange(FindObjectsByType<TrapFloor>(FindObjectsSortMode.None));

            foreach (var trapFloor in trapFloorsInScene)
            {
                trapFloor.UpdateId();
            }
        }

        public void UpdateId()
        {
            m_id = string.Empty;
            m_id = "TrapFloor_" + System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        #endif
    }
}
