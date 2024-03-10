using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class ShowObject : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_targets;
        [SerializeField] private Camera m_camera;
        [SerializeField] private GameObject m_appearEffectPrefab;
        [SerializeField] private float m_showTime = 2.0f;

        public void ShowTarget()
        {
            StartCoroutine(SimpleShowRoutine());
        }

        public void ShowSingleTargetAppear()
        {
            if (m_targets[0].activeInHierarchy) return;

            StartCoroutine(AppearSingleRoutine());
        }

        public void ShowMultipleTargetsAppear()
        {
            if (m_targets[0].activeInHierarchy) return;

            StartCoroutine(AppearMultipleRoutine());
        }

        #region Coroutines

        private IEnumerator SimpleShowRoutine()
        {
            StoryEventManager.Instance.StartMicroEvent();

            if (PlayerCamera.Instance != null)
                PlayerCamera.Instance.Camera.SetActive(false);

            m_camera.gameObject.SetActive(true);

            yield return new WaitForSeconds(m_showTime);

            StoryEventManager.Instance.EndMicroEvent();

            m_camera.gameObject.SetActive(false);

            if (PlayerCamera.Instance != null)
                PlayerCamera.Instance.Camera.SetActive(true);
        }

        private IEnumerator AppearSingleRoutine()
        {
            StoryEventManager.Instance.StartMicroEvent();

            yield return new WaitForSeconds(0.5f);

            if (PlayerCamera.Instance != null)
                PlayerCamera.Instance.Camera.SetActive(false);

            m_camera.gameObject.SetActive(true);

            if (m_appearEffectPrefab != null)
            {
                var effect = Instantiate(m_appearEffectPrefab, m_targets[0].transform.position, Quaternion.identity);

                Destroy(effect, m_showTime);
            }

            yield return new WaitForSeconds(1.0f);

            m_targets[0].SetActive(true);

            yield return new WaitForSeconds(m_showTime);

            StoryEventManager.Instance.EndMicroEvent();

            m_camera.gameObject.SetActive(false);

            if (PlayerCamera.Instance != null)
                PlayerCamera.Instance.Camera.SetActive(true);
        }

        private IEnumerator AppearMultipleRoutine()
        {
            StoryEventManager.Instance.StartMicroEvent();

            yield return new WaitForSeconds(0.5f);

            if (PlayerCamera.Instance != null)
                PlayerCamera.Instance.Camera.SetActive(false);

            m_camera.gameObject.SetActive(true);

            if (m_appearEffectPrefab != null)
            {
                foreach(var target in m_targets)
                {
                    var effect = Instantiate(m_appearEffectPrefab, target.transform.position, Quaternion.identity);

                    Destroy(effect, m_showTime);
                }
            }

            yield return new WaitForSeconds(1.0f);

            foreach (var target in m_targets)
            {
                target.SetActive(true);
            }

            yield return new WaitForSeconds(m_showTime);

            StoryEventManager.Instance.EndMicroEvent();

            m_camera.gameObject.SetActive(false);

            if (PlayerCamera.Instance != null)
                PlayerCamera.Instance.Camera.SetActive(true);
        }

        #endregion
    }
}
