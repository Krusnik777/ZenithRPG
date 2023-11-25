using UnityEngine;

namespace DC_ARPG
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private Character m_character;
        [Header("Equipment")]
        [SerializeField] private UIInventorySlot m_weaponSlot;
        [SerializeField] private UIInventorySlot m_magicSlot;
        [SerializeField] private UIInventorySlot m_shieldSlot;
        [SerializeField] private UIInventorySlot m_armorSlot;
        [Header("ActiveUsableItem")]
        [SerializeField] private UIInventorySlot[] m_activeItemSlots;
        [Header("MainPocket")]
        [SerializeField] private UIInventoryPocket m_mainPocket;
        [Header("ExtraPocket")]
        [SerializeField] private UIInventoryPocket m_extraPocket;
        [Header("InfoPanel")]
        [SerializeField] private UIItemInfoPanelController m_uiItemInfoPanelController;
        public UIItemInfoPanelController InfoPanelController => m_uiItemInfoPanelController;

        private Inventory m_inventory;
        public Inventory Inventory => m_inventory;

        private UIInventorySlot[] m_allUIInventorySlots;

        private void OnEnable()
        {
            if (m_inventory == null) m_inventory = m_character.Inventory;

            m_allUIInventorySlots = GetComponentsInChildren<UIInventorySlot>();

            m_inventory.EventOnItemRemoved += OnItemRemoved;
            m_inventory.EventOnItemUsed += OnItemUsed;
            m_inventory.EventOnTransitCompleted += OnTransitCompleted;

            SetInventory();
        }

        private void OnDisable()
        {
            m_inventory.EventOnItemRemoved -= OnItemRemoved;
            m_inventory.EventOnItemUsed -= OnItemUsed;
            m_inventory.EventOnTransitCompleted -= OnTransitCompleted;
        }

        private void SetInventory()
        {
            if (m_inventory.UsableItemSlots.Length != m_activeItemSlots.Length)
            {
                Debug.LogError("Inventory.UsableItemSlots.Length != UIActiveItemSlots.Length");
                return;
            }

            m_weaponSlot.SetSlot(this, m_inventory.WeaponItemSlot);
            m_magicSlot.SetSlot(this, m_inventory.MagicItemSlot);
            m_shieldSlot.SetSlot(this, m_inventory.ShieldItemSlot);
            m_armorSlot.SetSlot(this, m_inventory.ArmorItemSlot);

            for (int i = 0; i < m_inventory.UsableItemSlots.Length; i++)
            {
                m_activeItemSlots[i].SetSlot(this, m_inventory.UsableItemSlots[i]);
            }

            m_mainPocket.SetSlots(this, m_inventory.MainPocket);
        }

        private void OnItemRemoved(object sender, IItemSlot slot)
        {
            FindAndSetSlot(slot);
        }

        private void OnItemUsed(object sender, IItemSlot slot)
        {
            FindAndSetSlot(slot);
        }

        private void OnTransitCompleted(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            FindAndSetSlot(fromSlot);
            FindAndSetSlot(toSlot);
        }

        private void FindAndSetSlot(IItemSlot slot)
        {
            foreach (var checkSlot in m_allUIInventorySlots)
            {
                if (checkSlot.InventorySlot == slot)
                {
                    checkSlot.SetSlot(this, slot);
                    return;
                }
            }
        }

    }
}
