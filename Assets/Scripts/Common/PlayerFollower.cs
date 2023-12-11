using UnityEngine;

namespace DC_ARPG
{
    public class PlayerFollower : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private float m_posYOffset;
        [SerializeField] private float m_jumpTimeUpdate;

        private float timer = 0;

        private void Update()
        {
            if (m_player.IsJumping)
            {
                if (timer > m_jumpTimeUpdate)
                {
                    transform.position = new Vector3(m_player.transform.position.x, m_posYOffset, m_player.transform.position.z);
                    timer = 0;
                }
                else timer += Time.deltaTime;
            }
            else timer = 0;

            if (!m_player.InIdleState) return;

            transform.position = new Vector3(m_player.transform.position.x, m_posYOffset, m_player.transform.position.z);
        }

    }
}
