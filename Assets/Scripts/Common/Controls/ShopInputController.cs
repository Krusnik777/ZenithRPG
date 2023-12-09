using UnityEngine;
using UnityEngine.InputSystem;

namespace DC_ARPG
{
    public class ShopInputController : MonoBehaviour, IDependency<ControlsManager>
    {
        [SerializeField] private UIInventory m_uIInventory;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private Controls _controls;

        private UISelectableButtonContainer m_buttonContainer => UIShop.Instance.ActiveButtonContainer;

        private void OnEnable()
        {
            if (_controls == null) _controls = m_controlsManager.Controls;

            _controls.Shop.Enable();

            _controls.Shop.Cancel.performed += BackToSelection;

            _controls.Shop.MoveCursor.performed += OnMoveCursor;

            _controls.Shop.Confirm.performed += OnConfirm;
            _controls.Shop.MoveItem.performed += OnMoveItem;
        }

        private void OnDisable()
        {
            _controls.Shop.Cancel.performed -= BackToSelection;

            _controls.Shop.MoveCursor.performed -= OnMoveCursor;

            _controls.Shop.Confirm.performed -= OnConfirm;
            _controls.Shop.MoveItem.performed -= OnMoveItem;

            _controls.Shop.Disable();
        }

        private void BackToSelection(InputAction.CallbackContext obj)
        {
            if (UIShop.Instance.State == UIShop.ShopState.Sell)
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
            }

            UIShop.Instance.ReturnToSelection();
        }

        private void OnMoveCursor(InputAction.CallbackContext obj)
        {
            if (UIShop.Instance.State == UIShop.ShopState.Talk) return;

            var value = _controls.Shop.MoveCursor.ReadValue<Vector2>();

            if (UIShop.Instance.State == UIShop.ShopState.Selection)
            {
                if (value.x == 1) m_buttonContainer.SelectNext();
                if (value.x == -1) m_buttonContainer.SelectPrevious();
            }

            if (UIShop.Instance.State == UIShop.ShopState.Buy)
            {
                if (value.y == 1) m_buttonContainer.SelectPrevious();
                if (value.y == -1) m_buttonContainer.SelectNext();
            }

            if (UIShop.Instance.State == UIShop.ShopState.Sell)
            {
                if (value.x == 1) m_buttonContainer.SelectRight();
                if (value.x == -1) m_buttonContainer.SelectLeft();
                if (value.y == 1) m_buttonContainer.SelectUp();
                if (value.y == -1) m_buttonContainer.SelectDown();
            }
        }

        private void OnConfirm(InputAction.CallbackContext obj)
        {
            if (UIShop.Instance.State == UIShop.ShopState.Selection)
            {
                m_buttonContainer.SelectedButton.OnButtonClick();
                return;
            }

            if (UIShop.Instance.State == UIShop.ShopState.Buy)
            {
                m_buttonContainer.SelectedButton.OnButtonClick();
                return;
            }

            if (UIShop.Instance.State == UIShop.ShopState.Sell)
            {
                if (m_buttonContainer.SelectedButton is UIInventorySlotButton)
                {
                    (m_buttonContainer.SelectedButton as UIInventorySlotButton).OnButtonUse();
                    return;
                }

                if (m_buttonContainer.SelectedButton is UIExtraPocketButton)
                {
                    m_buttonContainer.SelectedButton.OnButtonClick();
                    return;
                }
            }

            if (UIShop.Instance.State == UIShop.ShopState.Talk)
            {
                UIShop.Instance.ShopkeeperSpeech.ContinueSpeech();
            }
        }

        private void OnMoveItem(InputAction.CallbackContext obj)
        {
            if (UIShop.Instance.State != UIShop.ShopState.Sell) return;

            if (!(m_buttonContainer.SelectedButton is UIInventorySlotButton)) return;

            (m_buttonContainer.SelectedButton as UIInventorySlotButton).OnButtonMove();
        }

    }
}
