using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform fleetTarget = null;

    // Start is called before the first frame update
    void Start()
    {
        var boatAgents = GetComponentsInChildren<BoatAgent>();

        foreach (BoatAgent agent in boatAgents)
        {
            agent.destination = fleetTarget;
        }
        Unselect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDestination(Vector3 destination)
    {
        fleetTarget.position = destination;
    }

    public void FireLeft()
    {
        var boatBinders = GetComponentsInChildren<EventBinder>();

        foreach (EventBinder binder in boatBinders)
        {
            binder.CallEvent("Fire Left");
        }
    }
    public void FireRight()
    {
        var boatBinders = GetComponentsInChildren<EventBinder>();

        foreach (EventBinder binder in boatBinders)
        {
            binder.CallEvent("Fire Right");
        }
    }

    public void Select()
    {
        var boatBinders = GetComponentsInChildren<EventBinder>();

        foreach (EventBinder binder in boatBinders)
        {
            binder.CallEvent("Select");
        }
        fleetTarget.GetComponent<EventBinder>().CallEvent("Select");
    }
    public void Unselect()
    {
        var boatBinders = GetComponentsInChildren<EventBinder>();

        foreach (EventBinder binder in boatBinders)
        {
            binder.CallEvent("Unselect");
        }
        fleetTarget.GetComponent<EventBinder>().CallEvent("Unselect");
    }
}
