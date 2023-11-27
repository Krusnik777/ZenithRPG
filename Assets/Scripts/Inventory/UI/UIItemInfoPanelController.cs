using UnityEngine;


namespace DC_ARPG
{
    public class UIItemInfoPanelController : MonoBehaviour
    {
        [Header("StandardPanel")]
        [SerializeField] private UIItemInfoPanel m_standardpanel;
        [Header("SpecialPanel")]
        [SerializeField] private UISpecialItemInfoPanel m_specialpanel;

        public void ShowInfoPanel(IItemSlot slot)
        {
            if (slot.IsEmpty)
            {
                m_standardpanel.gameObject.SetActive(false);
                m_specialpanel.gameObject.SetActive(false);
                return;
            }

            if (slot.Item is WeaponItem || slot.Item is EquipItem || slot.Item is MagicItem)
            {
                m_standardpanel.gameObject.SetActive(false);

                m_specialpanel.gameObject.SetActive(true);
                m_specialpanel.SetInfoPanel(slot);
            }
            else
            {
                m_specialpanel.gameObject.SetActive(false);

                m_standardpanel.gameObject.SetActive(true);
                m_standardpanel.SetInfoPanel(slot);
            }
        }

        public void HideInfoPanel()
        {
            m_standardpanel.gameObject.SetActive(false);

            m_specialpanel.gameObject.SetActive(false);
        }


    }
}
