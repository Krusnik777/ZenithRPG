using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIShopItem : MonoBehaviour
    {
        [SerializeField] private Image m_itemIcon;
        [SerializeField] private TextMeshProUGUI m_itemAmountText;
        [SerializeField] private TextMeshProUGUI m_priceValue;

        private IItem m_item;
        public IItem Item => m_item;

        public void SetShopItem(Shop shop, IItem item)
        {
            m_item = item;

            m_itemIcon.sprite = item.Info.Icon;

            int price = (int)(shop.ShopInfo.Surcharge * item.Price) + item.Price;

            if (price <= 0) price = ((int)(shop.ShopInfo.Surcharge * shop.DefaultPrice) + shop.DefaultPrice) * item.Amount;

            m_priceValue.text = price.ToString();

            if (item is UsableItem || item is NotUsableItem)
            {
                if (item.MaxAmount != 1)
                {
                    m_itemAmountText.text = item.Amount.ToString();
                    m_itemAmountText.gameObject.SetActive(true);
                }
                else
                {
                    m_itemAmountText.gameObject.SetActive(false);
                }

                return;
            }

            if (item is WeaponItem)
            {
                var weapon = item as WeaponItem;

                if (!weapon.HasInfiniteUses)
                {
                    m_itemAmountText.text = weapon.Uses.ToString();
                    m_itemAmountText.gameObject.SetActive(true);
                }
                else m_itemAmountText.gameObject.SetActive(false);

                return;
            }

            if (item is MagicItem)
            {
                var magicItem = item as MagicItem;

                if (!magicItem.HasInfiniteUses)
                {
                    m_itemAmountText.text = magicItem.Uses.ToString();
                    m_itemAmountText.gameObject.SetActive(true);
                }
                else m_itemAmountText.gameObject.SetActive(false);

                return;
            }

            m_itemAmountText.gameObject.SetActive(false);
        }
    }
}
