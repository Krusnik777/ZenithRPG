using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class StoryEvent : MonoBehaviour
    {
        [SerializeField] private StoryEventInfo m_storyEventInfo;
        [Space]
        public UnityEvent EventOnStoryEventEnd;

        public void StartStoryEvent()
        {
            if (m_storyEventInfo == null) return;

            StoryEventManager.Instance.StartEvent(m_storyEventInfo);

            StoryEventManager.Instance.EventOnStoryEventEnded += OnStoryEventEnded;
        }

        private void OnStoryEventEnded()
        {
            StoryEventManager.Instance.EventOnStoryEventEnded -= OnStoryEventEnded;

            EventOnStoryEventEnd?.Invoke();
        }
    }
}
