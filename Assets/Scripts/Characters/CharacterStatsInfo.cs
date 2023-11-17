using UnityEngine;

namespace DC_ARPG
{
    public abstract class CharacterStatsInfo : ScriptableObject
    {
        [Header("BaseStats")]
        public int Level;
        public int HitPoints;
        public int MagicPoints;
        public int Strength;
        public int Intelligence;
        public int MagicResist;
        public int Luck;

        
    }
}
