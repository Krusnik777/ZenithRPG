using UnityEngine;

namespace DC_ARPG
{
    public abstract class MagicEffect : ScriptableObject
    {
        public abstract void Use(Player player, MagicItem item);
    }
}
