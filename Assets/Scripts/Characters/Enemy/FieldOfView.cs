using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private float m_radius;
        public float Radius => m_radius;
        [SerializeField] [Range(0,360)] private float m_angle;
        public float Angle => m_angle;
        [SerializeField] private LayerMask m_targetMask;
        [SerializeField] private LayerMask m_obstructionMask;

        private GameObject m_playerGameObject;
        public GameObject PlayerGameObject => m_playerGameObject;

        private bool canSeePlayer;
        public bool CanSeePlayer => canSeePlayer;

        private void Start()
        {
            m_playerGameObject = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(FOVRoutine());
        }

        private void CheckFieldOfView()
        {
            Vector3 pos = transform.position;
            pos.y = 0.5f;

            Collider[] rangeChecks = Physics.OverlapSphere(pos, m_radius, m_targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < m_angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(pos, directionToTarget, distanceToTarget, m_obstructionMask, QueryTriggerInteraction.Ignore)) canSeePlayer = true;
                    else canSeePlayer = false;
                }
                else canSeePlayer = false;
            }
            else if (canSeePlayer) canSeePlayer = false;
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while(true)
            {
                yield return wait;
                CheckFieldOfView();
            }
        }
    }
}
