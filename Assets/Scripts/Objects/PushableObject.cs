using System.Collections;
using UnityEngine;

namespace DC_ARPG
{
    public class PushableObject : InspectableObject, IKickable, IMovable
    {
        [SerializeField] protected float m_transitionMoveSpeed = 2.0f;
        [SerializeField] protected float m_transitionFallSpeed = 2.0f;
        [SerializeField] private Collider m_collider;

        private Tile currentTile;
        public Tile CurrentTile => currentTile;

        private bool inMovement;
        public bool InMovement => inMovement;

        private bool isFalling;

        public Tile GetCurrentTile()
        {
            Tile tile = null;

            Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), -Vector3.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1f, 1, QueryTriggerInteraction.Ignore))
            {
                tile = hit.collider.GetComponentInParent<Tile>();
            }

            return tile;
        }

        public void OnKicked(Vector3 direction)
        {
            if (inMovement || isFalling) return;

            Tile targetTile = currentTile.FindNeighbourByDirection(direction);

            if (targetTile == null) return;

            StartCoroutine(MoveByPush(targetTile));
        }

        public override void OnInspection(Player player)
        {
            ShortMessage.Instance.ShowMessage("Похоже, это можно толкнуть.");

            base.OnInspection(player);
        }

        private void Awake()
        {
            currentTile = GetCurrentTile();

            if (currentTile != null) currentTile.SetTileOccupied(this);
        }

        private void UpdateNewPosition()
        {
            if (currentTile == null) return;

            currentTile.SetTileOccupied(null);

            if (currentTile.Type == TileType.Mechanism) currentTile.ReturnMechanismToDefault();

            currentTile = GetCurrentTile();

            if (currentTile != null)
            {
                if (currentTile.Type == TileType.Mechanism)
                {
                    currentTile.GetTileReaction(this);
                }

                if (currentTile.Type == TileType.Pit)
                {
                    currentTile.GetTileReaction();
                    if (!isFalling) StartCoroutine(FallToPit());
                }
            }
        }

        private IEnumerator MoveByPush(Tile targetTile)
        {
            inMovement = true;

            targetTile.SetTileOccupied(this);

            var startPosition = transform.position;
            var targetPosition = targetTile.transform.position;

            var elapsed = 0.0f;

            while (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed * m_transitionMoveSpeed);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            UpdateNewPosition();

            inMovement = false;
        }

        private IEnumerator FallToPit()
        {
            isFalling = true;

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = new Vector3(transform.position.x, -1, transform.position.z);

            var elapsed = 0.0f;

            while (Vector3.Distance(transform.position, targetPosition) >= 0.05f)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed * m_transitionFallSpeed);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = targetPosition;

            currentTile.SetTileOccupied(null);
            currentTile.FillPit();

            isFalling = false;

            m_collider.enabled = false;
        }
    }
}
