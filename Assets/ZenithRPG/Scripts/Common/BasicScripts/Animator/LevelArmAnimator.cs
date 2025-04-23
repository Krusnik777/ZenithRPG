using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class LevelArmAnimator : TwoStateAnimator
    {
        [SerializeField] private Transform m_cylinder;
        //[SerializeField] private ParticleSystem m_dust;
        [SerializeField] private float m_defaultAngle = 0f;
        [SerializeField] private float m_targetAngle = -80f;
        [SerializeField] private float m_speed = 180f;

        public override bool InActiveState => activated && !inProgress;
        public override bool InInitState => !activated && !inProgress;

        private bool activated;
        private bool inProgress;

        public void ChangeInitialState(bool active)
        {
            if (active)
            {
                m_cylinder.transform.localRotation = Quaternion.Euler(m_targetAngle, 0, 0);
            }
            else
            {
                m_cylinder.transform.localRotation = Quaternion.Euler(m_defaultAngle, 0, 0);
            }

            activated = active;
        }

        public override void Play()
        {
            if (!InInitState) return;

            inProgress = true;

            StartCoroutine(LowerArm());
        }

        public override void ResetToInit()
        {
            if (!InActiveState) return;

            inProgress = true;

            StartCoroutine(UpArm());
        }

        private IEnumerator LowerArm()
        {
            var targetRotation = Quaternion.Euler(m_targetAngle, 0, 0);

            //m_dust.Play();

            while (m_cylinder.transform.localRotation != targetRotation)
            {
                m_cylinder.transform.localRotation = Quaternion.RotateTowards(m_cylinder.transform.localRotation, targetRotation, Time.deltaTime * m_speed);
                yield return null;
            }

            activated = true;
            inProgress = false;
        }

        private IEnumerator UpArm()
        {
            var targetRotation = Quaternion.Euler(m_defaultAngle, 0, 0);

            //m_dust.Play();

            while (m_cylinder.transform.localRotation != targetRotation)
            {
                m_cylinder.transform.localRotation = Quaternion.RotateTowards(m_cylinder.transform.localRotation, targetRotation, Time.deltaTime * m_speed);
                yield return null;
            }

            activated = false;
            inProgress = false;
        }
    }
}
