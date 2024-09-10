using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class InspectableObject : MonoBehaviour
    {
        [SerializeField] protected string m_infoText;
        public UnityEvent EventOnInspection;

        public virtual bool Disabled => true;

        public virtual string InfoText => m_infoText;

        public void ShowInfoMessage(string message)
        {
            ShortMessage.Instance.ShowMessage(message);
        }

        public virtual void OnInspection(Player player)
        {
            EventOnInspection?.Invoke();
        }
    }
}
