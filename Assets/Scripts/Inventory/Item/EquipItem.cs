using UnityEngine;

namespace DC_ARPG
{
    public class EquipItem : Item<EquipItemInfo>
    {
        public enum EquipItemType
        {
            Armor,
            Shield
        }

        [SerializeField] private EquipItemType m_type;
        [SerializeField] private bool m_isEquipped;
    }
}
