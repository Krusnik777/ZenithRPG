using UnityEngine;

namespace DC_ARPG
{
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private int m_damage;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Player player))
            {
                player.Character.PlayerStats.ChangeCurrentHitPoints(this, -m_damage);
            }

            if (other.transform.parent.TryGetComponent(out Enemy enemy))
            {
                if (enemy.State != EnemyState.Patrol) enemy.Character.EnemyStats.ChangeCurrentHitPoints(this, -m_damage);
            }
        }
    }
}
