using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(DoorAnimator),typeof(AudioSource))]
    public class ShopDoor : InspectableObject
    {
        //[SerializeField] private Animator m_animator;
        [SerializeField] private DoorAnimator m_animator;

        public UnityEvent EventOnShopEntered;
        public UnityEvent EventOnShopExited;

        private Shop m_shop;

        private PositionTrigger m_positionTrigger;

        public bool StandingInFrontOfShopDoor => m_positionTrigger != null ? m_positionTrigger.InRightPosition : false;

        private AudioSource m_audio;

        //private bool inClosedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("ClosedState");
        private bool inClosedState => m_animator.InInitState;
        //private bool inOpenedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("OpenedState");
        private bool inOpenedState => m_animator.InActiveState;

        public override void OnInspection(Player player)
        {
            if (!StandingInFrontOfShopDoor)
            {
                // Not Possible but just to be sure

                ShortMessage.Instance.ShowMessage("�����. � ���� ������� �� �������.");
                return;
            }

            if (inClosedState) EnterShop();
            if (inOpenedState) return; // just to be safe

            EventOnInspection?.Invoke();
        }

        private void Awake()
        {
            m_audio = GetComponent<AudioSource>();
            m_shop = GetComponent<Shop>();
            m_positionTrigger = GetComponentInChildren<PositionTrigger>();
        }

        private void Start()
        {
            UIShop.Instance.EventOnShopExit += OnShopExited;
        }

        private void OnDestroy()
        {
            UIShop.Instance.EventOnShopExit -= OnShopExited;
        }

        private void EnterShop()
        {
            //m_animator.SetTrigger("Open");
            m_animator.Play();
            m_audio.Play();

            UIShop.Instance.ShowShop(m_shop);

            LevelState.Instance.StopAllActivity();

            EventOnShopEntered?.Invoke();
        }

        private void OnShopExited()
        {
            //m_animator.SetTrigger("Close");
            m_animator.ResetToInit();
            m_audio.Play();

            LevelState.Instance.ResumeAllActivity();

            EventOnShopExited?.Invoke();
        }
    }
}
