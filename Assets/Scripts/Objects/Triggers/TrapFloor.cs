using UnityEngine;

namespace DC_ARPG
{
    public class TrapFloor : MonoBehaviour
    {
        [SerializeField] private float m_destroyTime = 0.2f;
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private GameObject m_floorBreakEffectPrefab;

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                Destroy(gameObject, m_destroyTime);
                m_audioSource.Play();
                if (m_floorBreakEffectPrefab != null)
                {
                    var effect = Instantiate(m_floorBreakEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, m_destroyTime+0.1f);
                }
            }
        }
    }
}
