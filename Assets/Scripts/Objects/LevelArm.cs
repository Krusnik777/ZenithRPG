using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(Animator))]
    public class LevelArm : InspectableObject
    {
        [SerializeField] private bool m_canReset;
        public bool CanReset => m_canReset;

        public UnityEvent OnUsed;
        public UnityEvent OnReseted;

        private Animator m_animator;

        private bool inUpperState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("UpperState") : true;
        public bool Unused => inUpperState;
        private bool inLoweredState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("LoweredState") : false;

        public override void OnInspection(Player player)
        {
            if (inUpperState) UseLevelArm();
            if (inLoweredState) ResetLevelArm();

            EventOnInspection?.Invoke();
        }

        private void UseLevelArm()
        {
            m_animator.SetTrigger("Use");
            OnUsed?.Invoke();
        }

        private void ResetLevelArm()
        {
            if (!m_canReset) return;

            m_animator.SetTrigger("Reset");
            OnReseted?.Invoke();
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

    }
}
