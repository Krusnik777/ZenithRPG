namespace DC_ARPG
{
    public class Timer
    {
        private float m_currentTime;

        public bool IsFinished => m_currentTime <= 0;

        public Timer(float startTime)
        {
            Start(startTime);
        }

        public void Start(float startTime)
        {
            m_currentTime = startTime;
        }

        public void RemoveTime(float deltaTime)
        {
            if (m_currentTime <= 0) return;

            m_currentTime -= deltaTime;
        }

    }
}
