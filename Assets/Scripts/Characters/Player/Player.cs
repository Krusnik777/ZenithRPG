using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class Player : CharacterAvatar, IDataPersistence
    {
        public enum PlayerState
        {
            Active,
            Rest
        }
        [Header("PlayerCharacter")]
        [SerializeField] private PlayerCharacter m_character;
        [Header("Magic")]
        [SerializeField] private float m_castDelay = 1.0f;

        public override Tile CurrentTile => currentTile ?? GetCurrentTile();

        public bool ActionsIsAvailable => !(IsFallingOrFallen || CurrentTile.Type == TileType.Mechanism);

        public override CharacterBase Character => m_character;

        private Ray m_lookRay;

        private PlayerState m_playerState;
        public PlayerState State => m_playerState;

        private bool isCasting;
        public bool IsCasting => isCasting;

        protected override bool inIdleState => !(inMovement || isJumping || isFalling || isAttacking || isBlocking || isCasting);

        public override void LandAfterFall()
        {
            base.LandAfterFall();

            if (currentTile.Type == TileType.Pit)
            {
                int damageAfterFall = currentTile.GetDamageFromPit();
                m_character.Stats.ChangeCurrentHitPoints(currentTile, -damageAfterFall);
            }
        }

        #region PlayerActions

        public override void Attack()
        {
            if (m_character.Inventory.WeaponItemSlot.IsEmpty) return;

            if (Mathf.Sign(transform.position.y) < 0) return;

            base.Attack();
        }

        public override void Block(string name)
        {
            if (m_character.Inventory.ShieldItemSlot.IsEmpty) return;

            if (Mathf.Sign(transform.position.y) < 0) return;

            base.Block(name);
        }

        public void Inspect()
        {
            if (!inIdleState) return;

            var inspectableObject = CheckForwardGridForInspectableObject();

            if (inspectableObject != null)
            {
                inspectableObject.OnInspection(this);
            }
            else
            {
                ShortMessage.Instance.ShowMessage("Ничего интересного.");
            }
        }

        public void UseMagic()
        {
            if (!inIdleState) return;

            if (m_character.Inventory.MagicItemSlot.IsEmpty) return;

            if (Mathf.Sign(transform.position.y) < 0) return;

            StartCoroutine(CastMagic());
        }

        public void ChangeRestState()
        {
            if (!inIdleState) return;

            if (!ActionsIsAvailable)
            {
                ShortMessage.Instance.ShowMessage("Здесь не получится отдохнуть.");

                return;
            }

            if (m_playerState == PlayerState.Active)
            {
                RestState.Instance.StartRest();

                m_playerState = PlayerState.Rest;

                GameState.State = GameState.GameplayState.NotActive;

                return;
            }

            if (m_playerState == PlayerState.Rest)
            {
                RestState.Instance.EndRest();

                m_playerState = PlayerState.Active;

                GameState.State = GameState.GameplayState.Active;
            }
        }

        public void UseActiveItem()
        {
            if (!inIdleState) return;

            m_character.Inventory.UseItem(this, m_character.Inventory.ActiveItemSlot);
        }

        public void ChooseLeftActiveItem()
        {
            m_character.Inventory.SetActiveItemSlot(this, 0);
        }

        public void ChooseMiddleActiveItem()
        {
            m_character.Inventory.SetActiveItemSlot(this, 1);
        }

        public void ChooseRightActiveItem()
        {
            m_character.Inventory.SetActiveItemSlot(this, 2);
        }

        #endregion

        public InspectableObject CheckForwardGridForInspectableObject()
        {
            RaycastHit hit;

            if (Physics.Raycast(m_lookRay, out hit, 1f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.transform.parent.TryGetComponent(out InspectableObject inspectableObject))
                    {
                        return inspectableObject;
                    }
                }
            }
            return null;
        }

        public Enemy CheckForwardGridForEnemy()
        {
            RaycastHit hit;

            if (Physics.Raycast(m_lookRay, out hit, 1f, 1, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.transform.parent.TryGetComponent(out Enemy enemy))
                    {
                        return enemy;
                    }
                }
            }
            return null;
        }

        public bool CheckForwardGridIsEmpty()
        {
            RaycastHit hit;

            if (Physics.Raycast(m_lookRay, out hit, 1f))
            {
                if (hit.collider && !hit.collider.isTrigger)
                {
                    return false;
                }
            }

            return true;
        }

        private void Update()
        {
            m_lookRay = new Ray(transform.position + new Vector3(0, 0.1f, 0), transform.forward);
        }

        #region Coroutines

        private IEnumerator CastMagic()
        {
            isCasting = true;

            m_animator.SetTrigger("CastMagic");

            var elapsed = 0.0f;

            yield return new WaitUntil(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("CastMagic"));

            while (elapsed < m_castDelay)
            {
                elapsed += Time.deltaTime;

                yield return null;
            }

            m_character.Inventory.MagicItemSlot.UseMagic(this, this);

            yield return new WaitWhile(() => m_animator.GetCurrentAnimatorStateInfo(0).IsName("CastMagic"));

            isCasting = false;
        }

        #endregion

        #region Serialize

        [System.Serializable]
        public class DataState
        {
            public Vector3 position;
            public Quaternion rotation;

            public DataState() { }
        }

        [Header("Serialize")]
        [SerializeField] private string m_prefabId;
        [SerializeField] private string m_id;
        [SerializeField] private bool m_isSerializable = true;
        public string PrefabId => m_prefabId;
        public string EntityId => m_id;
        public bool IsCreated => false;

        private bool saveCheckpointPosition = false;
        private Vector3 savedPosition = new Vector3();

        public void ResetCheckpointPosition() => saveCheckpointPosition = false;

        public void SavePosition(Vector3 pos)
        {
            saveCheckpointPosition = true;
            savedPosition = pos;
        }

        public bool IsSerializable() => m_isSerializable;

        public string SerializeState()
        {
            DataState s = new DataState();

            if (saveCheckpointPosition)
            {
                s.position = savedPosition;
            }
            else
            {
                s.position = transform.position;
            }

            s.rotation = transform.rotation;

            return JsonUtility.ToJson(s);
        }

        public void DeserializeState(string state)
        {
            DataState s = JsonUtility.FromJson<DataState>(state);

            transform.position = s.position;
            transform.rotation = s.rotation;
        }

        public void SetupCreatedDataPersistenceObject(string entityId, bool isCreated, string state) { }

        #endregion
    }
}
