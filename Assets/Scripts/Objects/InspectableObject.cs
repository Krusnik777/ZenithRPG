using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public abstract class InspectableObject : MonoBehaviour
    {
        public UnityEvent EventOnInspection;
        public virtual void OnInspection(Player player)
        {
            EventOnInspection?.Invoke();
        }
    }
}
