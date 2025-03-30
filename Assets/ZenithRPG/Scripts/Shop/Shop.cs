using UnityEngine;

namespace DC_ARPG
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private ShopInfo m_shopInfo;
        [SerializeField] private int m_defaultPrice = 10;
        public ShopInfo ShopInfo => m_shopInfo;
        public int DefaultPrice => m_defaultPrice;

        private Shopkeeper shopkeeper;
        public Shopkeeper Shopkeeper => shopkeeper;

        private IItem[] m_shopItems;
        public IItem[] ShopItems => m_shopItems;

        private void Start()
        {
            m_shopItems = new IItem[m_shopInfo.ShopItemsData.Length];

            for (int i = 0; i < m_shopInfo.ShopItemsData.Length; i++)
            {
                m_shopItems[i] = m_shopInfo.ShopItemsData[i].CreateItem();
            }

            shopkeeper = new Shopkeeper(m_shopInfo.Shopkeeper);
        }
    }
}
