using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class MainMenu : MonoBehaviour
    {
        public enum MenuState
        {
            Start,
            Selection,
            Load,
            Settings,
            Credits,
            Quit
        }

        [SerializeField] private GameObject m_startPanel;
        [SerializeField] private UISelectableButtonContainer m_baseButtons;
        [SerializeField] private UISelectableButtonContainer m_settingsButtons;
        [SerializeField] private ConfirmPanel m_confirmPanel;

        public UISelectableButtonContainer ActiveButtonContainer { get; private set; }

        private MenuState m_menuState;
        public MenuState State => m_menuState;

        private Coroutine startPanelRoutine;

        public void StartNewGame()
        {

        }

        public void ShowLoadMenu(bool state)
        {

        }

        public void StartTutorial()
        {
            SceneCommander.Instance.StartTutorialLevel();
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

        public void ShowConfirmQuitGamePanel()
        {
            m_baseButtons.SetInteractable(false);

            m_confirmPanel.ShowConfirmPanel(ConfirmPanel.ConfirmType.QuitGame);
            ActiveButtonContainer = m_confirmPanel.ButtonContainer;

            m_menuState = MenuState.Quit;
        }

        public void HideConfirmPanel()
        {
            m_confirmPanel.HideConfirmPanel();

            m_baseButtons.SetInteractable(true);
            ActiveButtonContainer = m_baseButtons;

            m_menuState = MenuState.Selection;
        }

        public void TurnOffStartPanel()
        {
            if (startPanelRoutine != null) return;

            startPanelRoutine = StartCoroutine(StartPanelRoutine());
        }

        private void Start()
        {
            ActiveButtonContainer = m_baseButtons;

            m_menuState = MenuState.Start;
        }

        #region Coroutines

        private IEnumerator StartPanelRoutine()
        {
            var animator = m_startPanel.GetComponent<Animator>();

            animator.SetTrigger("Disappear");

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Disappear"));

            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Disappear") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99);

            m_startPanel.SetActive(false);

            MusicCommander.Instance.PlayMainMenuTheme();

            m_menuState = MenuState.Selection;
        }

        #endregion

    }
}
