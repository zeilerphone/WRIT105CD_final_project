using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder 
{
    public List<OverlayTile> FindRange(OverlayTile start, int range)
    {
        int stepCount = 0;

        List<OverlayTile> inRangeTiles = new List<OverlayTile>();
        inRangeTiles.Add(start);

        List<OverlayTile> tileForPrevious = new List<OverlayTile>();
        tileForPrevious.Add(start);

        while(stepCount < range)
        {
            List<OverlayTile> surrounding = new List<OverlayTile>();

            foreach(OverlayTile tile in tileForPrevious)
            {
                if(!tile.isBlocked)
                surrounding.AddRange(MapManager.Instance.GetNeighborTiles(tile, new List<OverlayTile>()));
            }

            inRangeTiles.AddRange(surrounding);
            tileForPrevious = surrounding.Distinct().ToList();
            stepCount++;
        }
        return inRangeTiles.Distinct().ToList();
    }
}
