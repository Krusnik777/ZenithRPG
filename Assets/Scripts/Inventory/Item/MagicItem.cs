using UnityEngine;

namespace DC_ARPG
{
    public class MagicItem : Item<MagicItemInfo>
    {
        [SerializeField] private int m_uses;
        [SerializeField] private bool m_isEquipped;
    }
}
