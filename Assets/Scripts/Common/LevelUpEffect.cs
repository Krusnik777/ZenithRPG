using UnityEngine;

namespace DC_ARPG
{
    public class LevelUpEffect : MonoBehaviour
    {
        [SerializeField] private GameObject m_levelUpEffect;

        private PlayerCharacter m_playerCharacter;

        private void Start()
        {
            m_playerCharacter = GetComponent<PlayerCharacter>();

            m_playerCharacter.PlayerStats.EventOnLevelUp += OnLevelUp;
        }

        private void OnDestroy()
        {
            m_playerCharacter.PlayerStats.EventOnLevelUp -= OnLevelUp;
        }

        private void OnLevelUp()
        {
            if (m_levelUpEffect == null) return;

            ShortMessage.Instance.ShowMessage("спнбемэ онбшьем!");

            m_levelUpEffect.SetActive(true);

            CancelInvoke("TurnOffEffect");
            Invoke("TurnOffEffect", 2.0f);
        }

        private void TurnOffEffect()
        {
            if (m_levelUpEffect == null) return;

            m_levelUpEffect.SetActive(false);
        }
    }
}
