using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class UseEffect
    {
        public enum UseType
        {
            Heal,
            Unlock
        }

        [SerializeField] private UseType m_useEffect;

        public void Use(Player player, IItem item)
        {
            if (m_useEffect == UseType.Heal) Heal(player, item);
            if (m_useEffect == UseType.Unlock) Unlock(player, item);
        }

        private void Heal(Player player, IItem item)
        {
            if (player == null)
            {
                Debug.LogError("Player - null");
                return;
            }

            player.Character.PlayerStats.ChangeCurrentHitPoints(9999);
            item.Amount--;
        }

        private void Unlock(Player player, IItem item)
        {
            if (player == null)
            {
                Debug.LogError("Player - null");
                return;
            }

            if (!player.CheckForwardGridIsEmpty())
            {
                Debug.Log("Unlock");
                item.Amount--;
            }
        }
        
    }
}
