using UnityEngine;

namespace DC_ARPG
{
    public class ItemContainer : InspectableObject, IDataPersistence
    {
        [SerializeField] protected ItemData m_itemData;

        protected IItem m_item;

        public override void OnInspection(Player player)
        {
            if (m_item == null)
            {
                ShortMessage.Instance.ShowMessage("Пусто.");
                Destroy(gameObject);
                return;
            }

            if (player.Character.Inventory.TryToAddItem(this, m_item) == true)
            {
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

        public void AssignItem(IItem item)
        {
            m_item = item.Clone();
            m_itemData.ItemInfo = m_item.Info;

            if (m_item is WeaponItem)
            {
                m_itemData.Amount = (m_item as WeaponItem).Uses;
            }
            else if (m_item is MagicItem)
            {
                m_itemData.Amount = (m_item as MagicItem).Uses;
            }
            else
            {
                m_itemData.Amount = m_item.Amount;
            }
        }

        private void Start()
        {
            if (m_item == null) m_item = m_itemData.CreateItem();
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
                s.itemData = new ItemData(m_itemData);
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
