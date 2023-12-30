using UnityEngine;

namespace DC_ARPG
{
    public class CharacterSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip m_footstepSound;
        [SerializeField] private AudioClip m_jumpSound;
        [SerializeField] private AudioClip m_landSound;
        [SerializeField] private AudioClip m_attackSound;

        private AudioSource m_audioSource;

        public void PlayFootstepSound() => m_audioSource.PlayOneShot(m_footstepSound);
        public void PlayJumpSound() => m_audioSource.PlayOneShot(m_jumpSound);
        public void PlayLandSound() => m_audioSource.PlayOneShot(m_landSound);
        public void PlayAttackSound() => m_audioSource.PlayOneShot(m_attackSound);

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

    }
}
