using UnityEngine;

namespace DC_ARPG
{
    public class DeletableObstacle : MonoBehaviour
    {
        public void DeleteObstacle()
        {
            // sfx + animation?
            Destroy(gameObject);
        }
    }
}
