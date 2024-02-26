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

                // TEMP
                // m_levelNameText.text = gameData.ActiveScene.SceneId.ToString();
            }
        }

    }
}
