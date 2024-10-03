using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public event UnityAction EventOnHit;
        public abstract CharacterStats Stats { get; }

        protected void OnHit()
        {
            EventOnHit?.Invoke();
        }
    }
}
