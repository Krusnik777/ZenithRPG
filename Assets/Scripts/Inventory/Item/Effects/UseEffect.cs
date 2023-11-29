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
            SpecificUnlock
        }

        [SerializeField] private UseType m_useEffect;

        public void Use(Player player, IItem item)
        {
            if (m_useEffect == UseType.Heal) Heal(player, item);
            if (m_useEffect == UseType.Unlock) Unlock(player, item);
            if (m_useEffect == UseType.SpecificUnlock) SpecificUnlock(player, item);
        }

        private void Heal(Player player, IItem item)
        {
            player.Character.PlayerStats.ChangeCurrentHitPoints(9999);
            item.Amount--;
        }

        private void Unlock(Player player, IItem item)
        {
            var potentialUnlockable = player.CheckForwardGridForInsectableObject();

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
                        ShortMessage.Instance.ShowMessage("Похоже, здесь нужен особый ключ.");
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
                        ShortMessage.Instance.ShowMessage("Похоже, здесь нужен особый ключ.");
                    }
                }
            }
        }

        private void SpecificUnlock(Player player, IItem item)
        {
            var potentialUnlockable = player.CheckForwardGridForInsectableObject();

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
                        ShortMessage.Instance.ShowMessage("Похоже, здесь нужен особый ключ.");
                    }
                }
            }
        }
        
    }
}
