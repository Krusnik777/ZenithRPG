using UnityEngine;

namespace DC_ARPG
{
    public class SaveSlotsMenu : MonoBehaviour
    {
        private SaveSlot[] m_saveSlots;

        public void ActivateMenu()
        {
            var profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

            foreach (var saveSlot in m_saveSlots)
            {
                GameData profileData = null;

                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);

                saveSlot.SetData(profileData);
            }
        }

        public void OnSaveSlotClicked(SaveSlot saveSlot)
        {
            DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

            DataPersistenceManager.Instance.NewGame();

            DataPersistenceManager.Instance.SaveGame();

            //SceneManager.LoadScenAsync("SampleScene");
        }

        private void Awake()
        {
            m_saveSlots = GetComponentsInChildren<SaveSlot>();
        }

        private void Start()
        {
            ActivateMenu();
        }
    }
}
