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
        [SerializeField] private AudioClip m_mainMenuTheme;
        [SerializeField] private AudioClip m_gameOverTheme;
        [SerializeField] private AudioClip m_shopTheme;
        public SceneBGM[] LevelsBGM => m_levelsBGM;
        public AudioClip GameOverTheme => m_gameOverTheme;
        public AudioClip MainMenuTheme => m_mainMenuTheme;
        public AudioClip ShopTheme => m_shopTheme;
    }
}
