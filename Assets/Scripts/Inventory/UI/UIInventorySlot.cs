using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image m_itemIcon;
        [SerializeField] private TextMeshProUGUI m_itemAmountText;

        public void SetSlot(IItemSlot inventorySlot)
        {
            if (inventorySlot.IsEmpty)
            {
                m_itemIcon.gameObject.SetActive(false);
                m_itemAmountText.gameObject.SetActive(false);
            }
            else
            {
                var item = inventorySlot.Item;
                m_itemIcon.sprite = item.Info.Icon;

                if (item is UsableItem || item is NotUsableItem)
                {
                    m_itemAmountText.gameObject.SetActive(true);
                    if (item.Amount > 1) m_itemAmountText.text = item.Amount.ToString();

                    return;
                }

                if (item is WeaponItem)
                {
                    var weapon = item as WeaponItem;

                    if (!weapon.HasInfiniteUses)
                    {
                        m_itemAmountText.gameObject.SetActive(true);
                        m_itemAmountText.text = weapon.Uses.ToString();
                    }
                    else m_itemAmountText.gameObject.SetActive(false);

                    return;
                }

                if (item is MagicItem)
                {
                    var magicItem = item as MagicItem;

                    if (!magicItem.HasInfiniteUses)
                    {
                        m_itemAmountText.gameObject.SetActive(true);
                        m_itemAmountText.text = magicItem.Uses.ToString();
                    }
                    else m_itemAmountText.gameObject.SetActive(false);

                    return;
                }

                m_itemAmountText.gameObject.SetActive(false);
            }
        }

    }
}
