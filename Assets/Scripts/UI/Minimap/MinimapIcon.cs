using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class MinimapIcon : MonoBehaviour
    {
        [SerializeField] private string m_minimapIconId;
        public string MinimapIconId => m_minimapIconId;

        private Player m_explorer;

        private SpriteRenderer m_iconRenderer;

        private float m_distance = 1.85f;

        private bool m_discovered = false;
        public bool Discovered => m_discovered;

        public void SetDiscovered(bool state)
        {
            m_discovered = state;
            if (state) EnableIcon();
        }

        public void EnableIcon()
        {
            if (m_iconRenderer == null)
                m_iconRenderer = GetComponent<SpriteRenderer>();

            m_iconRenderer.enabled = true;
            enabled = false;
        }

        private void Start()
        {
            m_iconRenderer = GetComponent<SpriteRenderer>();
            m_explorer = FindObjectOfType<Player>();

            if (m_iconRenderer.enabled == true)
                m_iconRenderer.enabled = false;
        }

        private void Update()
        {
            if (m_discovered) return;

            Vector2 pos = new Vector2(transform.position.x, transform.position.z);
            Vector2 explorerPos = new Vector2(m_explorer.transform.position.x, m_explorer.transform.position.z);

            if (Vector2.Distance(pos, explorerPos) <= m_distance)
            {
                m_discovered = true;
                EnableIcon();
            }
        }

        #region Collection

        private static HashSet<MinimapIcon> m_allMinimapIcons;

        public static IReadOnlyCollection<MinimapIcon> AllMinimapIcons => m_allMinimapIcons;

        private void OnEnable()
        {
            if (m_allMinimapIcons == null)
                m_allMinimapIcons = new HashSet<MinimapIcon>();

            m_allMinimapIcons.Add(this);
        }

        private void OnDestroy()
        {
            m_allMinimapIcons.Remove(this);
        }

        #endregion

        #if UNITY_EDITOR

        [ContextMenu(nameof(GenerateIdForAllMinimapIcons))]
        private void GenerateIdForAllMinimapIcons()
        {
            if (Application.isPlaying) return;

            List<MinimapIcon> minimapIconsInScene = new List<MinimapIcon>();
            minimapIconsInScene.AddRange(FindObjectsOfType<MinimapIcon>());

            foreach (var minimapIcon in minimapIconsInScene)
            {
                minimapIcon.UpdateId();
            }
        }

        public void UpdateId()
        {
            m_minimapIconId = string.Empty;
            m_minimapIconId = "MinimapIcon_" + System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        #endif
    }
}
