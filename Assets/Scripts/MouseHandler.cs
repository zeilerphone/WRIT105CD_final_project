using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;


//using System.Numerics;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MouseHandler : MonoBehaviour
{
    public float speed;
    public int range;
    public int numCoins;
    public int NPCTalkIndex;
    private bool isTalking = false;
    private bool isMoving = false;
    public CinemachineVirtualCamera vcam;
    public DialogueManager conversation;

    public GameObject characterPrefab;
    private CharacterInfo character;

    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    private List<OverlayTile> path;
    private List<OverlayTile> inRange;

    //public GameObject cursor;
    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
        path = new List<OverlayTile>();
        inRange = new List<OverlayTile>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!conversation.isTalking)
        {
            if(!isMoving)
            {
                RaycastHit2D? hit = GetMouseHoverTile();

                if(hit.HasValue)
                {
                    OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                    transform.position = tile.transform.position;
                    GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
    
                    if (Input.GetMouseButtonDown(0))
                    {
                        if(character == null)
                        {
                            character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                            PositionCharacterOnTile(tile);
                            TilesInRange();
                            character.activeTile = tile;
                            vcam.Follow = character.transform;
                            //vcam.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, vcam.transform.position.z + 10);
                            //vcam.LookAt = character.transform;
                        } else
                        {
                            Debug.Log(tile.isBlocked);
                            if(tile.isBlocked)
                            {
                                return;
                            }
                            //Debug.Log("Character is not null");
                            path = pathFinder.FindPath(character.activeTile, tile, inRange);
                            //Debug.Log("Path Found with length: " + path.Count);
                        }
                    }
                }
            }

            if(!(character == null))
            {
                //Debug.Log("Path Count: " + path.Count);
                if(path.Count > 0)
                {   
                    //Debug.Log("Moving Character");
                    MoveCharacter();
                }
                if(character.activeTile.GetComponent<OverlayTile>().specialTag == "NPC talk" && !isTalking)
                {
                    Debug.Log("Talking to NPC");
                    isTalking = true;
                    if(NPCTalkIndex <= 1){
                        conversation.setIndex(NPCTalkIndex);
                        NPCTalkIndex ++;
                    }
                    StartConversation();
                    
                } else if (character.activeTile.GetComponent<OverlayTile>().specialTag != "NPC talk"){
                    isTalking = false;
                }
            }
        }
    }

    private void TilesInRange()
    {
        foreach(OverlayTile tile in inRange)
        {
            tile.SetHidden();
        }

        if(range == 0)
        {
            inRange = MapManager.Instance.map.Values.ToList();
            character.activeTile.SetVisible();
        } else
        {
            inRange = rangeFinder.FindRange(character.activeTile, range);
            foreach(OverlayTile tile in inRange)
            {
                tile.SetVisible();
            }
        }
        // inRange = rangeFinder.FindRange(character.activeTile, range);

        //character.activeTile.GetComponent<SpriteRenderer>().color = character.activeTile.active;
    }
    private void MoveCharacter()
    {   
        isMoving = true;
        float step = speed * Time.deltaTime;
        float z = path[0].transform.position.z;

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, z);

        if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }

        if(path.Count == 0)
        {
            TilesInRange();
            isMoving = false;
        }
    }

    public RaycastHit2D? GetMouseHoverTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new(mousePos.x, mousePos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
        
        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.transform.position.z).First();
        }

        if(hits.Length >= 2)
        {
            Debug.Log("Hit: " + hits[0].transform.position.z + " and " + hits[1].transform.position.z);
        }

        return null;
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z );
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        character.activeTile = tile;
        if(tile.hasCoin)
        {
            numCoins++;
            tile.hasCoin = false;
            conversation.setCoins(numCoins);
            MapManager.Instance.deleteCoinAt(tile.grid2DPosition);
        }
    }

    void StartConversation()
    {
        conversation.isTalking = true;
    }
}
