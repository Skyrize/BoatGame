using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAgent : MonoBehaviour
{
    public Transform destination;
    public float speed = 1;
    public Vector3[] corners;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.position);
        agent.updatePosition = false;
        agent.updateRotation = false;
        // agent.speed = 0;
        // InvokeRepeating("LatePrint", 1f, 5f);
    }

    void LatePrint()
    {
        Debug.Log("BeginPrint");
        foreach (var item in agent.path.corners)
        {
            Debug.Log(item.ToString());
        }
        Debug.Log("EndPrint");
    }

    // Update is called once per frame
    void Update()
    {
        corners = agent.path.corners;
        
        // Debug.Log(agent.nextPosition.ToString());
    }
}
