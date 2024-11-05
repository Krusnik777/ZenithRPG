using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(Animator))]
    public class ButtonPlate : InspectableObject, IActivableObject, IReturnableObject, IDataPersistence
    {
        [SerializeField] private bool m_hasSpring;
        [SerializeField] private AudioSource m_audioSFX;
        [SerializeField] private Animator m_animator;
        [Space]
        public UnityEvent OnButtonPressed;
        public UnityEvent OnButtonUnpressed;

        public override bool Disabled => disabled;

        private bool disabled;

        private bool inPressedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("PressedState");
        private bool inUnpressedState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("UnpressedState");

        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Нажимная плита.");

            base.OnInspection(player);
        }

        public void Activate(IMovable movable = null)
        {
            PressButton();
        }

        public void ReturnToDefault()
        {
            if (m_hasSpring) UnpressButton();
        }

        public void SetButtonActive(bool state)
        {
            disabled = !state;

            if (disabled)
            {
                if (inUnpressedState)
                {
                    m_animator.SetTrigger("Press");
                    m_audioSFX.Play();
                }
            }
            else
            {
                if (inPressedState && m_hasSpring)
                {
                    m_animator.SetTrigger("Unpress");
                    m_audioSFX.Play();
                }
            }
        }

        public void PressButton()
        {
            if (disabled) return;

            if (inUnpressedState)
            {
                m_animator.SetTrigger("Press");
                m_audioSFX.Play();
                OnButtonPressed?.Invoke();
            }
        }

        public void UnpressButton()
        {
            if (disabled) return;

            if (inPressedState)
            {
                m_animator.SetTrigger("Unpress");
                m_audioSFX.Play();
                OnButtonUnpressed?.Invoke();
            }
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public bool triggerDisabled;
            public int animatorState;

            public DataState() { }
        }

        [Header("Serialize")]
        [SerializeField] private string m_prefabId;
        [SerializeField] private string m_id;
        [SerializeField] private bool m_isSerializable = true;
        public string PrefabId => m_prefabId;
        public string EntityId => m_id;
        public bool IsCreated => false;

        public bool IsSerializable() => m_isSerializable;

        public string SerializeState()
        {
            DataState s = new DataState();

            s.enabled = gameObject.activeInHierarchy;
            if (gameObject.activeInHierarchy)
            {
                s.animatorState = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
                s.triggerDisabled = disabled;
            }

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
            if (s.enabled)
            {
                m_animator.Play(s.animatorState);
                disabled = s.triggerDisabled;
            }
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
