using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image m_itemIcon;
        [SerializeField] private TextMeshProUGUI m_itemAmountText;

        private IItemSlot m_inventorySlot;
        public IItemSlot InventorySlot => m_inventorySlot;

        private UIInventory m_uiInventory;
        public UIInventory UIInventory => m_uiInventory;
        public Inventory Inventory => m_uiInventory.Inventory;

        public void UseItem()
        {
            Inventory.UseItem(this, m_inventorySlot);
        }

        public void RemoveItem()
        {
            if (m_uiInventory.Player.CheckForwardGridIsEmpty() == false) return;

            var itemContainer = Instantiate(m_inventorySlot.Item.Info.Prefab, m_uiInventory.Player.transform.position + m_uiInventory.Player.transform.forward, Quaternion.identity);
            itemContainer.GetComponent<ItemContainer>().AssignItem(m_inventorySlot.Item);

            Inventory.RemoveItemFromInventory(this, m_inventorySlot);
        }

        public void StartTransit()
        {
            Inventory.SetFromSlot(m_inventorySlot);
        }

        public void CompleteTransit()
        {
            Inventory.TransitToSlot(this, m_inventorySlot);
        }

        public void SetSlot(UIInventory uIInventory,IItemSlot inventorySlot)
        {
            m_uiInventory = uIInventory;
            m_inventorySlot = inventorySlot;

            if (inventorySlot.IsEmpty)
            {
                m_itemIcon.gameObject.SetActive(false);
                m_itemAmountText.gameObject.SetActive(false);
            }
            else
            {
                var item = inventorySlot.Item;
                m_itemIcon.sprite = item.Info.Icon;
                m_itemIcon.gameObject.SetActive(true);

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
}
