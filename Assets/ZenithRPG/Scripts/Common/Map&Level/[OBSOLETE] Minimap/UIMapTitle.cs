using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class UIMapTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_titleText;

        private void Start()
        {
            m_titleText.text = SceneCommander.Instance.GetCurrentLevelTitle();
        }
    }
}
