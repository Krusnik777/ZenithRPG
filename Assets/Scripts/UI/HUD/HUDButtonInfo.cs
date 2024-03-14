using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class HUDButtonInfo : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private GameObject m_buttonInfoPanel;
        [SerializeField] private TextMeshProUGUI m_useText;

        private void LateUpdate()
        {
            UpdateButtonInfo(m_player.CheckForwardGridForInsectableObject());
        }

        private void UpdateButtonInfo(InspectableObject inspectableObject)
        {
            if (inspectableObject != null && GameState.State == GameState.GameplayState.Active)
            {
                if (inspectableObject.InfoText != string.Empty)
                {
                    m_buttonInfoPanel.SetActive(true);
                    m_useText.text = inspectableObject.InfoText;

                    return;
                }

                m_buttonInfoPanel.SetActive(false);
                m_useText.text = "Использовать";
            }
            else
            {
                m_buttonInfoPanel.SetActive(false);
            }
        }
    }
}
