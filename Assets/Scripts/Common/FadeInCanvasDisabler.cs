using UnityEngine;

namespace DC_ARPG
{
    public class FadeInCanvasDisabler : MonoBehaviour
    {
        private void Start()
        {
            Invoke("TurnOffCanvas", 1f);
        }

        private void TurnOffCanvas()
        {
            gameObject.SetActive(false);
        }
    }
}
