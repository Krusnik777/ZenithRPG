using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DC_ARPG
{
    public class UIStatsTest : MonoBehaviour, IDependency<PlayerCharacter>
    {
        [SerializeField] private GameObject m_panel;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI m_levelValue;
        [SerializeField] private TextMeshProUGUI m_currentExpValue;
        [SerializeField] private TextMeshProUGUI m_ExpToNextLevelValue;

        [SerializeField] private TextMeshProUGUI m_hitPointsValue;

        [SerializeField] private TextMeshProUGUI m_magicPointsValue;

        [SerializeField] private TextMeshProUGUI m_strengthValue;
        [SerializeField] private TextMeshProUGUI m_currentStrengthExpValue;
        [SerializeField] private TextMeshProUGUI m_ExpToStrengthUpValue;

        [SerializeField] private TextMeshProUGUI m_intelligenceValue;
        [SerializeField] private TextMeshProUGUI m_currentIntelligenceExpValue;
        [SerializeField] private TextMeshProUGUI m_ExpToIntelligenceUpValue;

        [SerializeField] private TextMeshProUGUI m_magicResistValue;
        [SerializeField] private TextMeshProUGUI m_currentMagicResistExpValue;
        [SerializeField] private TextMeshProUGUI m_ExpToMagicResistUpValue;

        [SerializeField] private TextMeshProUGUI m_luckValue;
        [SerializeField] private TextMeshProUGUI m_attackValue;
        [SerializeField] private TextMeshProUGUI m_defenseValue;

        [Header("RandomPassiveStats")]
        [SerializeField] private TextMeshProUGUI m_weaponDamageValue;
        [SerializeField] private TextMeshProUGUI m_armorDefenseValue;

        [Header("Buttons")]
        [SerializeField] private Button m_takeDamageButton;
        [SerializeField] private Button m_healButton;
        [SerializeField] private Button m_getExpButton;
        [SerializeField] private Button m_getSTRExpButton;
        [SerializeField] private Button m_getINTExpButton;
        [SerializeField] private Button m_getMagResExpButton;
        [SerializeField] private Button m_equipWeaponButton;
        [SerializeField] private Button m_equipArmorButton;

        private PlayerCharacter m_character;
        public void Construct(PlayerCharacter character) => m_character = character;

        public void TurnStatsPanel(bool state)
        {
            m_panel.SetActive(state);

            if (state == true && m_character.PlayerStats != null)
            {
                SetUpStats();
            }
        }

        private void Start()
        {
            m_takeDamageButton.onClick.AddListener(TakeDamage);
            m_healButton.onClick.AddListener(Heal);
            m_getExpButton.onClick.AddListener(GetExp);
            m_getSTRExpButton.onClick.AddListener(GetStregthExp);
            m_getINTExpButton.onClick.AddListener(GetIntelligenceExp);
            m_getMagResExpButton.onClick.AddListener(GetMagicResistExp);
            m_equipWeaponButton.onClick.AddListener(EquipWeapon);
            m_equipArmorButton.onClick.AddListener(EquipArmor);

            m_character.PlayerStats.EventOnHitPointsChange += OnHitPointsChange;

            m_character.PlayerStats.EventOnLevelUp += OnLevelUp;
            m_character.PlayerStats.EventOnStrengthUp += OnStrengthUp;
            m_character.PlayerStats.EventOnIntelligenceUp += OnIntelligenceUp;
            m_character.PlayerStats.EventOnMagicResistUp += OnMagicResistUp;

            m_character.PlayerStats.EventOnAttackChange += OnAttackChange;
            m_character.PlayerStats.EventOnDefenseChange += OnDefenseChange;

            m_character.PlayerStats.EventOnExperienceChange += OnExperienceChange;
            m_character.PlayerStats.EventOnStrengthExperienceChange += OnStrengthExperienceChange;
            m_character.PlayerStats.EventOnIntelligenceExperienceChange += OnIntelligenceExperienceChange;
            m_character.PlayerStats.EventOnMagicResistExperienceChange += OnMagicResistExperienceChange;

        }

        private void OnMagicResistExperienceChange()
        {
            m_currentMagicResistExpValue.text = m_character.PlayerStats.CurrentMagicResistExperiencePoints.ToString();
            m_ExpToMagicResistUpValue.text = m_character.PlayerStats.GetExperienceForMagicResistUp().ToString();
        }

        private void OnIntelligenceExperienceChange()
        {
            m_currentIntelligenceExpValue.text = m_character.PlayerStats.CurrentIntelligenceExperiencePoints.ToString();
            m_ExpToIntelligenceUpValue.text = m_character.PlayerStats.GetExperienceForIntelligenceUp().ToString();
        }

        private void OnStrengthExperienceChange()
        {
            m_currentStrengthExpValue.text = m_character.PlayerStats.CurrentStrengthExperiencePoints.ToString();
            m_ExpToStrengthUpValue.text = m_character.PlayerStats.GetExperienceForStrengthUp().ToString();
        }

        private void OnExperienceChange()
        {
            m_currentExpValue.text = m_character.PlayerStats.CurrentExperiencePoints.ToString();
            m_ExpToNextLevelValue.text = m_character.PlayerStats.GetExperienceForLevelUp().ToString();
        }

        private void OnDefenseChange()
        {
            m_defenseValue.text = m_character.PlayerStats.Defense.ToString();
        }

        private void OnAttackChange()
        {
            m_attackValue.text = m_character.PlayerStats.Attack.ToString();
        }

        private void OnMagicResistUp()
        {
            m_magicResistValue.text = m_character.PlayerStats.MagicResist.ToString();
        }

        private void OnIntelligenceUp()
        {
            m_magicPointsValue.text = $"{m_character.PlayerStats.CurrentMagicPoints}/{m_character.PlayerStats.MagicPoints}";

            m_intelligenceValue.text = m_character.PlayerStats.Intelligence.ToString();

        }

        private void OnStrengthUp()
        {
            m_strengthValue.text = m_character.PlayerStats.Strength.ToString();
        }

        private void OnLevelUp()
        {
            m_levelValue.text = m_character.PlayerStats.Level.ToString();

            m_hitPointsValue.text = $"{m_character.PlayerStats.CurrentHitPoints}/{m_character.PlayerStats.HitPoints}";
            m_magicPointsValue.text = $"{m_character.PlayerStats.CurrentMagicPoints}/{m_character.PlayerStats.MagicPoints}";

            m_currentStrengthExpValue.text = m_character.PlayerStats.CurrentStrengthExperiencePoints.ToString();
            m_ExpToStrengthUpValue.text = m_character.PlayerStats.GetExperienceForStrengthUp().ToString();

            m_currentIntelligenceExpValue.text = m_character.PlayerStats.CurrentIntelligenceExperiencePoints.ToString();
            m_ExpToIntelligenceUpValue.text = m_character.PlayerStats.GetExperienceForIntelligenceUp().ToString();

            m_currentMagicResistExpValue.text = m_character.PlayerStats.CurrentMagicResistExperiencePoints.ToString();
            m_ExpToMagicResistUpValue.text = m_character.PlayerStats.GetExperienceForMagicResistUp().ToString();

            m_luckValue.text = m_character.PlayerStats.Luck.ToString();
        }

        private void OnHitPointsChange()
        {
            m_hitPointsValue.text = $"{m_character.PlayerStats.CurrentHitPoints}/{m_character.PlayerStats.HitPoints}";
        }

        private void OnDestroy()
        {
            m_takeDamageButton.onClick.RemoveListener(TakeDamage);
            m_healButton.onClick.RemoveListener(Heal);
            m_getExpButton.onClick.RemoveListener(GetExp);
            m_getSTRExpButton.onClick.RemoveListener(GetStregthExp);
            m_getINTExpButton.onClick.RemoveListener(GetIntelligenceExp);
            m_getMagResExpButton.onClick.RemoveListener(GetMagicResistExp);
            m_equipWeaponButton.onClick.RemoveListener(EquipWeapon);
            m_equipArmorButton.onClick.RemoveListener(EquipArmor);

            m_character.PlayerStats.EventOnHitPointsChange -= OnHitPointsChange;

            m_character.PlayerStats.EventOnLevelUp -= OnLevelUp;
            m_character.PlayerStats.EventOnStrengthUp -= OnStrengthUp;
            m_character.PlayerStats.EventOnIntelligenceUp -= OnIntelligenceUp;
            m_character.PlayerStats.EventOnMagicResistUp -= OnMagicResistUp;

            m_character.PlayerStats.EventOnAttackChange -= OnAttackChange;
            m_character.PlayerStats.EventOnDefenseChange -= OnDefenseChange;

            m_character.PlayerStats.EventOnExperienceChange -= OnExperienceChange;
            m_character.PlayerStats.EventOnStrengthExperienceChange -= OnStrengthExperienceChange;
            m_character.PlayerStats.EventOnIntelligenceExperienceChange -= OnIntelligenceExperienceChange;
            m_character.PlayerStats.EventOnMagicResistExperienceChange -= OnMagicResistExperienceChange;
        }

        private void SetUpStats()
        {
            m_levelValue.text = m_character.PlayerStats.Level.ToString();
            m_currentExpValue.text = m_character.PlayerStats.CurrentExperiencePoints.ToString();
            m_ExpToNextLevelValue.text = m_character.PlayerStats.GetExperienceForLevelUp().ToString();

            m_hitPointsValue.text = $"{m_character.PlayerStats.CurrentHitPoints}/{m_character.PlayerStats.HitPoints}";
            m_magicPointsValue.text = $"{m_character.PlayerStats.CurrentMagicPoints}/{m_character.PlayerStats.MagicPoints}";

            m_strengthValue.text = m_character.PlayerStats.Strength.ToString();
            m_currentStrengthExpValue.text = m_character.PlayerStats.CurrentStrengthExperiencePoints.ToString();
            m_ExpToStrengthUpValue.text = m_character.PlayerStats.GetExperienceForStrengthUp().ToString();

            m_intelligenceValue.text = m_character.PlayerStats.Intelligence.ToString();
            m_currentIntelligenceExpValue.text = m_character.PlayerStats.CurrentIntelligenceExperiencePoints.ToString();
            m_ExpToIntelligenceUpValue.text = m_character.PlayerStats.GetExperienceForIntelligenceUp().ToString();

            m_magicResistValue.text = m_character.PlayerStats.MagicResist.ToString();
            m_currentMagicResistExpValue.text = m_character.PlayerStats.CurrentMagicResistExperiencePoints.ToString();
            m_ExpToMagicResistUpValue.text = m_character.PlayerStats.GetExperienceForMagicResistUp().ToString();

            m_luckValue.text = m_character.PlayerStats.Luck.ToString();
            m_attackValue.text = m_character.PlayerStats.Attack.ToString();
            m_defenseValue.text = m_character.PlayerStats.Defense.ToString();

            m_weaponDamageValue.text = "0";
            m_armorDefenseValue.text = "0";
        }

        private void TakeDamage()
        {
            m_character.PlayerStats.ChangeCurrentHitPoints(this, -5);
        }

        private void Heal()
        {
            m_character.PlayerStats.ChangeCurrentHitPoints(this, 5);
        }

        private void GetExp()
        {
            m_character.PlayerStats.AddExperience(20);
        }

        private void GetStregthExp()
        {
            m_character.PlayerStats.AddStrengthExperience(5);
        }

        private void GetIntelligenceExp()
        {
            m_character.PlayerStats.AddIntelligenceExperience(3);
        }

        private void GetMagicResistExp()
        {
            m_character.PlayerStats.AddMagicResistExperience(1);
        }

        private void EquipWeapon()
        {
            int random = Random.Range(0, 151);
            m_weaponDamageValue.text = random.ToString();

            m_character.PlayerStats.SetWeaponDamage(random);
        }

        private void EquipArmor()
        {
            int random = Random.Range(0, 501);
            m_armorDefenseValue.text = random.ToString();

            m_character.PlayerStats.SetArmorDefense(random);
        }

    }
}
