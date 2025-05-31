using System.Collections.Generic;
using UnityEngine;

namespace DC_ARPG
{
    public class PathFinder
    {
        private CharacterAvatar characterAvatar;

        private List<Tile> clearList = new List<Tile>();

        public PathFinder(CharacterAvatar characterAvatar)
        {
            this.characterAvatar = characterAvatar;
        }

        public Stack<Tile> CalculatePath(Tile target, bool ignoreMechanismTiles, bool targetNeighbourTile = false)
        {
            if (characterAvatar == null || target == null) return null;

            var path = new Stack<Tile>();

            var currentTile = characterAvatar.CurrentTile;

            if (currentTile == null) characterAvatar.SetCurrentTile(characterAvatar.GetCurrentTile());

            List<Tile> openList = new List<Tile>();
            List<Tile> closedList = new List<Tile>();
            clearList.Clear();

            openList.Add(currentTile);
            clearList.Add(currentTile);
            currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
            currentTile.f = currentTile.h;

            while (openList.Count > 0)
            {
                Tile t = FindLowestF(openList);

                closedList.Add(t);

                if (t == target)
                {
                    var targetTile = t;

                    if (targetNeighbourTile) targetTile = FindEndTile(t);

                    path = ConstructPathToTarget(targetTile);

                    return path;
                }

                foreach (var tile in t.NeighborTiles)
                {
                    // Tile Checks START

                    if (ignoreMechanismTiles && tile.Type == TileType.Mechanism && !tile.CheckMechanismDisabled()) continue;

                    if (tile.Type == TileType.Pit || tile.Type == TileType.Obstacle) continue;

                    if (tile.Type == TileType.Closable && tile.CheckClosed()) continue;

                    if (tile.OccupiedBy != null && tile.OccupiedBy != (IMovable) characterAvatar && tile != target) continue;

                    // Tile Checks END

                    clearList.Add(tile);

                    if (closedList.Contains(tile))
                    {
                        // Do nothing, already processed
                    }
                    else if (openList.Contains(tile))
                    {
                        float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                        if (tempG < tile.g)
                        {
                            tile.ParentTile = t;

                            tile.g = tempG;
                            tile.f = tile.g + tile.h;
                        }
                    }
                    else
                    {
                        tile.ParentTile = t;

                        tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                        tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                        tile.f = tile.g + tile.h;

                        openList.Add(tile);
                    }
                }
            }

            // what to do if no path to target
            Debug.Log("Path not found");

            ResetPathFinding();

            return path;
        }

        public Stack<Tile> GetShortestPath(List<Tile> targets, out Tile targetedTile)
        {
            targetedTile = null;

            var paths = CalculateMultiplePaths(targets);

            if (paths == null || paths.Count == 0) return null;

            var path = new Stack<Tile>();

            foreach (var p in  paths)
            {
                if (path.Count == 0 || path.Count > p.Value.Count)
                {
                    path = p.Value;
                    targetedTile = p.Key;
                }
            }

            return path;
        }

        private Dictionary<Tile, Stack<Tile>> CalculateMultiplePaths(List<Tile> targets)
        {
            List<Tile> realTargets = new List<Tile>();

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].OccupiedBy != null && targets[i].OccupiedBy != (IMovable) characterAvatar) continue;

                if (targets[i].TargetedBy != null && targets[i].TargetedBy != characterAvatar) continue;

                realTargets.Add(targets[i]);
            }

            if (realTargets.Count == 0) return null;

            var paths = new Dictionary<Tile, Stack<Tile>>();

            for (int i = 0; i < realTargets.Count; i++)
            {
                var p = CalculatePath(realTargets[i], false);

                if (p == null || p.Count == 0) continue;

                paths.Add(realTargets[i], p);
            }

            return paths;
        }

        private Tile FindLowestF(List<Tile> list)
        {
            Tile lowest = list[0];

            foreach (var t in list)
            {
                if (t.f < lowest.f)
                {
                    lowest = t;
                }
            }

            list.Remove(lowest);

            return lowest;
        }

        private Tile FindEndTile(Tile t)
        {
            Stack<Tile> tempPath = new Stack<Tile>();

            Tile next = t.ParentTile;

            while (next != null)
            {
                tempPath.Push(next);
                next = next.ParentTile;
            }

            return t.ParentTile;
        }

        private Stack<Tile> ConstructPathToTarget(Tile tile)
        {
            Stack<Tile> path = new Stack<Tile>();

            Tile next = tile;

            while (next != null)
            {
                path.Push(next);

                next = next.ParentTile;
            }

            ResetPathFinding();

            return path;
        }

        private void ResetPathFinding()
        {
            foreach (var tile in clearList)
            {
                tile.ResetPathFindingValues();
            }

            clearList.Clear();
        }
    }
}
