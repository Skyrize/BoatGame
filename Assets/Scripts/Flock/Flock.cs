using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Flock : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected FlockAgent[] agents;
    [SerializeField] protected FlockBehavior behavior;



    private void Awake() {
    }

    public void Populate()
    {
        agents = GetComponentsInChildren<FlockAgent>();
    }

    virtual public void Generate()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
        foreach (FlockAgent agent in agents)
        {
            agent.flock = this;
        }
    }

    public abstract void UpdateAgent(FlockAgent agent);

    virtual public void UpdateAgents()
    {
        foreach (FlockAgent agent in agents)
        {
            if (agent && agent.isActiveAndEnabled) {

                UpdateAgent(agent);
            }
        }

    }

}
