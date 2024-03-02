using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class MinimapIconCollector : MonoBehaviour, IDataPersistence
    {
        private List<MinimapIcon> m_minimapIcons;

        public float GetMapCompletionPercent()
        {
            if (m_minimapIcons == null) GetMinimapIcons();

            var discoveredIcons = new List<MinimapIcon>();

            foreach (var minimapIcon in m_minimapIcons)
            {
                if (minimapIcon.Discovered)
                    discoveredIcons.Add(minimapIcon);
            }

            float percent = (float) discoveredIcons.Count / m_minimapIcons.Count * 100;

            return percent;
        }

        private void Start()
        {
            GetMinimapIcons();
        }

        private void GetMinimapIcons()
        {
            if (m_minimapIcons != null) return;

            m_minimapIcons = new List<MinimapIcon>();
            m_minimapIcons.AddRange(MinimapIcon.AllMinimapIcons);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public List<string> discoveredIconsId;
            public float mapCompletionPercent;

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

            s.discoveredIconsId = new List<string>();

            foreach (var minimapIcon in m_minimapIcons)
            {
                if (minimapIcon.Discovered)
                    s.discoveredIconsId.Add(minimapIcon.MinimapIconId);
            }

            s.mapCompletionPercent = GetMapCompletionPercent();

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);

            GetMinimapIcons();

            foreach (var discoveredIconId in s.discoveredIconsId)
            {
                foreach (var minimapIcon in m_minimapIcons)
                {
                    if (minimapIcon.MinimapIconId == discoveredIconId)
                    {
                        minimapIcon.SetDiscovered(true);
                        break;
                    }
                }
            }
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
