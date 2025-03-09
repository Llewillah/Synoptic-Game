using UnityEngine;

public class GridNode
{
    private Grid grid;
    private int x, y;
    private Sprite sprite;
    private GameObject tile;
    public GridNode(Grid grid, int x, int y, GameObject tile, Sprite sprite) 
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
}
