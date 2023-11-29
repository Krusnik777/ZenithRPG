using UnityEngine;

namespace DC_ARPG
{
    public class Enemy : MonoBehaviour
    {
        private EnemyCharacter m_enemyCharacter;
        public EnemyCharacter EnemyCharacter => m_enemyCharacter;

        private void Start()
        {
            m_enemyCharacter = GetComponent<EnemyCharacter>();
        }
    }
}
