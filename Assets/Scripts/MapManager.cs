using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public GameObject overlayPrefab;
    public GameObject overlayContainer;
    public Tilemap tileMap;
    public Tilemap colliders;
    public enum Region{
        start,
        stripmall,
        stripmall_fix,
        suburb,
        suburb_fix,
        mid_density,
        mid_density_fix
    }

    public Dictionary<Region, Vector2Int> spawnPoints;

    public Region currentRegion;

    public Dictionary<Vector2Int, OverlayTile> map;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new Dictionary<Vector2Int, OverlayTile>();

        currentRegion = Region.start;

        BoundsInt bounds = tileMap.cellBounds;

        // Loop through the bounds of the tilemap and check if a tile exists at each position
        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    Vector3Int localPlace = new(x, y, z);
                    Vector2Int tileKey = new(x, y);
                    if (tileMap.HasTile(localPlace) && !map.ContainsKey(tileKey) && !(tileMap.GetTile(localPlace).name == "water"))
                    {
                        //Tile at "place"
                        GameObject overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                        Vector3 place = tileMap.GetCellCenterWorld(localPlace);
                        overlayTile.transform.position = new Vector3(place.x, place.y, place.z + 1);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.GetComponent<OverlayTile>().gridPosition = localPlace;
                        map.Add(tileKey, overlayTile.GetComponent<OverlayTile>());

                        overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.0f);

                        if (colliders.HasTile(localPlace))
                        {
                            if(colliders.GetTile(localPlace).name == "NPC"){
                                overlayTile.GetComponent<OverlayTile>().hasNPC = true;
                                overlayTile.GetComponent<OverlayTile>().isBlocked = true;
                            }
                            if(colliders.GetTile(localPlace).name == "highlighted"){
                                overlayTile.GetComponent<OverlayTile>().hasNPCTrigger = true;
                            }
                            
                            
                        }
                        if(tileMap.GetTile(localPlace).name == "reserved" || tileMap.GetTile(localPlace).name == "reserved plant"){
                                overlayTile.GetComponent<OverlayTile>().isBlocked = true;
                        }
                    }
                }
            }
        }
    }

    public List<OverlayTile> GetNeighborTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchable = null)
    {        
        Dictionary<Vector2Int, OverlayTile> toSearch = new Dictionary<Vector2Int, OverlayTile>();

        if(searchable.Count > 0)
        {
            foreach(OverlayTile tile in searchable)
            {
                toSearch.Add(tile.grid2DPosition, tile);
            }
        } else
        {
            toSearch = map;
        }
        List<OverlayTile> neighborTiles = new List<OverlayTile>();
        //
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(currentOverlayTile.gridPosition.x, currentOverlayTile.gridPosition.y + 1),
            new Vector2Int(currentOverlayTile.gridPosition.x, currentOverlayTile.gridPosition.y - 1),
            new Vector2Int(currentOverlayTile.gridPosition.x + 1, currentOverlayTile.gridPosition.y),
            new Vector2Int(currentOverlayTile.gridPosition.x - 1, currentOverlayTile.gridPosition.y)
        };

        for(int i = 0; i < directions.Length; i++)
        {
            Vector2Int positionToCheck = directions[i];
            // Debug.Log("Checking position: " + positionToCheck + " at index i: " + i);

            if (toSearch.ContainsKey(positionToCheck))
            {
                if(Mathf.Abs(currentOverlayTile.transform.position.z - toSearch[positionToCheck].transform.position.z) < 2)
                    neighborTiles.Add(toSearch[positionToCheck].GetComponent<OverlayTile>());
            }
        }

        // Debug.Log(neighborTiles);
        return neighborTiles;
    }
}
