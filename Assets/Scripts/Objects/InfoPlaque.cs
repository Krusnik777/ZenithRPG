using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class InfoPlaque : InspectableObject
    {
        [SerializeField] private StoryEventInfo m_storyEventInfo;
        [Space]
        public UnityEvent EventOnInspectionEnd;

        public override void OnInspection(Player player)
        {
            StoryEventManager.Instance.StartEvent(m_storyEventInfo);

            StoryEventManager.Instance.EventOnStoryEventEnded += OnStoryEventEnded;

            base.OnInspection(player);
        }

        private void OnStoryEventEnded()
        {
            EventOnInspectionEnd?.Invoke();

            StoryEventManager.Instance.EventOnStoryEventEnded -= OnStoryEventEnded;
        }
    }
}
