using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class TrapPlate : InspectableObject, IDataPersistence
    {
        [SerializeField] private DamageZone m_trap;

        public void SetTrapActive(bool state) => m_trap.SetTrapActive(state);

        public override bool Disabled => m_trap.Disabled;

        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Ловушка!");

            base.OnInspection(player);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public bool trapDisabled;

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

            s.trapDisabled = m_trap.Disabled;
            s.enabled = gameObject.activeInHierarchy;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            SetTrapActive(!s.trapDisabled);
            gameObject.SetActive(s.enabled);
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion

        #if UNITY_EDITOR

        [ContextMenu(nameof(GenerateIdForAllTraps))]
        private void GenerateIdForAllTraps()
        {
            if (Application.isPlaying) return;

            List<TrapPlate> trapsInScene = new List<TrapPlate>();
            trapsInScene.AddRange(FindObjectsOfType<TrapPlate>());

            for(int i = 0; i < trapsInScene.Count; i++)
            {
                trapsInScene[i].UpdateId(i);
            }
        }

        public void UpdateId(int number)
        {
            string name = "Trap_" + number.ToString("D3");
            gameObject.name = name;
            m_id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        }

        #endif
    }
}
