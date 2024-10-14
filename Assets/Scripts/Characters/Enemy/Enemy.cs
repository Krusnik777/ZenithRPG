using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class Enemy : CharacterAvatar, IDataPersistence
    {
        [SerializeField][Range(0, 1)] private float m_comboChance = 0.33f;
        [Header("EnemyCharacter")]
        [SerializeField] private EnemyCharacter m_character;
        
        public event UnityAction<Enemy> EventOnDeath;

        public override CharacterBase Character => m_character;

        private EnemyAIController enemyAI;
        public EnemyAIController EnemyAI => enemyAI == null ? GetComponent<EnemyAIController>() : enemyAI;

        public override void Attack()
        {
            if (isAttacking) return;

            isAttacking = true;

            if (Random.value > 1 - m_comboChance)
            {
                m_animator.SetTrigger("Attack1");
                m_animator.SetBool("ComboAttack", true);

                return;
            }

            int attackCount = Random.Range(0, m_attackHits);
            attackCount++;

            m_animator.SetTrigger("Attack" + attackCount);
        }

        public void StopAttack()
        {
            if (!isAttacking) return;

            for (int i = 1; i <= m_attackHits; i++)
            {
                m_animator.ResetTrigger("Attack" + i);
            }

            m_animator.SetBool("ComboAttack", false);

            isAttacking = false;
        }

        public void StartMovement()
        {
            m_animator.SetFloat("MovementZ", 1);
        }

        public void StopMovement()
        {
            m_animator.SetFloat("MovementZ", 0);
            m_animator.SetTrigger("IdleTrigger");
        }

        private void Start()
        {
            enemyAI = GetComponent<EnemyAIController>();

            m_character.Stats.EventOnDeath += EventOnDeathEnemyCharacter;
        }

        private void OnDestroy()
        {
            m_character.Stats.EventOnDeath -= EventOnDeathEnemyCharacter;
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
