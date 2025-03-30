using UnityEngine;

namespace DC_ARPG
{
    public class CheckpointMarker : MonoBehaviour
    {
        [SerializeField] private GameObject m_panel;

        private void Start()
        {
            SceneSerializer.Instance.EventOnSaved += OnSaved;
        }

        private void OnDestroy()
        {
            SceneSerializer.Instance.EventOnSaved -= OnSaved;
        }

        private void OnSaved()
        {
            m_panel.SetActive(true);

            CancelInvoke("TurnOff");
            Invoke("TurnOff", 1.1f);
        }

        private void TurnOff()
        {
            m_panel.SetActive(false);
        }
    }
}
