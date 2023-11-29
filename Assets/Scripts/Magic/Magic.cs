using UnityEngine;

namespace DC_ARPG
{
    public class Magic : MonoBehaviour
    {
        [SerializeField] private GameObject m_fireBallPrefab;

        public void CreateFireball(Vector3 position, Quaternion rotation)
        {
            Instantiate(m_fireBallPrefab, position, rotation);
        }
    }
}
