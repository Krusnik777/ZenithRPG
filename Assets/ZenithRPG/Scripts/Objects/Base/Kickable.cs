using UnityEngine;

namespace DC_ARPG
{
    public class Kickable : MonoBehaviour
    {
        private IKickable kickable;

        private void Start()
        {
            kickable = GetComponent<IKickable>();

            if (kickable == null) enabled = false;
        }

        public void OnKicked(Vector3 direction)
        {
            kickable.OnKicked(direction);
        }
    }
}
