using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "WeaponItemInfo", menuName = "ScriptableObjects/ItemInfo/WeaponItemInfo")]
    public class WeaponItemInfo : EquipItemInfo
    {
        public int Uses;
        public bool HasInfiniteUses;
    }
}
