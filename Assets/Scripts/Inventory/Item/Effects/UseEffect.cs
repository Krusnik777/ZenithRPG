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
            BreakWall
        }

        [SerializeField] private UseType m_useEffect;
        [SerializeField] private AudioClip m_useSound;

        public void Use(Player player, IItem item)
        {
            if (m_useEffect == UseType.Heal) Heal(player, item);
            if (m_useEffect == UseType.Unlock) Unlock(player, item);
            if (m_useEffect == UseType.SpecificUnlock) SpecificUnlock(player, item);
            if (m_useEffect == UseType.BreakWall) BreakWall(player, item);
        }

        private void Heal(Player player, IItem item)
        {
            player.Character.PlayerStats.ChangeCurrentHitPoints(this, 9999);
            item.Amount--;
            UISounds.Instance.PlayItemUsedSound(m_useSound);
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
                        if (chest.SpecificKeyItemInfo != null) ShortMessage.Instance.ShowMessage("������, ����� ����� ������ ����.");
                        else ShortMessage.Instance.ShowMessage("������, ����������� �� ������.");
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
                        if (door.SpecificKeyItemInfo != null) ShortMessage.Instance.ShowMessage("������, ����� ����� ������ ����.");
                        else ShortMessage.Instance.ShowMessage("������, ����������� �� ������.");
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
                        ShortMessage.Instance.ShowMessage("���� ���� ���� �� ��������.");
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
                        ShortMessage.Instance.ShowMessage("���� ���� ���� �� ��������.");
                    }
                }
            }
        }

        private void BreakWall(Player player, IItem item)
        {
            var potentialBreakableWall = player.CheckForwardGridForInsectableObject();

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
