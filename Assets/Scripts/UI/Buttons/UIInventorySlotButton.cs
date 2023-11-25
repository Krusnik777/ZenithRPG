using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class UIInventorySlotButton : UISelectableButton
    {
        [Header("InventorySlotEvents")]
        public UnityEvent OnButtonUseClick;
        public UnityEvent OnButtonRemoveClick;
        public UnityEvent OnButtonMoveClick;

        public static bool inTransit = false;

        private UIInventorySlot uiSlot => GetComponent<UIInventorySlot>();

        public override void SetFocus()
        {
            if (!m_interactable) return;

            base.SetFocus();

            uiSlot.UIInventory.InfoPanelController.ShowInfoPanel(uiSlot.InventorySlot);
        }

        public void OnButtonUse()
        {
            if (!m_interactable) return;

            uiSlot.UseItem();

            OnButtonUseClick?.Invoke();
        }

        public void OnButtonRemove()
        {
            if (!m_interactable) return;

            uiSlot.RemoveItem();

            OnButtonRemoveClick?.Invoke();
        }

        public void OnButtonMove()
        {
            if (!m_interactable) return;

            if (!inTransit)
            {
                if (uiSlot.InventorySlot.IsEmpty) return;

                uiSlot.StartTransit();
                inTransit = true;
            }
            else
            {
                uiSlot.CompleteTransit();
                inTransit = false;
            }

            OnButtonMoveClick?.Invoke();
        }
    }
}
