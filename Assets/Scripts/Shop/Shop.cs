using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private ShopInfo m_shopInfo;
        public ShopInfo ShopInfo => m_shopInfo;

        private IItem[] m_shopItems;
        public IItem[] ShopItems => m_shopItems;

        public event UnityAction EventOnNotEnoughMoneyForItem;
        public event UnityAction EventOnNotHavingPlaceForItem;
        public event UnityAction EventOnItemPurchaseCompleted;

        public void BuyItem(object sender, PlayerCharacter playerCharacter, IItem item)
        {
            if (playerCharacter.Money < item.Price)
            {
                EventOnNotEnoughMoneyForItem?.Invoke();
                return;
            }
            if (playerCharacter.Inventory.TryToAddItem(sender, item) == false)
            {
                EventOnNotHavingPlaceForItem?.Invoke();
                return;
            }

            playerCharacter.SpendMoney(item.Price);
            EventOnItemPurchaseCompleted?.Invoke();
        }

        private void Start()
        {
            m_shopItems = new IItem[m_shopInfo.ShopItemsData.Length];

            for (int i = 0; i < m_shopInfo.ShopItemsData.Length; i++)
            {
                m_shopItems[i] = m_shopInfo.ShopItemsData[i].CreateItem();
            }
        }
    }
}
