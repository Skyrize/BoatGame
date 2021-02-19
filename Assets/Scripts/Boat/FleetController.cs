using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform fleetTarget = null;

    private bool hasSetDestination = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDestination(Vector3 destination)
    {
        fleetTarget.position = destination;
        if (!hasSetDestination) {
            hasSetDestination = true;
            var boatAgents = GetComponentsInChildren<BoatAgent>();

            foreach (BoatAgent agent in boatAgents)
            {
                agent.destination = fleetTarget;
            }
        }
    }

    public void Select()
    {
        var boatBinders = GetComponentsInChildren<EventBinder>();

        foreach (EventBinder binder in boatBinders)
        {
            binder.CallEvent("Select");
        }
    }
    public void Unselect()
    {
        var boatBinders = GetComponentsInChildren<EventBinder>();

        foreach (EventBinder binder in boatBinders)
        {
            binder.CallEvent("Unselect");
        }
    }
}
