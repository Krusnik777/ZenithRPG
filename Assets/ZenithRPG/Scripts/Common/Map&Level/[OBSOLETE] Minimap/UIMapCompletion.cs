using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class UIMapCompletion : MonoBehaviour
    {
        //[SerializeField] private MinimapIconCollector m_minimapIconCollector;
        [SerializeField] private TextMeshProUGUI m_percentText;

        private void OnEnable()
        {
            // TEMP - TO DO: Get Map that opened in menu

            if (LevelState.Instance == null) return;

            m_percentText.text = LevelState.Instance.CurrentMap.GetMapCompletionPercent().ToString("F1") + "%";
        }
    }
}
