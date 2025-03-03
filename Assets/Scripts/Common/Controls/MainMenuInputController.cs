using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class MainMenuInputController : MonoBehaviour, IDependency<ControlsManager>
    {
        [SerializeField] private MainMenu m_mainMenu;
        [SerializeField] private UIMainMenuSounds m_mainMenuSounds;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Controls _controls;

        private UISelectableButtonContainer m_buttonContainer => m_mainMenu.ActiveButtonContainer;

        private void OnEnable()
        {
            if (_controls == null) _controls = m_controlsManager.Controls;

            _controls.Menu.Enable();

            _controls.Menu.Confirm.performed += OnConfirm;
            _controls.Menu.Cancel.performed += OnCancel;
            _controls.Menu.CloseMenu.performed += OnStartPanel;

            _controls.Menu.Move.performed += OnMove;
            _controls.Menu.ChangeParameters.performed += OnChangeParameters;
        }

        private void OnDisable()
        {
            _controls.Menu.Confirm.performed -= OnConfirm;
            _controls.Menu.Cancel.performed -= OnCancel;
            _controls.Menu.CloseMenu.performed -= OnStartPanel;

            _controls.Menu.Move.performed -= OnMove;
            _controls.Menu.ChangeParameters.performed -= OnChangeParameters;

            _controls.Menu.Disable();
        }

        private void OnConfirm(InputAction.CallbackContext obj)
        {
            if (m_mainMenu.State == MainMenu.MenuState.Credits) return;

            if (m_mainMenu.State == MainMenu.MenuState.Start)
            {
                OnStartPanel(obj);
            }
            else
            {
                m_buttonContainer.SelectedButton.OnButtonClick();
            }  
        }

        private void OnCancel(InputAction.CallbackContext obj)
        {
            if (m_mainMenu.State == MainMenu.MenuState.Start || m_mainMenu.State == MainMenu.MenuState.Selection)
                return;

            m_mainMenuSounds.PlayBackSound();

            if (m_mainMenu.State == MainMenu.MenuState.Load)
            {
                m_mainMenu.ShowLoadMenu(false);
                return;
            }

            if (m_mainMenu.State == MainMenu.MenuState.Settings)
            {
                m_mainMenu.ShowSettings(false);
                return;
            }

            if (m_mainMenu.State == MainMenu.MenuState.Credits)
            {
                m_mainMenu.ShowCredits(false);
                return;
            }

            if (m_mainMenu.State == MainMenu.MenuState.Quit)
            {
                m_mainMenu.HideConfirmPanel();
                return;
            }
        }

        private void OnStartPanel(InputAction.CallbackContext obj)
        {
            if (m_mainMenu.State != MainMenu.MenuState.Start || m_mainMenu.StartPanelDisappearing) return;

            m_mainMenuSounds.PlayStartPressedSound();

            m_mainMenu.TurnOffStartPanel();
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            if (m_mainMenu.State == MainMenu.MenuState.Start || m_mainMenu.State == MainMenu.MenuState.Quit) return;

            var value = _controls.Menu.Move.ReadValue<float>();

            if (m_mainMenu.State == MainMenu.MenuState.Credits)
            {
                m_mainMenu.ScrollCredits(value);
            }
            else
            {
                if (value == 1) m_buttonContainer.SelectPrevious();
                if (value == -1) m_buttonContainer.SelectNext();
            }
        }
        private void OnChangeParameters(InputAction.CallbackContext obj)
        {
            var value = _controls.Menu.ChangeParameters.ReadValue<float>();

            if (m_mainMenu.State == MainMenu.MenuState.Settings)
            {
                if (value == 1)
                {
                    if (m_buttonContainer.SelectedButton is UISettingButton) (m_buttonContainer.SelectedButton as UISettingButton).SetNextValueSetting();
                }

                if (value == -1)
                {
                    if (m_buttonContainer.SelectedButton is UISettingButton) (m_buttonContainer.SelectedButton as UISettingButton).SetPreviousValueSetting();
                }
                return;
            }

            if (m_mainMenu.State == MainMenu.MenuState.Quit)
            {
                if (value == 1) m_buttonContainer.SelectNext();
                if (value == -1) m_buttonContainer.SelectPrevious();
            }
        }
    }
}
