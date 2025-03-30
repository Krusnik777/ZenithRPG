using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIShopItemInfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_titleText;
        [SerializeField] private TextMeshProUGUI m_amountText;
        [SerializeField] private Image m_typeIconImage;
        [SerializeField] private TextMeshProUGUI m_specialInfoParamName;
        [SerializeField] private TextMeshProUGUI m_specialInfoParamText;
        [SerializeField] private TextMeshProUGUI m_description;

        public void ShowInfo(IItem item)
        {
            m_titleText.text = item.Info.Title;
            m_typeIconImage.sprite = item.Info.ItemTypeIcon;
            m_description.text = item.Info.Description;

            if (item is UsableItem || item is NotUsableItem)
            {
                m_specialInfoParamName.enabled = false;
                m_specialInfoParamText.enabled = false;

                if (item.MaxAmount != 1)
                {
                    m_amountText.text = item.Amount.ToString();
                    m_amountText.enabled = true;
                }
                else
                {
                    m_amountText.enabled = false;
                }
            }

            if (item is WeaponItem)
            {
                var weaponItem = item as WeaponItem;

                m_specialInfoParamName.text = "АТК";
                m_specialInfoParamName.enabled = true;
                m_specialInfoParamText.text = weaponItem.AttackIncrease.ToString();
                m_specialInfoParamText.enabled = true;

                if (!weaponItem.HasInfiniteUses)
                {
                    m_amountText.text = weaponItem.Uses.ToString();
                    m_amountText.enabled = true;
                }
                else
                {
                    m_amountText.enabled = false;
                }
            }

            if (item is EquipItem)
            {
                var equipItem = item as EquipItem;

                m_specialInfoParamName.text = "ЗАЩ";
                m_specialInfoParamName.enabled = true;
                m_specialInfoParamText.text = equipItem.DefenseIncrease.ToString();
                m_specialInfoParamText.enabled = true;

                m_amountText.enabled = false;
            }

            if (item is MagicItem)
            {
                var magicItem = item as MagicItem;

                if (!magicItem.HasInfiniteUses)
                {
                    m_specialInfoParamName.text = "Без расхода маны";
                    m_specialInfoParamName.enabled = true;
                    m_amountText.text = magicItem.Uses.ToString();
                    m_amountText.enabled = true;

                    m_specialInfoParamText.enabled = false;
                }
                else
                {
                    m_specialInfoParamName.text = "Расход маны";
                    m_specialInfoParamName.enabled = true;
                    m_specialInfoParamText.text = magicItem.MagicPointsForUse.ToString();
                    m_specialInfoParamText.enabled = true;

                    m_amountText.enabled = false;
                }
            }
        }
    }
}
