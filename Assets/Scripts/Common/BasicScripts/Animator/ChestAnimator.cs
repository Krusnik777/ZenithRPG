using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class ChestAnimator : TwoStateAnimator
    {
        [SerializeField] private Transform m_armature;
        [SerializeField] private float m_defaultAngle = -90f;
        [SerializeField] private float m_targetAngle = -215f;
        [SerializeField] private float m_speed = 400f;

        public override bool InActiveState => activated && !inProgress;
        public override bool InInitState => !activated && !inProgress;

        private bool activated;
        private bool inProgress;

        public void ChangeInitialState(bool active)
        {
            if (active)
            {
                m_armature.transform.localRotation = Quaternion.Euler(m_targetAngle, 0, 0);
            }
            else
            {
                m_armature.transform.localRotation = Quaternion.Euler(m_defaultAngle, 0, 0);
            }

            activated = active;
        }

        public override void Play()
        {
            if (!InInitState) return;

            inProgress = true;

            StartCoroutine(Open());
        }

        public override void ResetToInit()
        {
            if (!InActiveState) return;

            inProgress = true;

            StartCoroutine(Close());
        }

        private IEnumerator Open()
        {
            var targetRotation = Quaternion.Euler(m_targetAngle, 0, 0);

            while (m_armature.transform.localRotation != targetRotation)
            {
                m_armature.transform.localRotation = Quaternion.RotateTowards(m_armature.transform.localRotation, targetRotation, Time.deltaTime * m_speed);
                yield return null;
            }

            activated = true;
            inProgress = false;
        }

        private IEnumerator Close()
        {
            var targetRotation = Quaternion.Euler(m_defaultAngle, 0, 0);

            while (m_armature.transform.localRotation != targetRotation)
            {
                m_armature.transform.localRotation = Quaternion.RotateTowards(m_armature.transform.localRotation, targetRotation, Time.deltaTime * m_speed);
                yield return null;
            }

            activated = false;
            inProgress = false;
        }
    }
}
