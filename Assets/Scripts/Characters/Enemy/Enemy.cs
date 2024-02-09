using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Battle
    }

    public class Enemy : CharacterAvatar
    {
        public event UnityAction<Enemy> EventOnDeath;

        private EnemyState state;
        public EnemyState State => state;

        private FieldOfView enemyFOV;
        public FieldOfView EnemyFieldOfView => enemyFOV;

        private EnemyCharacter m_character;
        public EnemyCharacter Character => m_character;

        private EnemyAIController enemyAI;
        public EnemyAIController EnemyAI => enemyAI == null ? GetComponent<EnemyAIController>() : enemyAI;

        public GameObject DetectedPlayerGameObject => enemyFOV?.PlayerGameObject;

        public bool CheckForPlayerInSightRange() => enemyFOV.CanSeePlayer;

        public bool CheckForPlayerInAttackRange()
        {
            Ray attackRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);

            if (Physics.Raycast(attackRay, 1f, enemyFOV.TargetMask))
            {
                return true;
            }

            return false;
        }

        public bool CheckForwardGridForObstacle()
        {
            Ray checkRay = new Ray(transform.position + new Vector3(0, 0.25f, 0), transform.forward);

            // TEMP CHECK -> MAYBE DO LAYER MASK FOR SPECIFIC OBSCTACLES

            if (Physics.Raycast(checkRay, out RaycastHit hit, 1.2f, 1, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.transform.parent.TryGetComponent(out Door door))
                {
                    return true;
                }
            }

            return false;
        }

        public void StartChase() => StartState(EnemyState.Chase);

        public void StartPatrol() => StartState(EnemyState.Patrol);

        public void StartAttack() => StartState(EnemyState.Battle);

        private void Start()
        {
            m_character = GetComponent<EnemyCharacter>();
            enemyFOV = GetComponent<FieldOfView>();
            enemyAI = GetComponent<EnemyAIController>();

            m_character.EnemyStats.EventOnDeath += EventOnDeathEnemyCharacter;
        }

        private void OnDestroy()
        {
            m_character.EnemyStats.EventOnDeath -= EventOnDeathEnemyCharacter;
        }

        private void StartState(EnemyState state)
        {
            this.state = state;
        }

        private void EventOnDeathEnemyCharacter(object sender) => EventOnDeath?.Invoke(this);
    }
}
