using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], Boolean> callback;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], Boolean> _callback)
    {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}

public class CentralProcessingData : MonoBehaviour
{
    public GameObject pathFinderGO;
    public UnityEngine.UI.Text droneStats;

    List<Vector3> targets;
    Queue<PathRequest> pathRequestQ = new Queue<PathRequest>();
    Queue<Int32> requestIDs = new Queue<int>();
    List<Int32> droneID;
    PathRequest currentPR;
    PathFinding pathfinder;
    bool isProcessingPath;
    Int32 totalDroneCount;

    public static CentralProcessingData instance;

    void Awake()
    {
        instance = this;
        totalDroneCount = 0;
        droneID = new List<int>();
        targets = new List<Vector3>();
        pathfinder = pathFinderGO.GetComponent<PathFinding>();
        GameObject[] targetsGO = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < targetsGO.Length; i++)
        {
            targets.Add(targetsGO[i].transform.position);
        }
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Int32 ID, Action<Vector3[], Boolean> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQ.Enqueue(newRequest);
        instance.requestIDs.Enqueue(ID);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQ.Count > 0)
        {
            currentPR = pathRequestQ.Dequeue();
            isProcessingPath = true;
            pathfinder.StartFindPath(currentPR.pathStart, currentPR.pathEnd, requestIDs.Dequeue());
            
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPR.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    public static void ClearPath(Int32 ID)
    {
        instance.totalDroneCount--;
        instance.pathfinder.ClearPath(ID);
        instance.droneID.Remove(ID);
        instance.droneStats.text = instance.totalDroneCount.ToString();
    }

    public static Int32 GenerateID()
    {
        Int32 ID = UnityEngine.Random.Range(1000,9999);
        while (instance.droneID.Contains(ID))
            ID = UnityEngine.Random.Range(1000, 9999);
        return ID;
    }

    public static Vector3 GetRandomTarget()
    {
        return instance.targets[UnityEngine.Random.Range(0, instance.targets.Count)];
    }

    public static Boolean IsNodeClear (Vector3 position)
    {
        return instance.pathfinder.GetNode(position).walkable;
    }

    public static void RegisterDrone()
    {
        instance.totalDroneCount++;
        instance.droneStats.text = instance.totalDroneCount.ToString();
    }
}
