using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(AudioSource))]
    public class UISounds : MonoSingleton<UISounds>
    {
        [SerializeField] private AudioClip m_clickSound;
        [SerializeField] private AudioClip m_hoverSound;
        [SerializeField] private AudioClip m_backSound;
        [Header("Shop")]
        [SerializeField] private AudioClip m_purchaseSound;
        [SerializeField] private AudioClip m_purchaseFailureSound;
        [Header("Inventory")]
        [SerializeField] private AudioClip m_itemCollectedSound;
        [SerializeField] private AudioClip m_equipSound;
        [SerializeField] private AudioClip m_transitSound;
        [SerializeField] private AudioClip m_removedSound;
        [SerializeField] private AudioClip m_inventoryActionFailureSound;
        [SerializeField] private AudioClip m_swordBreakSound;
        [SerializeField] private AudioClip m_magicItemDisappearSound;
        [SerializeField] private AudioClip m_activeItemChangeSound;

        private AudioSource m_audioSource;

        private UIButton[] uIButtons;

        public void PlayBackSound() => m_audioSource.PlayOneShot(m_backSound);
        public void PlayPurchaseSound() => m_audioSource.PlayOneShot(m_purchaseSound);
        public void PlayPurchaseFailureSound() => m_audioSource.PlayOneShot(m_purchaseFailureSound);

        public void PlayItemUsedSound(AudioClip useSound)
        {
            if (useSound != null) m_audioSource.PlayOneShot(useSound);
        }
        public void PlayItemCollectedSound() => m_audioSource.PlayOneShot(m_itemCollectedSound);
        public void PlayItemEquipSound() => m_audioSource.PlayOneShot(m_equipSound);
        public void PlayItemRemovedSound() => m_audioSource.PlayOneShot(m_removedSound);
        public void PlayInventoryActionFailureSound() => m_audioSource.PlayOneShot(m_inventoryActionFailureSound);
        public void PlaySwordBreakSound() => m_audioSource.PlayOneShot(m_swordBreakSound);
        public void PlayMagicItemDisappearSound() => m_audioSource.PlayOneShot(m_magicItemDisappearSound);
        public void PlayActiveItemChangeSound() => m_audioSource.PlayOneShot(m_activeItemChangeSound);

        private void Start()
        {
            m_audioSource = GetComponent<AudioSource>();

            uIButtons = GetComponentsInChildren<UIButton>(true);

            for (int i = 0; i < uIButtons.Length; i++)
            {
                if (uIButtons[i] is UISelectableButton) (uIButtons[i] as UISelectableButton).OnSelect.AddListener(OnSelectButton);

                if (uIButtons[i] is UIInventorySlotButton)
                {
                    var uiInventorySlotButton = uIButtons[i] as UIInventorySlotButton;
                    uiInventorySlotButton.OnButtonMoveClick.AddListener(OnItemMove);
                }
                
                uIButtons[i].OnClick.AddListener(OnPointerClick);

                //uIButtons[i].EventOnPointerEnter += OnPointerEnter;
                //uIButtons[i].EventOnPointerClick += OnPointerClick;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < uIButtons.Length; i++)
            {
                
                if (uIButtons[i] is UISelectableButton) (uIButtons[i] as UISelectableButton).OnSelect.RemoveListener(OnSelectButton);

                if (uIButtons[i] is UIInventorySlotButton)
                {
                    var uiInventorySlotButton = uIButtons[i] as UIInventorySlotButton;
                    uiInventorySlotButton.OnButtonMoveClick.RemoveListener(OnItemMove);
                }
                
                uIButtons[i].OnClick.RemoveListener(OnPointerClick);

                //uIButtons[i].EventOnPointerEnter -= OnPointerEnter;
                //uIButtons[i].EventOnPointerClick -= OnPointerClick;
            }
        }

        private void OnSelectButton()
        {
            m_audioSource.PlayOneShot(m_hoverSound);
        }

        private void OnPointerClick()
        {
            m_audioSource.PlayOneShot(m_clickSound);
        }

        private void OnItemMove()
        {
            m_audioSource.PlayOneShot(m_transitSound);
        }

        /*
        private void OnPointerEnter(UIButton button)
        {
            m_audioSource.PlayOneShot(m_hoverSound);
        }

        private void OnPointerClick(UIButton button)
        {
            m_audioSource.PlayOneShot(m_clickSound);
        }*/
    }
}
