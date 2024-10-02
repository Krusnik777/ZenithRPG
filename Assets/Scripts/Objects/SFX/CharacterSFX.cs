using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class CharacterSFX : MonoBehaviour
    {
        [SerializeField] private CharacterBase m_character;
        [SerializeField] private CharacterAvatar m_characterAvatar;
        [Header("Sounds")]
        [SerializeField] private AudioClip m_footstepSound;
        [SerializeField] private AudioClip m_jumpSound;
        [SerializeField] private AudioClip m_landSound;
        [SerializeField] private AudioClip m_attackSound;
        [Header("Effects")]
        [SerializeField] private GameObject m_blockEffect;
        [SerializeField] private GameObject m_hitEffectPrefab;
        [SerializeField] private GameObject m_deathEffectPrefab;
        [SerializeField] private GameObject m_brokenSwordEffectPrefab;

        private AudioSource m_audioSource;

        private Coroutine blockCoroutine;

        public void PlayFootstepSound() => m_audioSource.PlayOneShot(m_footstepSound);
        public void PlayJumpSound() => m_audioSource.PlayOneShot(m_jumpSound);
        public void PlayLandSound() => m_audioSource.PlayOneShot(m_landSound);
        public void PlayAttackSound() => m_audioSource.PlayOneShot(m_attackSound);
        public void PlayDeathSFX(Vector3 position)
        {
            if (m_deathEffectPrefab != null)
            {
                var deathEffect = Instantiate(m_deathEffectPrefab, position, Quaternion.identity);

                Destroy(deathEffect, 1.0f);
            }
        }

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

        public void PlayBrokenSwordEffect(Vector3 position)
        {
            if (m_brokenSwordEffectPrefab != null)
            {
                var effect = Instantiate(m_brokenSwordEffectPrefab, position, Quaternion.identity);

                Destroy(effect, 1.0f);
            }
        }

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            
            if (m_blockEffect != null)
                if (m_blockEffect.activeInHierarchy) m_blockEffect.SetActive(false);

            m_character.EventOnHit += OnHit;
            if (m_brokenSwordEffectPrefab != null) m_characterAvatar.Weapon.EventOnBrokenWeapon += OnBrokenWeapon;
            m_characterAvatar.EventOnBlock += OnBlock;
        }

        private void OnDestroy()
        {
            m_character.EventOnHit -= OnHit;
            m_characterAvatar.Weapon.EventOnBrokenWeapon -= OnBrokenWeapon;
            m_characterAvatar.EventOnBlock -= OnBlock;
        }

        private void OnHit()
        {
            PlayGetHitSFX(m_character.transform.position);
        }
               

        private void OnBrokenWeapon()
        {
            PlayBrokenSwordEffect(m_characterAvatar.Weapon.transform.position);
        }

        private void OnBlock()
        {
            PlayBlockSFX();
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
