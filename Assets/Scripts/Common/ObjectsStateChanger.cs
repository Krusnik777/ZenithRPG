using UnityEngine;

namespace DC_ARPG
{
    public class ObjectsStateChanger : MonoBehaviour
    {
        [SerializeField] private TrapPlate[] m_traps;
        [SerializeField] private ButtonPlate[] m_buttons;
        [SerializeField] private Door[] m_doors;

        public void SetTrapsActive(bool state)
        {
            foreach (var trap in m_traps)
                trap.SetTrapActive(state);
        }

        public void SetButtonsActive(bool state)
        {
            foreach (var button in m_buttons)
                button.SetButtonActive(state);
        }

        public void ChangeDoorsOpenableState(bool state)
        {
            foreach (var door in m_doors)
                door.ChangeOpenableDirectly(state);
        }
    }
}
