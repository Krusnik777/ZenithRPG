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
            if (m_uiInventory.State == UIInventory.InteractionState.Normal) Inventory.UseItem(this, m_inventorySlot);

            if (m_uiInventory.State == UIInventory.InteractionState.Shop) UIShop.Instance.SellItem(this, m_uiInventory.Player.Character, m_inventorySlot);
        }

        public void RemoveItem()
        {
            if (m_uiInventory.Player.CheckForwardGridIsEmpty() == true)
            {
                var potentialPit = m_uiInventory.Player.CheckForwardGridForInspectableObject();

                if (potentialPit is Pit)
                {
                    if (potentialPit is HiddenPit)
                    {
                        if ((potentialPit as HiddenPit).TrapFloor != null)
                        {
                            (potentialPit as HiddenPit).UnveilHiddenPit();
                        }
                    }

                    ShortMessage.Instance.ShowMessage("Предмет пропал в недрах ямы.");

                    Inventory.RemoveItemFromInventory(this, m_inventorySlot);

                    UISounds.Instance.PlayItemRemovedSound();

                    return;
                }

                var itemContainer = Instantiate(m_inventorySlot.Item.Info.Prefab, m_uiInventory.Player.transform.position + m_uiInventory.Player.transform.forward, Quaternion.identity);
                itemContainer.GetComponent<ItemContainer>().SetupCreatedContainer();
                itemContainer.GetComponent<ItemContainer>().AssignItem(m_inventorySlot.Item);

                Inventory.RemoveItemFromInventory(this, m_inventorySlot);

                UISounds.Instance.PlayItemRemovedSound();
            }
            else
            {
                var potentialItemContainer = m_uiInventory.Player.CheckForwardGridForInspectableObject();

                if (potentialItemContainer is Chest)
                {
                    var chest = potentialItemContainer as Chest;

                    if (chest.Item != null || chest.HasSpecialContents)
                    {
                        ShortMessage.Instance.ShowMessage("Больше нет места.");

                        UISounds.Instance.PlayInventoryActionFailureSound();

                        return;
                    }

                    chest.AssignItem(m_inventorySlot.Item);

                    if (chest.Opened) chest.Close();

                    Inventory.RemoveItemFromInventory(this, m_inventorySlot);

                    UISounds.Instance.PlayItemRemovedSound();

                    return;
                }

                ShortMessage.Instance.ShowMessage("Некуда класть.");

                UISounds.Instance.PlayInventoryActionFailureSound();
            }  
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
