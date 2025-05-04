using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingGrid: MonoBehaviour
{
    Grid mainGrid, tempBuildGrid;

    private const int STRAIGHT_COST = 10, DIAGONAL_COST = 14;

    [SerializeField] int gridX, gridY;
    [SerializeField] float cellSize;
    [SerializeField] Sprite[] sprites;

    public List<GameObject> buildings = new List<GameObject>();

    public static BuildingGrid instance;

    private bool building = false;

    private Stack<(int x,int y)> prevOperation;

    private List<GridNode> openList;
    private List<GridNode> closeList;
    private void Start()
    {
        mainGrid = new Grid(gridX, gridY, cellSize);
        instance = this;
        prevOperation = new Stack<(int, int)>();
    }

    private void Update()
    {
        
    }

    public GameObject BuildBuilding(int buildNum) 
    {
        building = true;
        mainGrid.GetGridPos(Camera.main.ScreenToWorldPoint(Input.mousePosition), out int x,  out int y);
        GameObject newBuilding = SpawningManager.Instance.SpawnBuilding(new Vector3(x, y, -1), cellSize, buildNum);
        return newBuilding;
    }

    public void CompleteBuilding(GameObject building) 
    {
        this.building = false;
        mainGrid.SetBuilding(building, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        buildings.Add(building);
    }

    public bool CheckAdjacent(Vector3 pos) 
    {
        mainGrid.GetGridPos(pos, out int x, out int y);

        if (buildings.Count < 1) 
        {
            return true;
        } 

        if (mainGrid.GetNode(x + 1, y).GetBuilding() != null || mainGrid.GetNode(x - 1, y).GetBuilding() != null || mainGrid.GetNode(x, y + 1).GetBuilding() != null || mainGrid.GetNode(x, y - 1).GetBuilding() != null) 
        {
            return true;
        }

        return false;
    }

    public Grid GetGrid() 
    {
        return mainGrid;
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos) 
    {
        mainGrid.GetGridPos(startPos, out int startX, out int startY);
        mainGrid.GetGridPos(endPos, out int endX, out int endY);

        List<GridNode> path = FindPath(startX, startY, endX, endY);

        if (path == null)
        {
            return null;
        }
        else 
        { 
            List<Vector3> vectorPath = new List<Vector3>();

            foreach (GridNode node in path) 
            {
                vectorPath.Add(new Vector3(node.x, node.y) * cellSize + Vector3.one * cellSize * .5f);
            }

            return vectorPath;
        }


    }

    private List<GridNode> FindPath(int startX, int startY, int endX, int endY) 
    {
        GridNode startNode = mainGrid.GetNode(startX, startY);
        GridNode endNode = mainGrid.GetNode(endX, endY);

        openList = new List<GridNode>() { startNode };
        closeList = new List<GridNode>();

        for (int x = 0; x < gridX; x++) 
        {
            for (int y = 0; y < gridY; y++) 
            {
                GridNode gridNode = mainGrid.GetNode(x, y);
                gridNode.gCost = int.MaxValue;
                gridNode.CalculateFCost();
                gridNode.prevNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0) 
        {
            GridNode curNode = GetLowestFCost(openList);

            if (curNode == endNode) 
            {
                return CalculatePath(endNode);
            }

            openList.Remove(curNode);
            closeList.Add(curNode);

            foreach (GridNode neighbour in GetNeighborList(curNode)) 
            {
                if (closeList.Contains(neighbour)) continue;
                if (!neighbour.walkable) 
                {
                    closeList.Add(neighbour);
                    continue;
                }
                int tentativeGCost = curNode.gCost + CalculateDistance(curNode, neighbour);

                if (tentativeGCost < neighbour.gCost) 
                {
                    neighbour.prevNode = curNode;
                    neighbour.gCost = tentativeGCost;
                    neighbour.hCost = CalculateDistance(neighbour, endNode);
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour)) 
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private List<GridNode> CalculatePath(GridNode endNode) 
    {
        List<GridNode> path = new List<GridNode>();
        path.Add(endNode);
        GridNode curNode = endNode;

        while (curNode.prevNode != null) 
        {
            path.Add(curNode.prevNode);
            curNode = curNode.prevNode;
        }
        path.Reverse();
        return path;
    }
    private int CalculateDistance(GridNode a, GridNode b) 
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remainder = Mathf.Abs(xDistance - yDistance);

        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remainder;
    }

    private GridNode GetLowestFCost(List<GridNode> nodeList) 
    {
        GridNode lowestFCost = nodeList[0];

        for (int i = 0; i < nodeList.Count; i++) 
        {
            if (nodeList[i].fCost < lowestFCost.fCost) 
            { 
                lowestFCost = nodeList[i];
            }
        }

        return lowestFCost;
    }

    public List<GridNode> GetNeighborList(GridNode curNode)
    {
        List<GridNode> neighbourList = new List<GridNode>();

        if (curNode.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(curNode.x - 1, curNode.y));

            if (curNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(curNode.x - 1, curNode.y - 1));
            }

            if (curNode.y + 1 < gridY)
            {
                neighbourList.Add(GetNode(curNode.x - 1, curNode.y + 1));
            }
        }
        if (curNode.x + 1 < gridX)
        {
            neighbourList.Add(GetNode(curNode.x + 1, curNode.y));

            if (curNode.y - 1 >= 0)
            {
                neighbourList.Add(GetNode(curNode.x + 1, curNode.y - 1));
            }

            if (curNode.y + 1 < gridY)
            {
                neighbourList.Add(GetNode(curNode.x + 1, curNode.y + 1));
            }
        }

        if (curNode.y - 1 >= 0)
        {
            neighbourList.Add(GetNode(curNode.x, curNode.y - 1));
        }

        if (curNode.y + 1 < gridY)
        {
            neighbourList.Add(GetNode(curNode.x, curNode.y + 1));
        }

        return neighbourList;
    }

    private GridNode GetNode(int x, int y)
    {
        return mainGrid.GetNode(x, y);
    }

    public void RemoveBuilding(int ID) 
    {
        foreach (GameObject b in buildings) 
        {
            foreach (BuildingAttachable ba in b.GetComponent<BuildingAttachable>().prevBuildings) 
            { 
                if (ba.ID == ID) 
                {
                    b.GetComponent<BuildingAttachable>().prevBuildings.Remove(ba);
                }
            }
        }
    }
}
