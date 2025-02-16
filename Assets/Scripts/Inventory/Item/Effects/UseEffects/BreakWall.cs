using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "BreakWall", menuName = "ScriptableObjects/UseEffects/BreakWall")]
    public class BreakWall : UseEffect
    {
        public override void Use(IItem item)
        {
            var player = LevelState.Instance.Player;

            if (player == null)
            {
                Debug.LogError("On Use Item - Not Found Player");
                return;
            }

            var potentialBreakableWall = player.CheckForwardGridForInspectableObject();

            if (potentialBreakableWall is BreakableWall)
            {
                var breakableWall = potentialBreakableWall as BreakableWall;

                if (breakableWall.StandingInFrontOfWall)
                {
                    breakableWall.BreakWall();
                    item.Amount--;
                }
            }
        }
    }
}
