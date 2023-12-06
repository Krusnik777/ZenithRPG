using UnityEngine;

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
        private EnemyState state;
        public EnemyState State => state;

        private FieldOfView enemyFOV;
        public FieldOfView EnemyFieldOfView => enemyFOV;

        private EnemyAIController enemyAI;
        public EnemyAIController EnemyAI => enemyAI;

        private EnemyCharacter m_character;
        public EnemyCharacter Character => m_character;

        public GameObject PlayerGameObject => enemyFOV?.PlayerGameObject;

        public bool CheckForPlayerInSightRange() => enemyFOV.CanSeePlayer;

        public bool CheckForPlayerInAttackRange()
        {
            Ray attackRay = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(attackRay, out hit, 1f))
            {
                if (hit.collider.GetComponentInParent<Player>())
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
        }

        private void StartState(EnemyState state)
        {
            this.state = state;
        }
    }
}
