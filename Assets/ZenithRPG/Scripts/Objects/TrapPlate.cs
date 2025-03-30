using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DC_ARPG
{
    public class TrapPlate : InspectableObject, IActivableObject, IDataPersistence
    {
        [SerializeField] protected int m_damage;
        [SerializeField] private Animator m_animator;
        [SerializeField] private AudioSource m_audioSource;

        protected bool disabled;

        public void SetDamage(int damage) => m_damage = damage;

        public void SetTrapActive(bool state) => disabled = !state;

        public override bool Disabled => disabled;

        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Ловушка!");

            base.OnInspection(player);
        }

        public void Activate(IMovable movable = null)
        {
            if (movable == null) return;

            if (disabled) return;

            if (movable is CharacterAvatar)
            {
                var characterAvatar = movable as CharacterAvatar;
                characterAvatar.Character.Stats.ChangeCurrentHitPoints(this, -m_damage);
            }

            SetAnimationAndSound();
        }

        private void SetAnimationAndSound()
        {
            m_animator.enabled = true;
            m_audioSource.Play();
        }

        private void OnAnimationEnd()
        {
            m_animator.enabled = false;
        }

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public bool enabled;
            public bool trapDisabled;

            public DataState() { }
        }

        [Header("Serialize")]
        [SerializeField] private string m_prefabId;
        [SerializeField] private string m_id;
        [SerializeField] private bool m_isSerializable = true;
        public string PrefabId => m_prefabId;
        public string EntityId => m_id;
        public bool IsCreated => false;

        public bool IsSerializable() => m_isSerializable;

        public string SerializeState()
        {
            DataState s = new DataState();

            s.trapDisabled = disabled;
            s.enabled = gameObject.activeInHierarchy;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            SetTrapActive(!s.trapDisabled);
            gameObject.SetActive(s.enabled);
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion

        #if UNITY_EDITOR

        [ContextMenu(nameof(GenerateIdForAllTraps))]
        private void GenerateIdForAllTraps()
        {
            if (Application.isPlaying) return;

            List<TrapPlate> trapsInScene = new List<TrapPlate>();
            trapsInScene.AddRange(FindObjectsOfType<TrapPlate>());

            for(int i = 0; i < trapsInScene.Count; i++)
            {
                trapsInScene[i].UpdateId(i);
            }
        }

        public void UpdateId(int number)
        {
            string name = "Trap_" + number.ToString("D3");
            gameObject.name = name;
            m_id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu(nameof(ChangeDamageForAllTraps))]
        private void ChangeDamageForAllTraps()
        {
            if (Application.isPlaying) return;

            List<TrapPlate> trapsInScene = new List<TrapPlate>();
            trapsInScene.AddRange(FindObjectsOfType<TrapPlate>());

            foreach (var trap in trapsInScene)
            {
                trap.SetDamage(m_damage);
                UnityEditor.EditorUtility.SetDirty(trap);
            }
        }

        #endif
    }
}
