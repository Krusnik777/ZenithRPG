using UnityEngine;

namespace DC_ARPG
{
    public class UsableItem : Item<UsableItemInfo>
    {
        [SerializeField] private int m_amount;
    }
}
