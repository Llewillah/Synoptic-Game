using UnityEngine;

public class Grid
{
    private GridNode[,] gridArray;

    public Grid(int width, int height) 
    {
        gridArray = new GridNode[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridNode(this, x, y);
            }
        }
    }   
}
