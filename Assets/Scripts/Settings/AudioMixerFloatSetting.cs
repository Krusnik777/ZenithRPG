using UnityEngine;
using UnityEngine.Audio;

namespace DC_ARPG
{
    [CreateAssetMenu(fileName = "AudioMixerFloatSetting", menuName = "ScriptableObjects/Settings/AudioMixerFloatSetting")]
    public class AudioMixerFloatSetting : Setting
    {
        [SerializeField] private AudioMixer m_audioMixer;
        [SerializeField] private string m_nameParameter;

        [SerializeField] private float m_minRealValue;
        [SerializeField] private float m_maxRealValue;

        [SerializeField] private float m_virtualStep;
        [SerializeField] private float m_minVirtualValue;
        [SerializeField] private float m_maxVirtualValue;

        private float currentValue = 0;

        public override bool isMinValue { get => currentValue == m_minRealValue; }
        public override bool isMaxValue { get => currentValue == m_maxRealValue; }

        #region Public

        public override void SetNextValue()
        {
            AddValue(Mathf.Abs(m_maxRealValue - m_minRealValue) / m_virtualStep);
        }

        public override void SetPreviousValue()
        {
            AddValue(-Mathf.Abs(m_maxRealValue - m_minRealValue) / m_virtualStep);
        }

        public override string GetStringValue()
        {
            return Mathf.Lerp(m_minVirtualValue, m_maxVirtualValue, (currentValue - m_minRealValue) / (m_maxRealValue - m_minRealValue)).ToString();
        }

        public override object GetValue()
        {
            return currentValue;
        }

        public override void Apply()
        {
            m_audioMixer.SetFloat(m_nameParameter, currentValue);

            Save();
        }

        public override void Load()
        {
            SettingsSaver.Load(m_title, ref currentValue);

            //currentValue = PlayerPrefs.GetFloat(m_title, 0);
        }

        #endregion

        #region Private

        private void AddValue(float value)
        {
            currentValue += value;
            currentValue = Mathf.Clamp(currentValue, m_minRealValue, m_maxRealValue);
        }

        private void Save()
        {
            SettingsSaver.Save(m_title, currentValue);

            //PlayerPrefs.SetFloat(m_title, currentValue);
        }

        #endregion
    }
}
