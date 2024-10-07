using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class RestartMenu : MonoBehaviour, IDependency<ControlsManager>
    {
        public enum MenuState
        {
            Default,
            Load
        }

        [SerializeField] private PlayerCharacter m_playerCharacter;
        [SerializeField] private GameObject m_panel;
        [SerializeField] private UISelectableButtonContainer m_buttons;
        [Header("Buttons")]
        [SerializeField] private GameObject m_restartFromCheckpointButton;
        [SerializeField] private GameObject m_loadButton;
        [Header("LoadMenu")]
        [SerializeField] private UISelectableButtonContainer m_loadButtons;
        [SerializeField] private SaveSlotsMenu m_saveSlotsMenu;

        private MenuState m_state;
        public MenuState State => m_state;

        public UISelectableButtonContainer ActiveButtonContainer { get; private set; }

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        public void RestartFromCheckpoint() => SceneCommander.Instance.RestartCurrentLevelFromCheckpoint();
        public void RestartFromStart() => SceneCommander.Instance.RestartCurrentLevelFromStart();

        public void ReturnToMainMenu() => SceneCommander.Instance.ReturnToMainMenu();

        public void ShowPanel()
        {
            m_controlsManager.TurnOffAllControls();
            m_controlsManager.SetSimpleMenuControlsActive(true);

            SetupButtonPanel();

            m_panel.SetActive(true);

            LevelState.Instance.StopAllActivity();

            m_state = MenuState.Default;
            ActiveButtonContainer = m_buttons;
        }

        public void ShowLoadMenu(bool state)
        {
            m_buttons.SetInteractable(!state);
            m_loadButtons.gameObject.SetActive(state);
            m_saveSlotsMenu.SetState(SaveSlotsMenu.MenuState.Load);

            if (state)
            {
                m_state = MenuState.Load;
                ActiveButtonContainer = m_loadButtons;
            }
            else
            {
                m_state = MenuState.Default;
                ActiveButtonContainer = m_buttons;
            }
        }

        private void SetupButtonPanel()
        {
            m_restartFromCheckpointButton.SetActive(SceneSerializer.Instance.CheckCheckpointExists());

            m_loadButton.SetActive(DataPersistenceManager.Instance.CheckAnySaveFilesExists());
        }

        private void Start()
        {
            m_playerCharacter.EventOnDeath.AddListener(ShowPanel);
        }

        private void OnDestroy()
        {
            m_playerCharacter.EventOnDeath.RemoveListener(ShowPanel);
        }
    }
}
