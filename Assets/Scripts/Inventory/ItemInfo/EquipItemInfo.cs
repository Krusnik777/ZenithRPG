using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "EquipItemInfo", menuName = "ScriptableObjects/ItemInfo/EquipItemInfo")]
    public class EquipItemInfo : ItemInfo
    {
        [Header("EquipItem")]
        public int StatIncrease;
    }
}
