using UnityEngine;

namespace DC_ARPG
{
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

                UISounds.Instance.PlayItemCollectedSound();

                EventOnInspection?.Invoke();

                Destroy(gameObject);
            }
            else
            {
                ShortMessage.Instance.ShowMessage("Нет места в инвентаре.");
                UISounds.Instance.PlayInventoryActionFailureSound();
            }
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
