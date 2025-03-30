using UnityEngine;

namespace DC_ARPG
{
    public abstract class UseEffect : ScriptableObject
    {
        [SerializeField] protected AudioClip m_useSound;

        public abstract void Use(IItem item);
    }
}
