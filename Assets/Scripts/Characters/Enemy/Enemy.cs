using UnityEngine;

namespace DC_ARPG
{
    public class Enemy : CharacterAvatar
    {
        private EnemyCharacter m_character;
        public EnemyCharacter Character => m_character;

        private void Start()
        {
            m_character = GetComponent<EnemyCharacter>();
        }
    }
}