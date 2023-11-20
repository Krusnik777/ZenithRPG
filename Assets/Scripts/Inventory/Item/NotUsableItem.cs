using UnityEngine;

namespace DC_ARPG
{
    public class NotUsableItem : Item<NotUsableItemInfo>
    {
        [SerializeField] private bool m_state; // in Main Bag or not
    }
}
