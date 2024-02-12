using UnityEngine;

namespace DC_ARPG
{
    public class PlayerFollower : MonoBehaviour
    {
        [SerializeField] private Player m_player;
        [SerializeField] private float m_posYOffset;
        [SerializeField] private float m_jumpTimeUpdate;
        [SerializeField] private float m_movemntTimeUpdate = 0.55f;

        private float jumpTimer = 0;
        private float movementTimer = 0;

        private void Update()
        {
            if (m_player.IsJumping)
            {
                if (m_player.JumpedAndLanded)
                {
                    transform.position = new Vector3(m_player.transform.position.x, m_posYOffset, m_player.transform.position.z);
                    jumpTimer = 0;
                    return;
                }

                if (jumpTimer > m_jumpTimeUpdate)
                {
                    transform.position = new Vector3(m_player.transform.position.x, m_posYOffset, m_player.transform.position.z);
                    jumpTimer = 0;
                }
                else jumpTimer += Time.deltaTime;

                return;
            }
            else jumpTimer = 0;

            if (m_player.InMovement)
            {
                if (movementTimer > m_movemntTimeUpdate)
                {
                    transform.position = new Vector3(m_player.transform.position.x, m_posYOffset, m_player.transform.position.z);
                    movementTimer = 0;
                }
                else movementTimer += Time.deltaTime;

                return;
            }
            else movementTimer = 0;

            if (m_player.InIdleState) transform.position = new Vector3(m_player.transform.position.x, m_posYOffset, m_player.transform.position.z);
        }

    }
}
