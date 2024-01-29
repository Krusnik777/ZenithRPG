using UnityEngine;
using TMPro;

namespace DC_ARPG
{
    public class HUDButtonInfo : MonoBehaviour, IDependency<Player>
    {
        [SerializeField] private GameObject m_buttonInfoPanel;
        [SerializeField] private TextMeshProUGUI m_useText;

        private Player m_player;
        public void Construct(Player player) => m_player = player;

        private void Update()
        {
            UpdateButtonInfo(m_player.CheckForwardGridForInsectableObject());
        }

        private void UpdateButtonInfo(InspectableObject inspectableObject)
        {
            if (inspectableObject != null && GameState.State == GameState.GameplayState.Active)
            {
                if (inspectableObject is InfoPlaque)
                {
                    m_buttonInfoPanel.SetActive(true);
                    m_useText.text = "Изучить";

                    return;
                }

                if (inspectableObject is LevelArm)
                {
                    var levelArm = inspectableObject as LevelArm;

                    if (levelArm.CanReset)
                    {
                        m_buttonInfoPanel.SetActive(true);
                        m_useText.text = "Использовать";
                    }
                    else
                    {
                        if (levelArm.Unused)
                        {
                            m_buttonInfoPanel.SetActive(true);
                            m_useText.text = "Использовать";
                        }
                        else m_buttonInfoPanel.SetActive(false);
                    }

                    return;
                }

                if (inspectableObject is ShopDoor)
                {
                    m_buttonInfoPanel.SetActive(true);
                    m_useText.text = "Зайти";

                    return;
                }

                if (inspectableObject is Door)
                {
                    var door = inspectableObject as Door;

                    m_buttonInfoPanel.SetActive(true);

                    if (door.Closed) m_useText.text = "Открыть";
                    if (door.Opened) m_useText.text = "Закрыть";

                    return;
                }

                if (inspectableObject is Chest)
                {
                    var chest = inspectableObject as Chest;

                    m_buttonInfoPanel.SetActive(true);

                    if (chest.Closed) m_useText.text = "Открыть";
                    if (chest.Opened) m_useText.text = "Закрыть";

                    return;
                }

                if (inspectableObject is ItemContainer)
                {
                    m_buttonInfoPanel.SetActive(true);

                    m_useText.text = "Подобрать";

                    return;
                }

                m_buttonInfoPanel.SetActive(false);
                m_useText.text = "Использовать";
            }
            else
            {
                m_buttonInfoPanel.SetActive(false);
            }
        }
    }
}
