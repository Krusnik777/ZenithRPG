using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class MagicEffect
    {
        public enum MagicType
        {
            FireBall,
            DoubleAttack
        }

        [SerializeField] private MagicType m_magic;

        public void Use(Player player, MagicItem item)
        {
            if (m_magic == MagicType.FireBall) UseFireball(player, item);
            if (m_magic == MagicType.DoubleAttack) UseDoubleAttack(player, item);
        }

        private void UseFireball(Player player, MagicItem item)
        {
            var playerCharacter = player.Character as PlayerCharacter;

            if (!item.HasInfiniteUses)
            {
                playerCharacter.AvailableMagic.CreateFireball(player.gameObject, player.transform.position, player.transform.rotation);

                item.Uses--;

                if (item.Uses <= 0)
                {
                    playerCharacter.Inventory.RemoveItemFromInventory(this, (player.Character as PlayerCharacter).Inventory.MagicItemSlot);

                    UISounds.Instance.PlayMagicItemDisappearSound();
                }
            }
            else
            {
                if (player.Character.Stats.TryUseMagicPoints(item.MagicPointsForUse))
                {
                    playerCharacter.AvailableMagic.CreateFireball(player.gameObject, player.transform.position, player.transform.rotation);
                }
                else
                {
                    ShortMessage.Instance.ShowMessage("Не хватает маны.");
                }
            }
        }

        private void UseDoubleAttack(Player player, MagicItem item)
        {
            if (!item.HasInfiniteUses)
            {
                ShortMessage.Instance.ShowMessage("СИЛА!");

                // Do Magic

                item.Uses--;

                if (item.Uses <= 0) (player.Character as PlayerCharacter).Inventory.RemoveItemFromInventory(this, (player.Character as PlayerCharacter).Inventory.MagicItemSlot);
            }
            else
            {
                if (player.Character.Stats.TryUseMagicPoints(item.MagicPointsForUse))
                {
                    ShortMessage.Instance.ShowMessage("СИЛА!");

                    // Do Magic
                }
                else
                {
                    ShortMessage.Instance.ShowMessage("Не хватает маны.");
                }
            }
        }
    }
}
