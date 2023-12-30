using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(AudioSource))]
    public class SpikeTrap : DamageZone
    {
        [SerializeField] private Animator m_animator;

        private AudioSource m_audioSource;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            // Maybe TEMP

            if (other.transform.parent.TryGetComponent(out Enemy enemy))
            {
                if (enemy.State == EnemyState.Patrol) return;
            }

            //

            m_animator.SetTrigger("Activate");
            m_audioSource.Play();
        }
    }
}
