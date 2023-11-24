using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class ControlsManager : MonoSingleton<ControlsManager>
    {
        [SerializeField] private PlayerInputController m_playerInputController;
        [SerializeField] private MenuInputController m_menuInputController;
        [SerializeField] private InventoryInputController m_inventoryInputController;

        private Controls _controls;
        public Controls Controls => _controls;

        public void SetPlayerControlsActive(bool state)
        {
            m_playerInputController.enabled = state;
        }

        public void SetMenuControlsActive(bool state)
        {
            m_menuInputController.enabled = state;
        }

        public void SetInventoryControlsActive(bool state)
        {
            m_inventoryInputController.enabled = state;
        }

        protected override void Awake()
        {
            base.Awake();

            _controls = new Controls();
        }

        private void OnEnable()
        {
            _controls.Enable();
            SetPlayerControlsActive(true);
        }

        private void OnDisable()
        {
            _controls.Disable();
        }
    }
}
