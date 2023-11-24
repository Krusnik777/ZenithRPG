using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class InventoryInputController : MonoBehaviour
    {
        [SerializeField] private ControlsManager m_controlsManager;
        [SerializeField] private UIInventory m_uIInventory;

        private Controls _controls;

        private void OnEnable()
        {
            _controls = m_controlsManager.Controls;
            _controls.Inventory.Enable();

            m_uIInventory.gameObject.SetActive(true);

            _controls.Inventory.Cancel.performed += OnCancel;
        }

        private void OnDisable()
        {
            _controls.Inventory.Cancel.performed -= OnCancel;

            _controls.Inventory.Disable();
        }

        private void OnCancel(InputAction.CallbackContext obj)
        {
            // DEBUG

            m_uIInventory.gameObject.SetActive(false);

            m_controlsManager.SetPlayerControlsActive(true);
            m_controlsManager.SetInventoryControlsActive(false);
        }
    }
}
