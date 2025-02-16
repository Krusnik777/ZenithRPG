using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [System.Serializable]
    public class ItemData
    {
        public ItemInfo ItemInfo { get; set; }

        public string ItemInfoId;
        /// <summary>
        /// For some item types: Amount = Uses
        /// </summary>
        public int Amount;

        public ItemData(ItemInfo itemInfo, int amount)
        {
            ItemInfoId = itemInfo.ID;
            ItemInfo = ItemInfosDataBase.Instance.GetItemInfoFromId(ItemInfoId);
            Amount = amount;
        }

        public ItemData(IItem item)
        {
            if (item != null)
            {
                ItemInfoId = item.Info.ID;
                ItemInfo = ItemInfosDataBase.Instance.GetItemInfoFromId(ItemInfoId);

                if (item is WeaponItem)
                {
                    Amount = (item as WeaponItem).Uses;
                }
                else if (item is MagicItem)
                {
                    Amount = (item as MagicItem).Uses;
                }
                else
                {
                    Amount = item.Amount;
                }
            }
            else
            {
                ItemInfo = null;
                Amount = 0;
            }
        }

        public ItemData(ItemData itemData)
        {
            ItemInfoId = itemData.ItemInfoId;
            ItemInfo = ItemInfosDataBase.Instance.GetItemInfoFromId(ItemInfoId);
            Amount = itemData.Amount;
        }

        public IItem CreateItem()
        {
            if (Amount <= 0) Amount = 1;

            if (ItemInfo == null) ItemInfo = ItemInfosDataBase.Instance.GetItemInfoFromId(ItemInfoId);

            if (ItemInfo is UsableItemInfo) return new UsableItem(ItemInfo as UsableItemInfo, Amount);

            if (ItemInfo is NotUsableItemInfo) return new NotUsableItem(ItemInfo as NotUsableItemInfo, Amount);

            if (ItemInfo is EquipItemInfo) return new EquipItem(ItemInfo as EquipItemInfo);

            if (ItemInfo is WeaponItemInfo) return new WeaponItem(ItemInfo as WeaponItemInfo, Amount);

            if (ItemInfo is MagicItemInfo) return new MagicItem(ItemInfo as MagicItemInfo, Amount);

            return null;
        }

        public void Reset()
        {
            ItemInfoId = "";
            ItemInfo = null;
            Amount = 0;
        }
    }

    public interface IItem
    {
        ItemInfo Info { get;}
        int Amount { get; set; }
        int MaxAmount { get; }

        int Price { get; }

        IItem Clone();

        void Use(object sender, Inventory inventory, IItemSlot slot, UnityAction<object, IItemSlot> onUse = null);
    }
}
