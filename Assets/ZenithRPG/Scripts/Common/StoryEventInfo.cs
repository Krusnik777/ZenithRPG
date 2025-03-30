using UnityEngine;

namespace DC_ARPG
{
    public enum StoryEventType
    {
        Dialogue, // BackgroundImage + MessageBox + possible ImageBoxImage
        Plaque // ImageBoxImage + ImageBoxText
    }

    [System.Serializable]
    public class StorySegment
    {
        public string SpeakerName;
        public Sprite BackgroundImage;
        public Sprite ImageBoxImage;
        [TextArea(1, 5)] public string[] Lines;
    }

    [CreateAssetMenu(fileName = "StoryEventInfo", menuName = "ScriptableObjects/StoryEventInfo")]
    public class StoryEventInfo : ScriptableObject
    {
        public StoryEventType StoryEventType;
        public StorySegment[] StorySegments;
    }
}
