using UnityEngine;

namespace DC_ARPG
{
    public class UIExtraButtonsController : MonoBehaviour
    {
        private UIExtraPocketButton[] extraPocketsButtons;

        private UIInventory m_uiInventory;

        public void SetExtraButtons(UIInventory uiInventory, int unlockedPockets)
        {
            extraPocketsButtons = GetComponentsInChildren<UIExtraPocketButton>();

            for (int i = 0; i < unlockedPockets; i++)
            {
                extraPocketsButtons[i].SetAvailableState(true);
            }

            for(int i = extraPocketsButtons.Length - 1; i >= unlockedPockets; i--)
            {
                extraPocketsButtons[i].SetAvailableState(false);
            }

            m_uiInventory = uiInventory;
        }

        public void ShowExtraPocket(UIExtraPocketButton button)
        {
            int index = -1;

            for (int i = 0; i < extraPocketsButtons.Length; i++)
            {
                if (extraPocketsButtons[i] == button)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                Debug.LogError("Index for button is not detected");
                return;
            }

            m_uiInventory.SetExtraPocket(index);
        }

        public void HideExtraPocket()
        {
            m_uiInventory.HideExtraPocket();
        }

        public void UnsetAlreadyPressedButton(UIExtraPocketButton exceptThisButton)
        {
            foreach (var button in extraPocketsButtons)
            {
                if (button.IsPressed && button != exceptThisButton)
                {
                    button.SetButtonState(false);
                    return;
                }
            }
        }
    }
}
