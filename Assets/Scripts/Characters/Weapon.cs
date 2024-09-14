using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject m_parent;
        [SerializeField] private GameObject m_trail;

        public event UnityAction EventOnBrokenWeapon;
        public event UnityAction EventOnBlock;

        private Collider m_collider;

        public void SetWeaponActive(bool state)
        {
            if (m_collider == null) m_collider = GetComponent<Collider>();

            m_collider.enabled = state;
            if (m_trail != null) m_trail.SetActive(state);
        }

        private void Start()
        {
            m_collider = GetComponent<Collider>();

            m_collider.enabled = false;
            if (m_trail != null) m_trail.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == m_parent) return;

            if (m_parent.TryGetComponent(out Player parentPlayer))
            {
                if (other.transform.parent.TryGetComponent(out Enemy enemy))
                {
                    if (enemy.Character.EnemyStats.CurrentHitPoints > 0)
                    {
                        enemy.Character.EnemyStats.ChangeCurrentHitPoints(parentPlayer, -parentPlayer.Character.PlayerStats.Attack, DamageType.Physic);
                        parentPlayer.Character.PlayerStats.AddStrengthExperience(enemy.Character.EnemyStats.Level);


                        var weaponItem = parentPlayer.Character.Inventory.WeaponItemSlot.Item as WeaponItem;
                        if (!weaponItem.HasInfiniteUses) parentPlayer.Character.Inventory.WeaponItemSlot.UseWeapon(this, parentPlayer, EventOnBrokenWeapon);
                    }

                    SetWeaponActive(false);
                }
            }

            if (m_parent.TryGetComponent(out Enemy parentEnemy))
            {
                if (other.transform.parent.TryGetComponent(out Player player))
                {
                    if (player.IsBlocking && player.CheckForwardGridForEnemy() == parentEnemy)
                    {
                        EventOnBlock?.Invoke();

                        return;
                    }

                    if (player.State == Player.PlayerState.Rest)
                    {
                        player.Character.PlayerStats.ChangeCurrentHitPoints(m_parent, -parentEnemy.Character.EnemyStats.Attack * 2, DamageType.Physic); // Damage x2
                        player.ChangeRestState();

                        return;
                    }

                    player.Character.PlayerStats.ChangeCurrentHitPoints(m_parent, -parentEnemy.Character.EnemyStats.Attack, DamageType.Physic);
                    SetWeaponActive(false);
                }
            }
        }
    }
}
