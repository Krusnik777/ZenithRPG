using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class LevelData
    {
        public string SceneName;
        public string LevelTitle;
    }

    [CreateAssetMenu(fileName = "LevelsDataBase", menuName = "ScriptableObjects/LevelsDataBase")]
    public class LevelsDataBase : ScriptableObject
    {
        [SerializeField] private LevelData m_tutorialLevel;
        [SerializeField] private LevelData[] m_levels;

        public LevelData TutorialLevel => m_tutorialLevel;
        public LevelData[] Levels => m_levels;
    }
}
