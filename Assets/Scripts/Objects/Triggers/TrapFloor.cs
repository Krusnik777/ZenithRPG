using UnityEngine;

namespace DC_ARPG
{
    public class TrapFloor : MonoBehaviour
    {
        [SerializeField] private float m_destroyTime = 0.3f;


        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                Destroy(gameObject, m_destroyTime);
            }
        }
    }
}
