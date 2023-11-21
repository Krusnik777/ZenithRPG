using UnityEngine;

namespace DC_ARPG
{
    public abstract class Item : MonoBehaviour
    {
        public virtual ItemInfo Info { get;}
        public virtual int Amount => 1;
        public virtual int MaxAmount => 1;
    }
}
