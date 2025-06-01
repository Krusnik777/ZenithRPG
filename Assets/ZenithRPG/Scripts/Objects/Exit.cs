namespace DC_ARPG
{
    public class Exit : InspectableObject
    {
        public static bool ChangedLevels = false;

        private PositionTrigger m_positionTrigger;
        public bool StandingInFrontOfExit => m_positionTrigger != null ? m_positionTrigger.InRightPosition : false;

        public void ToNextLevel(string sceneName)
        {
            if (SceneSerializer.Instance != null)
                SceneSerializer.Instance.DeleteCheckpoints();

            DataPersistenceManager.Instance.SaveTempData();

            ChangedLevels = true;
            SceneCommander.Instance.StartLevel(sceneName);
        }

        public void ExitToMainMenu() => SceneCommander.Instance.ReturnToMainMenu();

        public override void OnInspection(Player player)
        {
            if (!StandingInFrontOfExit)
            {
                // Not Possible but just to be sure

                ShortMessage.Instance.ShowMessage("Выход. С этой стороны не уйти.");
                return;
            }

            base.OnInspection(player);
        }

        private void Awake()
        {
            m_positionTrigger = GetComponentInChildren<PositionTrigger>();
        }
    }
}
