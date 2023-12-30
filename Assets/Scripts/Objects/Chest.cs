using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(Animator))]
    public class Chest : ItemContainer
    {
        [SerializeField] private bool m_locked;
        [SerializeField] private bool m_requireSpecialKey;
        [SerializeField] private UsableItemInfo m_specificKeyItemInfo;
        public bool Locked => m_locked;
        public bool RequireSpecialKey => m_requireSpecialKey;
        public UsableItemInfo SpecificKeyItemInfo => m_specificKeyItemInfo;

        public IItem Item => m_item;

        private PositionTrigger m_positionTrigger;
        public bool StandingInFrontOfChest => m_positionTrigger != null ? m_positionTrigger.InRightPosition : false;

        private Animator m_animator;
        private ChestSFX m_chestSFX;

        private bool inClosedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosedState") : true;
        public bool Closed => inClosedState;
        private bool inOpenedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenedState") : false;
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
            m_animator = GetComponent<Animator>();
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
                m_item = null;

                EventOnInspection?.Invoke();
            }
            else
            {
                ShortMessage.Instance.ShowMessage("Нет места в инвентаре.");
                Close();
            }
        }

        private void Close()
        {
            m_animator.SetTrigger("Close");
            m_chestSFX.PlayCloseSound();
        }
    }
}
