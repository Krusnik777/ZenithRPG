using UnityEngine;

namespace DC_ARPG
{
    public abstract class PassiveEffect : ScriptableObject
    {
        public enum PassiveType
        {
            Revival,
            LuckUp
        }

        public abstract PassiveType Type { get; } 
        public abstract void GetEffect(Player player, IItemSlot slot);
    }
}
