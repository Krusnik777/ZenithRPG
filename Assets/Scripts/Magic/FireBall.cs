using UnityEngine;

namespace DC_ARPG
{
    public class FireBall : MonoBehaviour
    {
        // Maybe FireBall Info???

        [SerializeField] protected float m_velocity;
        [SerializeField] protected float m_lifeTime;
        [SerializeField] protected int m_damage;
        [SerializeField] private int m_experienceForHit = 3; // TEMP BALANCE
        [SerializeField] private GameObject m_hitPrefab;

        private GameObject m_parent;

        public void SetParent(GameObject parent)
        {
            m_parent = parent;
        }

        private void Start()
        {
            // Setup FireBallInfo???

            Destroy(gameObject, m_lifeTime);
        }

        private void Update()
        {
            transform.position += transform.forward * m_velocity * Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == m_parent) return;

            if (m_parent.TryGetComponent(out Player parentPlayer))
            {
                if (collision.gameObject.TryGetComponent(out Enemy enemy))
                {
                    enemy.Character.EnemyStats.ChangeCurrentHitPoints(parentPlayer, -m_damage, DamageType.Magic);
                    parentPlayer.Character.PlayerStats.AddIntelligenceExperience(m_experienceForHit);
                }
            }
            else
            {
                if (collision.gameObject.TryGetComponent(out Player player))
                {
                    player.Character.PlayerStats.ChangeCurrentHitPoints(m_parent, -m_damage, DamageType.Magic);
                    player.Character.PlayerStats.AddMagicResistExperience(m_experienceForHit);
                }
            }

            if (m_hitPrefab != null)
            {
                var hitEffect = Instantiate(m_hitPrefab, collision.transform.position, Quaternion.identity);

                Destroy(hitEffect, 0.5f);
            }

            Destroy(gameObject);
        }

    }
}
