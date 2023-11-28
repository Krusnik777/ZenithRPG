using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class UIInventorySlotButton : UISelectableButton
    {
        [Header("InventorySlotImages")]
        [SerializeField] private Image m_transitSelectImage;
        [SerializeField] private Image m_transitImage;

        [Header("InventorySlotEvents")]
        public UnityEvent OnButtonUseClick;
        public UnityEvent OnButtonRemoveClick;
        public UnityEvent OnButtonMoveClick;

        public static bool InTransit = false;

        public static Image TransitImage;

        private UIInventorySlot uiSlot => GetComponent<UIInventorySlot>();

        public override void SetFocus()
        {
            if (!m_interactable) return;

            m_selectImage.enabled = true;
            if (InTransit) m_transitSelectImage.enabled = true;

            OnSelect?.Invoke();

            uiSlot.UIInventory.InfoPanelController.ShowInfoPanel(uiSlot.InventorySlot);
        }

        public override void UnsetFocus()
        {
            if (!m_interactable) return;

            m_selectImage.enabled = false;
            if (m_transitSelectImage.isActiveAndEnabled) m_transitSelectImage.enabled = false;

            OnUnselect?.Invoke();

            uiSlot.UIInventory.InfoPanelController.HideInfoPanel();
        }

        public void OnButtonUse()
        {
            if (!m_interactable || InTransit) return;

            uiSlot.UseItem();

            uiSlot.UIInventory.InfoPanelController.ShowInfoPanel(uiSlot.InventorySlot);

            OnButtonUseClick?.Invoke();
        }

        public void OnButtonRemove()
        {
            if (!m_interactable || InTransit) return;

            uiSlot.RemoveItem();

            OnButtonRemoveClick?.Invoke();
        }

        public void OnButtonMove()
        {
            if (!m_interactable) return;

            if (!InTransit)
            {
                if (uiSlot.InventorySlot.IsEmpty) return;

                m_transitSelectImage.enabled = true;

                uiSlot.StartTransit();
                InTransit = true;

                SetTransitImage(m_transitImage);
            }
            else
            {
                uiSlot.CompleteTransit();
                InTransit = false;

                m_transitSelectImage.enabled = false;

                ResetTransitImage();

                uiSlot.UIInventory.InfoPanelController.ShowInfoPanel(uiSlot.InventorySlot);
            }

            OnButtonMoveClick?.Invoke();
        }

        private void SetTransitImage(Image img)
        {
            TransitImage = img;
            TransitImage.enabled = true;
        }

        private void ResetTransitImage()
        {
            if (TransitImage == null) return;

            TransitImage.enabled = false;
            TransitImage = null;
        }
    }
}
