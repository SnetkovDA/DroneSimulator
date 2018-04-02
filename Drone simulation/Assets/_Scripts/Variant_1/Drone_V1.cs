using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_V1 : MonoBehaviour
{
    public Vector3 target;
    public Single speed = 5;

    Vector3[] path;
    Int32 targetIndex;
    public LineRenderer lineRenderer;
    public Int32 ID;

    void Start()
    {
        ID = CentralProcessingData.GenerateID();
        CentralProcessingData.RequestPath(transform.position, target, ID, OnPathFound);
        lineRenderer.startColor = new Color(UnityEngine.Random.Range(0.0f, 1), 0, UnityEngine.Random.Range(0.0f, 1));
        lineRenderer.endColor = lineRenderer.startColor; 
    }

    void FixedUpdate()
    {
        
    }

    void OnPathFound(Vector3[] newPath, Boolean pathSuccessfull)
    {
        if (pathSuccessfull)
        {
            path = newPath;
            CentralProcessingData.RegisterDrone();
            lineRenderer.positionCount = path.Length+1;
            lineRenderer.SetPosition(0, transform.position);
            for (int i = 0; i < path.Length; i++)
            {
                lineRenderer.SetPosition(i+1, path[i]);
            }
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        else
            DestroyImmediate(gameObject);
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                    break;
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(currentWaypoint - transform.position);
            yield return new WaitForEndOfFrame();
        }
        CentralProcessingData.ClearPath(ID);
        yield return null;
        Destroy(gameObject, 1.0f);
    }

    //void OnDrawGizmos()
    //{
    //    if (path != null)
    //    {
    //        for (int i = targetIndex; i < path.Length; i++)
    //        {
    //            Gizmos.color = Color.black;
    //            Gizmos.DrawCube(path[i], Vector3.one);
    //            if (i == targetIndex)
    //            {
    //                Gizmos.DrawLine(transform.position, path[i]);
    //            }
    //            else
    //            {
    //                Gizmos.DrawLine(path[i - 1], path[i]);
    //            }
    //        }
    //    }
    //}
    }
