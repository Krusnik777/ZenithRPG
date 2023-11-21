using UnityEngine;

namespace DC_ARPG
{
    public class NotUsableItem : Item
    {
        [SerializeField] protected NotUsableItemInfo m_itemInfo;

        [SerializeField] private bool m_isActive;
        public override ItemInfo Info => m_itemInfo;
        public bool IsActive => m_isActive;

        public NotUsableItem(NotUsableItemInfo info, bool isActive)
        {
            m_itemInfo = info;
            m_isActive = isActive;
        }

        public NotUsableItem Clone()
        {
            var clonedItem = new NotUsableItem(m_itemInfo, m_isActive);
            return clonedItem;
        }
    }
}
