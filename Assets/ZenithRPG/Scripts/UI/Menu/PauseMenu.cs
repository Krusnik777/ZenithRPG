using UnityEngine;

namespace DC_ARPG
{
    public class PauseMenu : MonoSingleton<PauseMenu>, IDependency<ControlsManager>
    {
        public enum MenuState
        {
            Selection,
            Status,
            Map,
            Settings,
            Data,
            SaveMenu,
            Quit
        }

        public enum ConfirmationState
        {
            None,
            Require
        }

        [SerializeField] private PlayerCharacter m_playerCharacter;
        [Header("Panels")]
        [SerializeField] private GameObject m_pauseMenuPanel;
        [SerializeField] private StatusPanel m_statusPanel;
        [SerializeField] private GameObject m_mapPanel;
        [Header("ButtonContainers")]
        [SerializeField] private UISelectableButtonContainer m_baseButtons;
        [SerializeField] private UISelectableButtonContainer m_settingsButtons;
        [SerializeField] private UISelectableButtonContainer m_dataButtons;
        [SerializeField] private UISelectableButtonContainer m_quitButtons;
        [SerializeField] private ConfirmPanel m_confirmPanel;
        [Header("SaveLoad")]
        [SerializeField] private UISelectableButtonContainer m_saveloadButtons;
        [SerializeField] private SaveSlotsMenu m_saveSlotsMenu;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        public UISelectableButtonContainer ActiveButtonContainer { get; private set; }

        private MenuState m_menuState;
        public MenuState State => m_menuState;

        private ConfirmationState m_confirmationState;
        public ConfirmationState RequireConfirm => m_confirmationState;

        public void ShowPauseMenu()
        {
            m_controlsManager.SetPlayerControlsActive(false);
            m_controlsManager.SetMenuControlsActive(true);

            m_pauseMenuPanel.SetActive(true);

            ActiveButtonContainer = m_baseButtons;

            m_menuState = MenuState.Selection;

            LevelState.Instance.StopAllActivity();
        }

        public void ShowStatus(bool state)
        {
            m_statusPanel.gameObject.SetActive(state);

            if (state == true)
            {
                m_statusPanel.UpdateStatus(m_playerCharacter);
                m_menuState = MenuState.Status;
            }
            else m_menuState = MenuState.Selection;
        }

        public void ShowMap(bool state)
        {
            m_mapPanel.SetActive(state);

            if (state == true) m_menuState = MenuState.Map;
            else m_menuState = MenuState.Selection;
        }

        public void ShowSettings(bool state)
        {
            m_baseButtons.SetInteractable(!state);
            m_settingsButtons.gameObject.SetActive(state);

            if (state == true)
            {
                m_menuState = MenuState.Settings;
                ActiveButtonContainer = m_settingsButtons;
            }
            else
            {
                m_menuState = MenuState.Selection;
                ActiveButtonContainer = m_baseButtons;
            }
        }

        public void ShowDataButtons(bool state)
        {
            m_baseButtons.SetInteractable(!state);
            m_dataButtons.gameObject.SetActive(state);

            if (state == true)
            {
                m_menuState = MenuState.Data;
                ActiveButtonContainer = m_dataButtons;
            }
            else
            {
                m_menuState = MenuState.Selection;
                ActiveButtonContainer = m_baseButtons;
            }
        }

        public void ShowSaveMenu()
        {
            ShowSaveLoadMenu(SaveSlotsMenu.MenuState.Save);
        }

        public void ShowLoadMenu()
        {
            ShowSaveLoadMenu(SaveSlotsMenu.MenuState.Load);
        }

        public void ShowEraseMenu()
        {
            ShowSaveLoadMenu(SaveSlotsMenu.MenuState.Erase);
        }

        public void HideSaveLoadMenu()
        {
            m_saveloadButtons.gameObject.SetActive(false);
            m_dataButtons.SetInteractable(true);

            m_menuState = MenuState.Data;
            ActiveButtonContainer = m_dataButtons;
        }

        public void ShowQuitButtons(bool state)
        {
            m_baseButtons.SetInteractable(!state);
            m_quitButtons.gameObject.SetActive(state);

            if (state == true)
            {
                m_menuState = MenuState.Quit;
                ActiveButtonContainer = m_quitButtons;
            }
            else
            {
                m_menuState = MenuState.Selection;
                ActiveButtonContainer = m_baseButtons;
            }
        }

        public void ShowConfirmReturnToMainMenuPanel()
        {
            m_quitButtons.SetInteractable(false);

            m_confirmPanel.ShowConfirmPanel(ConfirmPanel.ConfirmType.ReturnToMainMenu);
            m_confirmationState = ConfirmationState.Require;
            ActiveButtonContainer = m_confirmPanel.ButtonContainer;
        }

        public void ShowConfirmQuitGamePanel()
        {
            m_quitButtons.SetInteractable(false);

            m_confirmPanel.ShowConfirmPanel(ConfirmPanel.ConfirmType.QuitGame);
            m_confirmationState = ConfirmationState.Require;
            ActiveButtonContainer = m_confirmPanel.ButtonContainer;
        }

        public void HideConfirmPanel()
        {
            m_confirmPanel.HideConfirmPanel();
            m_confirmationState = ConfirmationState.None;

            m_quitButtons.SetInteractable(true);
            ActiveButtonContainer = m_quitButtons;
        }

        public void HidePauseMenu()
        {
            m_pauseMenuPanel.SetActive(false);

            m_controlsManager.SetMenuControlsActive(false);
            m_controlsManager.SetPlayerControlsActive(true);

            LevelState.Instance.ResumeAllActivity();
        }
        private void ShowSaveLoadMenu(SaveSlotsMenu.MenuState saveSlotMenuState)
        {
            m_dataButtons.SetInteractable(false);
            m_saveloadButtons.gameObject.SetActive(true);
            m_saveSlotsMenu.SetState(saveSlotMenuState);

            m_menuState = MenuState.SaveMenu;
            ActiveButtonContainer = m_saveloadButtons;
        }
    }
}
