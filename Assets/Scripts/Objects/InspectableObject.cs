using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class InspectableObject : MonoBehaviour
    {
        public UnityEvent EventOnInspection;
        public virtual void OnInspection()
        {
            EventOnInspection?.Invoke();
        }
    }
}
