namespace DC_ARPG
{
    public interface IItem
    {
        ItemInfo Info { get;}
        int Amount { get; set; }
        int MaxAmount { get; }

        int Price { get; }

        IItem Clone();
    }
}
