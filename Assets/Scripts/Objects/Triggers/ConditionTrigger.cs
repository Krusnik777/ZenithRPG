using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class ConditionTrigger : MonoBehaviour
    {
        public UnityEvent OnTrigger;

        public event UnityAction<ConditionTrigger> EventOnConditionFulfilled;

        protected bool isTriggered = false;
        public bool IsTriggered => isTriggered;

        public void OnConditionFulfilled()
        {
            if (isTriggered) return;

            isTriggered = true;

            OnTrigger?.Invoke();
            EventOnConditionFulfilled?.Invoke(this);
        }
    }
}
