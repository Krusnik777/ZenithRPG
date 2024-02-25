using UnityEngine;

namespace DC_ARPG
{
    public class EquipmentMeshUpdater : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter m_playerCharacter;
        [SerializeField] private SkinnedMeshRenderer m_swordMesh;
        [SerializeField] private SkinnedMeshRenderer m_shieldMesh;

        private void Start()
        {
            m_swordMesh.enabled = !m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty;
            m_shieldMesh.enabled = !m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty;

            m_playerCharacter.Inventory.EventOnTransitCompleted += OnEquipItemChange;
            m_playerCharacter.Inventory.EventOnItemRemoved += OnEquipItemRemoved; 
        }

        private void OnDestroy()
        {
            m_playerCharacter.Inventory.EventOnTransitCompleted -= OnEquipItemChange;
            m_playerCharacter.Inventory.EventOnItemRemoved -= OnEquipItemRemoved;
        }

        private void OnEquipItemRemoved(object sender, IItemSlot slot)
        {
            if (slot == m_playerCharacter.Inventory.WeaponItemSlot) 
                m_swordMesh.enabled = !m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty;

            if (slot == m_playerCharacter.Inventory.ShieldItemSlot)
                m_shieldMesh.enabled = !m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty;
        }

        private void OnEquipItemChange(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            if (fromSlot == m_playerCharacter.Inventory.WeaponItemSlot || toSlot == m_playerCharacter.Inventory.WeaponItemSlot)
                m_swordMesh.enabled = !m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty;

            if (fromSlot == m_playerCharacter.Inventory.ShieldItemSlot || toSlot == m_playerCharacter.Inventory.ShieldItemSlot)
                m_shieldMesh.enabled = !m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty;
        }

    }
}
