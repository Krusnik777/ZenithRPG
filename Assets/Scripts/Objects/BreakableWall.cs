using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class BreakableWall : InspectableObject, IDataPersistence
    {
        [SerializeField] private GameObject m_wallMiddlePart;
        [SerializeField] private GameObject m_wallBreakEffectPrefab;
        [SerializeField] private Collider m_collider;
        [Header("PositionTriggers")]
        [SerializeField] private PositionTrigger m_forwardPositionTrigger;
        [SerializeField] private PositionTrigger m_backwardPositionTrigger;

        public UnityEvent OnWallBroken;

        public bool StandingInFrontOfWall => m_forwardPositionTrigger.InRightPosition || m_backwardPositionTrigger.InRightPosition;

        public void BreakWall()
        {
            if (m_wallBreakEffectPrefab != null)
            {
                var effect = Instantiate(m_wallBreakEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 1.0f);
            }

            Destroy(m_wallMiddlePart);
            //m_collider.enabled = false;
            Destroy(m_collider.gameObject);

            OnWallBroken?.Invoke();
        }

        public override void OnInspection(Player player)
        {
            if (!StandingInFrontOfWall)
            {
                // Not Possible but just to be sure

                ShortMessage.Instance.ShowMessage("Стена с трещиной. С этой стороны не рассмотреть.");
                return;
            }

            ShortMessage.Instance.ShowMessage("Один сильный удар - и эта стена развалится.");

            base.OnInspection(player);
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool destroyed;

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

            s.destroyed = m_wallMiddlePart == null;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            if (s.destroyed)
            {
                Destroy(m_wallMiddlePart);
                //m_collider.enabled = false;
                Destroy(m_collider.gameObject);
            }
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
