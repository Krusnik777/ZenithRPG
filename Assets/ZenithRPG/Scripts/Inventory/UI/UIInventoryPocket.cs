using UnityEngine;

namespace DC_ARPG
{
    public class UIInventoryPocket : MonoBehaviour
    {
        [SerializeField] private UIInventorySlot[] m_uiInventorySlots;

        private InventoryPocket m_pocket;
        public InventoryPocket InventoryPocket => m_pocket;
        public void SetSlots(UIInventory uIInventory, InventoryPocket pocket)
        {
            if (pocket.ItemSlots.Length != m_uiInventorySlots.Length)
            {
                Debug.LogError(pocket.Type + " InventoryPocket.Length != UIInventoryPocket.Length");
                return;
            }

            m_pocket = pocket;

            for (int i = 0; i < pocket.ItemSlots.Length; i++)
            {
                m_uiInventorySlots[i].SetSlot(uIInventory, pocket.ItemSlots[i]);
            }
        }
    }
}
