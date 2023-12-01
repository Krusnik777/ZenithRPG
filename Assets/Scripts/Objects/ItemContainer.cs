using UnityEngine;

namespace DC_ARPG
{
    public class ItemContainer : InspectableObject
    {
        [SerializeField] private ItemInfo m_itemInfo;
        [SerializeField] private int m_amount;

        protected IItem m_item;

        public override void OnInspection(Player player)
        {
            if (m_item == null)
            {
                ShortMessage.Instance.ShowMessage("Пусто.");
                Destroy(gameObject);
                return;
            }

            if (player.Character.Inventory.TryToAddItem(this, m_item) == true)
            {
                ShortMessage.Instance.ShowMessage("Добавлено в инвентарь: " + m_item.Info.Title + ".");
                base.OnInspection(player);

                Destroy(gameObject);
            }
        }

        public void AssignItem(IItem item)
        {
            m_item = item.Clone();
        }

        private void Start()
        {
            if (m_item == null) m_item = CreateItem();
        }

        private IItem CreateItem()
        {
            if (m_itemInfo == null) return null;

            if (m_amount <= 0) m_amount = 1;

            if (m_itemInfo is UsableItemInfo) return new UsableItem(m_itemInfo as UsableItemInfo, m_amount);

            if (m_itemInfo is NotUsableItemInfo) return new NotUsableItem(m_itemInfo as NotUsableItemInfo, m_amount);

            if (m_itemInfo is EquipItemInfo) return new EquipItem(m_itemInfo as EquipItemInfo);

            if (m_itemInfo is WeaponItemInfo) return new WeaponItem(m_itemInfo as WeaponItemInfo, m_amount);

            if (m_itemInfo is MagicItemInfo) return new MagicItem(m_itemInfo as MagicItemInfo, m_amount);

            return null;
        }

    }
}
