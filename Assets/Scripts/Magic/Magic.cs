using UnityEngine;

namespace DC_ARPG
{
    public class Magic : MonoBehaviour
    {
        [SerializeField] private GameObject m_fireBallPrefab;

        public void CreateFireball(GameObject parent, Vector3 position, Quaternion rotation)
        {
            var fireBall = Instantiate(m_fireBallPrefab, position, rotation);
            fireBall.GetComponent<FireBall>().SetParent(parent);
        }
    }
}
