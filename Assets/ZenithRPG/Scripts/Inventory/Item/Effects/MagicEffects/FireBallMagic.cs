using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "FireBallMagic", menuName = "ScriptableObjects/MagicEffects/FireBallMagic")]
    public class FireBallMagic : MagicEffect
    {
        [SerializeField] private FireBallFlight m_fireBallPrefab;

        public override void Use(Player player, MagicItem item)
        {
            var playerCharacter = player.Character as PlayerCharacter;

            if (!item.HasInfiniteUses)
            {
                var fireBall = Instantiate(m_fireBallPrefab, player.transform.position, player.transform.rotation);
                fireBall.SetParent(player.gameObject);

                item.Uses--;
            }
            else
            {
                if (player.Character.Stats.TryUseMagicPoints(item.MagicPointsForUse))
                {
                    var fireBall = Instantiate(m_fireBallPrefab, player.transform.position, player.transform.rotation);
                    fireBall.SetParent(player.gameObject);
                }
                else
                {
                    ShortMessage.Instance.ShowMessage("Не хватает маны.");
                }
            }
        }
    }
}
