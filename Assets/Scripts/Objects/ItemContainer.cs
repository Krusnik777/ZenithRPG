using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class ItemData
    {
        public ItemInfo ItemInfo;
        /// <summary>
        /// For some items Amount = Uses
        /// </summary>
        public int Amount;

        public IItem CreateItem()
        {
            if (Amount <= 0) Amount = 1;

            if (ItemInfo is UsableItemInfo) return new UsableItem(ItemInfo as UsableItemInfo, Amount);

            if (ItemInfo is NotUsableItemInfo) return new NotUsableItem(ItemInfo as NotUsableItemInfo, Amount);

            if (ItemInfo is EquipItemInfo) return new EquipItem(ItemInfo as EquipItemInfo);

            if (ItemInfo is WeaponItemInfo) return new WeaponItem(ItemInfo as WeaponItemInfo, Amount);

            if (ItemInfo is MagicItemInfo) return new MagicItem(ItemInfo as MagicItemInfo, Amount);

            return null;
        }
    }

    public class ItemContainer : InspectableObject
    {
        [SerializeField] private ItemData m_itemData;

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
            else ShortMessage.Instance.ShowMessage("Нет места в инвентаре.");
        }

        public void AssignItem(IItem item)
        {
            m_item = item.Clone();
        }

        private void Start()
        {
            if (m_item == null) m_item = m_itemData.CreateItem();
        }
    }
}
