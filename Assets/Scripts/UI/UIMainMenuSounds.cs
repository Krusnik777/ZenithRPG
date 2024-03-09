using UnityEngine;

namespace DC_ARPG
{
    [RequireComponent(typeof(AudioSource))]
    public class UIMainMenuSounds : MonoBehaviour
    {
        [SerializeField] private AudioClip m_clickSound;
        [SerializeField] private AudioClip m_hoverSound;
        [SerializeField] private AudioClip m_backSound;
        [SerializeField] private AudioClip m_startPressedSound;

        private AudioSource m_audioSource;

        private UIButton[] uIButtons;

        public void PlayBackSound() => m_audioSource.PlayOneShot(m_backSound);
        public void PlayStartPressedSound() => m_audioSource.PlayOneShot(m_startPressedSound);

        private void Start()
        {
            m_audioSource = GetComponent<AudioSource>();

            uIButtons = GetComponentsInChildren<UIButton>(true);

            for (int i = 0; i < uIButtons.Length; i++)
            {
                if (uIButtons[i] is UISelectableButton) (uIButtons[i] as UISelectableButton).OnSelect.AddListener(OnSelectButton);

                uIButtons[i].OnClick.AddListener(OnPointerClick);

                //uIButtons[i].EventOnPointerEnter += OnPointerEnter;
                //uIButtons[i].EventOnPointerClick += OnPointerClick;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < uIButtons.Length; i++)
            {
                if (uIButtons[i] is UISelectableButton) (uIButtons[i] as UISelectableButton).OnSelect.RemoveListener(OnSelectButton);

                uIButtons[i].OnClick.RemoveListener(OnPointerClick);

                //uIButtons[i].EventOnPointerEnter -= OnPointerEnter;
                //uIButtons[i].EventOnPointerClick -= OnPointerClick;
            }
        }

        private void OnSelectButton()
        {
            m_audioSource.PlayOneShot(m_hoverSound);
        }

        private void OnPointerClick()
        {
            m_audioSource.PlayOneShot(m_clickSound);
        }

        /*
        private void OnPointerEnter(UIButton button)
        {
            m_audioSource.PlayOneShot(m_hoverSound);
        }

        private void OnPointerClick(UIButton button)
        {
            m_audioSource.PlayOneShot(m_clickSound);
        }*/
    }
}
