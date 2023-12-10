using UnityEngine;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class UISelectableButtonContainer : MonoBehaviour
    {
        [SerializeField] private Transform m_buttonsContainer;

        public bool Interactable = true;
        public void SetInteractable(bool interactable) => Interactable = interactable;

        private UISelectableButton[] buttons;
        public UISelectableButton[] Buttons => buttons;

        private RectTransform[] buttonsTransforms;

        private int selectButtonIndex = 0;
        public int SelectedButtonIndex => selectButtonIndex;

        public UISelectableButton SelectedButton => buttons[selectButtonIndex];

        public Transform ButtonsContainer => m_buttonsContainer;

        public event UnityAction EventOnButtonsCollected;

        #region Public

        public void SetButton(int index = 0) => SelectButton(buttons[index]);

        public void SelectNext()
        {
            var newButtonIndex = selectButtonIndex;
            newButtonIndex++;
            if (newButtonIndex >= buttons.Length) return;
            SelectButton(buttons[newButtonIndex]);
        }

        public void SelectPrevious()
        {
            var newButtonIndex = selectButtonIndex;
            newButtonIndex--;
            if (newButtonIndex < 0) return;
            SelectButton(buttons[newButtonIndex]);
        }

        public void ActivateButton()
        {
            buttons[selectButtonIndex].OnButtonClick();
        }

        public void SelectLeft()
        {
            var newButtonIndex = selectButtonIndex;

            do
            {
                newButtonIndex--;
                if (newButtonIndex < 0) return;
            } while (buttonsTransforms[selectButtonIndex].anchoredPosition.y != buttonsTransforms[newButtonIndex].anchoredPosition.y);

            SelectButton(buttons[newButtonIndex]);
        }

        public void SelectRight()
        {
            var newButtonIndex = selectButtonIndex;

            do
            {
                newButtonIndex++;
                if (newButtonIndex >= buttons.Length) return;
            } while (buttonsTransforms[selectButtonIndex].anchoredPosition.y != buttonsTransforms[newButtonIndex].anchoredPosition.y);

            SelectButton(buttons[newButtonIndex]);
        }

        public void SelectUp()
        {
            var newButtonIndex = selectButtonIndex;

            do
            {
                newButtonIndex--;
                if (newButtonIndex < 0) return;
            } while (buttonsTransforms[selectButtonIndex].anchoredPosition.x != buttonsTransforms[newButtonIndex].anchoredPosition.x);

            SelectButton(buttons[newButtonIndex]);
        }

        public void SelectDown()
        {
            var newButtonIndex = selectButtonIndex;

            do
            {
                newButtonIndex++;
                if (newButtonIndex >= buttons.Length) return;

            } while (buttonsTransforms[selectButtonIndex].anchoredPosition.x != buttonsTransforms[newButtonIndex].anchoredPosition.x);

            SelectButton(buttons[newButtonIndex]);
        }

        public void UpdateContainer()
        {
            var tempButtons = m_buttonsContainer.GetComponentsInChildren<UISelectableButton>();

            if (tempButtons.Length != buttons?.Length)
            {
                if (buttons != null)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].EventOnPointerEnter -= OnPointerEnter;
                    }
                }

                buttons = tempButtons;

                buttonsTransforms = new RectTransform[buttons.Length];

                for (int i = 0; i < buttons.Length; i++)
                {
                    buttonsTransforms[i] = buttons[i].transform.GetComponent<RectTransform>();

                    buttons[i].EventOnPointerEnter += OnPointerEnter;
                }
            }

            if (selectButtonIndex >= buttons.Length)
            {
                selectButtonIndex = buttons.Length - 1;
                buttons[selectButtonIndex].SetFocus();
            }
        }

        #endregion

        #region Private

        private void OnEnable()
        {
            buttons = m_buttonsContainer.GetComponentsInChildren<UISelectableButton>();

            if (buttons == null) Debug.LogError("Button List is Empty!");

            buttonsTransforms = new RectTransform[buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                buttonsTransforms[i] = buttons[i].transform.GetComponent<RectTransform>();

                buttons[i].EventOnPointerEnter += OnPointerEnter;
            }

            if (!Interactable) return;

            buttons[selectButtonIndex].SetFocus();

            EventOnButtonsCollected?.Invoke();
        }

        private void OnDisable()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].EventOnPointerEnter -= OnPointerEnter;
            }

            buttons[selectButtonIndex].UnsetFocus();
        }

        private void OnPointerEnter(UIButton button)
        {
            SelectButton(button);
        }

        private void SelectButton(UIButton button)
        {
            if (!Interactable) return;

            buttons[selectButtonIndex].UnsetFocus();

            for (int i = 0; i < buttons.Length; i++)
            {
                if (button == buttons[i])
                {
                    selectButtonIndex = i;
                    button.SetFocus();
                    break;
                }
            }
        }

        #endregion

    }
}
