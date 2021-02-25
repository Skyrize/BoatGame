using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

[System.Serializable]
public class FleetEvent : UnityEvent<FleetController>
{
}
public class FleetController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] public FleetEvent onFleetDestroyed = new FleetEvent();
    [Header("References")]
    [SerializeField] protected Transform fleetTarget = null;
    [Header("Runtime")]
    [SerializeField] protected int remainingBoats = -1;
    public int RemainingBoats {
        get {
            return remainingBoats;
        }
    }

    public Vector3 Position {
        get {
            return transform.GetChild(0).position;
        }
    }

    // Start is called before the first frame update
    virtual protected void Start()
    {
        var boatAgents = GetComponentsInChildren<BoatAgent>();

        remainingBoats = boatAgents.Length;
        foreach (BoatAgent agent in boatAgents)
        {
            agent.destination = fleetTarget;
            agent.GetComponent<HealthComponent>().onDeathEvent.AddListener(this.OnBoatDestroyed);
        }
        Unselect();
    }

    public void OnBoatDestroyed()
    {
        remainingBoats--;
        if (remainingBoats == 0) {
            onFleetDestroyed.Invoke(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDestination(Vector3 destination)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(destination, out hit, 1000f, NavMesh.AllAreas)) {
            fleetTarget.position = hit.position;
        }
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
