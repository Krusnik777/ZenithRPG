using System;
using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class SaveSlot : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private string m_profileId = "";
        [Header("Content")]
        [SerializeField] private GameObject m_noDataContent;
        [SerializeField] private GameObject m_hasDataContent;
        [SerializeField] private TextMeshProUGUI m_levelNameText;
        [SerializeField] private TextMeshProUGUI m_saveDateText;
        [SerializeField] private TextMeshProUGUI m_playerLevelText;
        [SerializeField] private TextMeshProUGUI m_playTimeText;

        public string GetProfileId() => m_profileId;

        public void SetData(GameData gameData)
        {
            if (gameData == null)
            {
                m_noDataContent.SetActive(true);
                m_hasDataContent.SetActive(false);
            }
            else
            {
                m_noDataContent.SetActive(false);
                m_hasDataContent.SetActive(true);

                m_levelNameText.text = SceneCommander.Instance.GetTitleOfLevel(gameData.ActiveSceneName);
                m_saveDateText.text = DateTime.FromBinary(gameData.LastUpdated).ToString("HH':'mm' 'dd'.'MM'.'yy");
                //m_saveDateText.text = DateTime.FromBinary(gameData.LastUpdated).ToString("g");
                m_playerLevelText.text = "Уровень: " + gameData.PlayerData.PlayerStatsData.Level.ToString();

                TimeSpan time = new TimeSpan();
                if (gameData.PlayTime > TimeSpan.MaxValue.TotalSeconds) time = TimeSpan.MaxValue;
                else time = TimeSpan.FromSeconds(gameData.PlayTime);
                m_playTimeText.text = "Время игры: " + time.ToString(@"hh\:mm\:ss");
            }
        }

        public void LoadData()
        {
            DataPersistenceManager.Instance.ChangeSelectedProfileId(m_profileId);

            DataPersistenceManager.Instance.LoadGame();
        }

        public void SaveData()
        {
            DataPersistenceManager.Instance.ChangeSelectedProfileId(m_profileId);

            DataPersistenceManager.Instance.SaveGame();

            SetData(DataPersistenceManager.Instance.GetGameDataByProfile(m_profileId));
        }

        public void DeleteData()
        {
            DataPersistenceManager.Instance.DeleteProfileData(m_profileId);

            SetData(DataPersistenceManager.Instance.GetGameDataByProfile(m_profileId));
        }

    }
}
