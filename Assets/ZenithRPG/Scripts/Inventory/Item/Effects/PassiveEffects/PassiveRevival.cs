using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "PassiveRevival", menuName = "ScriptableObjects/PassiveEffects/PassiveRevival")]
    public class PassiveRevival : PassiveEffect
    {
        [SerializeField] private GameObject m_sFXPrefab;
        public GameObject SFX => m_sFXPrefab;

        public override PassiveType Type => PassiveType.Revival;

        public override void GetEffect(Player player, IItemSlot slot)
        {
            player.Character.Stats.ChangeCurrentHitPoints(this, 9999);
            (player.Character as PlayerCharacter).Inventory.RemoveItemFromInventory(this, slot);

            if (m_sFXPrefab != null)
            {
                var sfx = Instantiate(m_sFXPrefab, player.transform.position, Quaternion.identity);

                Destroy(sfx, 1.0f);
            }

            ShortMessage.Instance.ShowMessage("Что-то сломалось в рюкзаке, но чувствуется прилив сил.");
        }
    }
}
