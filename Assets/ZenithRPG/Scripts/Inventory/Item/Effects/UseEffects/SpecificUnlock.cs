using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "SpecificUnlock", menuName = "ScriptableObjects/UseEffect/SpecificUnlock")]
    public class SpecificUnlock : UseEffect
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
                    if (chest.RequireSpecialKey && chest.SpecificKeyItemInfo == item.Info)
                    {
                        chest.Unlock();
                        item.Amount--;
                    }
                    else
                    {
                        ShortMessage.Instance.ShowMessage("Этот ключ сюда не подходит.");
                        OnFailure();
                    }
                }
                else OnFailure();
            }

            if (potentialUnlockable is Door)
            {
                var door = potentialUnlockable as Door;

                if (door.StandingInFrontOfDoor && door.Locked)
                {
                    if (door.RequireSpecialKey && door.SpecificKeyItemInfo == item.Info)
                    {
                        door.Unlock();
                        item.Amount--;
                    }
                    else
                    {
                        ShortMessage.Instance.ShowMessage("Этот ключ сюда не подходит.");
                        OnFailure();
                    }
                }
                else OnFailure();
            }
            else OnFailure();
        }

        private void OnFailure()
        {
            UISounds.Instance.PlayInventoryActionFailureSound();
        }
    }
}
