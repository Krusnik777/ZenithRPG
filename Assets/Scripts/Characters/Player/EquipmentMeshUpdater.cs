using UnityEngine;

namespace DC_ARPG
{
    public class EquipmentMeshUpdater : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter m_playerCharacter;
        [SerializeField] private GameObject m_swordMesh;
        [SerializeField] private GameObject m_brokenWeaponMesh;
        [SerializeField] private GameObject m_shieldMesh;

        private void Start()
        {
            if (!m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty)
                m_brokenWeaponMesh.SetActive(m_playerCharacter.Inventory.ShieldItemSlot.ItemInfo == m_playerCharacter.BrokenWeapon.Info);
            else
                m_brokenWeaponMesh.SetActive(false);

            if (!m_brokenWeaponMesh.activeInHierarchy)
                m_swordMesh.SetActive(!m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty);
            else
                m_swordMesh.SetActive(false);

            m_shieldMesh.SetActive(!m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty);

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
            m_brokenWeaponMesh.SetActive(m_playerCharacter.Inventory.WeaponItemSlot.ItemInfo == m_playerCharacter.BrokenWeapon.Info);

            if (!m_brokenWeaponMesh.activeInHierarchy)
            {
                if (slot == m_playerCharacter.Inventory.WeaponItemSlot)
                    m_swordMesh.SetActive(!m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty);
            }
            else
                m_swordMesh.SetActive(false);

            if (slot == m_playerCharacter.Inventory.ShieldItemSlot)
                m_shieldMesh.SetActive(!m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty);
        }

        private void OnEquipItemChange(object sender, IItemSlot fromSlot, IItemSlot toSlot)
        {
            m_brokenWeaponMesh.SetActive(m_playerCharacter.Inventory.WeaponItemSlot.ItemInfo == m_playerCharacter.BrokenWeapon.Info);

            if (!m_brokenWeaponMesh.activeInHierarchy)
            {
                if (fromSlot == m_playerCharacter.Inventory.WeaponItemSlot || toSlot == m_playerCharacter.Inventory.WeaponItemSlot)
                    m_swordMesh.SetActive(!m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty);
            }
            else
                m_swordMesh.SetActive(false);

            if (fromSlot == m_playerCharacter.Inventory.ShieldItemSlot || toSlot == m_playerCharacter.Inventory.ShieldItemSlot)
                m_shieldMesh.SetActive(!m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty);

            
        }

    }
}
