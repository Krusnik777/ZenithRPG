using UnityEngine;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "ResolutionSetting", menuName = "ScriptableObjects/Settings/ResolutionSetting")]
    public class ResolutionSetting : Setting
    {
        [SerializeField]
        private Vector2Int[] m_availableResolutions = new Vector2Int[]
        {
            new Vector2Int (800, 600),
            new Vector2Int (1280, 720),
            new Vector2Int (1600, 900),
            new Vector2Int (1920, 1080)
        };

        private int currentResolutionIndex = 0;

        public override bool isMinValue { get => currentResolutionIndex == 0; }
        public override bool isMaxValue { get => currentResolutionIndex == m_availableResolutions.Length - 1; }

        public override void SetNextValue()
        {
            if (!isMaxValue)
            {
                currentResolutionIndex++;
            }
        }

        public override void SetPreviousValue()
        {
            if (!isMinValue)
            {
                currentResolutionIndex--;
            }
        }

        public override object GetValue()
        {
            return m_availableResolutions[currentResolutionIndex];
        }

        public override string GetStringValue()
        {
            return m_availableResolutions[currentResolutionIndex].x + "x" + m_availableResolutions[currentResolutionIndex].y;
        }

        public override void Apply()
        {
            Screen.SetResolution(m_availableResolutions[currentResolutionIndex].x, m_availableResolutions[currentResolutionIndex].y, true);

            Save();
        }

        public override void Load()
        {
            SettingsSaver.Load(m_title, ref currentResolutionIndex);

            //currentResolutionIndex = PlayerPrefs.GetInt(m_title, m_availableResolutions.Length - 1);
        }

        private void Save()
        {
            SettingsSaver.Save(m_title, currentResolutionIndex);

            //PlayerPrefs.SetInt(m_title, currentResolutionIndex);
        }
    }
}
