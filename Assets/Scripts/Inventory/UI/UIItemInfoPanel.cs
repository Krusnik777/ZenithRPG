using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIItemInfoPanel : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI m_titleText;
        [SerializeField] protected TextMeshProUGUI m_priceText;
        [SerializeField] protected TextMeshProUGUI m_amountText;
        [SerializeField] protected Image m_typeIconImage;
        [SerializeField] protected TextMeshProUGUI m_description;

        public virtual void SetInfoPanel(IItemSlot slot)
        {
            var item = slot.Item;

            m_titleText.text = item.Info.Title;
            m_priceText.text = item.Info.Price.ToString(); // TEMP

            if (item is UsableItem || item is NotUsableItem)
            {
                if (item.MaxAmount != 1)
                {
                    m_amountText.text = item.Amount.ToString();
                    m_amountText.gameObject.SetActive(true);
                }
                else
                {
                    m_amountText.gameObject.SetActive(false);
                }
            }

            m_typeIconImage.sprite = slot.ItemInfo.Icon; // TEMP

            m_description.text = slot.ItemInfo.Description;

        }
    }
}
