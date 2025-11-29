using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class UIDeviceControlledImage : MonoBehaviour
    {
        [SerializeField] private Image m_icon;
        [SerializeField] private Sprite m_gamepadSprite;
        [SerializeField] private Sprite m_keyboardSprite;

        private void OnEnable()
        {
            ChangeButtonIcon(ControlsManager.CurrentControlDevice);
            ControlsManager.OnControlDeviceChanged += ChangeButtonIcon;
        }

        private void OnDisable()
        {
            ControlsManager.OnControlDeviceChanged -= ChangeButtonIcon;
        }

        private void ChangeButtonIcon(UnityEngine.InputSystem.InputDevice device)
        {
            if (device is UnityEngine.InputSystem.Gamepad)
            {
                m_icon.sprite = m_gamepadSprite;
            }
            else
            {
                m_icon.sprite = m_keyboardSprite;
            }
        }
    }
}