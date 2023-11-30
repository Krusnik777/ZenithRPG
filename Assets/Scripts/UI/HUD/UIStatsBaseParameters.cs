using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIStatsBaseParameters : MonoBehaviour, IDependency<PlayerCharacter>
    {
        [Header("HitPoints")]
        [SerializeField] private TextMeshProUGUI m_hitPointsValueText;
        [SerializeField] private Image m_hitPointsFillImage;
        [Header("MagicPoints")]
        [SerializeField] private TextMeshProUGUI m_magicPointsValueText;
        [SerializeField] private Image m_magicPointsFillImage;

        private PlayerCharacter m_playerCharacter;
        public void Construct(PlayerCharacter playerCharacter) => m_playerCharacter = playerCharacter;

        private void Start()
        {
            m_playerCharacter.PlayerStats.EventOnHitPointsChange += SetHitPoints;
            m_playerCharacter.PlayerStats.EventOnMagicPointsChange += SetMagicPoints;
            m_playerCharacter.PlayerStats.EventOnIntelligenceUp += SetMagicPoints;

            m_playerCharacter.PlayerStats.EventOnLevelUp += SetBothPoints;

            SetBothPoints();
        }

        private void OnDestroy()
        {
            m_playerCharacter.PlayerStats.EventOnHitPointsChange -= SetHitPoints;
            m_playerCharacter.PlayerStats.EventOnMagicPointsChange -= SetMagicPoints;
            m_playerCharacter.PlayerStats.EventOnIntelligenceUp -= SetMagicPoints;

            m_playerCharacter.PlayerStats.EventOnLevelUp -= SetBothPoints;
        }

        private void SetHitPoints()
        {
            m_hitPointsValueText.text = $"{m_playerCharacter.PlayerStats.CurrentHitPoints}/{m_playerCharacter.PlayerStats.HitPoints}";
            m_hitPointsFillImage.fillAmount = (float) m_playerCharacter.PlayerStats.CurrentHitPoints / (float) m_playerCharacter.PlayerStats.HitPoints;
        }

        private void SetMagicPoints()
        {
            m_magicPointsValueText.text = $"{m_playerCharacter.PlayerStats.CurrentMagicPoints}/{m_playerCharacter.PlayerStats.MagicPoints}";
            m_magicPointsFillImage.fillAmount = (float) m_playerCharacter.PlayerStats.CurrentMagicPoints / (float) m_playerCharacter.PlayerStats.MagicPoints;
        }

        private void SetBothPoints()
        {
            SetHitPoints();
            SetMagicPoints();
        }
    }
}
