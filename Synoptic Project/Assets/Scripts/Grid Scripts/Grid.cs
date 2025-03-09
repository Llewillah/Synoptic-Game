using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid
{
    Sprite[] sprites;
    private GridNode[,] gridArray;
    private float cellSize;
    public Grid(int width, int height, float cellSize, Sprite[] sprites) 
    {
        this.cellSize = cellSize;
        this.sprites = sprites;

        gridArray = new GridNode[width, height];

        //fills grid arrray with GridNodes
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridNode(this, x, y, SpawningManager.Instance.SpawnTile(GetWorldPos(x,y), cellSize), sprites[0]);
            }
        }

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //Draws Grid in unity for testing <--------------------- DELETE LATER
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.red, 100f);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.red, 100f);
            }
        }
    }

    private void GetGridPos(Vector3 worldPos, out int x, out int y) 
    {
        //turns unity world float x,y to grid int x,y
        x = Mathf.FloorToInt(worldPos.x / cellSize);
        y = Mathf.FloorToInt(worldPos.y / cellSize);
    }

    private Vector3 GetWorldPos(int x, int y) 
    {
        //turns grid x,y into unity world x,y
        return new Vector3(x , y) * cellSize;
    }

    public void DrawBuilding(Vector3 worldPos,Vector3 mousePos) 
    {
        
    }

}
