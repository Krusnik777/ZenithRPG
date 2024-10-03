using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject m_parent;
        [SerializeField] private GameObject m_trail;

        public event UnityAction EventOnBrokenWeapon;

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
                    if (enemy.Character.Stats.CurrentHitPoints > 0)
                    {
                        enemy.Character.Stats.ChangeCurrentHitPoints(parentPlayer, -parentPlayer.Character.Stats.Attack, DamageType.Physic);
                        (parentPlayer.Character.Stats as PlayerStats).AddStrengthExperience(enemy.Character.Stats.Level);


                        var weaponItem = (parentPlayer.Character as PlayerCharacter).Inventory.WeaponItemSlot.Item as WeaponItem;
                        if (!weaponItem.HasInfiniteUses) (parentPlayer.Character as PlayerCharacter).Inventory.WeaponItemSlot.UseWeapon(this, parentPlayer, EventOnBrokenWeapon);
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
                        player.OnBlock();

                        return;
                    }

                    if (player.State == Player.PlayerState.Rest)
                    {
                        player.Character.Stats.ChangeCurrentHitPoints(m_parent, -parentEnemy.Character.Stats.Attack * 2, DamageType.Physic); // Damage x2
                        player.ChangeRestState();

                        return;
                    }

                    player.Character.Stats.ChangeCurrentHitPoints(m_parent, -parentEnemy.Character.Stats.Attack, DamageType.Physic);
                    SetWeaponActive(false);
                }
            }
        }
    }
}
