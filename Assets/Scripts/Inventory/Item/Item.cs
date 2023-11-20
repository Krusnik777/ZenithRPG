using UnityEngine;

namespace DC_ARPG
{
    public abstract class Item<T> : MonoBehaviour where T: ItemInfo
    {
        [SerializeField] protected T m_itemInfo;
    }
}
