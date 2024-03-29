using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(BoxCollider))]
    public class EnemyEnabler : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private List<Enemy> m_enemies;

        private bool activated = false;

        public void Activate()
        {
            activated = true;
            gameObject.SetActive(activated);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Player>())
            {
                MakeEnemiesAppear();
            }
        }

        private void MakeEnemiesAppear()
        {
            // Check if some enemies already appeared or destroyed
            foreach(var enemy in m_enemies)
            {
                if (enemy == null || enemy.gameObject.activeInHierarchy)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }

            foreach(var enemy in m_enemies)
            {
                enemy.gameObject.SetActive(true);
            }

            gameObject.SetActive(false);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool activated;

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

            s.activated = activated;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            if (s.activated) Activate();
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion

    }
}
