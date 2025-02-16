using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/UseEffects/Heal")]
    public class Heal : UseEffect
    {
        public override void Use(IItem item)
        {
            var player = LevelState.Instance.Player;

            if (player == null)
            {
                Debug.LogError("On Use Item - Not Found Player");
                return;
            }

            player.Character.Stats.ChangeCurrentHitPoints(this, 9999);
            item.Amount--;
            UISounds.Instance.PlayItemUsedSound(m_useSound);
        }
    }
}
