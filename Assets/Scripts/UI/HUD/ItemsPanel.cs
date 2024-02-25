using UnityEngine;
using UnityEngine.UI;

namespace DC_ARPG
{
    public class ItemsPanel : MonoBehaviour
    {
        [System.Serializable]
        public class ActiveUsableItem
        {
            public UIInventorySlot UsableItemSlot;
            public Image SelectImage;
        }

        [SerializeField] private GameObject m_itemsPanel;
        [Header("EquippedItems")]
        [SerializeField] private UIInventorySlot m_weaponSlot;
        [SerializeField] private UIInventorySlot m_magicSlot;
        [Header("ActiveUsableItems")]
        [SerializeField] private ActiveUsableItem[] m_activeUsableItems;

        private UIInventory m_uiInventory;
        public UIInventory UIInventory => m_uiInventory;

        public void TurnPanel(bool state)
        {
            m_itemsPanel.SetActive(state);
        }

        public void SetPanel(UIInventory uIInventory)
        {
            m_uiInventory = uIInventory;

            m_weaponSlot.SetSlot(m_uiInventory, m_uiInventory.Inventory.WeaponItemSlot);
            m_magicSlot.SetSlot(m_uiInventory, m_uiInventory.Inventory.MagicItemSlot);

            m_uiInventory.Inventory.MagicItemSlot.EventOnMagicUsed += OnMagicUsed;
            m_uiInventory.Inventory.WeaponItemSlot.EventOnAttack += OnAttack;

            for (int i = 0; i < m_uiInventory.Inventory.UsableItemSlots.Length; i++)
            {
                m_activeUsableItems[i].UsableItemSlot.SetSlot(m_uiInventory, m_uiInventory.Inventory.UsableItemSlots[i]);
            }

            m_activeUsableItems[0].SelectImage.enabled = true;

            m_uiInventory.Inventory.EventOnActiveItemChanged += OnActiveItemChanged;
            m_uiInventory.Inventory.EventOnItemUsed += OnItemUsed;
            m_uiInventory.Inventory.EventOnItemRemoved += OnItemRemoved;
            m_uiInventory.Inventory.EventOnTransitCompleted += OnTransitCompleted;
        }

        private void OnDestroy()
        {
            m_uiInventory.Inventory.MagicItemSlot.EventOnMagicUsed -= OnMagicUsed;
            m_uiInventory.Inventory.WeaponItemSlot.EventOnAttack -= OnAttack;

            m_uiInventory.Inventory.EventOnActiveItemChanged -= OnActiveItemChanged;
            m_uiInventory.Inventory.EventOnItemUsed -= OnItemUsed;
            m_uiInventory.Inventory.EventOnItemRemoved -= OnItemRemoved;
            m_uiInventory.Inventory.EventOnTransitCompleted -= OnTransitCompleted;
        }

        private void OnMagicUsed(object sender, MagicItem magicItem)
        {
            m_magicSlot.SetSlot(m_uiInventory, m_uiInventory.Inventory.MagicItemSlot);
        }

        private void OnAttack(object sender)
        {
            m_weaponSlot.SetSlot(m_uiInventory, m_uiInventory.Inventory.WeaponItemSlot);
        }

        private void OnActiveItemChanged(object sender, int number)
        {
            TurnOffSelectImages();

            m_activeUsableItems[number].SelectImage.enabled = true;

            UISounds.Instance.PlayActiveItemChangeSound();
        }

        private void OnItemUsed(object sender, IItemSlot slot)
        {
            UpdateUsableItemSlots(slot);
        }

        private void OnItemRemoved(object sender, IItemSlot slot)
        {
            UpdateUsableItemSlots(slot);
            UpdateWeaponSlot(slot);
            UpdateMagicSlot(slot);
        }

        private void OnTransitCompleted(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            UpdateUsableItemSlots(fromSlot);
            UpdateUsableItemSlots(toSlot);
            UpdateWeaponSlot(fromSlot);
            UpdateWeaponSlot(toSlot);
            UpdateMagicSlot(fromSlot);
            UpdateMagicSlot(toSlot);
        }

        private void TurnOffSelectImages()
        {
            foreach (var activeUsableItem in m_activeUsableItems)
            {
                activeUsableItem.SelectImage.enabled = false;
            }
        }

        private void UpdateUsableItemSlots(IItemSlot slot)
        {
            for(int i = 0; i < m_activeUsableItems.Length; i++)
            {
                if (m_activeUsableItems[i].UsableItemSlot.InventorySlot == slot)
                {
                    m_activeUsableItems[i].UsableItemSlot.SetSlot(m_uiInventory, slot);
                }
            }
        }

        private void UpdateWeaponSlot(IItemSlot slot)
        {
            if (slot == m_uiInventory.Inventory.WeaponItemSlot)
            {
                m_weaponSlot.SetSlot(m_uiInventory, slot);
            }
        }

        private void UpdateMagicSlot(IItemSlot slot)
        {
            if (slot == m_uiInventory.Inventory.MagicItemSlot)
            {
                m_magicSlot.SetSlot(m_uiInventory, slot);
            }
        }

    }
}
