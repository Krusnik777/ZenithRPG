using UnityEngine;

namespace DC_ARPG
{
    public class TrapFloor : MonoBehaviour
    {
        [SerializeField] private float m_destroyTime = 0.3f;
        [SerializeField] private AudioSource m_audioSource;

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                Destroy(gameObject, m_destroyTime);
                m_audioSource.Play();
            }
        }
    }
}
