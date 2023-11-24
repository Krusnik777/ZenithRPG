using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class MenuInputController : MonoBehaviour
    {
        [SerializeField] private ControlsManager m_controlsManager;
        [SerializeField] private UIStatsTest m_uiStatsTest;

        private Controls _controls;

        private void OnEnable()
        {
            _controls = m_controlsManager.Controls;

            _controls.Menu.Enable();

            m_uiStatsTest.TurnStatsPanel(true);

            _controls.Menu.Cancel.performed += OnUnpause;
        }

        private void OnDisable()
        {
            _controls.Menu.Cancel.performed -= OnUnpause;

            _controls.Menu.Disable();
        }

        private void OnUnpause(InputAction.CallbackContext obj)
        {
            // DEBUG

            m_uiStatsTest.TurnStatsPanel(false);

            m_controlsManager.SetPlayerControlsActive(true);
            m_controlsManager.SetMenuControlsActive(false);
        }
    }
}
