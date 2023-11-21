namespace DC_ARPG
{
    public class InventoryPocket
    {
        public enum PocketType
        {
            Main,
            Extra
        }

        public AnyItemSlot[] ItemSlots { get; private set; }
        public bool IsFull => CheckFullness();
        public PocketType Type { get; private set; }

        public InventoryPocket(int itemSlotsAmount, PocketType type)
        {
            ItemSlots = new AnyItemSlot[itemSlotsAmount];
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                ItemSlots[i] = new AnyItemSlot();
            }
            Type = type;
        }

        private bool CheckFullness()
        {
            foreach (var slot in ItemSlots)
            {
                if (slot.IsEmpty)
                    return false;
            }
            return true;
        }
    }
}
