using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    [RequireComponent(typeof(BoxCollider))]
    public class StoryEventTrigger : MonoBehaviour
    {
        [SerializeField] private StoryEventInfo m_storyEventInfo;
        [Space]
        public UnityEvent EventOnStoryEventEnd;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.GetComponent<Player>())
            {
                StoryEventManager.Instance.StartEvent(m_storyEventInfo);

                StoryEventManager.Instance.EventOnStoryEventEnded += OnStoryEventEnded;
            }
        }

        private void OnStoryEventEnded()
        {
            EventOnStoryEventEnd?.Invoke();

            StoryEventManager.Instance.EventOnStoryEventEnded -= OnStoryEventEnded;

            Destroy(gameObject);
        }
    }
}
