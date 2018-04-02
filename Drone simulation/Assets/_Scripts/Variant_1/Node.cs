using System;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Vector3 worldPosition;   //position of node on the scene
    public Single gCost; // distance from starting node
    public Single hCost; // distance from end node
    public Int32 gridX; //position in the grid
    public Int32 gridY; //
    public Boolean walkable;        //is able to walk
    public Node parent;
    public Node child;
    Int32 heapIndex;
    public Single fCost
    {
        get { return gCost + hCost; }
    }
    public Int32 HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public Node(Boolean _walkable, Vector3 _pos, Int32 _gridX, Int32 _gridY)
    {
        walkable = _walkable;
        worldPosition = _pos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public Int32 CompareTo(Node node)
    {
        Int32 compare = fCost.CompareTo(node.fCost);
        if (compare == 0)
            compare = hCost.CompareTo(hCost);
        return -compare;
    }
}
