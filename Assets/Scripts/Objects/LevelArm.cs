using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(LevelArmAnimator))]
    public class LevelArm : InspectableObject, IDataPersistence
    {
        //[SerializeField] private Animator m_animator;
        [SerializeField] private LevelArmAnimator m_animator;
        [SerializeField] private AudioSource m_audioSFX;
        [SerializeField] private bool m_canReset;
        public bool CanReset => m_canReset;

        public UnityEvent OnUsed;
        public UnityEvent OnReseted;

        //private bool inUpperState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("UpperState");
        private bool inUpperState => m_animator.InInitState;
        public bool Used => inLoweredState;
        //private bool inLoweredState => m_animator.GetCurrentAnimatorStateInfo(0).IsName("LoweredState");
        private bool inLoweredState => m_animator.InActiveState;

        public override string InfoText
        {
            get
            {
                if (!m_canReset && Used)
                {
                    m_infoText = string.Empty;
                }
                else m_infoText = "Использовать";

                return m_infoText;
            }
        }

        public override void OnInspection(Player player)
        {
            if (inUpperState) UseLevelArm();
            if (inLoweredState) ResetLevelArm();

            EventOnInspection?.Invoke();
        }

        private void UseLevelArm()
        {
            if (m_audioSFX != null) m_audioSFX.Play();
            //m_animator.SetTrigger("Use");
            m_animator.Play();
            OnUsed?.Invoke();
        }

        private void ResetLevelArm()
        {
            if (!m_canReset) return;

            if (m_audioSFX != null) m_audioSFX.Play();
            //m_animator.SetTrigger("Reset");
            m_animator.ResetToInit();
            OnReseted?.Invoke();
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            //public int animatorState;
            public bool animatorActiveState;

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
                //s.animatorState = m_animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
                s.animatorActiveState = m_animator.InActiveState;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
            if (s.enabled)
                //m_animator.Play(s.animatorState);
                m_animator.ChangeInitialState(s.animatorActiveState);
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion

    }
}
