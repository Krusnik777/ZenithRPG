using UnityEngine;

namespace DC_ARPG
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private float m_linearMovementSpeed;

        [SerializeField] private float m_cameraPositionZOffset = -0.5f;
        [SerializeField] private float m_cameraPositionYOffset = 2.25f;
        [SerializeField] private float m_cameraRotationXOffset = 65f;

        public void SetTarget(Transform newTarget)
        {
            m_target = newTarget;
        }

        private void Update()
        {
            if (m_target == null) return;

            Vector3 cameraPos = transform.position;
            Vector3 targetPos = m_target.position;
            targetPos.z += m_cameraPositionZOffset;
            targetPos.y += m_cameraPositionYOffset;

            Vector3 newCamPos = Vector3.Lerp(cameraPos, targetPos, m_linearMovementSpeed * Time.deltaTime);

            transform.position = newCamPos;

            //transform.rotation = Quaternion.Lerp(transform.rotation, m_target.rotation, m_linearMovementSpeed * Time.deltaTime);

            //var targetRotation = m_target.rotation;

            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_linearMovementSpeed * Time.deltaTime);

            transform.LookAt(m_target.position);
        }
    }
}
