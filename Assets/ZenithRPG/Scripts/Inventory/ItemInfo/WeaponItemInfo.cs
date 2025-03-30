using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "WeaponItemInfo", menuName = "ScriptableObjects/ItemInfo/WeaponItemInfo")]
    public class WeaponItemInfo : ItemInfo
    {
        [Header("WeaponItem")]
        public int AttackIncrease;
        public bool HasInfiniteUses;
    }
}
