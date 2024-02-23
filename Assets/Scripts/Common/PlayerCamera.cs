using UnityEngine;

namespace DC_ARPG
{
    public class PlayerCamera : MonoSingleton<PlayerCamera>
    {
        [SerializeField] private Camera m_camera;
        public GameObject Camera => m_camera.gameObject;
    }
}
