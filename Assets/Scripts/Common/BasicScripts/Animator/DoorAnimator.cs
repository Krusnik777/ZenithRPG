using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class DoorAnimator : TwoStateAnimator
    {
        [SerializeField] private Transform m_leftDoor;
        [SerializeField] private Transform m_rightDoor;
        [SerializeField] private float m_defaultAngle = 0f;
        [SerializeField] private float m_targetAngleL = -90f;
        [SerializeField] private float m_targetAngleR = 90f;
        [SerializeField] private float m_speed = 200f;

        public override bool InActiveState => activated && !inProgress;
        public override bool InInitState => !activated && !inProgress;

        private bool activated;
        private bool inProgress;

        public void ChangeInitialState(bool active)
        {
            if (active)
            {
                m_leftDoor.transform.localRotation = Quaternion.Euler(0, m_targetAngleL, 0);
                m_rightDoor.transform.localRotation = Quaternion.Euler(0, m_targetAngleR, 0);

            }
            else
            {
                m_leftDoor.transform.localRotation = Quaternion.Euler(0, m_defaultAngle, 0);
                m_rightDoor.transform.localRotation = Quaternion.Euler(0, m_defaultAngle, 0);
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
            var leftTargetRotation = Quaternion.Euler(0, m_targetAngleL, 0);
            var rightTargetRotation = Quaternion.Euler(0, m_targetAngleR, 0);

            while (m_leftDoor.transform.localRotation != leftTargetRotation && m_rightDoor.transform.localRotation != rightTargetRotation)
            {
                m_leftDoor.transform.localRotation = Quaternion.RotateTowards(m_leftDoor.transform.localRotation, leftTargetRotation, Time.deltaTime * m_speed);
                m_rightDoor.transform.localRotation = Quaternion.RotateTowards(m_rightDoor.transform.localRotation, rightTargetRotation, Time.deltaTime * m_speed);
                yield return null;
            }

            activated = true;
            inProgress = false;
        }

        private IEnumerator Close()
        {
            var targetRotation = Quaternion.Euler(0, m_defaultAngle, 0);

            while (m_leftDoor.transform.localRotation != targetRotation && m_rightDoor.transform.localRotation != targetRotation)
            {
                m_leftDoor.transform.localRotation = Quaternion.RotateTowards(m_leftDoor.transform.localRotation, targetRotation, Time.deltaTime * m_speed);
                m_rightDoor.transform.localRotation = Quaternion.RotateTowards(m_rightDoor.transform.localRotation, targetRotation, Time.deltaTime * m_speed);
                yield return null;
            }

            activated = false;
            inProgress = false;
        }
    }
}
