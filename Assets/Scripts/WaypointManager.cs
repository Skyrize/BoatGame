using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform[] waypoints = null;
    [SerializeField] private Transform _home = null;

    public Transform home {
        get {
            return _home;
        }
    }
    public static WaypointManager Instance = null;
    
    private void Awake() {
        if (Instance) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public Transform GetRandomWaypoint()
    {
        return waypoints[Random.Range(0, waypoints.Length)];
    }

    public Transform GetFarthestWaypoint(Vector3 location)
    {
        Transform result = waypoints[0];

        foreach (Transform waypoint in waypoints)
        {
            if (Vector3.Distance(waypoint.position, location) > Vector3.Distance(result.position, location)) {
                result = waypoint;
            }
        }
        return result;
    }
}
