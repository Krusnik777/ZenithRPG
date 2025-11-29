using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIInventoryButtonsPanel : MonoBehaviour
    {
        [System.Serializable]
        public class Sprites
        {
            public Sprite gamepadSprite;
            public Sprite keyboardSprite;
        }

        [Header("Use")]
        [SerializeField] private Image m_useButtonIcon;
        [SerializeField] private Sprites m_useButtonSprites;
        [SerializeField] private TextMeshProUGUI m_useText;
        [Header("Remove")]
        [SerializeField] private Image m_removeButtonIcon;
        [SerializeField] private Sprites m_removeButtonSprites;
        [SerializeField] private Sprites m_backButtonSprites;
        [SerializeField] private TextMeshProUGUI m_removeText;
        [Header("Move")]
        [SerializeField] private Image m_moveButtonIcon;
        [SerializeField] private Sprites m_moveButtonSprites;
        [SerializeField] private TextMeshProUGUI m_moveText;

        private UIInventory.InteractionState lastInventoryState;

        public void UpdateButtonsPanel(UIInventory uIInventory, IItemSlot slot)
        {
            lastInventoryState = uIInventory.State;

            if (UIInventorySlotButton.InTransit == true)
            {
                m_moveText.text = "Выбрать";

                SetMoveInfoTransparencyToDefault();
                ChangeUseInfoTransparency();
                ChangeRemoveInfoTransparency();
            }
            else
            {
                UpdateUseInfo(uIInventory, slot);
                m_moveText.text = "Двигать";

                bool isGamepad = ControlsManager.CurrentControlDevice is UnityEngine.InputSystem.Gamepad;

                if (uIInventory.State == UIInventory.InteractionState.Normal)
                {
                    m_removeButtonIcon.sprite = isGamepad ? m_removeButtonSprites.gamepadSprite : m_removeButtonSprites.keyboardSprite;
                    m_removeText.text = "Бросить";
                }

                if (uIInventory.State == UIInventory.InteractionState.Shop)
                {
                    m_removeButtonIcon.sprite = isGamepad ? m_backButtonSprites.gamepadSprite : m_backButtonSprites.keyboardSprite;
                    m_removeText.text = "Назад";
                }

                if (slot.IsEmpty)
                {
                    ChangeMoveInfoTransparency();
                    if (uIInventory.State == UIInventory.InteractionState.Normal) ChangeRemoveInfoTransparency();
                }
                else
                {
                    SetMoveInfoTransparencyToDefault();
                    SetRemoveInfoTransparencyToDefault();
                }
            }
        }

        public void UnsetButtonPanel(UIInventory uIInventory)
        {
            ChangeUseInfoTransparency();
            ChangeMoveInfoTransparency();
            if (uIInventory.State == UIInventory.InteractionState.Normal) ChangeRemoveInfoTransparency();
        }

        private void OnEnable()
        {
            ChangeButtonsIcon(ControlsManager.CurrentControlDevice);
            ControlsManager.OnControlDeviceChanged += ChangeButtonsIcon;
        }

        private void OnDisable()
        {
            ControlsManager.OnControlDeviceChanged -= ChangeButtonsIcon;
        }

        private void SetMoveInfoTransparencyToDefault()
        {
            var tempColor = m_moveText.color;

            tempColor.a = 1f;
            m_moveButtonIcon.color = tempColor;
            m_moveText.color = tempColor;
        }

        private void ChangeMoveInfoTransparency()
        {
            var tempColor = m_moveText.color;

            tempColor.a = 0.2f;
            m_moveButtonIcon.color = tempColor;
            m_moveText.color = tempColor;
        }

        private void SetRemoveInfoTransparencyToDefault()
        {
            var tempColor = m_removeText.color;

            tempColor.a = 1f;
            m_removeButtonIcon.color = tempColor;
            m_removeText.color = tempColor;
        }

        private void ChangeRemoveInfoTransparency()
        {
            var tempColor = m_removeText.color;

            tempColor.a = 0.2f;
            m_removeButtonIcon.color = tempColor;
            m_removeText.color = tempColor;
        }

        private void SetUseInfoTransparencyToDefault()
        {
            var tempColor = m_useText.color;

            tempColor.a = 1f;
            m_useButtonIcon.color = tempColor;
            m_useText.color = tempColor;
        }

        private void ChangeUseInfoTransparency()
        {
            var tempColor = m_useText.color;

            tempColor.a = 0.2f;
            m_useButtonIcon.color = tempColor;
            m_useText.color = tempColor;
        }

        private void UpdateUseInfo(UIInventory uIInventory, IItemSlot slot)
        {
            if (uIInventory.State == UIInventory.InteractionState.Normal)
            {
                if (!(slot is AnyItemSlot || slot is UsableItemSlot) || slot.Item is NotUsableItem || slot.IsEmpty)
                {
                    ChangeUseInfoTransparency();
                }
                else
                {
                    SetUseInfoTransparencyToDefault();
                }

                if (slot.Item is UsableItem || slot.Item is NotUsableItem || slot.IsEmpty)
                {
                    m_useText.text = "Использовать";
                }

                if (slot.Item is WeaponItem || slot.Item is MagicItem || slot.Item is EquipItem)
                {
                    m_useText.text = "Экипировать";
                }
            }

            if (uIInventory.State == UIInventory.InteractionState.Shop)
            {
                m_useText.text = "Продать";

                if (slot.IsEmpty)
                {
                    ChangeUseInfoTransparency();
                }
                else
                {
                    SetUseInfoTransparencyToDefault();
                }
            }
        }

        private void ChangeButtonsIcon(UnityEngine.InputSystem.InputDevice device)
        {
            if (device is UnityEngine.InputSystem.Gamepad)
            {
                m_useButtonIcon.sprite = m_useButtonSprites.gamepadSprite;
                if (m_moveButtonIcon != null) m_moveButtonIcon.sprite = m_moveButtonSprites.gamepadSprite;

                m_removeButtonIcon.sprite = lastInventoryState == UIInventory.InteractionState.Shop ? m_backButtonSprites.gamepadSprite : m_removeButtonSprites.gamepadSprite;
            }
            else
            {
                m_useButtonIcon.sprite = m_useButtonSprites.keyboardSprite;
                if (m_moveButtonIcon != null) m_moveButtonIcon.sprite = m_moveButtonSprites.keyboardSprite;

                m_removeButtonIcon.sprite = lastInventoryState == UIInventory.InteractionState.Shop ? m_backButtonSprites.keyboardSprite : m_removeButtonSprites.keyboardSprite;
            }
        }
    }
}
