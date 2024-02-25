using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(AudioSource))]
    public class SpikeTrap : DamageZone
    {
        [SerializeField] private Animator m_animator;

        private AudioSource m_audioSource;

        private bool activated;

        private Collider enteredCollider;

        private void SetEnteredCollider(Collider other) => enteredCollider = other;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (activated) return;

            CheckForPlayer(other);

            if (other.transform.parent.TryGetComponent(out Enemy enemy))
            {
                if (enemy.EnemyAI.State == EnemyState.Patrol) return;

                enemy.Character.EnemyStats.ChangeCurrentHitPoints(this, -m_damage);

                activated = true;

                SetEnteredCollider(other);
            }

            SetAnimationAndSound();
        }

        private void OnTriggerStay(Collider other)
        {
            if (activated) return;

            CheckForPlayer(other);

            SetAnimationAndSound();
        }

        private void OnTriggerExit(Collider other)
        {
            if (activated && other == enteredCollider)
            {
                if (enteredCollider.transform.parent.TryGetComponent(out Player player))
                    player.RestIsAvailable = true;

                activated = false;
                enteredCollider = null;
            }
        }

        private void CheckForPlayer(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Player player))
            {
                if (player.IsJumping && !player.JumpedAndLanded) return;

                player.Character.PlayerStats.ChangeCurrentHitPoints(this, -m_damage);
                player.RestIsAvailable = false;

                activated = true;

                SetEnteredCollider(other);
            }
        }

        private void SetAnimationAndSound()
        {
            if (activated)
            {
                m_animator.SetTrigger("Activate");
                m_audioSource.Play();
            }
        }
    }
}
