using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class SimpleMenuInputController : MonoBehaviour, IDependency<ControlsManager>
    {
        [SerializeField] private RestartMenu m_restartMenu;


        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Controls _controls;

        private UISelectableButtonContainer m_buttonContainer => m_restartMenu.ActiveButtonContainer;

        private void OnEnable()
        {
            if (_controls == null) _controls = m_controlsManager.Controls;

            _controls.Menu.Enable();

            _controls.Menu.Confirm.performed += OnConfirm;
            _controls.Menu.Cancel.performed += OnCancel;

            _controls.Menu.Move.performed += OnMove;
            _controls.Menu.ChangeParameters.performed += OnChangeParameters;
        }

        private void OnDisable()
        {
            _controls.Menu.Confirm.performed -= OnConfirm;
            _controls.Menu.Cancel.performed -= OnCancel;

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

            return;
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            var value = _controls.Menu.Move.ReadValue<float>();

            if (value == 1) m_buttonContainer.SelectPrevious();
            if (value == -1) m_buttonContainer.SelectNext();
        }
        private void OnChangeParameters(InputAction.CallbackContext obj)
        {
            return;
            /*
            var value = _controls.Menu.ChangeParameters.ReadValue<float>();

            if (value == 1) m_buttonContainer.SelectNext();
            if (value == -1) m_buttonContainer.SelectPrevious();*/
        }
    }
}
