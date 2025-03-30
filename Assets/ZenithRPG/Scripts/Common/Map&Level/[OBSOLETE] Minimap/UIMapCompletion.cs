using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class UIMapCompletion : MonoBehaviour
    {
        [SerializeField] private MinimapIconCollector m_minimapIconCollector;
        [SerializeField] private TextMeshProUGUI m_percentText;

        private void OnEnable()
        {
            if (m_minimapIconCollector != null)
                m_percentText.text = m_minimapIconCollector.GetMapCompletionPercent().ToString("F1") + "%";
        }
    }
}
