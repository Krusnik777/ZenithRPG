using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class UseEffect
    {
        public enum UseType
        {
            Heal,
            Unlock,
            SpecificUnlock,
            BreakWall,
            UnveilHiddenPit
        }

        [SerializeField] private UseType m_useEffect;
        [SerializeField] private AudioClip m_useSound;

        public void Use(Player player, IItem item)
        {
            if (m_useEffect == UseType.Heal) Heal(player, item);
            if (m_useEffect == UseType.Unlock) Unlock(player, item);
            if (m_useEffect == UseType.SpecificUnlock) SpecificUnlock(player, item);
            if (m_useEffect == UseType.BreakWall) BreakWall(player, item);
            if (m_useEffect == UseType.UnveilHiddenPit) UnveilHiddenPit(player, item);
        }

        private void Heal(Player player, IItem item)
        {
            player.Character.PlayerStats.ChangeCurrentHitPoints(this, 9999);
            item.Amount--;
            UISounds.Instance.PlayItemUsedSound(m_useSound);
        }

        private void Unlock(Player player, IItem item)
        {
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

        private void SpecificUnlock(Player player, IItem item)
        {
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
                    }
                }
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
                    }
                }
            }
        }

        private void BreakWall(Player player, IItem item)
        {
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

        private void UnveilHiddenPit(Player player, IItem item)
        {
            var potentialHiddenPit = player.CheckForwardGridForInspectableObject();

            if (potentialHiddenPit is Pit)
            {
                if (potentialHiddenPit is HiddenPit)
                {
                    var hiddenPit = potentialHiddenPit as HiddenPit;

                    if (hiddenPit.TrapFloor != null)
                    {
                        ShortMessage.Instance.ShowMessage("Здесь яма!");
                        hiddenPit.UnveilHiddenPit();
                        item.Amount--;
                        return;
                    }
                    else
                    {
                        ShortMessage.Instance.ShowMessage("Шар упал в яму.");
                        item.Amount--;
                        return;
                    }
                }

                ShortMessage.Instance.ShowMessage("Шар упал в яму.");
                item.Amount--;
                return;
            }

            if (player.CheckForwardGridIsEmpty() == true)
            {
                ShortMessage.Instance.ShowMessage("Ямы впереди нет.");
            }
            else
            {
                ShortMessage.Instance.ShowMessage("Шар отскочил от препятствия и упал на ногу.");
                player.Character.PlayerStats.ChangeCurrentHitPoints(item, -1);
            }
        }


    }
}
