using UnityEngine;

namespace DC_ARPG
{
    public abstract class ItemInfo : ScriptableObject
    {
        [Header("Base")]
        public int ID;
        public string Title;
        [TextArea(1,5)]public string Description;
        public Sprite Icon;
        public Sprite ItemTypeIcon;
        public GameObject Prefab;
        public int Price;
    }
}
