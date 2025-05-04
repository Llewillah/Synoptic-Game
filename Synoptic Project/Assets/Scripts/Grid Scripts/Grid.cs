using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid
{
    private GridNode[,] gridArray;
    private float cellSize;

    private int width, height;
    public Grid(int width, int height, float cellSize) 
    {
        this.cellSize = cellSize;
        this.width = width;
        this.height = height;
        gridArray = new GridNode[width, height];

        //fills grid arrray with GridNodes
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridNode(this, x, y);
                SpawningManager.Instance.SpawnTile(GetWorldPos(x,y),cellSize);
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

    public void GetGridPos(Vector3 worldPos, out int x, out int y) 
    {
        //turns unity world float x,y to grid int x,y
        x = Mathf.FloorToInt(worldPos.x / cellSize);
        y = Mathf.FloorToInt(worldPos.y / cellSize);
    }

    public Vector3 GetWorldPos(int x, int y) 
    {
        //turns grid x,y into unity world x,y
        return new Vector3(x + 0.5f, y + 0.5f) * cellSize;
    }

    public void SetSprite(Sprite sprite, int x, int y) 
    {
        gridArray[x, y].SetSprite(sprite);
    }

    public void SetBuilding(GameObject building, Vector3 worldPos)   
    {
        GetGridPos(worldPos, out int x, out int y);
        gridArray[x,y].SetBuilding(building);
    }

    public GridNode GetNode(int x, int y) 
    { 
        return gridArray[x,y];
    }

    
}
