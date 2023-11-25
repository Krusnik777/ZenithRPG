using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UISpecialItemInfoPanel : UIItemInfoPanel
    {
        [SerializeField] private TextMeshProUGUI m_specialInfoParamName;
        [SerializeField] private TextMeshProUGUI m_specialInfoParamText;

        public override void SetInfoPanel(IItemSlot slot)
        {
            var item = slot.Item;

            m_titleText.text = item.Info.Title;
            m_priceText.text = item.Info.Price.ToString(); // TEMP

            if (item is WeaponItem)
            {
                var weaponItem = item as WeaponItem;

                m_specialInfoParamName.text = "АТК";
                m_specialInfoParamText.text = weaponItem.AttackIncrease.ToString();
                m_specialInfoParamText.gameObject.SetActive(true);

                if (!weaponItem.HasInfiniteUses)
                {
                    m_amountText.text = weaponItem.Uses.ToString();
                    m_amountText.gameObject.SetActive(true);
                }
                else
                {
                    m_amountText.gameObject.SetActive(false);
                }
            }

            if (item is EquipItem)
            {
                var equipItem = item as EquipItem;

                m_specialInfoParamName.text = "ЗАЩ";
                m_specialInfoParamText.text = equipItem.DefenseIncrease.ToString();
                m_specialInfoParamText.gameObject.SetActive(true);

                m_amountText.gameObject.SetActive(false);
            }

            if (item is MagicItem)
            {
                var magicItem = item as MagicItem;

                m_specialInfoParamName.text = "Название магии"; // TEMP

                if (!magicItem.HasInfiniteUses)
                {
                    m_amountText.text = magicItem.Uses.ToString();
                    m_amountText.gameObject.SetActive(true);

                    m_specialInfoParamText.gameObject.SetActive(false);
                }
                else
                {
                    m_specialInfoParamText.text = magicItem.MagicPointsForUse.ToString();
                    m_specialInfoParamText.gameObject.SetActive(true);

                    m_amountText.gameObject.SetActive(false);
                }
            }

            m_typeIconImage.sprite = slot.ItemInfo.Icon; // TEMP

            m_description.text = slot.ItemInfo.Description;
        }
    }
}
