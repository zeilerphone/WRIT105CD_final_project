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

    public Dictionary<Vector2Int, OverlayTile> map;
    public List<Vector2Int> coins;
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
        coins = new List<Vector2Int>();
        

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
                            if(colliders.GetTile(localPlace).name == "coin" && !coins.Contains(tileKey)){
                                coins.Add(tileKey);
                                overlayTile.GetComponent<OverlayTile>().hasCoin = true;
                            }
                            if(colliders.GetTile(localPlace).name == "NPC"){
                                // coins.Add(tileKey);
                                overlayTile.GetComponent<OverlayTile>().hasNPC = true;
                                overlayTile.GetComponent<OverlayTile>().isBlocked = true;
                            }
                            if(colliders.GetTile(localPlace).name == "NPC talk"){
                                // coins.Add(tileKey);
                                overlayTile.GetComponent<OverlayTile>().hasNPCTrigger = true;
                            }
                            if(colliders.GetTile(localPlace).name == "reserved" || colliders.GetTile(localPlace).name == "reserved plant"){
                                overlayTile.GetComponent<OverlayTile>().isBlocked = true;
                            }
                            
                        }

                        //Debug.Log("Tile name: " + tileMap.GetTile(localPlace).name + " at position: " + localPlace + " with key: " + tileKey);
                        // if(localPlace == new Vector3Int(3,4,4))
                        // {
                        //     overlayTile.GetComponent<OverlayTile>().specialTag = "NPC talk";
                        //     overlayTile.GetComponent<SpriteRenderer>().color = new Color(143,237,255, 0);
                        // }

                        // if(localPlace == new Vector3Int(4,4,4))
                        // {
                        //     overlayTile.GetComponent<OverlayTile>().specialTag = "NPC";
                        //     overlayTile.GetComponent<OverlayTile>().isBlocked = true;
                        // }

                        //Debug.Log("Tile name: " + tileMap.GetTile(localPlace).name);
                        // if(tileMap.GetTile(localPlace).name == "water"){
                        //     overlayTile.GetComponent<OverlayTile>().isBlocked = true;
                        // }
                    }
                    
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void deleteCoinAt(Vector2Int position)
    {
        if(coins.Contains(position))
        {
            coins.Remove(position);
            colliders.SetTile(new Vector3Int(position.x, position.y, 4), null);
        }
    }
    
}
