using UnityEngine;

namespace DC_ARPG
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private ShopInfo m_shopInfo;
        public ShopInfo ShopInfo => m_shopInfo;

        private ShopInfo clonedShopInfo;

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

            clonedShopInfo = Instantiate(m_shopInfo);

            shopkeeper = clonedShopInfo.Shopkeeper;
        }
    }
}
