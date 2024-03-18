using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    public int G, H;

    public int F { get { return G + H; } }
    public bool isBlocked = false;
    public bool hasCoin = false;
    public OverlayTile previous;
    public Vector3Int gridPosition;
    public Vector2Int grid2DPosition { get { return new Vector2Int(gridPosition.x, gridPosition.y); } }
    //public Color hidden = new Color(1, 1, 1, 0);
    //public Color visible = new Color(1, 1, 1, 0.3f);
    //public Color active = new Color(1, 0, 1, 0.7f);
    public string specialTag = "";

    public void SetVisible()
    {
        Color tempColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.3f);
    }
    public void SetHidden()
    {
        Color tempColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.0f);
    }
    // public void SetActive()
    // {
    //     GetComponent<SpriteRenderer>().color = active;
    // }
}
