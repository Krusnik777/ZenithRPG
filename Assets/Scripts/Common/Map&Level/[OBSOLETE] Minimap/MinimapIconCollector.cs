using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class MinimapIconCollector : MonoBehaviour, IDataPersistence
    {
        private List<MinimapIcon> discoveredIcons = new List<MinimapIcon>();

        public float GetMapCompletionPercent()
        {
            discoveredIcons.Clear();

            foreach (var minimapIcon in MinimapIcon.AllMinimapIcons)
            {
                if (minimapIcon.Discovered)
                    discoveredIcons.Add(minimapIcon);
            }

            float percent = (float) discoveredIcons.Count / MinimapIcon.AllMinimapIcons.Count * 100;

            return percent;
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

            foreach (var minimapIcon in MinimapIcon.AllMinimapIcons)
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

            foreach (var discoveredIconId in s.discoveredIconsId)
            {
                foreach (var minimapIcon in MinimapIcon.AllMinimapIcons)
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
