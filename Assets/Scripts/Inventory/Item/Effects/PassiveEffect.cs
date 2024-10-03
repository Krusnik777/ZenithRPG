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
        [SerializeField] private GameObject m_sFXPrefab;
        public GameObject SFX => m_sFXPrefab;

        public void GetEffect(Player player, IItemSlot slot)
        {
            if (m_passiveEffect == PassiveType.Revival) Revive(player, slot);
        }

        private void Revive(Player player, IItemSlot slot)
        {
            player.Character.Stats.ChangeCurrentHitPoints(this, 9999);
            (player.Character as PlayerCharacter).Inventory.RemoveItemFromInventory(this, slot);

            ShortMessage.Instance.ShowMessage("Что-то сломалось в рюкзаке, но чувствуется прилив сил.");
        }
    }
}
