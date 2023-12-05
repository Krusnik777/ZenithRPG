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

        private EnemyCharacter m_character;
        public EnemyCharacter Character => m_character;

        public GameObject PlayerGameObject => enemyFOV?.PlayerGameObject;

        public void CheckForPlayerInSightRange()
        {
            if (enemyFOV.CanSeePlayer) StartState(EnemyState.Chase);
            else StartState(EnemyState.Patrol);
        }

        public void CheckForPlayerInAttackRange()
        {
            Ray attackRay = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(attackRay, out hit, 1f))
            {
                if (hit.collider.GetComponentInParent<Player>())
                {
                    Debug.Log("here2");
                    StartState(EnemyState.Battle);
                }
            }
            else
            {
                CheckForPlayerInSightRange();
            }
        }

        private void Start()
        {
            m_character = GetComponent<EnemyCharacter>();
            enemyFOV = GetComponent<FieldOfView>();
        }

        private void StartState(EnemyState state)
        {
            this.state = state;
        }
    }
}
