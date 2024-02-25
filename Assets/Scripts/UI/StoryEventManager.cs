using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace DC_ARPG
{
    public class StoryEventManager : MonoSingleton<StoryEventManager>, IDependency<ControlsManager>
    {
        [SerializeField] private GameObject m_HUD;

        [SerializeField] private Image m_backgroundImage;
        [SerializeField] private Animator m_displayBoundsAnimator;
        [Header("ImageBox")]
        [SerializeField] private Image m_imageBoxImage;
        [SerializeField] private TextMeshProUGUI m_imageBoxText;
        [Header("MessageBox")]
        [SerializeField] private GameObject m_messageBox;
        [SerializeField] private TextMeshProUGUI m_messageBoxText;
        [SerializeField] private GameObject m_messageBoxNamePanel;
        [SerializeField] private TextMeshProUGUI m_messageBoxNameText;
        [SerializeField] private Image m_messageImage;

        public event UnityAction EventOnStoryEventStarted;
        public event UnityAction EventOnStoryEventEnded;

        private ControlsManager m_controlsManager;
        public void Construct(ControlsManager controlsManager) => m_controlsManager = controlsManager;

        private StorySegment[] currentStorySegments;
        private StoryEventType currentStoryType;
        private int currentLineNumber;
        private int currentStorySegmentNumber;

        public void StartMicroEvent()
        {
            m_controlsManager.SetPlayerControlsActive(false);

            m_HUD.SetActive(false);
            m_displayBoundsAnimator.SetTrigger("Appear");
        }

        public void EndMicroEvent()
        {
            m_controlsManager.SetPlayerControlsActive(true);

            m_displayBoundsAnimator.SetTrigger("Disappear");
            m_HUD.SetActive(true);
        }

        public void StartEvent(StoryEventInfo storyEventInfo)
        {
            m_controlsManager.SetPlayerControlsActive(false);
            m_controlsManager.SetStoryEventControlsActive(true);

            m_HUD.SetActive(false);
            m_displayBoundsAnimator.SetTrigger("Appear");

            currentStorySegmentNumber = 0;
            currentStorySegments = storyEventInfo.StorySegments;
            currentStoryType = storyEventInfo.StoryEventType;

            if (currentStoryType == StoryEventType.Dialogue)
            {
                m_messageBox.SetActive(true);
                SetupCurrentDialogueSegment();
            }

            if (currentStoryType == StoryEventType.Plaque)
            {
                m_imageBoxImage.enabled = true;
                m_imageBoxText.enabled = true;
                SetupCurrentPlaqueEventSegment();
            }

            EventOnStoryEventStarted?.Invoke();
        }

        public void ContinueEvent()
        {
            if (currentStoryType == StoryEventType.Dialogue) DoDialogue();

            if (currentStoryType == StoryEventType.Plaque) DoPlaqueEvent();
        }

        private void EndEvent()
        {
            m_controlsManager.SetStoryEventControlsActive(false);
            m_controlsManager.SetPlayerControlsActive(true);

            m_displayBoundsAnimator.SetTrigger("Disappear");
            m_HUD.SetActive(true);

            currentStorySegments = null;
            currentStorySegmentNumber = 0;

            EventOnStoryEventEnded?.Invoke();
        }

        private void SetupCurrentDialogueSegment()
        {
            if (currentStorySegmentNumber < currentStorySegments.Length)
            {
                if (currentStorySegments[currentStorySegmentNumber].BackgroundImage != null)
                {
                    m_backgroundImage.enabled = true;
                    m_backgroundImage.sprite = currentStorySegments[currentStorySegmentNumber].BackgroundImage;
                }
                else m_backgroundImage.enabled = false;

                if (currentStorySegments[currentStorySegmentNumber].ImageBoxImage != null)
                {
                    m_messageImage.enabled = true;
                    m_messageImage.sprite = currentStorySegments[currentStorySegmentNumber].ImageBoxImage;
                }
                else m_messageImage.enabled = false;

                if (currentStorySegments[currentStorySegmentNumber].SpeakerName != "")
                {
                    m_messageBoxNamePanel.SetActive(true);
                    m_messageBoxNameText.text = currentStorySegments[currentStorySegmentNumber].SpeakerName;
                }
                else m_messageBoxNamePanel.SetActive(false);

                currentLineNumber = 0;

                DoDialogue();
            }
            else
            {
                if (m_backgroundImage.isActiveAndEnabled) m_backgroundImage.enabled = false;
                if (m_messageImage.isActiveAndEnabled) m_messageImage.enabled = false;
                if (m_messageBoxNamePanel.activeInHierarchy) m_messageBoxNamePanel.SetActive(false);
                m_messageBox.SetActive(false);
                EndEvent();
            }
        }

        private void DoDialogue()
        {
            if (currentLineNumber < currentStorySegments[currentStorySegmentNumber].Lines.Length)
            {
                m_messageBoxText.text = currentStorySegments[currentStorySegmentNumber].Lines[currentLineNumber];

                currentLineNumber++;
            }
            else
            {
                currentStorySegmentNumber++;
                SetupCurrentDialogueSegment();
            }
        }

        private void SetupCurrentPlaqueEventSegment()
        {
            if (currentStorySegmentNumber < currentStorySegments.Length)
            {
                m_imageBoxImage.sprite = currentStorySegments[currentStorySegmentNumber].ImageBoxImage;

                currentLineNumber = 0;

                DoPlaqueEvent();
            }
            else
            {
                m_imageBoxImage.enabled = false;
                m_imageBoxText.enabled = false;
                EndEvent();
            }
        }

        private void DoPlaqueEvent()
        {
            if (currentLineNumber < currentStorySegments[currentStorySegmentNumber].Lines.Length)
            {
                m_imageBoxText.text = currentStorySegments[currentStorySegmentNumber].Lines[currentLineNumber];

                currentLineNumber++;
            }
            else
            {
                currentStorySegmentNumber++;
                SetupCurrentPlaqueEventSegment();
            }
        }
    }
}
