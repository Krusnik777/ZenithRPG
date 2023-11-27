using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class UIExtraPocketButton : UISelectableButton
    {
        [Header("ExtraPocketButton")]
        [SerializeField] private Image m_pushedStateImage;
        [SerializeField] private Image m_unavailableStateImage;

        private bool isPressed;
        public bool IsPressed => isPressed;

        private UIExtraButtonsController buttonsController;

        public void SetAvailableState(bool state)
        {
            SetInteractable(state);
            m_unavailableStateImage.enabled = !state;
        }

        public override void OnButtonClick()
        {
            if (!m_interactable) return;

            if (!isPressed)
            {
                buttonsController.UnsetAlreadyPressedButton(this);
                buttonsController.ShowExtraPocket(this);
                SetButtonState(true);
            }
            else
            {
                buttonsController.HideExtraPocket();
                SetButtonState(false);
            }

            OnClick?.Invoke();
        }

        public void SetButtonState(bool pressedState)
        {
            isPressed = pressedState;
            m_pushedStateImage.enabled = pressedState;
        }

        private void Start()
        {
            buttonsController = GetComponentInParent<UIExtraButtonsController>();
        }
    }
}
