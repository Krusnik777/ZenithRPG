using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(Animator))]
    public class ShopDoor : InspectableObject
    {
        private PositionTrigger m_positionTrigger;

        public event UnityAction EventOnShopEntered;
        public event UnityAction EventOnShopExited;

        public bool StandingInFrontOfShopDoor => m_positionTrigger != null ? m_positionTrigger.InRightPosition : false;

        private Animator m_animator;

        private bool inClosedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosedState") : true;
        private bool inOpenedState => m_animator != null ? m_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenedState") : false;

        public override void OnInspection(Player player)
        {
            if (!StandingInFrontOfShopDoor)
            {
                // Not Possible but just to be sure

                ShortMessage.Instance.ShowMessage("Дверь. С этой стороны не открыть.");
                return;
            }

            if (inClosedState) EnterShop();
            if (inOpenedState) return; // TEMP

            EventOnInspection?.Invoke();
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_positionTrigger = GetComponentInChildren<PositionTrigger>();
        }

        private void EnterShop()
        {
            m_animator.SetTrigger("Open");

            EventOnShopEntered?.Invoke(); // -> startShop
        }

        private void OnShopExited()
        {
            m_animator.SetTrigger("Close");

            EventOnShopExited?.Invoke();
        }
    }
}
