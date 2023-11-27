using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class MenuInputController : MonoBehaviour, IDependency<ControlsManager>
    {
        [SerializeField] private UIStatsTest m_uiStatsTest;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null) _controls = m_controlsManager.Controls;

            _controls.Menu.Enable();

            m_uiStatsTest.TurnStatsPanel(true);

            _controls.Menu.Cancel.performed += OnUnpause;
            _controls.Menu.CloseMenu.performed += OnUnpause;

            _controls.Menu.Move.performed += OnMove;
            _controls.Menu.ChangeParameters.performed += OnChangeParameters;
        }

        private void OnDisable()
        {
            _controls.Menu.Cancel.performed -= OnUnpause;
            _controls.Menu.CloseMenu.performed -= OnUnpause;

            _controls.Menu.Move.performed -= OnMove;
            _controls.Menu.ChangeParameters.performed -= OnChangeParameters;

            _controls.Menu.Disable();
        }

        private void OnUnpause(InputAction.CallbackContext obj)
        {
            // DEBUG

            m_uiStatsTest.TurnStatsPanel(false);

            m_controlsManager.SetPlayerControlsActive(true);
            m_controlsManager.SetMenuControlsActive(false);
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            var value = _controls.Menu.Move.ReadValue<float>();

            if (value == 1) { /*MoveUp*/ }
            if (value == -1) { /*MoveDown*/ }
        }
        private void OnChangeParameters(InputAction.CallbackContext obj)
        {
            var value = _controls.Menu.ChangeParameters.ReadValue<float>();

            if (value == 1) { /*MoveRight*/ }
            if (value == -1) { /*MoveLeft*/ }
        }
    }
}
