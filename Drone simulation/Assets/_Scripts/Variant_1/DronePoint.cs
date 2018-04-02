using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePoint : MonoBehaviour
{
    public GameObject drone;
    GameObject _d;

    void Start()
    {
        StartCoroutine(CreateDrone());
    }

    IEnumerator CreateDrone()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 5.0f));
        while (true)
        {
            if (_d == null && CentralProcessingData.IsNodeClear(transform.position))
            {
                _d = Instantiate(drone, transform.position, transform.rotation);
                _d.GetComponent<Drone_V1>().target = CentralProcessingData.GetRandomTarget();
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 7.0f));
        }
    }
}
