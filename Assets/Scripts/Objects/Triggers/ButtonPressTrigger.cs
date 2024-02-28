using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class ButtonPressTrigger : MonoBehaviour
    {
        [SerializeField] private bool m_hasSpring;
        [SerializeField] private AudioSource m_audioSFX;
        [SerializeField] private Animator m_animator;
        public bool HasSpring => m_hasSpring;
        public Animator Animator => m_animator;

        public UnityEvent OnButtonPressed;
        public UnityEvent OnButtonUnpressed;

        private bool inPressedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("PressedState");
        private bool inUnpressedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("UnpressedState");

        public void PressButton()
        {
            if (inUnpressedState)
            {
                m_animator.SetTrigger("Press");
                m_audioSFX.Play();
                OnButtonPressed?.Invoke();
            }
        }

        public void UnpressButton()
        {
            if (inPressedState)
            {
                m_animator.SetTrigger("Unpress");
                m_audioSFX.Play();
                OnButtonUnpressed?.Invoke();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Enemy enemy))
            {
                if (enemy.EnemyAI.State == EnemyState.Patrol) return;

                PressButton();
            }

            if (other.transform.parent.TryGetComponent(out Player player))
            {
                if (player.IsJumping && !player.JumpedAndLanded) return;

                player.ActionsIsAvailable = false;

                PressButton();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Enemy enemy))
            {
                if (enemy.EnemyAI.State == EnemyState.Patrol) return;

                if (m_hasSpring) UnpressButton();
            }

            if (other.transform.parent.TryGetComponent(out Player player))
            {
                if (m_hasSpring) UnpressButton();
                player.ActionsIsAvailable = true;
            }

        }
    }
}
