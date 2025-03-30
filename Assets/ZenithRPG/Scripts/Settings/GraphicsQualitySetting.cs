using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "GraphicsQualitySetting", menuName = "ScriptableObjects/Settings/GraphicsQualitySetting")]
    public class GraphicsQualitySetting : Setting
    {
        private int currentLevelIndex = 0;
        public override bool isMinValue { get => currentLevelIndex == 0; }
        public override bool isMaxValue { get => currentLevelIndex == QualitySettings.names.Length - 1; }

        public override void SetNextValue()
        {
            if (!isMaxValue)
            {
                currentLevelIndex++;
            }
        }

        public override void SetPreviousValue()
        {
            if (!isMinValue)
            {
                currentLevelIndex--;
            }
        }

        public override object GetValue()
        {
            return QualitySettings.names[currentLevelIndex];
        }

        public override string GetStringValue()
        {
            return QualitySettings.names[currentLevelIndex];
        }

        public override void Apply()
        {
            QualitySettings.SetQualityLevel(currentLevelIndex);

            Save();
        }

        public override void Load()
        {
            SettingsSaver.Load(m_title, ref currentLevelIndex);
            
            //currentLevelIndex = PlayerPrefs.GetInt(m_title, QualitySettings.names.Length - 1);
        }

        private void Save()
        {
            SettingsSaver.Save(m_title, currentLevelIndex);

            //PlayerPrefs.SetInt(m_title, currentLevelIndex);
        }
    }
}
