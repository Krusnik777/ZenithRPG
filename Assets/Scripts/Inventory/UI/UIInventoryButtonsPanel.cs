using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIInventoryButtonsPanel : MonoBehaviour
    {
        [Header("Use")]
        [SerializeField] private Image m_useButtonIcon;
        [SerializeField] private TextMeshProUGUI m_useText;
        [Header("Remove")]
        [SerializeField] private Image m_removeButtonIcon;
        [SerializeField] private TextMeshProUGUI m_removeText;
        [Header("Move")]
        [SerializeField] private Image m_moveButtonIcon;
        [SerializeField] private TextMeshProUGUI m_moveText;

        public void UpdateButtonsPanel(IItemSlot slot)
        {
            if (UIInventorySlotButton.InTransit == true)
            {
                m_moveText.text = "�������";

                SetMoveInfoTransparencyToDefault();
                ChangeUseInfoTransparency();
                ChangeRemoveInfoTransparency();
            }
            else
            {
                UpdateUseInfo(slot);
                m_moveText.text = "�������";

                if (slot.IsEmpty)
                {
                    ChangeMoveInfoTransparency();
                    ChangeRemoveInfoTransparency();
                }
                else
                {
                    SetMoveInfoTransparencyToDefault();
                    SetRemoveInfoTransparencyToDefault();
                }
            }
        }

        public void UnsetButtonPanel()
        {
            ChangeUseInfoTransparency();
            ChangeMoveInfoTransparency();
            ChangeRemoveInfoTransparency();
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

        private void UpdateUseInfo(IItemSlot slot)
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
                m_useText.text = "������������";
            }

            if (slot.Item is WeaponItem || slot.Item is MagicItem || slot.Item is EquipItem)
            {
                m_useText.text = "�����������";
            }
        }
    }
}
