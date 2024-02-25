namespace DC_ARPG
{
    [System.Serializable]
    public class ItemData
    {
        public ItemInfo ItemInfo;
        /// <summary>
        /// For some item types: Amount = Uses
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

    public interface IItem
    {
        ItemInfo Info { get;}
        int Amount { get; set; }
        int MaxAmount { get; }

        int Price { get; }

        IItem Clone();
    }
}
