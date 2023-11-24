using UnityEngine;

namespace DC_ARPG
{
    public class UIInventoryPocket : MonoBehaviour
    {
        [SerializeField] private UIInventorySlot[] m_uiInventorySlots;

        public void SetSlots(InventoryPocket pocket)
        {
            if (pocket.ItemSlots.Length != m_uiInventorySlots.Length)
            {
                Debug.LogError(pocket.Type + " InventoryPocket.Length != UIInventoryPocket.Length");
                return;
            }

            for (int i = 0; i < pocket.ItemSlots.Length; i++)
            {
                m_uiInventorySlots[i].SetSlot(pocket.ItemSlots[i]);
            }
        }
    }
}
