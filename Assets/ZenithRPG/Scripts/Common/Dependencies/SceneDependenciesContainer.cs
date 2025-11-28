using UnityEngine;

namespace DC_ARPG
{
    public class SceneDependenciesContainer : Dependency
    {
        [SerializeField] private Player m_player;
        [SerializeField] private PlayerCharacter m_playerCharacter;
        [SerializeField] private LevelState m_levelStateBeholder;

        protected override void BindAll(MonoBehaviour monoBehaviourInScene)
        {
            if (m_player == null)
            {
                m_player = FindFirstObjectByType<Player>(FindObjectsInactive.Include);
                m_playerCharacter = m_player.Character as PlayerCharacter;
            }

            Bind<Player>(m_player, monoBehaviourInScene);
            Bind<PlayerCharacter>(m_playerCharacter, monoBehaviourInScene);
            Bind<LevelState>(m_levelStateBeholder, monoBehaviourInScene);
        }

        private void Awake()
        {
            FindAllObjectsToBind();
        }
    }
}
