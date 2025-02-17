using UnityEngine;

public class GridNode
{
    private Grid grid;
    private int x, y;
    public GridNode(Grid grid, int x, int y) 
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
}
