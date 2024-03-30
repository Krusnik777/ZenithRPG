using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class SaveSlotsMenu : MonoBehaviour
    {
        public enum MenuState
        {
            Save,
            Load,
            Erase
        }

        [SerializeField] private TextMeshProUGUI m_title;

        private SaveSlot[] m_saveSlots;

        private MenuState m_state;
        public MenuState State => m_state;

        public void SetState(MenuState state)
        {
            m_state = state;
            if (m_title != null)
            {
                if (m_state == MenuState.Save) m_title.text = "Сохранить";
                if (m_state == MenuState.Load) m_title.text = "Загрузить";
                if (m_state == MenuState.Erase) m_title.text = "Удалить";
            }
        }

        public void OnSaveSlotClicked(SaveSlot saveSlot)
        {
            if (m_state == MenuState.Save) saveSlot.SaveData();
            if (m_state == MenuState.Load) saveSlot.LoadData();
            if (m_state == MenuState.Erase) saveSlot.DeleteData();
        }

        private void Awake()
        {
            m_saveSlots = GetComponentsInChildren<SaveSlot>();
        }

        private void Start()
        {
            ActivateMenu();
        }

        private void ActivateMenu()
        {
            var profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

            foreach (var saveSlot in m_saveSlots)
            {
                GameData profileData = null;

                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);

                saveSlot.SetData(profileData);
            }
        }
    }
}
