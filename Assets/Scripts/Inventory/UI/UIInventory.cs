using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        [Header("EquipInfo")]
        [SerializeField] private TextMeshProUGUI m_defenseAmountText;
        [SerializeField] private TextMeshProUGUI m_attackAmountText;
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


            UpdateEquipInfo();
        }

        private void OnItemRemoved(object sender, IItemSlot slot)
        {
            FindAndSetSlot(slot);

            if (slot == m_inventory.WeaponItemSlot) UpdateWeaponInfo();
            if (slot == m_inventory.ArmorItemSlot || slot == m_inventory.ShieldItemSlot) UpdateDefenseInfo();
        }

        private void OnItemUsed(object sender, IItemSlot slot)
        {
            FindAndSetSlot(slot);
        }

        private void OnTransitCompleted(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            FindAndSetSlot(fromSlot);
            FindAndSetSlot(toSlot);

            if (fromSlot == m_inventory.WeaponItemSlot || toSlot == m_inventory.WeaponItemSlot) UpdateWeaponInfo();
            if (fromSlot == m_inventory.ArmorItemSlot || toSlot == m_inventory.ArmorItemSlot ||
                fromSlot == m_inventory.ShieldItemSlot || toSlot == m_inventory.ShieldItemSlot) UpdateDefenseInfo();
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

        private void UpdateEquipInfo()
        {
            UpdateWeaponInfo();

            UpdateDefenseInfo();
        }

        private void UpdateWeaponInfo()
        {
            m_attackAmountText.text = m_inventory.WeaponItemSlot.IsEmpty ? "0" : (m_inventory.WeaponItemSlot.Item as WeaponItem).AttackIncrease.ToString();
        }

        private void UpdateDefenseInfo()
        {
            var defenseAmount = m_inventory.ShieldItemSlot.IsEmpty ? 0 : (m_inventory.ShieldItemSlot.Item as EquipItem).DefenseIncrease;
            defenseAmount += m_inventory.ArmorItemSlot.IsEmpty ? 0 : (m_inventory.ArmorItemSlot.Item as EquipItem).DefenseIncrease;

            m_defenseAmountText.text = defenseAmount.ToString();
        }

    }
}
