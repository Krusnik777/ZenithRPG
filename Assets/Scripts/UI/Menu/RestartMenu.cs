using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class RestartMenu : MonoBehaviour, IDependency<ControlsManager>
    {
        [SerializeField] private GameObject m_panel;
        [SerializeField] private UISelectableButtonContainer m_buttons;
        [Header("Buttons")]
        [SerializeField] private GameObject m_restartFromCheckpointButton;
        [SerializeField] private GameObject m_loadButton;

        public UISelectableButtonContainer ActiveButtonContainer => m_buttons;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        public void RestartFromCheckpoint() => SceneSerializer.Instance.Restart();
        public void RestartFromStart() => SceneSerializer.Instance.RestartFromStart();

        public void ShowPanel()
        {
            m_controlsManager.SetPlayerControlsActive(false);
            m_controlsManager.SetSimpleMenuControlsActive(true);

            SetupButtonPanel();

            m_panel.SetActive(true);

            LevelState.Instance.StopAllActivity();
        }

        private void SetupButtonPanel()
        {
            m_restartFromCheckpointButton.SetActive(SceneSerializer.Instance.CheckSaveExists());
            if (DataPersistenceManager.Instance != null)
                m_loadButton.SetActive(DataPersistenceManager.Instance.CheckSaveForCurrenProfileExists());
            else m_loadButton.SetActive(false);
        }
    }
}
