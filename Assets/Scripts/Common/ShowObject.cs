using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class ShowObject : MonoBehaviour
    {
        [SerializeField] private GameObject m_gameObject;
        [SerializeField] private Camera m_camera;
        [SerializeField] private GameObject m_appearEffectPrefab;
        [SerializeField] private float m_showTime = 2.0f;
        [SerializeField] private bool m_isFollowup;

        public void Show()
        {
            if (m_gameObject.activeInHierarchy) return;

            StartCoroutine(AppearRoutine());
        }

        #region Coroutines

        private IEnumerator AppearRoutine()
        {
            if (!m_isFollowup)
            {
                StoryEventManager.Instance.StartMicroEvent();

                yield return new WaitForSeconds(0.5f);
            }

            if (PlayerCamera.Instance != null)
                PlayerCamera.Instance.Camera.SetActive(false);

            m_camera.gameObject.SetActive(true);

            if (m_appearEffectPrefab != null)
            {
                var effect = Instantiate(m_appearEffectPrefab, m_gameObject.transform.position, Quaternion.identity);

                Destroy(effect, m_showTime);
            }

            yield return new WaitForSeconds(1.0f);

            m_gameObject.SetActive(true);

            yield return new WaitForSeconds(m_showTime);

            StoryEventManager.Instance.EndMicroEvent();

            m_camera.gameObject.SetActive(false);

            if (PlayerCamera.Instance != null)
                PlayerCamera.Instance.Camera.SetActive(true);
        }

        #endregion
    }
}
