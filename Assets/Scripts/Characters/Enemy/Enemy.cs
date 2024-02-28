using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class Enemy : CharacterAvatar, IDataPersistence
    {
        public event UnityAction<Enemy> EventOnDeath;

        private EnemyCharacter m_character;
        public EnemyCharacter Character => m_character;

        private EnemyAIController enemyAI;
        public EnemyAIController EnemyAI => enemyAI == null ? GetComponent<EnemyAIController>() : enemyAI;

        private void Start()
        {
            m_character = GetComponent<EnemyCharacter>();
            enemyAI = GetComponent<EnemyAIController>();

            m_character.EnemyStats.EventOnDeath += EventOnDeathEnemyCharacter;
        }

        private void OnDestroy()
        {
            m_character.EnemyStats.EventOnDeath -= EventOnDeathEnemyCharacter;
        }

        private void EventOnDeathEnemyCharacter(object sender) => EventOnDeath?.Invoke(this);

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public Vector3 position;
            public Quaternion rotation;

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
            s.position = transform.position;
            s.rotation = transform.rotation;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
            transform.position = s.position;
            transform.rotation = s.rotation;
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { /*Created By Spawner?*/ }

        #endregion
    }
}
