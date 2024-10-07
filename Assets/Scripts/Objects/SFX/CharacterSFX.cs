using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class CharacterSFX : MonoBehaviour
    {
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
        }

        private void Start()
        {
            if (m_blockEffect != null)
                if (m_blockEffect.activeInHierarchy) m_blockEffect.SetActive(false);

            m_characterAvatar.Character.EventOnHit += OnHit;
            m_characterAvatar.EventOnBlock += OnBlock;

            if (m_characterAvatar is Player)
            {
                (m_characterAvatar.Character as PlayerCharacter).Inventory.WeaponItemSlot.EventOnBrokenWeapon += OnBrokenWeapon;
            }
        }

        private void OnDestroy()
        {
            m_characterAvatar.Character.EventOnHit -= OnHit;
            m_characterAvatar.EventOnBlock -= OnBlock;

            if (m_characterAvatar is Player)
            {
                (m_characterAvatar.Character as PlayerCharacter).Inventory.WeaponItemSlot.EventOnBrokenWeapon -= OnBrokenWeapon;
            }
        }

        private void OnHit()
        {
            PlayGetHitSFX(m_characterAvatar.Character.transform.position);
        }

        private void OnBrokenWeapon(object sender)
        {
            PlayBrokenSwordEffect(m_characterAvatar.transform.position);
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
