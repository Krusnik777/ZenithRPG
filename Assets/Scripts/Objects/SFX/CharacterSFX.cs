using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class CharacterSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip m_footstepSound;
        [SerializeField] private AudioClip m_jumpSound;
        [SerializeField] private AudioClip m_landSound;
        [SerializeField] private AudioClip m_attackSound;
        [SerializeField] private AudioClip m_deathSound;

        [SerializeField] private GameObject m_blockEffect;
        [SerializeField] private GameObject m_hitEffectPrefab;

        private AudioSource m_audioSource;

        private Coroutine blockCoroutine;

        public void PlayFootstepSound() => m_audioSource.PlayOneShot(m_footstepSound);
        public void PlayJumpSound() => m_audioSource.PlayOneShot(m_jumpSound);
        public void PlayLandSound() => m_audioSource.PlayOneShot(m_landSound);
        public void PlayAttackSound() => m_audioSource.PlayOneShot(m_attackSound);
        public void PlayDeathSound() => m_audioSource.PlayOneShot(m_deathSound);

        public void PlayGetHitSFX(Vector3 position)
        {
            if (m_hitEffectPrefab != null)
            {
                var hitEffect = Instantiate(m_hitEffectPrefab, position, Quaternion.identity);

                Destroy(hitEffect, 1.0f);
            }
        }

        public void PlayBlockSFX()
        {
            if (blockCoroutine != null)
            {
                StopCoroutine(blockCoroutine);
                m_blockEffect.SetActive(false);
            }

            blockCoroutine = StartCoroutine(ActivateBlockEffect());
        }

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            
            if (m_blockEffect != null)
                if (m_blockEffect.activeInHierarchy) m_blockEffect.SetActive(false);
        }


        #region Coroutines

        private IEnumerator ActivateBlockEffect()
        {
            m_blockEffect.SetActive(true);

            yield return new WaitForSeconds(0.6f);

            m_blockEffect.SetActive(false);
        }

        #endregion
    }
}
