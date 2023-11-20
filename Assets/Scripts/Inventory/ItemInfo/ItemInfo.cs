using UnityEngine;

namespace DC_ARPG
{
    public abstract class ItemInfo : ScriptableObject
    {
        [Header("Base")]
        public int ID;
        public string Title;
        public string Description;
        public Sprite Icon;
        public int Price;
    }
}
