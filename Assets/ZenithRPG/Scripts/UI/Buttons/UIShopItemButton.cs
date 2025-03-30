namespace DC_ARPG
{
    public class UIShopItemButton : UISelectableButton
    {
        private UIShopItem uIShopItem => GetComponent<UIShopItem>();
        public UIShopItem UIShopItem => uIShopItem;

        public override void SetFocus()
        {
            if (!m_interactable) return;

            m_selectImage.enabled = true;

            OnSelect?.Invoke();

            UIShop.Instance.ShopItemInfoPanel.ShowInfo(uIShopItem.Item);
        }

        public override void OnButtonClick()
        {
            if (!m_interactable) return;

            base.OnButtonClick();

            UIShop.Instance.BuyItem(this, uIShopItem.Item);
        }
    }
}
