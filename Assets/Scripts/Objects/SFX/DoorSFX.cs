using UnityEngine;

namespace DC_ARPG
{
    public class DoorSFX : MonoBehaviour
    {
        [SerializeField] private AudioClip m_lockedSound;
        [SerializeField] private AudioClip m_unlockedSound;
        [SerializeField] private AudioClip m_useSound;

        private AudioSource m_audioSource;

        public void PlayLockedSound() => m_audioSource.PlayOneShot(m_lockedSound);
        public void PlayUnlockedSound() => m_audioSource.PlayOneShot(m_unlockedSound);
        public void PlayUseSound() => m_audioSource.PlayOneShot(m_useSound);

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }
    }
}
