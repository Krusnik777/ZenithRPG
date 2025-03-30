using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace DC_ARPG
{
    public class EquipmentMeshUpdater : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter m_playerCharacter;
        [SerializeField] private GameObject m_swordMesh;
        [SerializeField] private GameObject m_brokenWeaponMesh;
        [SerializeField] private GameObject m_shieldMesh;
        [SerializeField] private RigBuilder m_rigBuilder;

        private void Start()
        {
            if (!m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty)
                m_brokenWeaponMesh.SetActive(m_playerCharacter.Inventory.WeaponItemSlot.ItemInfo == m_playerCharacter.BrokenWeapon.Info);
            else
                m_brokenWeaponMesh.SetActive(false);

            if (!m_brokenWeaponMesh.activeInHierarchy)
                m_swordMesh.SetActive(!m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty);
            else
                m_swordMesh.SetActive(false);

            m_rigBuilder.layers[1].active = m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty;

            m_shieldMesh.SetActive(!m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty);

            m_rigBuilder.layers[0].active = m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty;

            m_playerCharacter.Inventory.EventOnTransitCompleted += OnEquipItemChange;
            m_playerCharacter.Inventory.EventOnItemRemoved += OnEquipItemRemoved;
            m_playerCharacter.Inventory.WeaponItemSlot.EventOnBrokenWeapon += OnBrokenWeapon;
        }

        private void OnDestroy()
        {
            m_playerCharacter.Inventory.EventOnTransitCompleted -= OnEquipItemChange;
            m_playerCharacter.Inventory.EventOnItemRemoved -= OnEquipItemRemoved;
            m_playerCharacter.Inventory.WeaponItemSlot.EventOnBrokenWeapon -= OnBrokenWeapon;
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

            m_rigBuilder.layers[1].active = m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty;

            if (slot == m_playerCharacter.Inventory.ShieldItemSlot)
                m_shieldMesh.SetActive(!m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty);

            m_rigBuilder.layers[0].active = m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty;
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

            m_rigBuilder.layers[1].active = m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty;

            if (fromSlot == m_playerCharacter.Inventory.ShieldItemSlot || toSlot == m_playerCharacter.Inventory.ShieldItemSlot)
                m_shieldMesh.SetActive(!m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty);

            m_rigBuilder.layers[0].active = m_playerCharacter.Inventory.ShieldItemSlot.IsEmpty;
        }

        private void OnBrokenWeapon(object sender)
        {
            m_brokenWeaponMesh.SetActive(m_playerCharacter.Inventory.WeaponItemSlot.ItemInfo == m_playerCharacter.BrokenWeapon.Info);

            if (!m_brokenWeaponMesh.activeInHierarchy)
            {
                m_swordMesh.SetActive(!m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty);
            }
            else
                m_swordMesh.SetActive(false);

            m_rigBuilder.layers[1].active = m_playerCharacter.Inventory.WeaponItemSlot.IsEmpty;
        }

    }
}
