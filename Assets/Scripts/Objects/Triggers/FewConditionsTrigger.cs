using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class FewConditionsTrigger : MonoBehaviour
    {
        [SerializeField] private List<ConditionTrigger> m_conditions;

        public UnityEvent OnAllConditionsFulfilled;

        private bool isTriggered = false;
        public bool IsTriggered => isTriggered;

        private void Start()
        {
            foreach (var condition in m_conditions)
            {
                condition.EventOnConditionFulfilled += OnConditionFulfilled;
            }
        }

        private void OnConditionFulfilled(ConditionTrigger condition)
        {
            if (isTriggered) return;

            if (CheckConditions())
            {
                isTriggered = true;

                OnAllConditionsFulfilled?.Invoke();
            }

            condition.EventOnConditionFulfilled -= OnConditionFulfilled;
        }

        private bool CheckConditions()
        {
            foreach (var condition in m_conditions)
            {
                if (!condition.IsTriggered)
                    return false;
            }

            return true;
        }
    }
}
