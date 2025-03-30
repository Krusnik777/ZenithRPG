using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class ConfirmPanel : MonoBehaviour
    {
        public enum ConfirmType
        {
            ReturnToMainMenu,
            QuitGame
        }

        [SerializeField] private GameObject m_panel;
        [SerializeField] private TextMeshProUGUI m_title;

        private UISelectableButtonContainer m_buttonContainer;
        public UISelectableButtonContainer ButtonContainer => m_buttonContainer;

        private ConfirmType m_type;

        private void OnEnable()
        {
            m_buttonContainer = GetComponent<UISelectableButtonContainer>();
            m_buttonContainer.SetInteractable(false);
        }

        public void ShowConfirmPanel(ConfirmType type)
        {
            m_panel.SetActive(true);

            m_type = type;

            if (m_type == ConfirmType.ReturnToMainMenu)
                m_title.text = "Выход в главное меню";

            if (m_type == ConfirmType.QuitGame)
                m_title.text = "Выход из игры";

            m_buttonContainer.SetInteractable(true);
        }

        public void HideConfirmPanel()
        {
            m_panel.SetActive(false);

            m_buttonContainer.SetInteractable(false);
        }

        public void OnConfirm()
        {
            if (m_type == ConfirmType.ReturnToMainMenu)
            {
                SceneCommander.Instance.ReturnToMainMenu();
            }

            if (m_type == ConfirmType.QuitGame)
            {
                SceneCommander.Instance.ExitGame();
            }   
        }
    }
}
