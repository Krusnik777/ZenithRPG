using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] protected bool m_interactable;

        public UnityEvent OnClick;

        public event UnityAction<UIButton> EventOnPointerEnter;
        public event UnityAction<UIButton> EventOnPointerExit;
        public event UnityAction<UIButton> EventOnPointerClick;

        private bool inFocus = false;
        public bool InFocus => inFocus;

        public virtual void SetFocus()
        {
            if (!m_interactable) return;

            inFocus = true;
        }

        public virtual void UnsetFocus()
        {
            if (!m_interactable) return;

            inFocus = false;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!m_interactable) return;

            EventOnPointerEnter?.Invoke(this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!m_interactable) return;

            EventOnPointerExit?.Invoke(this);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!m_interactable) return;

            EventOnPointerClick?.Invoke(this);
            OnClick?.Invoke();
        }

        public virtual void OnButtonClick()
        {
            if (!m_interactable) return;

            OnClick?.Invoke();
        }

        public virtual void SetInteractable(bool state)
        {
            m_interactable = state;
        }
    }
}
