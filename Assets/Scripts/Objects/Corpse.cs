using UnityEngine;
using static DC_ARPG.Chest;


namespace DC_ARPG
{
    public class Corpse : ItemContainer
    {
        [SerializeField] private int m_inspectionsToGetItem;
        [SerializeField] private string[] m_inspectionsLines;

        private int currentInspection = 0;

        private bool collected = false;

        public override void OnInspection(Player player)
        {
            if (collected)
            {
                ShortMessage.Instance.ShowMessage(m_inspectionsLines[Random.Range(0, m_inspectionsLines.Length)]);
                return;
            }

            if (currentInspection < m_inspectionsToGetItem)
            {
                ShortMessage.Instance.ShowMessage(m_inspectionsLines[Random.Range(0, m_inspectionsLines.Length)]);
                currentInspection++;
            }
            else
            {
                if (m_item == null)
                {
                    ShortMessage.Instance.ShowMessage("При тщательном обыске ничего не найдено.");
                    collected = true;
                    return;
                }

                if ((player.Character as PlayerCharacter).Inventory.TryToAddItem(this, m_item) == true)
                {
                    if ((m_item is UsableItem || m_item is NotUsableItem) && m_item.Amount > 1)
                        ShortMessage.Instance.ShowMessage("При тщательном обыске найдено: " + m_item.Info.Title + " x" + m_item.Amount + ".");
                    else
                        ShortMessage.Instance.ShowMessage("При тщательном обыске найдено: " + m_item.Info.Title + ".");

                    UISounds.Instance.PlayItemCollectedSound();

                    EventOnInspection?.Invoke();

                    collected = true;
                }
                else
                {
                    ShortMessage.Instance.ShowMessage("При тщательном обыске найдено: " + m_item.Info.Title + " x" + m_item.Amount + ", но в инвентаре нет места.");
                    UISounds.Instance.PlayInventoryActionFailureSound();
                }

            }
        }

        #region Serialize

        [System.Serializable]
        public class CorpseDataState
        {
            public bool enabled;
            public bool collected;

            public CorpseDataState() { }
        }

        public override bool IsCreated => false;

        public override string SerializeState()
        {
            CorpseDataState s = new CorpseDataState();

            s.enabled = gameObject.activeInHierarchy;
            s.collected = collected;

            return JsonUtility.ToJson(s);
        }

        public override void DeserializeState(string state)
        {
            CorpseDataState s = JsonUtility.FromJson<CorpseDataState>(state);

            collected = s.collected;
            gameObject.SetActive(s.enabled);
        }

        #endregion
    }
}
