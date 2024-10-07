using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class BlockStamina : MonoBehaviour
    {
        [SerializeField] private float m_defaultStaminaValue = 100f;
        [SerializeField] private float m_damageToStamina = 20f;
        [SerializeField] private float m_recoveryPerSecond = 10f;
        [SerializeField] private float m_recoveryPerSecondAfterBreak = 5f;

        public event UnityAction EventOnStaminaSpended;
        public event UnityAction EventOnDefenseBreaked;

        private CharacterAvatar parentCharacterAvatar;

        private float currentStamina;
        private Coroutine recoveryRoutine;

        private bool breaked;
        public bool DefenseBreaked => breaked;

        public void InitStamina(CharacterAvatar characterAvatar)
        {
            currentStamina = m_defaultStaminaValue;
            parentCharacterAvatar = characterAvatar;
        }

        public void SpendStamina()
        {
            currentStamina -= m_damageToStamina;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                BreakDefense();
            }
            else
            {
                EventOnStaminaSpended?.Invoke();
            }

            if (recoveryRoutine == null) recoveryRoutine = StartCoroutine(RecoveryRoutine());
        }

        private void BreakDefense()
        {
            breaked = true;
            EventOnDefenseBreaked?.Invoke();
        }

        private IEnumerator RecoveryRoutine()
        {
            while (currentStamina < m_defaultStaminaValue)
            {
                yield return new WaitForSeconds(1.0f);

                if (parentCharacterAvatar.IsBlocking) continue;

                if (breaked)
                    currentStamina += m_recoveryPerSecondAfterBreak;
                else
                    currentStamina += m_recoveryPerSecond;

                Debug.Log("CurrentStamina : " + currentStamina);
            }

            currentStamina = m_defaultStaminaValue;

            if (breaked) breaked = false;
            recoveryRoutine = null;
        }
    }
}
