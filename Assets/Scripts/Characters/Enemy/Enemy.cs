using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class Enemy : CharacterAvatar
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
    }
}
