using System;
using UnityEngine;

namespace DC_ARPG
{
    public class SettingsSaver : MonoSingleton<SettingsSaver>, IDependency<SettingLoader>
    {
        public const string Filename = "SettingsData.dat";

        [Serializable]
        private class DataInt
        {
            public Setting Setting;
            public int Value;
        }

        [Serializable]
        private class DataFloat
        {
            public Setting Setting;
            public float Value;
        }

        [Serializable]
        private class SettingsData
        {
            public DataInt[] SaveInt;
            public DataFloat[] SaveFloat;
        }

        [SerializeField] private SettingsData m_settingsData;

        private SettingLoader m_settingLoader;
        public void Construct(SettingLoader settingLoader) => m_settingLoader = settingLoader;

        #region Public

        public static void Save(string title, int value)
        {
            foreach (var item in Instance.m_settingsData.SaveInt)
            {
                if (item.Setting.Title == title)
                {
                    item.Value = value;
                    Saver<SettingsData>.Save(Filename, Instance.m_settingsData);
                    break;
                }
            }
        }

        public static void Save(string title, float value)
        {
            foreach (var item in Instance.m_settingsData.SaveFloat)
            {
                if (item.Setting.Title == title)
                {
                    item.Value = value;
                    Saver<SettingsData>.Save(Filename, Instance.m_settingsData);
                    break;
                }
            }
        }

        public static void Load(string title, ref int value)
        {
            foreach (var item in Instance.m_settingsData.SaveInt)
            {
                if (item.Setting.Title == title)
                {
                    value = item.Value;
                    break;
                }
            }
        }

        public static void Load(string title, ref float value)
        {
            foreach (var item in Instance.m_settingsData.SaveFloat)
            {
                if (item.Setting.Title == title)
                {
                    value = item.Value;
                    break;
                }
            }
        }

        #endregion

        #region Private

        private void Start()
        {
            Saver<SettingsData>.TryLoad(Filename, ref m_settingsData);
            m_settingLoader.LoadSettings();
        }

        #endregion

    }
}
