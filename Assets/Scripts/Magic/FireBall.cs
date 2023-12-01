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

        private object m_parent;

        public void SetParent(object parent)
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
            transform.position = Vector3.MoveTowards(transform.position, transform.forward*100, m_velocity * Time.deltaTime); 
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_parent is Player)
            {
                if (collision.gameObject.GetComponent<Player>() == true) return;

                if (collision.gameObject.TryGetComponent(out Enemy enemy))
                {
                    enemy.Character.EnemyStats.ChangeCurrentHitPoints(m_parent, -m_damage, DamageType.Magic);
                    (m_parent as Player).Character.PlayerStats.AddIntelligenceExperience(m_experienceForHit);
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

            Destroy(gameObject);
        }

    }
}
