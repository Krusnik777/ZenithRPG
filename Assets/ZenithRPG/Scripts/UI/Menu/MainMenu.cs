using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private UISelectableButtonContainer m_loadButtons;
        [SerializeField] private SaveSlotsMenu m_saveSlotsMenu;
        [SerializeField] private UISelectableButtonContainer m_settingsButtons;
        [SerializeField] private ConfirmPanel m_confirmPanel;
        [Header("CreditsPanel")]
        [SerializeField] private GameObject m_creditsPanel;
        [SerializeField] private Scrollbar m_scrollbar;
        [SerializeField] private float m_scrollStep = 0.1f;

        public UISelectableButtonContainer ActiveButtonContainer { get; private set; }

        private MenuState m_menuState;
        public MenuState State => m_menuState;

        private Coroutine startPanelRoutine;

        public bool StartPanelDisappearing => startPanelRoutine != null;

        public void StartNewGame()
        {
            SceneCommander.Instance.StartLevel("Level_01");
        }

        public void ShowLoadMenu(bool state)
        {
            m_baseButtons.SetInteractable(!state);
            m_loadButtons.gameObject.SetActive(state);

            if (state)
            {
                m_menuState = MenuState.Load;
                ActiveButtonContainer = m_loadButtons;
            }
            else
            {
                m_menuState = MenuState.Selection;
                ActiveButtonContainer = m_baseButtons;
            }
        }

        public void StartTutorial()
        {
            SceneCommander.Instance.StartTutorialLevel();
        }

        public void ShowSettings(bool state)
        {
            m_baseButtons.SetInteractable(!state);
            m_settingsButtons.gameObject.SetActive(state);

            if (state)
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

        public void ShowCredits(bool state)
        {
            m_baseButtons.SetInteractable(!state);
            m_creditsPanel.SetActive(state);

            if (state)
            {
                m_scrollbar.value = 1.0f;
                m_menuState = MenuState.Credits;
            }
            else
            {
                m_menuState = MenuState.Selection;
                ActiveButtonContainer = m_baseButtons;
            }
        }

        public void ScrollCredits(float input)
        {
            if (input == 0) return;

            if (input < 0 && m_scrollbar.value <= 0) return;
            if (input > 0 && m_scrollbar.value >= 1) return;

            m_scrollbar.value += input * m_scrollStep;

            if (m_scrollbar.value < 0) m_scrollbar.value = 0;
            if (m_scrollbar.value > 1) m_scrollbar.value = 1;
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

            m_saveSlotsMenu.SetState(SaveSlotsMenu.MenuState.Load);
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
