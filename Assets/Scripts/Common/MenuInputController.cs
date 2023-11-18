using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class MenuInputController : MonoBehaviour
    {
        [SerializeField] private PlayerInputController m_playerInputController;
        [SerializeField] private UIStatsTest m_uiStatsTest;

        private Controls _controls;

        private void Awake()
        {
            _controls = new Controls();
        }

        private void OnEnable()
        {
            _controls.Enable();

            m_uiStatsTest.TurnStatsPanel(true);

            _controls.Menu.Cancel.performed += OnUnpause;
        }

        private void OnDisable()
        {
            _controls.Menu.Cancel.performed -= OnUnpause;

            _controls.Disable();
        }

        private void OnUnpause(InputAction.CallbackContext obj)
        {
            // DEBUG

            m_uiStatsTest.TurnStatsPanel(false);

            m_playerInputController.enabled = true;

            enabled = false;
        }
    }
}
