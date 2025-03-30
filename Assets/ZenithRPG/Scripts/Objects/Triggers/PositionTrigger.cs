using UnityEngine;

namespace DC_ARPG
{
    public class PositionTrigger : MonoBehaviour
    {
        private bool inRightPosition;
        public bool InRightPosition => inRightPosition;

        private void Start()
        {
            inRightPosition = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.root.GetComponent<Player>())
            {
                inRightPosition = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.GetComponent<Player>())
            {
                inRightPosition = false;
            }
        }
    }
}
