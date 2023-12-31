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
            if (!item.HasInfiniteUses)
            {
                player.Character.AvailableMagic.CreateFireball(player.gameObject, player.transform.position, player.transform.rotation);

                item.Uses--;

                if (item.Uses <= 0)
                {
                    player.Character.Inventory.RemoveItemFromInventory(this, player.Character.Inventory.MagicItemSlot);

                    UISounds.Instance.PlayMagicItemDisappearSound();
                }
            }
            else
            {
                if (player.Character.PlayerStats.TryUseMagicPoints(item.MagicPointsForUse))
                {
                    player.Character.AvailableMagic.CreateFireball(player.gameObject, player.transform.position, player.transform.rotation);
                }
                else
                {
                    ShortMessage.Instance.ShowMessage("�� ������� ����.");
                }
            }
        }

        private void UseDoubleAttack(Player player, MagicItem item)
        {
            if (!item.HasInfiniteUses)
            {
                ShortMessage.Instance.ShowMessage("����!");

                // Do Magic

                item.Uses--;

                if (item.Uses <= 0) player.Character.Inventory.RemoveItemFromInventory(this, player.Character.Inventory.MagicItemSlot);
            }
            else
            {
                if (player.Character.PlayerStats.TryUseMagicPoints(item.MagicPointsForUse))
                {
                    ShortMessage.Instance.ShowMessage("����!");

                    // Do Magic
                }
                else
                {
                    ShortMessage.Instance.ShowMessage("�� ������� ����.");
                }
            }
        }
    }
}
