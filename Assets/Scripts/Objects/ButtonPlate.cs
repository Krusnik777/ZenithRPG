using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(Animator))]
    public class ButtonPlate : InspectableObject, IDataPersistence
    {
        [SerializeField] private ButtonPressTrigger m_buttonPressTrigger;
        public ButtonPressTrigger PressTrigger => m_buttonPressTrigger;

        public void SetButtonActive(bool state) => m_buttonPressTrigger.SetButtonActive(state);

        public void PressButton() => m_buttonPressTrigger.PressButton();

        public void UnpressButton() => m_buttonPressTrigger.UnpressButton();

        public override bool Disabled => m_buttonPressTrigger.ButtonDisabled;

        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Нажимная плита.");

            base.OnInspection(player);
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
                s.animatorState = m_buttonPressTrigger.Animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
                s.triggerDisabled = m_buttonPressTrigger.ButtonDisabled;
            }

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            gameObject.SetActive(s.enabled);
            if (s.enabled)
            {
                m_buttonPressTrigger.Animator.Play(s.animatorState);
                m_buttonPressTrigger.ButtonDisabled = s.triggerDisabled;
            }
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
