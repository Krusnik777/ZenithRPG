using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIStatsProgress : MonoBehaviour, IDependency<PlayerCharacter>
    {
        [SerializeField] private GameObject m_statsProgressPanel;
        [Header("Level")]
        [SerializeField] private TextMeshProUGUI m_levelValueText;
        [SerializeField] private Image m_levelFillImage;
        [Header("Strength")]
        [SerializeField] private TextMeshProUGUI m_strengthValueText;
        [SerializeField] private Image m_strengthFillImage;
        [Header("Intelligence")]
        [SerializeField] private TextMeshProUGUI m_intelligenceValueText;
        [SerializeField] private Image m_intelligenceFillImage;
        [Header("MagicResist")]
        [SerializeField] private TextMeshProUGUI m_magicResistValueText;
        [SerializeField] private Image m_magicResistFillImage;

        private PlayerCharacter m_playerCharacter;
        public void Construct(PlayerCharacter playerCharacter) => m_playerCharacter = playerCharacter;

        private void Start()
        {
            SetLevelValue();
            SetStrengthValue();
            SetIntelligenceValue();
            SetMagicResistValue();

            FillLevelBar();
            FillStrengthBar();
            FillIntelligenceBar();
            FillMagicResistBar();

            m_playerCharacter.PlayerStats.EventOnExperienceChange += FillLevelBar;
            m_playerCharacter.PlayerStats.EventOnStrengthExperienceChange += FillStrengthBar;
            m_playerCharacter.PlayerStats.EventOnIntelligenceExperienceChange += FillIntelligenceBar;
            m_playerCharacter.PlayerStats.EventOnMagicResistExperienceChange += FillMagicResistBar;

            m_playerCharacter.PlayerStats.EventOnLevelUp += SetLevelValue;
            m_playerCharacter.PlayerStats.EventOnStrengthUp += SetStrengthValue;
            m_playerCharacter.PlayerStats.EventOnIntelligenceUp += SetIntelligenceValue;
            m_playerCharacter.PlayerStats.EventOnMagicResistUp += SetMagicResistValue;

            m_playerCharacter.PlayerStats.EventOnStatsUpdated += UpdateAll;
        }

        private void OnDestroy()
        {
            m_playerCharacter.PlayerStats.EventOnExperienceChange -= FillLevelBar;
            m_playerCharacter.PlayerStats.EventOnStrengthExperienceChange -= FillStrengthBar;
            m_playerCharacter.PlayerStats.EventOnIntelligenceExperienceChange -= FillIntelligenceBar;
            m_playerCharacter.PlayerStats.EventOnMagicResistExperienceChange -= FillMagicResistBar;

            m_playerCharacter.PlayerStats.EventOnLevelUp -= SetLevelValue;
            m_playerCharacter.PlayerStats.EventOnStrengthUp -= SetStrengthValue;
            m_playerCharacter.PlayerStats.EventOnIntelligenceUp -= SetIntelligenceValue;
            m_playerCharacter.PlayerStats.EventOnMagicResistUp -= SetMagicResistValue;

            m_playerCharacter.PlayerStats.EventOnStatsUpdated -= UpdateAll;
        }

        public void FillLevelBar() => m_levelFillImage.fillAmount = m_playerCharacter.PlayerStats.Level >= m_playerCharacter.PlayerStats.MaxLevel ? 1 : (float) m_playerCharacter.PlayerStats.CurrentExperiencePoints / (float) m_playerCharacter.PlayerStats.GetExperienceForLevelUp();
        public void FillStrengthBar() => m_strengthFillImage.fillAmount = m_playerCharacter.PlayerStats.Strength >= m_playerCharacter.PlayerStats.MaxStatLevel ? 1 : (float) m_playerCharacter.PlayerStats.CurrentStrengthExperiencePoints / (float) m_playerCharacter.PlayerStats.GetExperienceForStrengthUp();
        public void FillIntelligenceBar() => m_intelligenceFillImage.fillAmount = m_playerCharacter.PlayerStats.Intelligence >= m_playerCharacter.PlayerStats.MaxStatLevel ? 1 : (float) m_playerCharacter.PlayerStats.CurrentIntelligenceExperiencePoints / (float) m_playerCharacter.PlayerStats.GetExperienceForIntelligenceUp();
        public void FillMagicResistBar() => m_magicResistFillImage.fillAmount = m_playerCharacter.PlayerStats.MagicResist >= m_playerCharacter.PlayerStats.MaxStatLevel ? 1 : (float) m_playerCharacter.PlayerStats.CurrentMagicResistExperiencePoints / (float) m_playerCharacter.PlayerStats.GetExperienceForMagicResistUp();

        public void SetLevelValue() => m_levelValueText.text = m_playerCharacter.PlayerStats.Level.ToString();
        public void SetStrengthValue() => m_strengthValueText.text = m_playerCharacter.PlayerStats.Strength.ToString();
        public void SetIntelligenceValue() => m_intelligenceValueText.text = m_playerCharacter.PlayerStats.Intelligence.ToString();
        public void SetMagicResistValue() => m_magicResistValueText.text = m_playerCharacter.PlayerStats.MagicResist.ToString();

        private void UpdateAll()
        {
            SetLevelValue();
            SetStrengthValue();
            SetIntelligenceValue();
            SetMagicResistValue();

            FillLevelBar();
            FillStrengthBar();
            FillIntelligenceBar();
            FillMagicResistBar();
        }
    }
}
