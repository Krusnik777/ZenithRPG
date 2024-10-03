using UnityEngine;

namespace DC_ARPG
{
    public class FireBall : MonoBehaviour
    {
        [SerializeField] protected float m_velocity;
        [SerializeField] protected float m_lifeTime;
        [SerializeField] private GameObject m_hitPrefab;
        [Header("Parameters")]
        [SerializeField] private int m_fireballLevel = 3;
        [SerializeField] protected int m_damage;

        private GameObject m_parent;

        public void SetParent(GameObject parent)
        {
            m_parent = parent;
        }

        private void Start()
        {
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
                    if (enemy.Character.Stats.CurrentHitPoints > 0)
                    {
                        if (enemy.EnemyAI.State != EnemyState.Chase || enemy.EnemyAI.State != EnemyState.Battle)
                        {
                            enemy.EnemyAI.StartChaseState();
                        }

                        enemy.Character.Stats.ChangeCurrentHitPoints(parentPlayer, -m_damage, DamageType.Magic);
                        (parentPlayer.Character.Stats as PlayerStats).AddIntelligenceExperience(m_fireballLevel);
                    }
                }
            }
            else
            {
                // For example if TrapGun is parent

                if (collision.gameObject.TryGetComponent(out Player player))
                {
                    if (player.State == Player.PlayerState.Rest)
                    {
                        player.Character.Stats.ChangeCurrentHitPoints(m_parent, -m_damage*2, DamageType.Magic); // Damage x2
                        (player.Character.Stats as PlayerStats).AddMagicResistExperience(m_fireballLevel);

                        player.ChangeRestState();
                    }
                    else
                    {
                        player.Character.Stats.ChangeCurrentHitPoints(m_parent, -m_damage, DamageType.Magic);
                        (player.Character.Stats as PlayerStats).AddMagicResistExperience(m_fireballLevel);
                    }
                }

                if (collision.gameObject.TryGetComponent(out Enemy enemy))
                {
                    if (enemy.EnemyAI.State != EnemyState.Patrol)
                    {
                        enemy.Character.Stats.ChangeCurrentHitPoints(m_parent, -m_damage, DamageType.Magic);
                    }
                }
            }

            if (collision.transform.parent != null)
            {
                if (collision.transform.parent.TryGetComponent(out MagicalTorch torch))
                {
                    torch.FireTorch();
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
