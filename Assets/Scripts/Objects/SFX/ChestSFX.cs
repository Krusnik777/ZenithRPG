using UnityEngine;

namespace DC_ARPG
{
    public class ChestSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip m_lockedSound;
        [SerializeField] private AudioClip m_unlockedSound;
        [SerializeField] private AudioClip m_openSound;
        [SerializeField] private AudioClip m_closeSound;

        private AudioSource m_audioSource;

        public void PlayLockedSound() => m_audioSource.PlayOneShot(m_lockedSound);
        public void PlayUnlockedSound() => m_audioSource.PlayOneShot(m_unlockedSound);
        public void PlayOpenSound() => m_audioSource.PlayOneShot(m_openSound);
        public void PlayCloseSound() => m_audioSource.PlayOneShot(m_closeSound);

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }
    }
}
