using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class InventoryInputController : MonoBehaviour, IDependency<ControlsManager>
    {
        [SerializeField] private UIInventory m_uIInventory;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Controls _controls;

        private UISelectableButtonContainer m_buttonContainer;

        private void OnEnable()
        {
            if (_controls == null) _controls = m_controlsManager.Controls;

            _controls.Inventory.Enable();

            m_buttonContainer = m_uIInventory.UISlotButtonsContainer;

            m_uIInventory.ShowInventory();

            _controls.Inventory.Cancel.performed += CloseInventory;
            _controls.Inventory.CloseInventory.performed += CloseInventory;

            _controls.Inventory.MoveCursor.performed += OnMoveCursor;

            _controls.Inventory.Confirm.performed += OnConfirm;
            _controls.Inventory.MoveItem.performed += OnMoveItem;
            _controls.Inventory.DropItem.performed += OnDropItem;
        }

        private void OnDisable()
        {
            _controls.Inventory.Cancel.performed -= CloseInventory;
            _controls.Inventory.CloseInventory.performed -= CloseInventory;

            _controls.Inventory.MoveCursor.performed -= OnMoveCursor;

            _controls.Inventory.Confirm.performed -= OnConfirm;
            _controls.Inventory.MoveItem.performed -= OnMoveItem;
            _controls.Inventory.DropItem.performed -= OnDropItem;

            _controls.Inventory.Disable();
        }

        private void CloseInventory(InputAction.CallbackContext obj)
        {
            if (UIInventorySlotButton.InTransit)
            {
                UIInventorySlotButton.ResetInTransit();
                if (m_buttonContainer.SelectedButton is UIInventorySlotButton)
                {
                    (m_buttonContainer.SelectedButton as UIInventorySlotButton).ResetTransitSelectImage();
                    m_uIInventory.ButtonsInfoPanel.UpdateButtonsPanel(m_uIInventory,(m_buttonContainer.SelectedButton as UIInventorySlotButton).UISlot.InventorySlot);
                }
            }

            UISounds.Instance.PlayBackSound();

            m_uIInventory.HideInventory();

            m_controlsManager.SetPlayerControlsActive(true);
            m_controlsManager.SetInventoryControlsActive(false);
        }

        private void OnMoveCursor(InputAction.CallbackContext obj)
        {
            var value = _controls.Inventory.MoveCursor.ReadValue<Vector2>();

            if (value.x == 1) m_buttonContainer.SelectRight();
            if (value.x == -1) m_buttonContainer.SelectLeft();
            if (value.y == 1) m_buttonContainer.SelectUp();
            if (value.y == -1) m_buttonContainer.SelectDown();
        }

        private void OnConfirm(InputAction.CallbackContext obj)
        {
            if (m_buttonContainer.SelectedButton is UIInventorySlotButton)
            {
                (m_buttonContainer.SelectedButton as UIInventorySlotButton).OnButtonUse();
                return;
            }

            if (m_buttonContainer.SelectedButton is UIExtraPocketButton)
            {
                m_buttonContainer.SelectedButton.OnButtonClick();
            }
        }

        private void OnMoveItem(InputAction.CallbackContext obj)
        {
            if (!(m_buttonContainer.SelectedButton is UIInventorySlotButton)) return;

            (m_buttonContainer.SelectedButton as UIInventorySlotButton).OnButtonMove();
        }

        private void OnDropItem(InputAction.CallbackContext obj)
        {
            if (!(m_buttonContainer.SelectedButton is UIInventorySlotButton)) return;

            (m_buttonContainer.SelectedButton as UIInventorySlotButton).OnButtonRemove();
        }

        
    }
}
