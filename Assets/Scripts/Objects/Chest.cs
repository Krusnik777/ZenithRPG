using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(Animator))]
    public class Chest : ItemContainer
    {
        [Header("Chest")]
        [SerializeField] private Animator m_animator;
        [SerializeField] private bool m_locked;
        [SerializeField] private bool m_requireSpecialKey;
        [SerializeField] private UsableItemInfo m_specificKeyItemInfo;
        public bool Locked => m_locked;
        public bool RequireSpecialKey => m_requireSpecialKey;
        public UsableItemInfo SpecificKeyItemInfo => m_specificKeyItemInfo;

        public IItem Item => m_item;

        private PositionTrigger m_positionTrigger;
        public bool StandingInFrontOfChest => m_positionTrigger != null ? m_positionTrigger.InRightPosition : false;

        
        private ChestSFX m_chestSFX;

        private bool inClosedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosedState");
        public bool Closed => inClosedState;
        private bool inOpenedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenedState");
        public bool Opened => inOpenedState;

        public void Lock()
        {
            m_locked = true;
            if (inOpenedState) Close();
        }

        public void Unlock()
        {
            m_locked = false;

            m_chestSFX.PlayUnlockedSound();

            if (StandingInFrontOfChest) ShortMessage.Instance.ShowMessage("Открыто.");
        }

        public void Close()
        {
            m_animator.SetTrigger("Close");
            m_chestSFX.PlayCloseSound();
        }

        public override void OnInspection(Player player)
        {
            if (!StandingInFrontOfChest)
            {
                ShortMessage.Instance.ShowMessage("Сундук. С этой стороны не открыть.");
                return;
            }

            if (m_locked)
            {
                if (!m_requireSpecialKey) ShortMessage.Instance.ShowMessage("Закрыто.");
                else ShortMessage.Instance.ShowMessage("Закрыто на необычный замок.");

                m_chestSFX.PlayLockedSound();

                return;
            }

            if (inClosedState) Open(player);
            if (inOpenedState) Close();
        }

        private void Awake()
        {
            m_chestSFX = GetComponentInChildren<ChestSFX>();
            m_positionTrigger = GetComponentInChildren<PositionTrigger>();
        }

        private void Open(Player player)
        {
            m_animator.SetTrigger("Open");
            m_chestSFX.PlayOpenSound();

            if (m_item == null)
            {
                ShortMessage.Instance.ShowMessage("Пусто.");
                return;
            }

            if (player.Character.Inventory.TryToAddItem(this, m_item) == true)
            {
                ShortMessage.Instance.ShowMessage("Добавлено в инвентарь: " + m_item.Info.Title + ".");

                UISounds.Instance.PlayItemCollectedSound();

                m_itemData.ItemInfo = null;
                m_itemData.Amount = 0;
                m_item = null;

                EventOnInspection?.Invoke();
            }
            else
            {
                ShortMessage.Instance.ShowMessage("Нет места в инвентаре.");
                UISounds.Instance.PlayInventoryActionFailureSound();
                Close();
            }
        }

        #region Serialize

        [System.Serializable]
        public class ChestDataState
        {
            public bool enabled;
            public bool locked;
            public ItemData itemData;
            public int animatorState;

            public ChestDataState() { }
        }
        public override bool IsCreated => false;

        public override string SerializeState()
        {
            ChestDataState s = new ChestDataState();

            s.enabled = gameObject.activeInHierarchy;
            s.locked = m_locked;
            s.itemData = new ItemData(m_itemData);
            if (gameObject.activeInHierarchy)
                s.animatorState = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

            return JsonUtility.ToJson(s);
        }

        public override void DeserializeState(string state)
        {
            ChestDataState s = JsonUtility.FromJson<ChestDataState>(state);

            gameObject.SetActive(s.enabled);
            m_locked = s.locked;
            m_itemData = new ItemData(s.itemData);
            m_item = m_itemData.CreateItem();
            if (s.enabled)
                m_animator.Play(s.animatorState);
        }

        #endregion

    }
}
