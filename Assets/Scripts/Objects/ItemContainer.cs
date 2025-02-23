using UnityEngine;

namespace DC_ARPG
{
    public class ItemContainer : InspectableObject, IDataPersistence
    {
        [Header("Start Item")]
        [SerializeField] protected ItemInfo m_itemInfo;
        [SerializeField] protected int m_amount;
        
        protected ItemData m_itemData;

        protected IItem m_item;

        public override void OnInspection(Player player)
        {
            if (m_item == null)
            {
                ShortMessage.Instance.ShowMessage("Пусто.");
                Destroy(gameObject);
                return;
            }

            if ((player.Character as PlayerCharacter).Inventory.TryToAddItem(this, m_item) == true)
            {
                if ((m_item is UsableItem || m_item is NotUsableItem) && m_item.Amount > 1)
                    ShortMessage.Instance.ShowMessage("Добавлено в инвентарь: " + m_item.Info.Title + " x" + m_item.Amount + ".");
                else
                    ShortMessage.Instance.ShowMessage("Добавлено в инвентарь: " + m_item.Info.Title + ".");

                UISounds.Instance.PlayItemCollectedSound();

                EventOnInspection?.Invoke();

                Destroy(gameObject);
            }
            else
            {
                ShortMessage.Instance.ShowMessage("Нет места в инвентаре.");
                UISounds.Instance.PlayInventoryActionFailureSound();
            }
        }

        public virtual void AssignItem(IItem item)
        {
            m_item = item.Clone();

            m_itemData = new ItemData(m_item);
            m_itemInfo = m_itemData.ItemInfo;
        }

        private void Start()
        {
            if (m_item == null)
            {
                if (m_itemData == null)
                {
                    m_itemData = new ItemData(m_itemInfo, m_amount);

                    m_item = m_itemData.CreateItem();
                }
                else
                {
                    m_item = m_itemData.CreateItem();
                }
            }
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public Vector3 position;
            public ItemData itemData;

            public DataState() { }
        }

        [Header("Serialize")]
        [SerializeField] private string m_prefabId;
        [SerializeField] private string m_id;
        [SerializeField] private bool m_isSerializable = true;
        private bool m_isCreated = false;
        public string PrefabId => m_prefabId;
        public string EntityId => m_id;
        public virtual bool IsCreated => m_isCreated;

        public bool IsSerializable() => m_isSerializable;

        public virtual string SerializeState()
        {
            DataState s = new DataState();

            s.enabled = gameObject.activeInHierarchy;

            if (m_isCreated)
            {
                s.position = transform.position;
                if (m_itemData != null) s.itemData = new ItemData(m_itemData);
                else s.itemData = new ItemData(m_itemInfo, m_amount);
            }

            return JsonUtility.ToJson(s);
        }

        public virtual void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            if (m_isCreated)
            {
                transform.position = s.position;
                m_itemData = new ItemData(s.itemData);
                m_item = m_itemData.CreateItem();
                m_itemInfo = m_itemData.ItemInfo;
            }

            gameObject.SetActive(s.enabled);
        }

        public void SetupCreatedContainer()
        {
            m_id = "CreatedContainer_";
            m_id = m_id + System.Guid.NewGuid().ToString();
            m_isCreated = true;
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state)
        {
            m_id = entityId;
            m_isCreated = isCreated;
            DeserializeState(state);
        }

        #endregion
    }
}
