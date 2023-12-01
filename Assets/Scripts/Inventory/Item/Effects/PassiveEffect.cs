using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class PassiveEffect
    {
        public enum PassiveType
        {
            None,
            Revival
        }

        [SerializeField] private PassiveType m_passiveEffect;
        public PassiveType EffectType => m_passiveEffect;

        public void GetEffect(Player player, IItemSlot slot)
        {
            if (m_passiveEffect == PassiveType.Revival) Revive(player, slot);
        }

        private void Revive(Player player, IItemSlot slot)
        {
            player.Character.PlayerStats.ChangeCurrentHitPoints(this, 9999);
            player.Character.Inventory.RemoveItemFromInventory(this, slot);

            Debug.Log("Revived");
        }
    }
}
