using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class ButtonPlateAnimator : TwoStateAnimator
    {
        [SerializeField] private Transform m_plate;
        [SerializeField] private ParticleSystem m_dust;
        [SerializeField] private float m_defaultPosition = 0.025f;
        [SerializeField] private float m_targetPosition = -0.025f;
        [SerializeField] private float m_speed = 180f;

        public override bool InActiveState => activated && !inProgress;
        public override bool InInitState => !activated && !inProgress;

        private bool activated;
        private bool inProgress;

        public void ChangeInitialState(bool active)
        {
            if (active)
            {
                m_plate.transform.localPosition = new Vector3(0, m_targetPosition, 0);
            }
            else
            {
                m_plate.transform.localPosition = new Vector3(0, m_defaultPosition, 0);
            }

            activated = active;
        }

        public override void Play()
        {
            if (!InInitState) return;

            inProgress = true;

            StartCoroutine(Press());
        }

        public override void ResetToInit()
        {
            if (!InActiveState) return;

            inProgress = true;

            StartCoroutine(Unpress());
        }

        private IEnumerator Press()
        {
            var targetPosition = new Vector3(0, m_targetPosition, 0);

            while (m_plate.transform.localPosition != targetPosition)
            {
                m_plate.transform.localPosition = Vector3.MoveTowards(m_plate.transform.localPosition, targetPosition, Time.deltaTime * m_speed);
                yield return null;
            }

            m_dust.Play();

            activated = true;
            inProgress = false;
        }

        private IEnumerator Unpress()
        {
            var targetPosition = new Vector3(0, m_defaultPosition, 0);

            while (m_plate.transform.localPosition != targetPosition)
            {
                m_plate.transform.localPosition = Vector3.MoveTowards(m_plate.transform.localPosition, targetPosition, Time.deltaTime * m_speed);
                yield return null;
            }

            m_dust.Play();

            activated = false;
            inProgress = false;
        }
    }
}
