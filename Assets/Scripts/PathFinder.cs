using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder 
{
    private Dictionary<Vector2Int, OverlayTile> searchableTiles;
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> inRangeTiles)
    {
        searchableTiles = new Dictionary<Vector2Int, OverlayTile>();

        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        if (inRangeTiles.Count > 0)
        {
            foreach (OverlayTile tile in inRangeTiles)
            {
                searchableTiles.Add(tile.grid2DPosition, tile);
            }
        }
        else
        {
            searchableTiles = MapManager.Instance.map;
        }

        openList.Add(start);

        // Debug.Log("Start Path Search");
        while(openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if(currentOverlayTile == end)
            {
                // finalize path
                return GetFinishedList(start, end);
            }

            List<OverlayTile> neighborTiles = MapManager.Instance.GetNeighborTiles(currentOverlayTile, inRangeTiles);

            foreach(OverlayTile tile in neighborTiles)
            {
                if (closedList.Contains(tile) || tile.isBlocked)
                {
                    // Debug.Log("Tile is blocked");
                    continue;
                }

                tile.G = GetManhattanDistance(start, tile);
                tile.H = GetManhattanDistance(end, tile);

                tile.previous = currentOverlayTile;

                if(!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;

        while(currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();
        return finishedList;
    }

    private int GetManhattanDistance(OverlayTile start, OverlayTile end)
    {
        return Mathf.Abs(start.gridPosition.x - end.gridPosition.x) + Mathf.Abs(start.gridPosition.y - end.gridPosition.y);
    }
    
}


