using UnityEngine;

namespace DC_ARPG
{
    public class InfoPlaque : InspectableObject
    {
        [SerializeField] private StoryEventInfo m_storyEventInfo;
        public override void OnInspection(Player player)
        {
            StoryEventManager.Instance.StartEvent(m_storyEventInfo);

            base.OnInspection(player);
        }
    }
}
