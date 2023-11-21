using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "MagicItemInfo", menuName = "ScriptableObjects/ItemInfo/MagicItemInfo")]
    public class MagicItemInfo : ItemInfo
    {
        [Header("MagicItem")]
        //public MagicType Magic;
        public bool HasInfiniteUses;
        public int MagicPointsForUse;
    }
}
