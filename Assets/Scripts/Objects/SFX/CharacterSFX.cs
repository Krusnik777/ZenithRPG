using UnityEngine;

namespace DC_ARPG
{
    public class CharacterSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip m_footstepSound;
        [SerializeField] private AudioClip m_jumpSound;
        [SerializeField] private AudioClip m_landSound;
        [SerializeField] private AudioClip m_attackSound;
        [SerializeField] private AudioClip m_getHitSound;
        [SerializeField] private AudioClip m_deathSound;
        [SerializeField] private AudioClip m_blockSound;

        private AudioSource m_audioSource;

        public void PlayFootstepSound() => m_audioSource.PlayOneShot(m_footstepSound);
        public void PlayJumpSound() => m_audioSource.PlayOneShot(m_jumpSound);
        public void PlayLandSound() => m_audioSource.PlayOneShot(m_landSound);
        public void PlayAttackSound() => m_audioSource.PlayOneShot(m_attackSound);
        public void PlayGetHitSound() => m_audioSource.PlayOneShot(m_getHitSound);
        public void PlayDeathSound() => m_audioSource.PlayOneShot(m_deathSound);
        public void PlayBlockSound() => m_audioSource.PlayOneShot(m_blockSound);

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

    }
}
