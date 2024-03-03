using UnityEngine;

namespace DC_ARPG
{
    [System.Serializable]
    public class SceneBGM
    {
        public string SceneName;
        public AudioClip BGM;
    }

    [CreateAssetMenu(fileName = "MusicDataBase", menuName = "ScriptableObjects/MusicDataBase")]
    public class MusicDataBase : ScriptableObject
    {
        [SerializeField] private SceneBGM[] m_levelsBGM;
        [SerializeField] private AudioClip m_gameOverTheme;
        public SceneBGM[] LevelsBGM => m_levelsBGM;
        public AudioClip GameOverTheme => m_gameOverTheme;
    }
}
