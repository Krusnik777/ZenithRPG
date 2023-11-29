using UnityEngine;

namespace DC_ARPG
{
    public class SceneDependenciesContainer : Dependency
    {
        [SerializeField] private Player m_player;
        [SerializeField] private PlayerCharacter m_playerCharacter;
        [SerializeField] private ControlsManager m_controlsManager;

        protected override void BindAll(MonoBehaviour monoBehaviourInScene)
        {
            Bind<Player>(m_player, monoBehaviourInScene);
            Bind<PlayerCharacter>(m_playerCharacter, monoBehaviourInScene);
            Bind<ControlsManager>(m_controlsManager, monoBehaviourInScene);
        }

        private void Awake()
        {
            FindAllObjectsToBind();
        }
    }
}
