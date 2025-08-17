using UnityEngine;

namespace DC_ARPG
{
    public class GameSpeedController : MonoBehaviour
    {
        [SerializeField][Range(0.5f, 3)] private float m_speedScale;

        private void OnEnable()
        {
            Time.timeScale = m_speedScale;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }
    }
}
