using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public event UnityAction EventOnHit;

        protected void OnHit()
        {
            EventOnHit?.Invoke();
        }
    }
}
