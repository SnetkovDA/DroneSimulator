using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public GameObject CPD_GameObject;
    public Dictionary<Int32, List<Node>> routes;
    CentralProcessingData CPD;
    GridClass grid;

    void Awake()
    {
        grid = GetComponent<GridClass>();
        CPD = CPD_GameObject.GetComponent<CentralProcessingData>();
        routes = new Dictionary<int, List<Node>>();
    }

    public void StartFindPath(Vector3 pathStart, Vector3 pathEnd, Int32 ID)
    {
        StartCoroutine(CreateRoute(pathStart, pathEnd, ID));
    }

    public void ClearPath(Int32 ID)
    {
        List<Node> path = new List<Node>();
        routes.TryGetValue(ID, out path);
        routes.Remove(ID);
        for (int i = 0; i < path.Count; i++)
        {
            path[i].walkable = true;
        }
    }

    IEnumerator CreateRoute(Vector3 startPos, Vector3 endPos, Int32 ID)
    {
        Vector3[] waypoints = new Vector3[0];
        Boolean pathSuccess = false;
        Node startNode = grid.NodeFromWorldPos(startPos);
        Node targetNode = grid.NodeFromWorldPos(endPos);
        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);
            Node currentNode = new Node(false, Vector3.zero, 0, 0);
            Int32 newMovCostToNeighb = 0;
            while (openSet.Count > 0)
            {
                currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }
                foreach (Node node in grid.GetNeighbors(currentNode))
                {
                    if (!node.walkable || closedSet.Contains(node))
                        continue;
                    newMovCostToNeighb = (int)currentNode.gCost + GetDistance(currentNode, node);
                    if (newMovCostToNeighb < node.gCost || !openSet.Contains(node))
                    {
                        node.gCost = newMovCostToNeighb;
                        node.hCost = GetDistance(node, targetNode);
                        node.parent = currentNode;
                        currentNode.child = node;
                        if (!openSet.Contains(node))
                            openSet.Add(node);
                        else
                            openSet.UpdateItem(node);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode, ID);
            pathSuccess = waypoints.Length > 0;
        }
        CPD.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode, Int32 ID)
    {
        List<Node> result = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            result.Add(currentNode);
            currentNode = currentNode.parent;
            currentNode.walkable = false;
        }
        routes.Add(ID, result);
        routes[ID].Add(startNode);
        Vector3[] waypoints = SimplifyPath(result);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;
        Vector2 newDirection = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            newDirection = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i-1].worldPosition);
            }
            oldDirection = newDirection;
        }
        return waypoints.ToArray();
    }
    
    Int32 GetDistance(Node nodeA, Node nodeB)
    {
        Int32 dstX = Math.Abs(nodeA.gridX - nodeB.gridX);
        Int32 dstY = Math.Abs(nodeA.gridY - nodeB.gridY);
        return dstX > dstY ? 14 * dstY + 10 * (dstX - dstY) : 14 * dstX + 10 * (dstY - dstX);
    }

    public Node GetNode(Vector3 pos)
    {
        return grid.NodeFromWorldPos(pos);
    }
}
