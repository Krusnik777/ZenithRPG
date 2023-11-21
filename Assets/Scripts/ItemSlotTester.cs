using UnityEngine;

namespace DC_ARPG
{
    public class ItemSlotTester : MonoBehaviour
    {
        [SerializeField] private UsableItem usableItem;
        [SerializeField] private WeaponItem weaponItem;

        private void Start()
        {
            AnyItemSlot[] itemSlots = new AnyItemSlot[2] { new AnyItemSlot(), new AnyItemSlot() };
            UsableItemSlot usableItemSlot = new UsableItemSlot();

            itemSlots[0].SetItemInSlot(usableItem);
            itemSlots[1].SetItemInSlot(weaponItem);

            usableItemSlot.SetItemInSlot(usableItem);
        }
    }
}
