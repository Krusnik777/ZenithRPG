using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "UsableItemInfo", menuName = "ScriptableObjects/ItemInfo/UsableItemInfo")]
    public class UsableItemInfo : ItemInfo
    {
        [Header("UsableItem")]
        public int MaxAmount;
        //public UseEffect UseEffect;
    }
}
