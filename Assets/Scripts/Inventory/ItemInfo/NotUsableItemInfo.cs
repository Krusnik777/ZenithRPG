using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "NotUsableItemInfo", menuName = "ScriptableObjects/ItemInfo/NotUsableItemInfo")]
    public class NotUsableItemInfo : ItemInfo
    {
        [Header("NotUsableItem")]
        public int MaxAmount;
        //public ItemEffect ItemEffect;
    }
}
