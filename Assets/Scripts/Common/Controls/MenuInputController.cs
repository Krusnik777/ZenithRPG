using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class MenuInputController : MonoBehaviour, IDependency<ControlsManager>
    {
        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Controls _controls;

        private UISelectableButtonContainer m_buttonContainer => PauseMenu.Instance.ActiveButtonContainer;

        private void OnEnable()
        {
            if (_controls == null) _controls = m_controlsManager.Controls;

            _controls.Menu.Enable();

            _controls.Menu.Confirm.performed += OnConfirm;
            _controls.Menu.Cancel.performed += OnCancel;
            _controls.Menu.CloseMenu.performed += OnCloseMenu;

            _controls.Menu.Move.performed += OnMove;
            _controls.Menu.ChangeParameters.performed += OnChangeParameters;
        }

        private void OnDisable()
        {
            _controls.Menu.Confirm.performed -= OnConfirm;
            _controls.Menu.Cancel.performed -= OnCancel;
            _controls.Menu.CloseMenu.performed -= OnCloseMenu;

            _controls.Menu.Move.performed -= OnMove;
            _controls.Menu.ChangeParameters.performed -= OnChangeParameters;

            _controls.Menu.Disable();
        }

        private void OnConfirm(InputAction.CallbackContext obj)
        {
            m_buttonContainer.SelectedButton.OnButtonClick();
        }

        private void OnCancel(InputAction.CallbackContext obj)
        {
            UISounds.Instance.PlayBackSound();

            if (PauseMenu.Instance.State == PauseMenu.MenuState.Selection)
            {
                PauseMenu.Instance.HidePauseMenu();
                return;
            }

            if (PauseMenu.Instance.State == PauseMenu.MenuState.Status)
            {
                PauseMenu.Instance.ShowStatus(false);
                return;
            }

            if (PauseMenu.Instance.State == PauseMenu.MenuState.Map)
            {
                PauseMenu.Instance.ShowMap(false);
                return;
            }

            if (PauseMenu.Instance.State == PauseMenu.MenuState.Settings)
            {
                PauseMenu.Instance.ShowSettings(false);
                return;
            }

            if (PauseMenu.Instance.State == PauseMenu.MenuState.Data)
            {
                PauseMenu.Instance.ShowDataButtons(false);
                return;
            }

            if (PauseMenu.Instance.State == PauseMenu.MenuState.SaveMenu)
            {
                PauseMenu.Instance.HideSaveLoadMenu();
                return;
            }

            if (PauseMenu.Instance.State == PauseMenu.MenuState.Quit)
            {
                if (PauseMenu.Instance.RequireConfirm == PauseMenu.ConfirmationState.None)
                {
                    PauseMenu.Instance.ShowQuitButtons(false);
                    return;
                }

                if (PauseMenu.Instance.RequireConfirm == PauseMenu.ConfirmationState.Require)
                {
                    PauseMenu.Instance.HideConfirmPanel();
                    return;
                }
            }
        }

        private void OnCloseMenu(InputAction.CallbackContext obj)
        {
            if (PauseMenu.Instance.State == PauseMenu.MenuState.Selection)
            {
                PauseMenu.Instance.HidePauseMenu();
            }
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            if ((PauseMenu.Instance.State == PauseMenu.MenuState.Quit && PauseMenu.Instance.RequireConfirm == PauseMenu.ConfirmationState.Require) ||
                PauseMenu.Instance.State == PauseMenu.MenuState.Status || PauseMenu.Instance.State == PauseMenu.MenuState.Map) return;

            var value = _controls.Menu.Move.ReadValue<float>();

            if (value == 1) m_buttonContainer.SelectPrevious();
            if (value == -1) m_buttonContainer.SelectNext();
        }
        private void OnChangeParameters(InputAction.CallbackContext obj)
        {
            var value = _controls.Menu.ChangeParameters.ReadValue<float>();

            if (PauseMenu.Instance.State == PauseMenu.MenuState.Settings)
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

            if (PauseMenu.Instance.State == PauseMenu.MenuState.Quit && PauseMenu.Instance.RequireConfirm == PauseMenu.ConfirmationState.Require)
            {
                if (value == 1) m_buttonContainer.SelectNext();
                if (value == -1) m_buttonContainer.SelectPrevious();
            }
        }
    }
}
