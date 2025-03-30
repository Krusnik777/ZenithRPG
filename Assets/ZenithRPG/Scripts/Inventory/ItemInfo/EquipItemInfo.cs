using UnityEngine;

namespace DC_ARPG
{
    public enum EquipItemType
    {
        Armor,
        Shield
    }

    [CreateAssetMenu(fileName = "EquipItemInfo", menuName = "ScriptableObjects/ItemInfo/EquipItemInfo")]
    public class EquipItemInfo : ItemInfo
    {
        [Header("EquipItem")]
        public EquipItemType EquipType;
        public int DefenseIncrease;
    }
}
