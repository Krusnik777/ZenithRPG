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

        private Inventory m_inventory;
        public Inventory Inventory => m_inventory;

        private void Start()
        {
            m_inventory = m_character.Inventory;
        }

        private void OnEnable()
        {
            SetInventory();
        }

        private void SetInventory()
        {
            if (m_inventory.UsableItemSlots.Length != m_activeItemSlots.Length)
            {
                Debug.LogError("Inventory.UsableItemSlots.Length != UIActiveItemSlots.Length");
                return;
            }

            m_weaponSlot.SetSlot(m_inventory.WeaponItemSlot);
            m_magicSlot.SetSlot(m_inventory.MagicItemSlot);
            m_shieldSlot.SetSlot(m_inventory.ShieldItemSlot);
            m_armorSlot.SetSlot(m_inventory.ArmorItemSlot);

            for (int i = 0; i < m_inventory.UsableItemSlots.Length; i++)
            {
                m_activeItemSlots[i].SetSlot(m_inventory.UsableItemSlots[i]);
            }

            m_mainPocket.SetSlots(m_inventory.MainPocket);
        }
    }
}
