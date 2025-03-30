using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIShop : MonoSingleton<UIShop>, IDependency<ControlsManager>
    {
        public enum ShopState
        {
            Selection,
            Buy,
            Sell,
            Talk
        }

        [Header("ShopBase")]
        [SerializeField] private GameObject m_shopPanel;
        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private GameObject m_shopNamePanel;
        [Header("ShopBuyItems")]
        [SerializeField] private GameObject m_shopButtonsPanel;
        [SerializeField] private GameObject m_shopItemsPanel;
        [SerializeField] private GameObject m_buttonsInfoPanel;
        [SerializeField] private UIShopItemInfoPanel m_uIShopItemInfoPanel;
        [Header("Other")]
        [SerializeField] private ShopkeeperSpeech m_shopkeeperSpeech;
        [SerializeField] private GameObject m_hudRightPanel;
        [SerializeField] private UIInventory m_uiInventory;

        public event UnityAction EventOnShopExit;

        private ShopState m_shopState;
        public ShopState State => m_shopState;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        public UIShopItemInfoPanel ShopItemInfoPanel => m_uIShopItemInfoPanel;

        public ShopkeeperSpeech ShopkeeperSpeech => m_shopkeeperSpeech;

        private Shop m_shop;
        public Shop Shop => m_shop;

        private UISelectableButtonContainer m_baseShopButtonContainer;
        private UISelectableButtonContainer m_shopItemsButtonContainer;

        public UISelectableButtonContainer ActiveButtonContainer { get; private set; }

        private UIShopItem[] m_uIShopItems;

        private Coroutine exitCoroutine;

        public void BuyItem(object sender, IItem item)
        {
            int index = 0;

            int price = (int)(m_shop.ShopInfo.Surcharge * item.Price) + item.Price;

            if (price <= 0) price = ((int)(m_shop.ShopInfo.Surcharge * m_shop.DefaultPrice) + m_shop.DefaultPrice) * item.Amount;

            if ((m_uiInventory.Player.Character as PlayerCharacter).Money < price)
            {
                index = Random.Range(0, m_shop.Shopkeeper.NotEnoughMoneyLines.Count);
                m_shopkeeperSpeech.ShowShortPhrase(m_shop.Shopkeeper.NotEnoughMoneyLines[index]);

                UISounds.Instance.PlayPurchaseFailureSound();

                return;
            }

            if (m_uiInventory.Inventory.TryToAddItem(sender, item) == false)
            {
                index = Random.Range(0, m_shop.Shopkeeper.NoPlaceLines.Count);
                m_shopkeeperSpeech.ShowShortPhrase(m_shop.Shopkeeper.NoPlaceLines[index]);

                UISounds.Instance.PlayPurchaseFailureSound();

                return;
            }

            (m_uiInventory.Player.Character as PlayerCharacter).SpendMoney(price);

            index = Random.Range(0, m_shop.Shopkeeper.PurchaseLines.Count);
            m_shopkeeperSpeech.ShowShortPhrase(m_shop.Shopkeeper.PurchaseLines[index]);

            UISounds.Instance.PlayPurchaseSound();
        }

        public void SellItem(object sender, PlayerCharacter playerCharacter, IItemSlot slot)
        {
            if (slot.IsEmpty) return;

            int index = 0;

            if (slot.Item.Price <= 0)
            {
                index = Random.Range(0, m_shop.Shopkeeper.SellFailureLines.Count);
                m_shopkeeperSpeech.ShowShortPhrase(m_shop.Shopkeeper.SellFailureLines[index]);

                UISounds.Instance.PlayPurchaseFailureSound();

                return;
            }

            playerCharacter.AddMoney(slot.Item.Price);
            playerCharacter.Inventory.RemoveItemFromInventory(sender, slot);

            index = Random.Range(0, m_shop.Shopkeeper.SellLines.Count);
            m_shopkeeperSpeech.ShowShortPhrase(m_shop.Shopkeeper.SellLines[index]);

            UISounds.Instance.PlayPurchaseSound();
        }

        public void ShowShop(Shop shop)
        {
            m_controlsManager.SetPlayerControlsActive(false);
            m_controlsManager.SetShopControlsActive(true);

            m_shop = shop;

            m_shopPanel.SetActive(true);
            m_backgroundImage.sprite = shop.ShopInfo.BackgroundImage;

            m_shopNamePanel.SetActive(true);
            m_shopNamePanel.GetComponentInChildren<TextMeshProUGUI>().text = shop.ShopInfo.ShopName;

            m_shopButtonsPanel.SetActive(true);

            for (int i = 0; i < m_uIShopItems.Length; i++)
            {
                m_uIShopItems[i].SetShopItem(shop, shop.ShopItems[i]);
            }

            m_buttonsInfoPanel.SetActive(false);

            m_hudRightPanel.SetActive(false);

            m_uiInventory.ShowInventory();
            m_uiInventory.SetInventoryButtonsInfoPanel(false);
            m_uiInventory.SetState(UIInventory.InteractionState.Shop);

            m_shopState = ShopState.Selection;

            m_baseShopButtonContainer.enabled = true;
            m_shopItemsButtonContainer.enabled = false;
            m_uiInventory.SetInventoryButtonContainer(false);

            ActiveButtonContainer = m_baseShopButtonContainer;

            int index = Random.Range(0, m_shop.Shopkeeper.WelcomeLines.Count);
            m_shopkeeperSpeech.ShowShortPhrase(m_shop.Shopkeeper.WelcomeLines[index]);

            MusicCommander.Instance.PlayShopTheme();
        }

        public void BuyItems()
        {
            m_shopNamePanel.SetActive(false);
            m_shopButtonsPanel.SetActive(false);

            m_buttonsInfoPanel.SetActive(true);

            m_uIShopItemInfoPanel.gameObject.SetActive(true);

            m_shopState = ShopState.Buy;

            m_baseShopButtonContainer.enabled = false;
            m_shopItemsButtonContainer.enabled = true;

            ActiveButtonContainer = m_shopItemsButtonContainer;
        }

        public void SellItems()
        {
            m_shopNamePanel.SetActive(false);
            m_shopButtonsPanel.SetActive(false);

            m_uiInventory.SetInventoryButtonsInfoPanel(true);

            m_shopState = ShopState.Sell;

            m_baseShopButtonContainer.enabled = false;
            m_uiInventory.SetInventoryButtonContainer(true);

            ActiveButtonContainer = m_uiInventory.UISlotButtonsContainer;
        }

        public void StartTalk()
        {
            m_shopkeeperSpeech.StartSpeech(m_shop.Shopkeeper);

            m_shopState = ShopState.Talk;
        }

        public void EndTalk() => m_shopState = ShopState.Selection;

        public void ExitShop()
        {
            if (exitCoroutine != null) return;

            exitCoroutine = StartCoroutine(ExitShopWithPhrase());
        }

        public void ReturnToSelection()
        {
            if (m_shopState == ShopState.Talk || (m_shopState == ShopState.Selection)) return;

            m_shopNamePanel.SetActive(true);
            m_shopButtonsPanel.SetActive(true);

            if (m_shopState == ShopState.Sell)
            {
                m_uiInventory.SetInventoryButtonsInfoPanel(false);
            }

            if (m_shopState == ShopState.Buy)
            {
                m_buttonsInfoPanel.SetActive(false);
                m_uIShopItemInfoPanel.gameObject.SetActive(false);
            }

            m_shopState = ShopState.Selection;

            m_baseShopButtonContainer.enabled = true;
            m_shopItemsButtonContainer.enabled = false;
            m_uiInventory.SetInventoryButtonContainer(false);

            ActiveButtonContainer = m_baseShopButtonContainer;
        }

        private void Start()
        {
            m_baseShopButtonContainer = m_shopButtonsPanel.GetComponent<UISelectableButtonContainer>();
            m_shopItemsButtonContainer = m_shopItemsPanel.GetComponent<UISelectableButtonContainer>();
            m_uIShopItems = m_shopItemsPanel.GetComponentsInChildren<UIShopItem>();

            ActiveButtonContainer = m_baseShopButtonContainer;
        }

        #region Coroutines

        private IEnumerator ExitShopWithPhrase()
        {
            int index = Random.Range(0, m_shop.ShopInfo.Shopkeeper.FarewellLines.Count);
            m_shopkeeperSpeech.ShowShortPhrase(m_shop.ShopInfo.Shopkeeper.FarewellLines[index]);

            var timer = 0f;

            while (timer < m_shopkeeperSpeech.ShortPhraseTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            m_shopPanel.SetActive(false);

            m_uiInventory.HideInventory();
            m_uiInventory.SetInventoryButtonsInfoPanel(true);
            m_uiInventory.SetState(UIInventory.InteractionState.Normal);

            m_hudRightPanel.SetActive(true);

            m_shop = null;

            m_controlsManager.SetShopControlsActive(false);
            m_controlsManager.SetPlayerControlsActive(true);

            exitCoroutine = null;

            EventOnShopExit?.Invoke();

            MusicCommander.Instance.PlayCurrentSceneBGM();
        }

        #endregion

    }
}
