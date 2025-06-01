using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "DoubleAttackMagic", menuName = "ScriptableObjects/MagicEffects/DoubleAttackMagic")]
    public class DoubleAttackMagic : MagicEffect
    {
        public override void Use(Player player, MagicItem item)
        {
            if (!item.HasInfiniteUses)
            {
                ShortMessage.Instance.ShowMessage("СИЛА!");

                // Do Magic

                item.Uses--;
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
