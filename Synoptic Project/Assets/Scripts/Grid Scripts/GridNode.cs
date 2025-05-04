using Unity.VisualScripting;
using UnityEngine;

public class GridNode
{
    private Grid grid;
    public int x, y;
    private GameObject tile;
    private SpriteRenderer spriteRenderer;
    private GameObject building;

    public int gCost, hCost, fCost;

    public GridNode prevNode;

    public bool buildable = true, walkable = true;
    public GridNode(Grid grid, int x, int y) 
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        //spriteRenderer = tile.GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite) 
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetBuilding(GameObject set) 
    {
        building = set;
        buildable = false;
    }

    public GameObject GetBuilding() 
    {
        return building;
    }

    public void CalculateFCost() 
    { 
        fCost = gCost - hCost;
    }
}
