using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class CharacterSFX : MonoBehaviour
    {
        [SerializeField] private CharacterAvatar m_characterAvatar;
        [SerializeField] private Transform m_rightHandPoint;
        [SerializeField] private Transform m_shieldHoldPoint;
        [Header("Sounds")]
        [SerializeField] private AudioClip m_footstepSound;
        [SerializeField] private AudioClip m_jumpSound;
        [SerializeField] private AudioClip m_landSound;
        [SerializeField] private AudioClip m_attackSound;
        [SerializeField] private AudioClip m_kickSound;
        [Header("Effects")]
        [SerializeField] private GameObject m_blockEffect;
        [SerializeField] private GameObject m_breakBlockEffectPrefab;
        [SerializeField] private GameObject m_parryEffectPrefab;
        [SerializeField] private GameObject m_hitEffectPrefab;
        [SerializeField] private GameObject m_deathEffectPrefab;
        [SerializeField] private GameObject m_brokenSwordEffectPrefab;
        [Header("AttackEffects")]
        [SerializeField] private GameObject m_attack1Effect;
        [SerializeField] private GameObject m_attack2Effect;
        [SerializeField] private GameObject m_attack3Effect;

        private AudioSource m_audioSource;

        private Coroutine blockCoroutine;

        public void PlayFootstepSound() => m_audioSource.PlayOneShot(m_footstepSound);
        public void PlayJumpSound() => m_audioSource.PlayOneShot(m_jumpSound);
        public void PlayLandSound() => m_audioSource.PlayOneShot(m_landSound);
        public void PlayAttackSound() => m_audioSource.PlayOneShot(m_attackSound);
        public void PlayKickSound() => m_audioSource.PlayOneShot(m_kickSound);
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

        public void PlayBreakBlockEffect(Vector3 position)
        {
            if (m_breakBlockEffectPrefab != null)
            {
                var effect = Instantiate(m_breakBlockEffectPrefab, position, Quaternion.identity);

                Destroy(effect, 2.0f);
            }
        }

        public void PlayParryEffect(Vector3 position)
        {
            if (m_parryEffectPrefab != null)
            {
                var effect = Instantiate(m_parryEffectPrefab, position, Quaternion.identity);

                Destroy(effect, 2.0f);
            }
        }

        public void PlayBrokenSwordEffect(Vector3 position)
        {
            if (m_brokenSwordEffectPrefab != null)
            {
                var effect = Instantiate(m_brokenSwordEffectPrefab, position, Quaternion.identity);

                Destroy(effect, 1.0f);
            }

            UISounds.Instance.PlaySwordBreakSound();
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
            m_characterAvatar.EventOnParry += OnParry;
            m_characterAvatar.EventOnBlock += OnBlock;
            m_characterAvatar.EventOnBlockBreak += OnBlockBreak;

            m_characterAvatar.EventOnAttack += OnAttack;

            if (m_characterAvatar is Player)
            {
                (m_characterAvatar.Character as PlayerCharacter).Inventory.WeaponItemSlot.EventOnBrokenWeapon += OnBrokenWeapon;
            }
        }

        private void OnDestroy()
        {
            m_characterAvatar.Character.EventOnHit -= OnHit;
            m_characterAvatar.EventOnParry -= OnParry;
            m_characterAvatar.EventOnBlock -= OnBlock;
            m_characterAvatar.EventOnBlockBreak -= OnBlockBreak;

            m_characterAvatar.EventOnAttack -= OnAttack;

            if (m_characterAvatar is Player)
            {
                (m_characterAvatar.Character as PlayerCharacter).Inventory.WeaponItemSlot.EventOnBrokenWeapon -= OnBrokenWeapon;
            }
        }

        private void OnAttack(int hitCount)
        {
            if (hitCount == 1) StartCoroutine(ActivateGameObject(m_attack1Effect, 0.1f));
            if (hitCount == 2) StartCoroutine(ActivateGameObject(m_attack2Effect, 0.4f));
            if (hitCount == 3) StartCoroutine(ActivateGameObject(m_attack3Effect, 0.2f));
        }

        private void OnHit()
        {
            PlayGetHitSFX(m_characterAvatar.Character.transform.position);
        }

        private void OnBrokenWeapon(object sender)
        {
            if (m_rightHandPoint == null) return;

            PlayBrokenSwordEffect(m_rightHandPoint.position);
        }

        private void OnParry()
        {
            if (m_shieldHoldPoint == null) return;

            PlayParryEffect(m_shieldHoldPoint.position);
        }

        private void OnBlock()
        {
            PlayBlockSFX();
        }

        private void OnBlockBreak()
        {
            if (m_shieldHoldPoint == null) return;

            PlayBreakBlockEffect(m_shieldHoldPoint.position);
        }

        #region Coroutines

        private IEnumerator ActivateBlockEffect()
        {
            m_blockEffect.SetActive(true);

            yield return new WaitForSeconds(0.6f);

            m_blockEffect.SetActive(false);
        }

        private IEnumerator ActivateGameObject(GameObject gameObject, float delay)
        {
            yield return new WaitForSeconds(delay);

            gameObject.SetActive(true);

            yield return new WaitForSeconds(1);

            gameObject.SetActive(false);
        }

        #endregion
    }
}
