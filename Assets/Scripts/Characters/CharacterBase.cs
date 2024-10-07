using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public event UnityAction EventOnHit;
        public UnityEvent EventOnDeath;
        public abstract CharacterStats Stats { get; }

        public abstract void DamageOpponent(CharacterAvatar opponent);

        protected void OnHit()
        {
            EventOnHit?.Invoke();
        }

        protected void OnDead()
        {
            EventOnDeath?.Invoke();
        }
    }
}
