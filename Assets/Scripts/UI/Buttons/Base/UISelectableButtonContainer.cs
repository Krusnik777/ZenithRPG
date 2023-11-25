using UnityEngine;

namespace DC_ARPG
{
    public class UISelectableButtonContainer : MonoBehaviour
    {
        [SerializeField] private Transform m_buttonsContainer;

        public bool Interactable = true;
        public void SetInteractable(bool interactable) => Interactable = interactable;

        private UISelectableButton[] buttons;

        private RectTransform[] buttonsTransforms;

        private int selectButtonIndex = 0;

        public UISelectableButton SelectedButton => buttons[selectButtonIndex];

        public Transform ButtonsContainer => m_buttonsContainer;

        #region Public

        public void SelectNext()
        {
            var newButtonIndex = selectButtonIndex;
            newButtonIndex++;
            if (newButtonIndex >= buttons.Length) newButtonIndex = 0;
            SelectButton(buttons[newButtonIndex]);
        }

        public void SelectPrevious()
        {
            var newButtonIndex = selectButtonIndex;
            newButtonIndex--;
            if (newButtonIndex < 0) newButtonIndex = buttons.Length - 1;
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
                if (newButtonIndex < 0) newButtonIndex = buttons.Length - 1;
            } while (buttonsTransforms[selectButtonIndex].anchoredPosition.y != buttonsTransforms[newButtonIndex].anchoredPosition.y);

            SelectButton(buttons[newButtonIndex]);
        }

        public void SelectRight()
        {
            var newButtonIndex = selectButtonIndex;

            do
            {
                newButtonIndex++;
                if (newButtonIndex >= buttons.Length) newButtonIndex = 0;
            } while (buttonsTransforms[selectButtonIndex].anchoredPosition.y != buttonsTransforms[newButtonIndex].anchoredPosition.y);

            SelectButton(buttons[newButtonIndex]);
        }

        public void SelectUp()
        {
            var newButtonIndex = selectButtonIndex;

            do
            {
                newButtonIndex--;
                if (newButtonIndex < 0) newButtonIndex = buttons.Length - 1;
            } while (buttonsTransforms[selectButtonIndex].anchoredPosition.x != buttonsTransforms[newButtonIndex].anchoredPosition.x);

            SelectButton(buttons[newButtonIndex]);
        }

        public void SelectDown()
        {
            var newButtonIndex = selectButtonIndex;

            do
            {
                newButtonIndex++;
                if (newButtonIndex >= buttons.Length) newButtonIndex = 0;
            } while (buttonsTransforms[selectButtonIndex].anchoredPosition.x != buttonsTransforms[newButtonIndex].anchoredPosition.x);

            SelectButton(buttons[newButtonIndex]);
        }


        #endregion

        #region Private

        private void Start()
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
        }

        private void OnDestroy()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].EventOnPointerEnter -= OnPointerEnter;
            }
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
