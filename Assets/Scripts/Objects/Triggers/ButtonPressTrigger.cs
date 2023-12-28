using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class ButtonPressTrigger : MonoBehaviour
    {
        [SerializeField] private bool m_hasSpring;

        private Animator m_animator;

        public UnityEvent OnButtonPressed;
        public UnityEvent OnButtonUnpressed;

        private bool inPressedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("PressedState") : false;
        private bool inUnpressedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("UnpressedState") : true;

        public void PressButton()
        {
            if (inUnpressedState)
            {
                m_animator.SetTrigger("Press");
                OnButtonPressed?.Invoke();
            }
        }

        public void UnpressButton()
        {
            if (inPressedState)
            {
                m_animator.SetTrigger("Unpress");
                OnButtonUnpressed?.Invoke();
            }
        }

        private void Start()
        {
            m_animator = GetComponentInParent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Enemy enemy))
            {
                if (enemy.State == EnemyState.Patrol) return;
            }

            PressButton();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.TryGetComponent(out Enemy enemy))
            {
                if (enemy.State == EnemyState.Patrol) return;
            }

            if (m_hasSpring) UnpressButton();
        }
    }
}
