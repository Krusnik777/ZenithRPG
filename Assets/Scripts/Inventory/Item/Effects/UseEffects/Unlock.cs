using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "Unlock", menuName = "ScriptableObjects/UseEffects/Unlock")]
    public class Unlock : UseEffect
    {
        public override void Use(IItem item)
        {
            var player = LevelState.Instance.Player;

            if (player == null)
            {
                Debug.LogError("On Use Item - Not Found Player");
                return;
            }

            var potentialUnlockable = player.CheckForwardGridForInspectableObject();

            if (potentialUnlockable is Chest)
            {
                var chest = potentialUnlockable as Chest;

                if (chest.StandingInFrontOfChest && chest.Locked)
                {
                    if (!chest.RequireSpecialKey)
                    {
                        chest.Unlock();
                        item.Amount--;
                    }
                    else
                    {
                        if (chest.SpecificKeyItemInfo != null) ShortMessage.Instance.ShowMessage("Похоже, здесь нужен особый ключ.");
                        else ShortMessage.Instance.ShowMessage("Похоже, открывается не ключем.");
                    }
                }
            }

            if (potentialUnlockable is Door)
            {
                var door = potentialUnlockable as Door;

                if (door.StandingInFrontOfDoor && door.Locked)
                {
                    if (!door.RequireSpecialKey)
                    {
                        door.Unlock();
                        item.Amount--;
                    }
                    else
                    {
                        if (door.SpecificKeyItemInfo != null) ShortMessage.Instance.ShowMessage("Похоже, здесь нужен особый ключ.");
                        else ShortMessage.Instance.ShowMessage("Похоже, открывается не ключем.");
                    }
                }
            }
        }
    }
}
