using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder 
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> searchable)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

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

            List<OverlayTile> neighborTiles = MapManager.Instance.GetNeighborTiles(currentOverlayTile, searchable);

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


