using UnityEngine;

namespace DC_ARPG
{
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] protected int m_damage;

        protected bool disabled;
        public bool Disabled => disabled;

        public void SetTrapActive(bool state)
        {
            disabled = !state;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Player player))
            {
                if (player.IsJumping && !player.JumpedAndLanded) return;

                if (!disabled) player.Character.PlayerStats.ChangeCurrentHitPoints(this, -m_damage);
                player.ActionsIsAvailable = false;
            }

            if (other.transform.parent.TryGetComponent(out Enemy enemy))
            {
                if (enemy.EnemyAI.State != EnemyState.Patrol && !disabled) enemy.Character.EnemyStats.ChangeCurrentHitPoints(this, -m_damage);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Player player))
            {
                player.ActionsIsAvailable = true;
            }
        }
    }
}
