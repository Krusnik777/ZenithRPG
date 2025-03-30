using UnityEngine;

namespace DC_ARPG
{
    public abstract class TwoStateAnimator : MonoBehaviour
    {
        public virtual bool InActiveState { get; }
        public virtual bool InInitState { get; }

        public abstract void Play();

        public abstract void ResetToInit();
    }
}
