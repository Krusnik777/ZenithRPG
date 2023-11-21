using UnityEngine;

namespace DC_ARPG
{
    public class MagicItem : Item
    {
        [SerializeField] protected MagicItemInfo m_itemInfo;

        [SerializeField] private int m_uses;
        [SerializeField] private bool m_isEquipped;
        public override ItemInfo Info => m_itemInfo;
        public int Uses => m_uses;
        public bool IsEquipped => m_isEquipped;

        public MagicItem(MagicItemInfo info, int uses, bool isEquipped)
        {
            m_itemInfo = info;
            m_uses = uses;
            m_isEquipped = isEquipped;
        }

        public MagicItem Clone()
        {
            var clonedItem = new MagicItem(m_itemInfo, m_uses, m_isEquipped);
            return clonedItem;
        }
    }
}
