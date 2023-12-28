using UnityEngine;

namespace DC_ARPG
{
    public class SpikeTrap : DamageZone
    {
        [SerializeField] private Animator m_animator;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            m_animator.SetTrigger("Activate");
        }
    }
}
