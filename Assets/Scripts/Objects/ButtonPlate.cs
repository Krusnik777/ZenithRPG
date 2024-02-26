using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(Animator))]
    public class ButtonPlate : InspectableObject
    {
        private ButtonPressTrigger m_buttonPressTrigger;
        public ButtonPressTrigger PressTrigger => m_buttonPressTrigger;

        public void PressButton() => m_buttonPressTrigger.PressButton();

        public void UnpressButton() => m_buttonPressTrigger.UnpressButton();

        private void Start()
        {
            m_buttonPressTrigger = GetComponentInChildren<ButtonPressTrigger>();
        }

        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Нажимная плита.");

            base.OnInspection(player);
        }
    }
}
