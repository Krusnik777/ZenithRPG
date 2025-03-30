using UnityEngine;

namespace DC_ARPG
{
    public class SettingLoader : MonoBehaviour
    {
        [SerializeField] private Setting[] m_allSettings;

        public void LoadSettings()
        {
            for (int i = 0; i < m_allSettings.Length; i++)
            {
                m_allSettings[i].Load();
                m_allSettings[i].Apply();
            }
        }
    }
}
