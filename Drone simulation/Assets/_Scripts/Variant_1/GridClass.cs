using System;
using System.Collections.Generic;
using UnityEngine;

//Y - represents Z in world space

public class GridClass : MonoBehaviour
{
    
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public Single nodeRadius; //How much individual node covers
    public Boolean displayGridGizmos;

    Node[,] grid; //all nodes on the scene
    Single nodeDiameter;
    Int32 gridSizeX, gridSizeY;

    public Int32 MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    void Awake()
    {
        nodeDiameter = 2 * nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        Debug.Log(gridSizeX + " : " + gridSizeY + " Total: " + (gridSizeX*gridSizeY));
        CreateGrid();
    }
    
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        Vector3 worldPoint = new Vector3();
        bool walkable = false;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                worldPoint.y = 11.0f;
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPos(Vector3 _worldPos)
    {
        Single percentX = (_worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        Single percentY = (_worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        Int32 x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        Int32 y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbours = new List<Node>();
        Int32 checkX = 0, checkY = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                checkX = node.gridX + x;
                checkY = node.gridY + y;
                //if this coordinats in the grid
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    //add this node to neighbours
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    void OnDrawGizmos()
    {
        if (grid != null && displayGridGizmos)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
            foreach (Node item in grid)
            {
                Gizmos.color = item.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(item.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
